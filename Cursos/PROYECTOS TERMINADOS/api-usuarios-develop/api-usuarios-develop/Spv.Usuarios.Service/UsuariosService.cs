using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.NSBTClient;
using Spv.Usuarios.Common.Dtos.PersonaService.Output;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Common.Errors;
using Spv.Usuarios.Common.Information;
using Spv.Usuarios.Common.LogEvents;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Domain.Exceptions;
using Spv.Usuarios.Domain.Interfaces;
using Spv.Usuarios.Domain.Services;
using Spv.Usuarios.Domain.Utils;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;
using Spv.Usuarios.Service.Utils;
using Spv.Usuarios.Service.Utils.Models;

namespace Spv.Usuarios.Service
{
    public class UsuariosService : IUsuariosService
    {
        private readonly ILogger<UsuariosService> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioV2Repository _usuarioV2Repository;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly IAuditoriaLogV2Repository _auditoriaV2Repository;
        private readonly IConfiguracionesService _configuracionesService;
        private readonly IConfiguracionesV2Service _configuracionesV2Service;
        private readonly IEncryption _encryption;
        private readonly IHelperDbServer _helperDbServer;
        private readonly IHelperDbServerV2 _helperDbServerV2;
        private readonly IPersonasRepository _personasRepository;
        private readonly IDatosUsuarioRepository _datosUsuarioRepository;
        private readonly INsbtRepository _nsbtRepository;
        private readonly ITDesEncryption _tDesEncryption;
        private readonly IHistorialClaveUsuariosV2Repository _historialClaveUsuariosV2Repository;
        private readonly IHistorialClaveUsuariosRepository _historialClaveUsuariosRepository;
        private readonly IHistorialUsuarioUsuariosV2Repository _historialUsuarioUsuariosV2Repository;
        private readonly IReglaValidacionV2Repository _reglaValidacionV2Repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private readonly IBtaRepository _btaRepository;

        private int? _cantidadDeIntentosDeLogin;
        private int? _cantidadDeIntentosDeLoginV2;
        private int? _cantidadHistorialDeCambiosDeClave;
        private int? _cantidadHistorialDeCambiosDeClaveV2;

        public UsuariosService(
            ILogger<UsuariosService> logger,
            IUsuarioRepository usuarioRepository,
            IUsuarioV2Repository usuarioV2Repository,
            IAuditoriaRepository auditoriaRepository,
            IAuditoriaLogV2Repository auditoriaV2Repository,
            IConfiguracionesService configuracionesService,
            IConfiguracionesV2Service configuracionesV2Service,
            IEncryption encryption,
            IHelperDbServer helperDbServer,
            IHelperDbServerV2 helperDbServerV2,
            IPersonasRepository personasRepository,
            IDatosUsuarioRepository datosUsuarioRepository,
            INsbtRepository nsbtRepository,
            ITDesEncryption tDesEncryption,
            IHistorialClaveUsuariosV2Repository historialClaveUsuariosV2Repository,
            IHistorialUsuarioUsuariosV2Repository historialUsuarioUsuariosV2Repository,
            IHistorialClaveUsuariosRepository historialClaveUsuariosRepository,
            IReglaValidacionV2Repository reglaValidacionV2Repository,
            IMapper mapper,
            IDistributedCache distributedCache,
            IBtaRepository btaRepository
        )
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _usuarioV2Repository = usuarioV2Repository;
            _auditoriaRepository = auditoriaRepository;
            _auditoriaV2Repository = auditoriaV2Repository;
            _configuracionesService = configuracionesService;
            _configuracionesV2Service = configuracionesV2Service;
            _encryption = encryption;
            _helperDbServer = helperDbServer;
            _helperDbServerV2 = helperDbServerV2;
            _personasRepository = personasRepository;
            _datosUsuarioRepository = datosUsuarioRepository;
            _nsbtRepository = nsbtRepository;
            _tDesEncryption = tDesEncryption;
            _historialClaveUsuariosV2Repository = historialClaveUsuariosV2Repository;
            _historialUsuarioUsuariosV2Repository = historialUsuarioUsuariosV2Repository;
            _historialClaveUsuariosRepository = historialClaveUsuariosRepository;
            _reglaValidacionV2Repository = reglaValidacionV2Repository;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _btaRepository = btaRepository;
        }

        public async Task<IResponse<AutenticacionModelOutput>> AutenticarAsync(
            IRequestBody<AutenticacionModelInput> autenticacionModel
        )
        {
            try
            {
                var result = await GetUsuarioFromDbAsync(
                    autenticacionModel.Body.UserName,
                    autenticacionModel.Body.DocumentNumber
                );

                var dateTime = await _helperDbServer.ObtenerFechaAsync();
                var dateTimeV2 = await _helperDbServerV2.ObtenerFechaAsync();
                var autenticacionModelOutput = new AutenticacionModelOutput();

                if (ValidarUsuario(result))
                {
                    var userId = 0;
                    if (result.Usuario == null)
                    {
                        autenticacionModelOutput.Codigo = Codigo.Incorrecto;
                        autenticacionModelOutput.Error = ErrorCode.UsuarioIncorrecto;
                    }
                    else
                    {
                        await AnalizarEstadoDeUsuarioAsync(
                            result.Usuario,
                            autenticacionModelOutput,
                            dateTime,
                            result.FromNewVersionDb
                        );
                        userId = result.FromNewVersionDb ? result.Usuario.GetUserId() : 0;
                    }

                    if (!autenticacionModel.RequestFromKeycloak())
                    {
                        await _auditoriaV2Repository.SaveAuditLogAsync(
                            userId,
                            EventTypes.Authentication,
                            EventResults.Error,
                            autenticacionModel.XCanal,
                            dateTimeV2,
                            JsonSerializer.Serialize(autenticacionModelOutput)
                        );
                    }

                    if (autenticacionModel.RequestFromKeycloak())
                    {
                        await _distributedCache.SetAsync(
                            DistributedCache.Usuario.Error(autenticacionModel.Body.Hash()),
                            DistributedCache.Serialize(autenticacionModelOutput),
                            DistributedCache.SlidingExpirationMinutes(1)
                        );
                    }

                    return Responses.Unauthorized<AutenticacionModelOutput>(
                        autenticacionModelOutput.Error?.Code,
                        autenticacionModelOutput.Error?.ErrorDescription
                    );
                }

                autenticacionModelOutput = await ValidarPasswordAsync(
                    autenticacionModel.Body.Password,
                    result.UsernameEncrypted,
                    result.Usuario,
                    result.FromNewVersionDb,
                    dateTime,
                    dateTimeV2
                );

                await SaveUsuario(result.Usuario, result.FromNewVersionDb);

                if (!autenticacionModel.RequestFromKeycloak())
                {
                    await _auditoriaV2Repository.SaveAuditLogAsync(
                        result.FromNewVersionDb ? result.Usuario.GetUserId() : 0,
                        EventTypes.Authentication,
                        autenticacionModelOutput.Codigo.Equals(Codigo.Aceptado) ? EventResults.Ok : EventResults.Error,
                        autenticacionModel.XCanal,
                        dateTimeV2,
                        JsonSerializer.Serialize(autenticacionModelOutput)
                    );
                }

                if (
                    autenticacionModel.RequestFromKeycloak() &&
                    !autenticacionModelOutput.Codigo.Equals(Codigo.Aceptado)
                )
                {
                    await _distributedCache.SetAsync(
                        DistributedCache.Usuario.Error(autenticacionModel.Body.Hash()),
                        DistributedCache.Serialize(autenticacionModelOutput),
                        DistributedCache.SlidingExpirationMinutes(1)
                    );
                }

                return autenticacionModelOutput.Codigo.Equals(Codigo.Aceptado)
                    ? Responses.Accepted(autenticacionModelOutput)
                    : Responses.Unauthorized<AutenticacionModelOutput>(
                        autenticacionModelOutput.Error?.Code,
                        autenticacionModelOutput.Error?.ErrorDescription
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingAutenticacion, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<AutenticacionClaveNumericaModelOutput>> AutenticarConClaveNumericaAsync(
            IRequestBody<AutenticacionClaveNumericaModelInput> autenticacionClaveNumericaModel
        )
        {
            try
            {
                var autenticacionModelOutput = new AutenticacionModelOutput();
                var autenticacionClaveNumericaModelOutput = new AutenticacionClaveNumericaModelOutput();
                var eventResults = EventResults.Error;
                var result = await GetUsuarioFromDbAsync(
                    autenticacionClaveNumericaModel.Body.DocumentCountryId,
                    autenticacionClaveNumericaModel.Body.DocumentTypeId,
                    autenticacionClaveNumericaModel.Body.DocumentNumber);

                await SetupCantidadDeIntentosDeLoginAsync();
                var dateTime = await _helperDbServer.ObtenerFechaAsync();
                var dateTimeV2 = await _helperDbServerV2.ObtenerFechaAsync();

                if (result.Usuario != null)
                {
                    if (result.Usuario.GetUserStatusId() != (byte)UserStatus.Active)
                    {
                        await AnalizarEstadoDeUsuarioAsync(result.Usuario, autenticacionModelOutput, dateTime,
                            result.FromNewVersionDb);

                        await _auditoriaV2Repository.SaveAuditLogAsync(
                            result.Usuario.GetUserId(),
                            EventTypes.AuthenticationNumericPassword,
                            eventResults,
                            autenticacionClaveNumericaModel.XCanal,
                            dateTimeV2,
                            JsonSerializer.Serialize(autenticacionModelOutput)
                        );

                        return Responses.Unauthorized<AutenticacionClaveNumericaModelOutput>(
                            autenticacionModelOutput.Error?.Code, autenticacionModelOutput.Error?.ErrorDescription);
                    }

                    autenticacionModelOutput = await ValidarPasswordAsync(
                        autenticacionClaveNumericaModel.Body.Password,
                        result.Usuario,
                        result.FromNewVersionDb,
                        dateTime,
                        dateTimeV2);

                    await SaveUsuario(result.Usuario, result.FromNewVersionDb);

                    if (autenticacionModelOutput.Codigo.Equals(Codigo.Aceptado))
                    {
                        var passwordExpired =
                            await ClaveVencidaAsync(result.Usuario.GetLastPasswordChange(), dateTimeV2.Now);
                        var passwordStatusMessage = PasswordStatusMessage(passwordExpired);

                        autenticacionClaveNumericaModelOutput.IdPersona = autenticacionModelOutput.IdPersona;
                        autenticacionClaveNumericaModelOutput.EstadoPassword = passwordStatusMessage;
                        autenticacionClaveNumericaModelOutput.FechaExpiracionPassword =
                            await GetPasswordExpiryDateAsync(result.Usuario.GetLastPasswordChange(),
                                result.FromNewVersionDb);

                        eventResults = EventResults.Ok;
                    }

                    await _auditoriaV2Repository.SaveAuditLogAsync(
                        result.Usuario.GetUserId(),
                        EventTypes.AuthenticationNumericPassword,
                        eventResults,
                        autenticacionClaveNumericaModel.XCanal,
                        dateTimeV2,
                        JsonSerializer.Serialize(autenticacionModelOutput)
                    );

                    return autenticacionModelOutput.Codigo.Equals(Codigo.Aceptado)
                        ? Responses.Accepted(autenticacionClaveNumericaModelOutput)
                        : Responses.Unauthorized<AutenticacionClaveNumericaModelOutput>(
                            autenticacionModelOutput.Error?.Code, autenticacionModelOutput.Error?.ErrorDescription);
                }
                else
                {
                    var resultNsbt = await _nsbtRepository.GetPinAsync(
                        autenticacionClaveNumericaModel.Body.DocumentCountryId.ToString(),
                        autenticacionClaveNumericaModel.Body.DocumentTypeId,
                        autenticacionClaveNumericaModel.Body.DocumentNumber);

                    var errorCode = await ValidarResultadoNsbtAsync(resultNsbt, autenticacionClaveNumericaModel);

                    autenticacionModelOutput.Error = errorCode;
                    autenticacionModelOutput.IdPersona =
                        await GetIdPersonaAsync(resultNsbt, autenticacionClaveNumericaModel);

                    if (errorCode == null)
                    {
                        autenticacionModelOutput.Codigo = Codigo.Aceptado;
                        eventResults = EventResults.Ok;

                        // se vuelve a cero el contador de intentos de bta
                        await _nsbtRepository.IncrementLoginAttemptsAsync(
                            autenticacionClaveNumericaModel.Body.DocumentCountryId.ToString(),
                            autenticacionClaveNumericaModel.Body.DocumentTypeId,
                            autenticacionClaveNumericaModel.Body.DocumentNumber,
                            resultNsbt?.Pin,
                            0);

                        var passwordExpired = ClaveVencidaBta(resultNsbt?.ExpirationDate, dateTimeV2.Now);
                        var passwordStatusMessage = PasswordStatusMessage(passwordExpired, true);

                        autenticacionClaveNumericaModelOutput.IdPersona = autenticacionModelOutput.IdPersona;
                        autenticacionClaveNumericaModelOutput.EstadoPassword = passwordStatusMessage;
                        autenticacionClaveNumericaModelOutput.FechaExpiracionPassword = resultNsbt?.ExpirationDate;
                    }

                    await _auditoriaV2Repository.SaveAuditLogAsync(
                        0,
                        EventTypes.AuthenticationNumericPassword,
                        eventResults,
                        autenticacionClaveNumericaModel.XCanal,
                        dateTimeV2,
                        JsonSerializer.Serialize(autenticacionModelOutput)
                    );

                    return autenticacionModelOutput.Codigo.Equals(Codigo.Aceptado)
                        ? Responses.Accepted(autenticacionClaveNumericaModelOutput)
                        : Responses.Unauthorized<AutenticacionClaveNumericaModelOutput>(
                            autenticacionModelOutput.Error?.Code, autenticacionModelOutput.Error?.ErrorDescription);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingAutenticacionConClaveNumerica, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<PerfilModelOutput>> ObtenerPerfilAsync(IRequestBody<PerfilModelInput> perfilModel)
        {
            try
            {
                var usuario = await _usuarioRepository.ObtenerPerfilUsuarioAsync(perfilModel.Body.UserName);

                if (usuario == null)
                {
                    return Responses.NotFound<PerfilModelOutput>(ErrorCode.UsuarioInexistente.ErrorDescription);
                }

                var perfil = new PerfilModelOutput
                {
                    LastLogon = usuario.LastLogon,
                    PersonId = usuario.GetPersonId(),
                    UserId = usuario.UserId,
                    Email = usuario.UserData.Mail,
                    FirstName = usuario.Name,
                    LastName = usuario.LastName
                };

                return Responses.Ok(perfil);
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingPerfil, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<PerfilModelOutput>> ObtenerPerfilAsync(
            IRequestBody<PerfilModelInputV2> perfilModelV2
        )
        {
            var outPutResult = await ObtenerPerfilAsync(perfilModelV2.Body.IdPersona);

            return outPutResult == null
                ? Responses.NotFound<PerfilModelOutput>(ErrorCode.UsuarioInexistente.ErrorDescription)
                : Responses.Ok(outPutResult);
        }

        public async Task<IResponse<PerfilMigradoModelOutput>> ObtenerPerfilAsync(
            IRequestBody<PerfilMigradoModelInput> perfilMigradoModel
        )
        {
            try
            {
                if (perfilMigradoModel.Body.IdPersona < 1)
                {
                    return Responses.BadRequest<PerfilMigradoModelOutput>(
                        ErrorCode.UsuarioInexistente.ErrorDescription
                    );
                }

                var usuarioMigrado = await _usuarioV2Repository.ObtenerUsuarioByPersonIdAsync(
                    perfilMigradoModel.Body.IdPersona
                );

                return usuarioMigrado == null
                    ? Responses.NotFound<PerfilMigradoModelOutput>(ErrorCode.UsuarioInexistente.ErrorDescription)
                    : Responses.Ok(_mapper.Map<PerfilMigradoModelOutput>(usuarioMigrado));
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingValidacionExistencia, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<RegistracionModelOutput>> RegistrarV1Async(
            IRequestBody<RegistracionModelInput> registracionModel
        )
        {
            try
            {
                var usuario = await _usuarioRepository.ObtenerUsuarioAsync(
                    registracionModel.Body.DocumentTypeId,
                    registracionModel.Body.DocumentNumber);

                if (usuario != null)
                    return Responses.Conflict<RegistracionModelOutput>(MessageConstants.UsuarioYaExiste);

                var dateTime = await _helperDbServer.ObtenerFechaAsync();

                var usuarioNuevo = new Usuario
                {
                    CustomerNumber = registracionModel.Body.CustomerNumber,
                    UserName = registracionModel.Body.UserName,
                    Password = _encryption.GetHash(registracionModel.Body.Password),
                    UserStatusId = (byte)UserStatus.Active,
                    Name = registracionModel.Body.FirstName,
                    LastName = registracionModel.Body.LastName,
                    LastPasswordChange = dateTime.Now,
                    CreatedDate = dateTime.Now,
                    DocumentCountryId = registracionModel.Body.DocumentCountryId,
                    DocumentTypeId = registracionModel.Body.DocumentTypeId,
                    DocumentNumber = registracionModel.Body.DocumentNumber,
                    ReceiptExtract = registracionModel.Body.ReceiptExtract,
                    ReceiptExtractDate = registracionModel.Body.ReceiptExtract != null ? dateTime.Now : (DateTime?)null,
                    CUIL = registracionModel.Body.Cuil,
                    FullControl = registracionModel.Body.FullControl
                };

                var datosUsuario = new DatosUsuario
                {
                    Mail = registracionModel.Body.Mail,
                    WorkPhone = registracionModel.Body.WorkPhone,
                    CellPhone = registracionModel.Body.CellPhone,
                    ReceiveMail = registracionModel.Body.ReceiveMail,
                    ReceiveSMS = registracionModel.Body.ReceiveSms,
                    CellCompanyId = registracionModel.Body.CellCompanyId,
                    WorkPhoneDate = registracionModel.Body.WorkPhone != null ? dateTime.Now : (DateTime?)null,
                    CellPhoneDate = registracionModel.Body.CellPhone != null ? dateTime.Now : (DateTime?)null,
                    PersonId = registracionModel.Body.PersonId
                };

                usuarioNuevo.UserData = datosUsuario;

                await _usuarioRepository.AddAsync(usuarioNuevo);
                await _usuarioRepository.SaveChangesAsync();

                return Responses.Created(new RegistracionModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingRegistroV1, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<RegistracionModelOutput>> RegistrarV2Async(
            IRequestBody<RegistracionModelInputV2> registracionModel
        )
        {
            try
            {
                var docNumber = $"DocumentNumber: {registracionModel.Body.DocumentNumber}";
                var dateTimeDbServerV2 = await _helperDbServerV2.ObtenerFechaAsync();
                var registacionNuevoModeloHabilitada = await _configuracionesService
                    .ObtenerConfiguracionRegistracionNuevoModeloEstaHabilitadaAsync();

                var validaciones = ValidarUsuarioRegistracion(registacionNuevoModeloHabilitada, registracionModel);

                if (validaciones.Length > 0)
                {
                    await _auditoriaV2Repository.SaveAuditLogAsync(
                        0,
                        EventTypes.Registration,
                        EventResults.Error,
                        registracionModel.XCanal,
                        dateTimeDbServerV2,
                        $"{docNumber} {validaciones}"
                    );

                    return Responses.BadRequest<RegistracionModelOutput>(validaciones.ToString());
                }

                var result = await _personasRepository.ObtenerInfoPersonaFisica(registracionModel.Body.PersonId);
                var usuarioNuevo = new UsuarioV2();

                if (result == null)
                {
                    await _auditoriaV2Repository.SaveAuditLogAsync(
                        0,
                        EventTypes.Registration,
                        EventResults.Error,
                        registracionModel.XCanal,
                        dateTimeDbServerV2,
                        $"{docNumber} {string.Format(MessageConstants.NoSeEncontroPersonaFisica, registracionModel.Body.PersonId)}"
                    );

                    return Responses.BadRequest<RegistracionModelOutput>(
                        string.Format(MessageConstants.NoSeEncontroPersonaFisica, registracionModel.Body.PersonId)
                    );
                }

                var inconsistencias = VerificarInconsistencias(registracionModel, result);

                if (inconsistencias.Length > 0)
                {
                    await _auditoriaV2Repository.SaveAuditLogAsync(
                        0,
                        EventTypes.Registration,
                        EventResults.Error,
                        registracionModel.XCanal,
                        dateTimeDbServerV2,
                        $"{docNumber} {inconsistencias}"
                    );

                    return Responses.BadRequest<RegistracionModelOutput>(inconsistencias.ToString());
                }

                if (registacionNuevoModeloHabilitada)
                {
                    IResponse<RegistracionModelOutput> response;
                    (usuarioNuevo, response) = await RegistracionNuevoModeloAsync(
                        registracionModel,
                        dateTimeDbServerV2,
                        docNumber,
                        usuarioNuevo
                    );

                    if (response != null)
                        return response;
                }
                else
                {
                    var response = await ValidarUsuarioLegacyAsync(
                        registracionModel,
                        dateTimeDbServerV2,
                        docNumber
                    );

                    if (response != null)
                        return response;
                }

                var usuarioHBI = await CrearUsuarioLegacyAsync(
                    registracionModel,
                    result,
                    registacionNuevoModeloHabilitada
                );

                var dateTime = await _helperDbServer.ObtenerFechaAsync();
                var userId = registacionNuevoModeloHabilitada ? usuarioNuevo.GetUserId() : 0;

                await _auditoriaRepository.SaveRegistrationAuditAsync(userId, dateTime.Now);

                await _auditoriaV2Repository.SaveAuditLogAsync(
                    userId,
                    EventTypes.Registration,
                    EventResults.Ok,
                    registracionModel.XCanal,
                    dateTimeDbServerV2,
                    JsonSerializer.Serialize(
                        new { userIdHBI = usuarioHBI.UserId, DocumentNumber = usuarioHBI.DocumentNumber })
                );

                return Responses.Created(new RegistracionModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingRegistro, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<MigracionModelOutput>> MigrarAsync(IRequestBody<MigracionModelInput> migracionModel)
        {
            try
            {
                var idPersona = $"IdPersona: {migracionModel.Body.IdPersona}";
                var dateTimeDbServerV2 = await _helperDbServerV2.ObtenerFechaAsync();

                var usuarioMigracion =
                    await _usuarioRepository.ObtenerUsuarioByPersonIdAsync(migracionModel.Body.IdPersona);

                if (usuarioMigracion == null)
                {
                    return Responses.NotFound<MigracionModelOutput>(ErrorCode.UsuarioInexistente.ErrorDescription);
                }

                var validaciones = ValidarUsuarioMigracion(migracionModel);
                if (validaciones.Length > 0)
                {
                    await _auditoriaV2Repository.SaveAuditLogAsync(0, EventTypes.Registration, EventResults.Error,
                        migracionModel.XCanal, dateTimeDbServerV2,
                        $"{idPersona} {validaciones}");

                    return Responses.BadRequest<MigracionModelOutput>(validaciones.ToString());
                }

                var usuario = await _usuarioV2Repository.ObtenerUsuarioByPersonIdAsync(migracionModel.Body.IdPersona);

                if (usuario != null) return Responses.Conflict<MigracionModelOutput>(MessageConstants.UsuarioYaExiste);

                var dateTime = await _helperDbServerV2.ObtenerFechaAsync();
                var dateTimeHbi = await _helperDbServer.ObtenerFechaAsync();

                int.TryParse(usuarioMigracion.DocumentCountryId, out int documentCountryId);

                var usuarioNuevo = new UsuarioV2
                {
                    PersonId = migracionModel.Body.IdPersona,
                    DocumentCountryId = documentCountryId,
                    DocumentTypeId = usuarioMigracion.DocumentTypeId ??
                                     throw new ArgumentException("DocumentTypeId no puede ser nulo."),
                    DocumentNumber = usuarioMigracion.DocumentNumber,
                    Username = _encryption.GetHash(migracionModel.Body.UserName),
                    Password = _encryption.GetHash(migracionModel.Body.Password),
                    UserStatusId = (byte)UserStatus.Active,
                    LastPasswordChange = dateTime.Now,
                    LastLogon = usuarioMigracion.LastLogon,
                    CreatedDate = usuarioMigracion.CreatedDate
                };

                await _usuarioV2Repository.AddAsync(usuarioNuevo);
                await _usuarioV2Repository.SaveChangesAsync();

                usuarioMigracion.UserName = usuarioMigracion.UserData.PersonId;
                usuarioMigracion.LastPasswordChange = dateTimeHbi.Now;
                await SaveUsuario(usuarioMigracion, false);

                await _auditoriaV2Repository.SaveAuditLogAsync(
                    usuarioNuevo.GetUserId(),
                    EventTypes.Migration,
                    EventResults.Ok,
                    migracionModel.XCanal,
                    dateTime
                );

                return Responses.Created(new MigracionModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingRegistro, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<ValidacionExistenciaModelOutput>> ValidarExistenciaAsync(
            IRequestBody<ValidacionExistenciaModelInput> validacionExistenciaModel
        )
        {
            try
            {
                IUsuario usuario = await _usuarioV2Repository
                    .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        validacionExistenciaModel.Body.DocumentCountryId,
                        validacionExistenciaModel.Body.DocumentTypeId,
                        validacionExistenciaModel.Body.DocumentNumber
                    );

                var existenciaModelOutput = new ValidacionExistenciaModelOutput();

                if (usuario == null)
                {
                    usuario = await _usuarioRepository
                        .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                            GetCountryByBanTotal(validacionExistenciaModel.Body.DocumentCountryId),
                            validacionExistenciaModel.Body.DocumentTypeId,
                            validacionExistenciaModel.Body.DocumentNumber
                        );

                    if (usuario == null)
                    {
                        return Responses.NotFound<ValidacionExistenciaModelOutput>(
                            ErrorCode.UsuarioInexistente.ErrorDescription
                        );
                    }

                    existenciaModelOutput.Migrated = false;
                    existenciaModelOutput.Username = usuario.GetUserName();
                }
                else
                {
                    existenciaModelOutput.Migrated = true;
                }

                existenciaModelOutput.PersonId = usuario.GetPersonId();

                if (existenciaModelOutput.PersonId == null || existenciaModelOutput.PersonId < 1)
                {
                    var personIdModelInput = _mapper.Map<ActualizarPersonIdModelInput>(validacionExistenciaModel.Body);

                    try
                    {
                        var personIdString = await ActualizarIdPersonaAsync(personIdModelInput);
                        long.TryParse(personIdString, out var personId);
                        existenciaModelOutput.PersonId = personId;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                existenciaModelOutput.UserStatusId = usuario.GetUserStatusId();

                return Responses.Ok(existenciaModelOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingValidacionExistencia, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<ValidacionExistenciaHbiModelOutput>> ValidarExistenciaHbiAsync(
            IRequestBody<ValidacionExistenciaHbiModelInput> validacionExistenciaHbiModel
        )
        {
            try
            {
                IUsuario usuario = await _usuarioRepository
                    .ObtenerUsuarioAsync(validacionExistenciaHbiModel.Body.UserName);

                var existenciaHbiModelOutput = new ValidacionExistenciaHbiModelOutput();

                if (usuario != null)
                {
                    existenciaHbiModelOutput.ExisteUsuario = true;
                }
                else
                {
                    existenciaHbiModelOutput.ExisteUsuario = false;
                }

                return Responses.Ok(existenciaHbiModelOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingValidacionExistenciaHbi, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<ValidacionExistenciaBtaModelOutput>> ValidarExistenciaBtaAsync(
            IRequestBody<ValidacionExistenciaBtaModelInput> validacionExistenciaBtaModel
        )
        {
            try
            {
                IUsuario usuario = await _usuarioV2Repository
                    .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        validacionExistenciaBtaModel.Body.DocumentCountryId,
                        validacionExistenciaBtaModel.Body.DocumentTypeId,
                        validacionExistenciaBtaModel.Body.DocumentNumber);

                var existenciaBtaModelOutput = new ValidacionExistenciaBtaModelOutput();

                if (usuario == null)
                {
                    usuario = await _usuarioRepository
                        .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                            GetCountryByBanTotal(validacionExistenciaBtaModel.Body.DocumentCountryId),
                            validacionExistenciaBtaModel.Body.DocumentTypeId,
                            validacionExistenciaBtaModel.Body.DocumentNumber);

                    existenciaBtaModelOutput.Registrado = BtaConstants.HBI;

                    if (usuario == null)
                    {
                        existenciaBtaModelOutput.Registrado = BtaConstants.NO;
                    }
                }
                else
                {
                    existenciaBtaModelOutput.Registrado = BtaConstants.BUU;
                }

                var personaBytes = await _distributedCache.GetAsync(
                    DistributedCache.Bta.ObtenerTokenBta("tokenBta")
                );

                var tokenbta = DistributedCache.Deserialize<TokenBtaModelOutput>(personaBytes);

                if (tokenbta == null)
                {
                    tokenbta = await _btaRepository.ObtenerToken();

                    if (tokenbta != null)
                    {
                        await _distributedCache.SetAsync(
                            DistributedCache.Bta.ObtenerTokenBta("tokenBta"),
                            DistributedCache.Serialize(tokenbta),
                            DistributedCache.SlidingExpirationHours(4)
                        );
                    }
                }

                ObtenerPinModelOutput resultBta = new ObtenerPinModelOutput();
                if (tokenbta != null)
                {
                    resultBta = await _btaRepository.ObtenerPinAsync(validacionExistenciaBtaModel.Body.DocumentNumber,
                                                                      validacionExistenciaBtaModel.Body.DocumentTypeId,
                                                                      validacionExistenciaBtaModel.Body.DocumentCountryId,
                                                                      tokenbta.SessionToken);
                }

                existenciaBtaModelOutput.ClaveBt = !string.IsNullOrWhiteSpace(resultBta.DatosPIN?.pin);

                existenciaBtaModelOutput.PersonId = usuario?.GetPersonId();
                return Responses.Ok(existenciaBtaModelOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingValidacionExistencia, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<CambioDeClaveModelOutput>> ModificarClaveAsync(
            IRequestBody<CambioDeClaveModelInputV2> cambioDeClaveModelV2
        )
        {
            try
            {
                var result = await GetUsuarioFromDbAsync(cambioDeClaveModelV2.Body.PersonId);
                var fechaDbServerV2 = await _helperDbServerV2.ObtenerFechaAsync();
                var usuario = result.Usuario;
                var gateway = cambioDeClaveModelV2.XGateway;

                if (string.IsNullOrWhiteSpace(gateway))
                    gateway = cambioDeClaveModelV2.XCanal;

                if (result.Usuario == null)
                {
                    var usuarioInexistente = new
                    {
                        IdPersona = cambioDeClaveModelV2.Body.PersonId,
                        Mensaje = ErrorCode.UsuarioInexistente,
                        Canal = gateway
                    };

                    await _auditoriaV2Repository.SaveAuditLogAsync(
                        0,
                        EventTypes.PasswordChange,
                        EventResults.Error,
                        gateway,
                        fechaDbServerV2,
                        JsonSerializer.Serialize(usuarioInexistente)
                    );

                    _logger.LogError(
                        UsuarioServiceEvents.ExceptionCallingCambioDeClaveV2, "{UsuarioInexistente}", usuarioInexistente
                    );

                    return Responses.NotFound<CambioDeClaveModelOutput>(ErrorCode.UsuarioInexistente.ErrorDescription);
                }

                if (
                    !result.FromNewVersionDb &&
                    AppConstants.CanalHabilitadoParaCambioDeClaveSoloModeloNuevo.Equals(cambioDeClaveModelV2.XCanal)
                )
                {
                    var operacionNoPermitida = new
                    {
                        IdPersona = usuario.GetPersonId(),
                        Mensaje = ErrorCode.OperacionNoHabilitada,
                        Canal = gateway
                    };

                    await _auditoriaV2Repository.SaveAuditLogAsync(
                        0,
                        EventTypes.PasswordChange,
                        EventResults.Error,
                        gateway,
                        fechaDbServerV2,
                        JsonSerializer.Serialize(operacionNoPermitida)
                    );

                    _logger.LogError(
                        UsuarioServiceEvents.ExceptionCallingCambioDeClaveV2,
                        "{OperacionNoPermitida}",
                        operacionNoPermitida
                    );

                    return Responses.Unauthorized<CambioDeClaveModelOutput>(
                        ErrorCode.OperacionNoHabilitada.Code,
                        ErrorCode.OperacionNoHabilitada.ErrorDescription
                    );
                }

                await SetupCantidadHistorialDeCambiosDeClaveAsync();

                var claveActualInconsistencias = ValidarClaveActual(cambioDeClaveModelV2.Body.CurrentPasword, result);
                var nuevaClaveEncriptada = _encryption.GetHash(cambioDeClaveModelV2.Body.NewPassword);

                var response = await ValidacionClavesAsync(
                    cambioDeClaveModelV2,
                    claveActualInconsistencias,
                    usuario,
                    gateway,
                    result,
                    fechaDbServerV2,
                    nuevaClaveEncriptada
                );

                if (response != null)
                    return response;

                // Actualizamos los datos del usuario
                usuario.SetPassword(nuevaClaveEncriptada);
                usuario.SetUserStatusId((byte)UserStatus.Active);
                usuario.SetLoginAttempts(0);

                await _auditoriaV2Repository.SaveChangesAsync();
                await SaveUsuario(usuario, result.FromNewVersionDb);
                return Responses.Ok(new CambioDeClaveModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingCambioDeClaveV2, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<CambioDeCredencialesModelOutput>> ModificarCredencialesAsync(
            IRequestBody<CambioDeCredencialesModelInputV2> cambioDeCredencialesModelV2
        )
        {
            try
            {
                var gateway = cambioDeCredencialesModelV2.XGateway;
                var fechaDbServerV2 = await _helperDbServerV2.ObtenerFechaAsync();

                if (string.IsNullOrWhiteSpace(gateway))
                    gateway = cambioDeCredencialesModelV2.XCanal;

                var usuario = await _usuarioV2Repository.ObtenerUsuarioByPersonIdAsync(
                    cambioDeCredencialesModelV2.Body.PersonId
                );

                if (usuario == null)
                {
                    var usuarioInexistente = new
                    {
                        IdPersona = (long?)cambioDeCredencialesModelV2.Body.PersonId,
                        Mensaje = ErrorCode.UsuarioInexistente,
                        Canal = gateway
                    };

                    await _auditoriaV2Repository.SaveAuditLogAsync(
                        0,
                        EventTypes.CredentialsChange,
                        EventResults.Error,
                        gateway,
                        fechaDbServerV2,
                        JsonSerializer.Serialize(usuarioInexistente)
                    );

                    return Responses.NotFound<CambioDeCredencialesModelOutput>(
                        ErrorCode.UsuarioInexistente.ErrorDescription
                    );
                }

                await SetupCantidadHistorialDeCambiosDeClaveAsync();

                var nuevoUsuarioEncriptado = _encryption.GetHash(cambioDeCredencialesModelV2.Body.NewUsername);
                var nuevaClaveEncriptada = _encryption.GetHash(cambioDeCredencialesModelV2.Body.NewPassword);
                var usernameYaUtilizado = await UsernameYaUtilizado(usuario, nuevoUsuarioEncriptado);
                var claveYaUtilizada = await ClaveYaUtilizada(usuario, nuevaClaveEncriptada);

                switch (usernameYaUtilizado)
                {
                    case true when claveYaUtilizada:
                        // Credenciales no válidas, ya utilizadas.
                        var claveYaUtilizadaError = new
                        {
                            IdPersona = usuario.GetPersonId(),
                            Mensaje = ErrorCode.CredencialesYaUtilizadas,
                            Canal = gateway
                        };

                        await _auditoriaV2Repository.SaveAuditLogAsync(
                            usuario.GetUserId(),
                            EventTypes.CredentialsChange,
                            EventResults.Error,
                            gateway,
                            fechaDbServerV2,
                            JsonSerializer.Serialize(claveYaUtilizadaError)
                        );

                        return Responses.Conflict<CambioDeCredencialesModelOutput>(
                            ErrorCode.CredencialesYaUtilizadas.Code,
                            ErrorCode.CredencialesYaUtilizadas.ErrorDescription);
                    case true:
                        // Username no válido, ya utilizado.
                        var usuarioYaUtilizadoError = new
                        {
                            IdPersona = usuario.GetPersonId(),
                            Mensaje = ErrorCode.UsuarioYaUtilizado,
                            Canal = gateway
                        };

                        await _auditoriaV2Repository.SaveAuditLogAsync(
                            usuario.GetUserId(),
                            EventTypes.CredentialsChange,
                            EventResults.Error,
                            gateway,
                            fechaDbServerV2,
                            JsonSerializer.Serialize(usuarioYaUtilizadoError)
                        );

                        return Responses.Conflict<CambioDeCredencialesModelOutput>(
                            ErrorCode.UsuarioYaUtilizado.Code,
                            ErrorCode.UsuarioYaUtilizado.ErrorDescription
                        );
                }

                if (claveYaUtilizada)
                {
                    // Clave no válida, ya utilizada.
                    var claveYaUtilizadaError = new
                    {
                        IdPersona = usuario.GetPersonId(),
                        Mensaje = ErrorCode.ClaveYaUtilizada,
                        Canal = gateway
                    };

                    await _auditoriaV2Repository.SaveAuditLogAsync(
                        usuario.GetUserId(),
                        EventTypes.CredentialsChange,
                        EventResults.Error,
                        gateway,
                        fechaDbServerV2,
                        JsonSerializer.Serialize(claveYaUtilizadaError)
                    );

                    return Responses.Conflict<CambioDeCredencialesModelOutput>(
                        ErrorCode.ClaveYaUtilizada.Code,
                        ErrorCode.ClaveYaUtilizada.ErrorDescription
                    );
                }

                // Credenciales válidas, grabamos audit log del cambio de credenciales
                var auditLog = await _auditoriaV2Repository.SaveAuditLogAsync(
                    usuario.GetUserId(),
                    EventTypes.CredentialsChange,
                    EventResults.Ok,
                    gateway,
                    fechaDbServerV2,
                    "{IdPersona: " + cambioDeCredencialesModelV2.Body.PersonId + "}"
                );

                // Guardamos username actual en el histórico
                await _historialUsuarioUsuariosV2Repository.AddAsync(
                    new HistorialUsuarioUsuariosV2
                    {
                        UserId = usuario.GetUserId(),
                        AuditLogId = auditLog.AuditLogId,
                        Username = usuario.Username,
                        CreationDate = fechaDbServerV2.Now
                    });

                // Guardamos clave actual en el histórico
                await _historialClaveUsuariosV2Repository.AddAsync(
                    new HistorialClaveUsuariosV2
                    {
                        UserId = usuario.GetUserId(),
                        AuditLogId = auditLog.AuditLogId,
                        Password = usuario.Password,
                        CreationDate = fechaDbServerV2.Now
                    }
                );

                // Guardamos el cambio de usuario y clave
                usuario.Username = nuevoUsuarioEncriptado;
                usuario.Password = nuevaClaveEncriptada;
                usuario.UserStatusId = (byte)UserStatus.Active;
                usuario.LoginAttempts = 0;
                usuario.LastPasswordChange = fechaDbServerV2.Now;
                _usuarioV2Repository.Update(usuario);

                // Hacemos SaveChanges de todos los repositorios a la vez para asegurarnos que se completó el proceso sin errores
                await _auditoriaV2Repository.SaveChangesAsync();
                await _historialUsuarioUsuariosV2Repository.SaveChangesAsync();
                await _historialClaveUsuariosV2Repository.SaveChangesAsync();
                await _usuarioV2Repository.SaveChangesAsync();

                var usuarioHbi = await _usuarioRepository.ObtenerUsuarioByPersonIdAsync(
                    cambioDeCredencialesModelV2.Body.PersonId
                );

                // Actualizamos LastPasswordChange del Legacy mientras siga el acoplamiento con HBI (CDIG-1903)
                if (usuarioHbi != null)
                {
                    var fechaDbServerHbi = await _helperDbServer.ObtenerFechaAsync();

                    usuarioHbi.SetLastPasswordChange(fechaDbServerHbi.Now);

                    await _usuarioRepository.SaveChangesAsync();
                }

                return Responses.Ok(new CambioDeCredencialesModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingCambioDeCredencialesV2, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<PerfilMigradoModelOutput[]>> ObtenerPerfilesAsync(
            IRequestBody<PerfilMigradoModelInput> perfilMigradoModel
        )
        {
            try
            {
                var usuariosMigrados = await _usuarioV2Repository
                    .ObtenerUsuariosByCountryIdDocumentTypeIdDocumentNumberAsync(
                        perfilMigradoModel.Body.DocumentCountryId,
                        perfilMigradoModel.Body.DocumentTypeId,
                        perfilMigradoModel.Body.DocumentNumber
                    ) ?? Array.Empty<UsuarioV2>();

                return usuariosMigrados.Any()
                    ? Responses.Ok(_mapper.Map<PerfilMigradoModelOutput[]>(usuariosMigrados))
                    : Responses.NotFound<PerfilMigradoModelOutput[]>(ErrorCode.UsuarioInexistente.ErrorDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingValidacionExistencia, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<object>> CambiarEstadoAsync(IRequestBody<CambioEstadoModelInput> cambioEstadoModel)
        {
            try
            {
                if (cambioEstadoModel.Body.PersonId < 1 || (byte)cambioEstadoModel.Body.EstadoId < 1)
                    return Responses.BadRequest<object>(ErrorCode.UsuarioInexistente.ErrorDescription);

                var user = await _usuarioV2Repository.FindAsync(
                    f => f.PersonId == cambioEstadoModel.Body.PersonId,
                    false
                );

                if (user == null)
                    return Responses.NotFound<object>(ErrorCode.UsuarioInexistente.ErrorDescription);

                if (user.UserStatusId == (byte)cambioEstadoModel.Body.EstadoId)
                    return Responses.Conflict<object>(ErrorCode.EstatusYaAsignado.ErrorDescription);

                var result = await _usuarioV2Repository.CambiarEstadoAsync(
                    cambioEstadoModel.Body.PersonId,
                    cambioEstadoModel.Body.EstadoId
                );

                if (!result)
                    return Responses.NotFound<object>(ErrorCode.UsuarioInexistente.ErrorDescription);

                var fechaDbServer = await _helperDbServerV2.ObtenerFechaAsync();

                await _auditoriaV2Repository.SaveAuditLogAsync(
                    user.UserId,
                    EventTypes.ChangeUserStatus,
                    EventResults.Ok,
                    cambioEstadoModel.XCanal,
                    fechaDbServer,
                    $"{{\"IdPersona\":{user.PersonId},\"EstadoIdAnterior\":{((UserStatus)user.UserStatusId)},\"EstadoIdActual\":{cambioEstadoModel.Body.EstadoId}}}"
                );

                return Responses.Ok(new object());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    UsuarioServiceEvents.ExceptionCambiandoEstatus,
                    ex.Message,
                    cambioEstadoModel.Body,
                    ex
                );

                throw;
            }
        }

        public async Task<IResponse<ActualizarPersonIdModelOutput>> ActualizarPersonIdAsync(
            IRequestBody<ActualizarPersonIdModelInput> actualizarPersonIdModel
        ) => await ActualizarPersonIdAsync(actualizarPersonIdModel.Body);

        public async Task<IResponse<PerfilModelOutput>> ObtenerUsuarioAsync(
            IRequestBody<ObtenerUsuarioModelInput> obtenerUsuarioModel
        )
        {
            var body = obtenerUsuarioModel.Body;
            var fromNewVersionDb = true;
            var username = body.Usuario;

            async Task<string> GetUserEncodedAsync()
            {
                if (!obtenerUsuarioModel.RequestFromKeycloak())
                    return null;

                var userEncode = UserEncoding.Base64Encode(
                    body.NumeroDocumento,
                    body.Usuario,
                    obtenerUsuarioModel.XCanal,
                    true
                );

                var userDecode = await _distributedCache.GetAsync(DistributedCache.Sso.Autenticacion(userEncode));

                return userDecode != null ? DistributedCache.Deserialize(userDecode) : null;
            }

            try
            {
                var userEncoded = await GetUserEncodedAsync();

                if (!string.IsNullOrWhiteSpace(userEncoded))
                {
                    var (_, userName, _) = UserEncoding.Base64Decode(userEncoded);
                    username = userName;
                }

                IUsuario usuario = await _usuarioV2Repository.ObtenerUsuarioAsync(
                    _encryption.GetHash(username),
                    body.NumeroDocumento
                );

                IUsuario usuarioV1 = await _usuarioRepository.ObtenerUsuarioAsync(username, body.NumeroDocumento);

                if (usuario == null)
                {
                    if (usuarioV1 == null)
                    {
                        await _distributedCache.SetAsync(
                            DistributedCache.Usuario.ObtenerUsuario(userEncoded),
                            DistributedCache.Serialize(ErrorCode.UsuarioIncorrecto),
                            DistributedCache.SlidingExpirationMinutes(1)
                        );

                        return Responses.NotFound<PerfilModelOutput>(ErrorCode.UsuarioInexistente.ErrorDescription);
                    }

                    usuario = usuarioV1;
                    fromNewVersionDb = false;
                }

                var perfil = await ObtenerUsuarioAsync(
                    usuario,
                    usuarioV1 ?? usuario,
                    fromNewVersionDb,
                    obtenerUsuarioModel.XCanal
                );

                perfil.Username = username;

                return Responses.Ok(perfil);
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingValidacionExistencia, ex.Message, ex);
                throw;
            }
        }

        private async Task<IResponse<ActualizarPersonIdModelOutput>> ActualizarPersonIdAsync(
            ActualizarPersonIdModelInput actualizarPersonIdModel
        )
        {
            var personId = await ActualizarIdPersonaAsync(actualizarPersonIdModel);

            if (string.IsNullOrWhiteSpace(personId))
                return Responses.NotFound<ActualizarPersonIdModelOutput>(ErrorCode.UsuarioInexistente.ErrorDescription);

            return Responses.Ok(
                new ActualizarPersonIdModelOutput
                {
                    PersonId = personId
                }
            );
        }

        private async Task<string> ActualizarIdPersonaAsync(ActualizarPersonIdModelInput actualizarPersonIdModel)
        {
            try
            {
                var usuario = await _usuarioRepository.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                    GetCountryByBanTotal(actualizarPersonIdModel.DocumentCountryId),
                    actualizarPersonIdModel.DocumentTypeId,
                    actualizarPersonIdModel.DocumentNumber
                );

                string personId = null;

                if (usuario != null)
                {
                    personId = await CompletarPersonIdUsuarioAsync(usuario);
                }

                return personId;
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingActualizarPersonId, ex.Message, ex);
                throw;
            }
        }

        private async Task<PerfilModelOutput> ObtenerPerfilAsync(long idPersona)
        {
            try
            {
                IUsuario usuario = await _usuarioV2Repository.ObtenerUsuarioByPersonIdAsync(idPersona);
                IUsuario usuarioV1 = await _usuarioRepository.ObtenerUsuarioByPersonIdAsync(idPersona);

                if (usuario != null)
                    return await ObtenerPerfilAsync(usuario, usuarioV1, true);

                if (usuarioV1 == null)
                    return null;

                usuario = usuarioV1;

                return await ObtenerPerfilAsync(usuario, usuarioV1, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingPerfil, ex.Message, ex);
                throw;
            }
        }

        private async Task<PerfilModelOutput> ObtenerPerfilAsync(
            IUsuario usuario,
            IUsuario usuarioV1,
            bool migrated
        )
        {
            try
            {
                int documentType;
                string firstName;
                string lastName;
                int country;

                var personId = await VerificarPersonId(usuario);
                var result = await _personasRepository.ObtenerInfoPersonaFisica(personId);

                if (result == null)
                {
                    throw new BusinessException(
                        string.Format(MessageConstants.NoSeEncontroPersonaFisica, personId),
                        0
                    );
                }

                var lastLogon = usuario.GetLastLogon();
                var userId = usuario.GetUserId();
                var documentNumber = usuario.GetDocumentNumber();
                var gender = result.genero;
                var lastPasswordChange = usuario.GetLastPasswordChange();
                var userStatusId = usuario.GetUserStatusId();
                var email = usuarioV1.GetDatosUsuario()?.Mail ?? string.Empty;
                var isEmployee = usuarioV1.GetIsEmployee();

                var passwordExpiryDate = await GetPasswordExpiryDateAsync(
                    lastPasswordChange ?? usuario.GetCreatedDate(),
                    migrated
                );

                if (migrated)
                {
                    firstName = result.nombre;
                    lastName = result.apellido;
                    documentType = result.tipo_documento;
                    country = result.pais_documento;
                }
                else
                {
                    firstName = usuarioV1.GetName();
                    lastName = usuarioV1.GetLastName();
                    documentType = usuarioV1.GetDocumentType();
                    country = usuarioV1.GetDocumentCountryId();
                }

                return new PerfilModelOutput
                {
                    LastLogon = lastLogon,
                    PersonId = personId,
                    UserId = userId,
                    DocumentNumber = documentNumber,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    DocumentType = documentType,
                    Gender = gender,
                    Country = country,
                    LastPasswordChange = lastPasswordChange,
                    PasswordExpiryDate = passwordExpiryDate,
                    IsEmployee = isEmployee,
                    UserStatusId = userStatusId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingPerfil, ex.Message, ex);
                throw;
            }
        }

        private async Task<PerfilModelOutput> ObtenerUsuarioAsync(
            IUsuario usuario,
            IUsuario usuarioV1,
            bool migrated,
            string canal
        )
        {
            try
            {
                var firstName = string.Empty;
                var lastName = string.Empty;
                var personId = await VerificarPersonId(usuario);
                var documentNumber = usuario.GetDocumentNumber();
                var username = usuarioV1.GetUserName();
                var email = usuarioV1.GetDatosUsuario()?.Mail ?? string.Empty;

                if (string.IsNullOrWhiteSpace(username))
                    username = usuario.GetUserName();

                if (migrated)
                {
                    var personaBytes = await _distributedCache.GetAsync(
                        DistributedCache.Usuario.ObtenerInfoPersonaFisica(personId.ToString())
                    );

                    var personaFisica = DistributedCache.Deserialize<PersonaFisicaInfoModelResponse>(personaBytes);

                    if (personaFisica == null)
                    {
                        personaFisica = await _personasRepository.ObtenerInfoPersonaFisica(personId);

                        if (personaFisica != null)
                        {
                            await _distributedCache.SetAsync(
                                DistributedCache.Usuario.ObtenerInfoPersonaFisica(personId.ToString()),
                                DistributedCache.Serialize(personaFisica),
                                DistributedCache.SlidingExpirationDays(30)
                            );

                            firstName = personaFisica.nombre;
                            lastName = personaFisica.apellido;
                        }
                    }
                }
                else
                {
                    firstName = usuarioV1.GetName();
                    lastName = usuarioV1.GetLastName();
                }

                return new PerfilModelOutput
                {
                    PersonId = personId,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Migrated = migrated,
                    //Props to generate Identifier
                    DocumentNumber = documentNumber,
                    Username = username,
                    Canal = canal
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UsuarioServiceEvents.CallingObtenerUsuarioMessage);
                throw;
            }
        }

        private async Task<long> VerificarPersonId(IUsuario usuario)
        {
            var personId = usuario.GetPersonId() ?? 0;

            if (personId >= 1)
                return personId;

            var personIdModelInput = new ActualizarPersonIdModelInput
            {
                DocumentNumber = usuario.GetDocumentNumber(),
                DocumentCountryId = usuario.GetDocumentCountryId(),
                DocumentTypeId = usuario.GetDocumentType()
            };

            try
            {
                var personIdString = await ActualizarIdPersonaAsync(personIdModelInput);
                long.TryParse(personIdString, out var personIdResult);
                return personIdResult;
            }
            catch (Exception)
            {
                // ignored
            }

            return personId;
        }

        private static bool ValidarUsuario(UserFromDbResult result) =>
            result.Usuario == null || result.Usuario.GetUserStatusId() != (int)UserStatus.Active;

        private async Task<IResponse<RegistracionModelOutput>> ValidarUsuarioLegacyAsync(
            IRequestBody<RegistracionModelInputV2> registracionModel,
            FechaDbServerV2 dateTimeDbServerV2,
            string docNumber
        )
        {
            var usuario = await _usuarioRepository.ObtenerUsuarioAsync(registracionModel.Body.UserName);

            if (usuario != null)
            {
                await _auditoriaV2Repository.SaveAuditLogAsync(
                    0,
                    EventTypes.Registration,
                    EventResults.Error,
                    registracionModel.XCanal,
                    dateTimeDbServerV2,
                    $"{docNumber} {MessageConstants.UsuarioYaUtilizado}"
                );

                return Responses.Conflict<RegistracionModelOutput>(
                    ErrorConstants.CodigoUsuarioYaUtilizado,
                    MessageConstants.UsuarioYaUtilizado
                );
            }

            usuario = await _usuarioRepository.ObtenerUsuarioAsync(
                registracionModel.Body.DocumentTypeId,
                registracionModel.Body.DocumentNumber
            );

            if (usuario == null)
                return null;

            await _auditoriaV2Repository.SaveAuditLogAsync(
                0,
                EventTypes.Registration,
                EventResults.Error,
                registracionModel.XCanal,
                dateTimeDbServerV2,
                $"{docNumber} {MessageConstants.UsuarioYaExiste}"
            );

            return Responses.Conflict<RegistracionModelOutput>(
                ErrorConstants.CodigoUsuarioYaExiste,
                MessageConstants.UsuarioYaExiste
            );
        }

        private async Task<(UsuarioV2 usuarioNuevo, IResponse<RegistracionModelOutput> response)>
            RegistracionNuevoModeloAsync(
                IRequestBody<RegistracionModelInputV2> registracionModel,
                FechaDbServerV2 dateTimeDbServerV2,
                string docNumber, UsuarioV2 usuarioNuevo
            )
        {
            var usuarioV2 = await _usuarioV2Repository.ObtenerUsuarioByPersonIdAsync(
                registracionModel.Body.PersonId
            );

            if (usuarioV2 == null)
            {
                var usuarioV1 = await _usuarioRepository
                    .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        GetCountryByBanTotal(registracionModel.Body.DocumentCountryId),
                        registracionModel.Body.DocumentTypeId,
                        registracionModel.Body.DocumentNumber
                    );

                if (usuarioV1 != null)
                {
                    await _auditoriaV2Repository.SaveAuditLogAsync(
                        0,
                        EventTypes.Registration,
                        EventResults.Error,
                        registracionModel.XCanal,
                        dateTimeDbServerV2,
                        $"{docNumber} {MessageConstants.UsuarioYaExiste}"
                    );

                    return (usuarioNuevo, Responses.Conflict<RegistracionModelOutput>(
                        ErrorConstants.CodigoUsuarioYaExiste,
                        MessageConstants.UsuarioYaExiste
                    ));
                }
            }
            else
            {
                await _auditoriaV2Repository.SaveAuditLogAsync(
                    0,
                    EventTypes.Registration,
                    EventResults.Error,
                    registracionModel.XCanal,
                    dateTimeDbServerV2,
                    $"{docNumber} {MessageConstants.UsuarioYaExiste}"
                );

                return (usuarioNuevo, Responses.Conflict<RegistracionModelOutput>(
                    ErrorConstants.CodigoUsuarioYaExiste,
                    MessageConstants.UsuarioYaExiste
                ));
            }

            usuarioNuevo = new UsuarioV2
            {
                PersonId = registracionModel.Body.PersonId,
                DocumentCountryId = registracionModel.Body.DocumentCountryId,
                DocumentTypeId = registracionModel.Body.DocumentTypeId,
                DocumentNumber = registracionModel.Body.DocumentNumber,
                Username = _encryption.GetHash(registracionModel.Body.UserName),
                Password = _encryption.GetHash(registracionModel.Body.Password),
                UserStatusId = (byte)UserStatus.Active,
                LastPasswordChange = dateTimeDbServerV2.Now,
                CreatedDate = dateTimeDbServerV2.Now
            };

            await _usuarioV2Repository.AddAsync(usuarioNuevo);
            await _usuarioV2Repository.SaveChangesAsync();
            return (usuarioNuevo, null);
        }

        private async Task<Usuario> CrearUsuarioLegacyAsync(
            IRequestBody<RegistracionModelInputV2> model,
            PersonaFisicaInfoModelResponse personaFisica,
            bool registacionNuevoModeloHabilitada
        )
        {
            var dateTime = await _helperDbServer.ObtenerFechaAsync();
            var telefonoDobleFactor = ObtenerTelefonoDobleFactorVerificadoPersona(personaFisica, model.XCanal);
            var usuarioHbi = new Usuario
            {
                CustomerNumber = GetCustomerNumberByBanTotal(
                    model.Body.DocumentCountryId,
                    model.Body.DocumentTypeId,
                    model.Body.DocumentNumber),
                UserName = registacionNuevoModeloHabilitada ? model.Body.PersonId.ToString() : model.Body.UserName,
                Password = registacionNuevoModeloHabilitada
                    ? Guid.NewGuid().ToString()
                    : _encryption.GetHash(model.Body.Password),
                UserStatusId = (byte)UserStatus.Active,
                Name = personaFisica.nombre,
                LastName = personaFisica.apellido,
                LastPasswordChange = dateTime.Now,
                CreatedDate = dateTime.Now,
                DocumentCountryId = GetCountryByBanTotal(model.Body.DocumentCountryId),
                DocumentTypeId = model.Body.DocumentTypeId,
                DocumentNumber = model.Body.DocumentNumber,
                IsEmployee = false,
                MobileEnabled = false,
                FullControl = true,
                LoginAttempts = 0,
                ChannelSource = model.XCanal
            };

            var datosUsuarioHbi = new DatosUsuario
            {
                ReceiveMail = false,
                ReceiveSMS = false,
                BlockCreditSituation = 0,
                Mail = ObtenerEmailPersona(personaFisica),
                ActiveKeySMS = telefonoDobleFactor != null,
                ActiveKeyDate = telefonoDobleFactor != null ? dateTime.Now : (DateTime?)null,
                CellPhoneDate = telefonoDobleFactor != null ? dateTime.Now : (DateTime?)null,
                CellCompanyId = 0,
                CellPhone = telefonoDobleFactor != null ? telefonoDobleFactor.numero : string.Empty,
                CellCompanyIdInformation = 0,
                CellPhoneInformation = telefonoDobleFactor != null ? telefonoDobleFactor.numero : string.Empty,
                PersonId = model.Body.PersonId.ToString()
            };

            usuarioHbi.UserData = datosUsuarioHbi;

            await _usuarioRepository.AddAsync(usuarioHbi);
            await _usuarioRepository.SaveChangesAsync();

            return usuarioHbi;
        }

        private string ValidarClaveActual(string currentPasword, UserFromDbResult result)
        {
            var inconsistencias = string.Empty;

            if (!string.IsNullOrWhiteSpace(currentPasword))
            {
                var claveEncripatada = _encryption.GetHash(currentPasword);

                if (result.Usuario.GetPassword().Equals(claveEncripatada))
                    return inconsistencias;

                inconsistencias = ErrorCode.ClaveActualNoCoincide.ErrorDescription;
                _logger.LogInformation(
                    UsuarioServiceEvents.InformationCallingValidarClaveActual,
                    inconsistencias,
                    result.Usuario
                );
            }
            else
            {
                _logger.LogInformation(
                    UsuarioServiceEvents.InformationCallingValidarClaveActual,
                    InformationCode.ClaveActualVacia.InformationDescription,
                    result.Usuario
                );
            }

            return inconsistencias;
        }

        private async Task<IResponse<CambioDeClaveModelOutput>> ValidacionClavesAsync(
            IRequestBody<CambioDeClaveModelInputV2> cambioDeClaveModelV2,
            string claveActualInconsistencias,
            IUsuario usuario,
            string gateway,
            UserFromDbResult result,
            FechaDbServerV2 fechaDbServerV2,
            string nuevaClaveEncriptada
        )
        {
            if (claveActualInconsistencias.Length > 0)
            {
                var claveActualInconsistenciasError = new
                {
                    IdPersona = usuario.GetPersonId(),
                    Mensaje = claveActualInconsistencias,
                    Canal = gateway
                };

                await _auditoriaV2Repository.SaveAuditLogAsync(
                    result.FromNewVersionDb ? usuario.GetUserId() : 0,
                    EventTypes.PasswordChange,
                    EventResults.Error,
                    gateway,
                    fechaDbServerV2,
                    JsonSerializer.Serialize(claveActualInconsistenciasError)
                );

                _logger.LogError(
                    UsuarioServiceEvents.ExceptionCallingCambioDeClaveV2,
                    "{ClaveActualInconsistenciasError}",
                    claveActualInconsistenciasError
                );

                return Responses.BadRequest<CambioDeClaveModelOutput>(claveActualInconsistencias);
            }

            var nuevaClaveInconsistente = VerificarReglasSegunModelo(
                cambioDeClaveModelV2.Body.NewPassword,
                result.FromNewVersionDb
            );

            if (nuevaClaveInconsistente.Length > 0)
            {
                var nuevaClaveInconsistenteError = new
                {
                    IdPersona = usuario.GetPersonId(),
                    Mensaje = nuevaClaveInconsistente,
                    Canal = gateway
                };

                await _auditoriaV2Repository.SaveAuditLogAsync(
                    result.FromNewVersionDb ? usuario.GetUserId() : 0,
                    EventTypes.PasswordChange,
                    EventResults.Error,
                    gateway,
                    fechaDbServerV2,
                    JsonSerializer.Serialize(nuevaClaveInconsistenteError)
                );

                _logger.LogError(
                    UsuarioServiceEvents.ExceptionCallingCambioDeClaveV2,
                    "{NuevaClaveInconsistenteError}",
                    nuevaClaveInconsistenteError
                );

                return Responses.BadRequest<CambioDeClaveModelOutput>(nuevaClaveInconsistente);
            }

            var claveYaUtilizada = await ClaveYaUtilizadaSegunModeloAsync(
                usuario,
                nuevaClaveEncriptada,
                result.FromNewVersionDb
            );

            if (claveYaUtilizada)
            {
                var claveYaUtilizadaError = new
                {
                    IdPersona = usuario.GetPersonId(),
                    Mensaje = ErrorCode.ClaveYaUtilizada,
                    Canal = gateway
                };

                await _auditoriaV2Repository.SaveAuditLogAsync(
                    result.FromNewVersionDb ? usuario.GetUserId() : 0,
                    EventTypes.PasswordChange,
                    EventResults.Error,
                    gateway,
                    fechaDbServerV2,
                    JsonSerializer.Serialize(claveYaUtilizadaError)
                );

                _logger.LogError(
                    UsuarioServiceEvents.ExceptionCallingCambioDeClaveV2,
                    "{ClaveYaUtilizadaError}",
                    claveYaUtilizadaError
                );

                return Responses.Conflict<CambioDeClaveModelOutput>(
                    ErrorCode.ClaveYaUtilizada.Code,
                    ErrorCode.ClaveYaUtilizada.ErrorDescription
                );
            }

            var fechaDbServer = await _helperDbServer.ObtenerFechaAsync();
            var auditLog = await _auditoriaV2Repository.SaveAuditLogAsync(
                result.FromNewVersionDb ? usuario.GetUserId() : 0,
                EventTypes.PasswordChange,
                EventResults.Ok,
                gateway,
                fechaDbServerV2,
                JsonSerializer.Serialize(new { IdPersona = usuario.GetPersonId() })
            );

            if (result.FromNewVersionDb)
            {
                // Guardamos clave actual en el histórico del NM
                await _historialClaveUsuariosV2Repository.AddAsync(
                    new HistorialClaveUsuariosV2
                    {
                        UserId = usuario.GetUserId(),
                        AuditLogId = auditLog.AuditLogId,
                        Password = usuario.GetPassword(),
                        CreationDate = fechaDbServerV2.Now
                    }
                );

                await _historialClaveUsuariosV2Repository.SaveChangesAsync();
                await ActualizarFechaUltimoCambioDeClaveEnHbiAsync(cambioDeClaveModelV2.Body.PersonId);
                usuario.SetLastPasswordChange(fechaDbServerV2.Now);
            }
            else
            {
                // Guardamos clave actual en el histórico del HBI
                await _historialClaveUsuariosRepository.AddAsync(
                    new HistorialClaveUsuarios
                    {
                        UserId = usuario.GetUserId(),
                        Password = usuario.GetPassword(),
                        CreationDate = fechaDbServer.Now
                    }
                );

                await _historialClaveUsuariosRepository.SaveChangesAsync();
                usuario.SetLastPasswordChange(fechaDbServer.Now);
            }

            return null;
        }

        #region Helpers

        private static string VerificarInconsistencias(
            IRequestBody<RegistracionModelInputV2> registracionModel,
            PersonaFisicaInfoModelResponse result
        )
        {
            var inconsistencias = new StringBuilder();

            if (!long.TryParse(result.numero_documento, out var nroDocApiPersona))
            {
                inconsistencias.Append(
                    string.Format(MessageConstants.NroDocumentoInvalidoDePersonaFisica, result.numero_documento));
            }

            if (!long.TryParse(registracionModel.Body.DocumentNumber, out var nroDoc))
            {
                inconsistencias.Append(
                    string.Format(MessageConstants.NroDocumentoInvalido, registracionModel.Body.DocumentNumber));
            }

            if (nroDocApiPersona != nroDoc)
            {
                inconsistencias.Append(
                    string.Format(MessageConstants.NroDocumentoNoCoincide, nroDoc, nroDocApiPersona));
            }

            if (result.tipo_documento != registracionModel.Body.DocumentTypeId)
            {
                inconsistencias.Append(
                    string.Format(MessageConstants.TipoDocumentoNoCoincide, registracionModel.Body.DocumentTypeId,
                        result.tipo_documento));
            }

            if (result.pais_documento != registracionModel.Body.DocumentCountryId)
            {
                inconsistencias.Append(
                    string.Format(MessageConstants.PaisNoCoincide, registracionModel.Body.DocumentCountryId,
                        result.pais_documento));
            }

            return inconsistencias.ToString();
        }

        private static void ActualizarIntentoFallido(IUsuario usuario, DateTime dateTime)
        {
            if (usuario.GetUserStatusId() != (byte)UserStatus.Active) return;

            usuario.SetLoginAttempts(usuario.GetLoginAttempts() + 1);
            usuario.SetLoginAttemptsDate(dateTime);
        }

        private bool ActualizarSiUsuarioBloqueado(IUsuario usuario, bool fromNewVersionDb)
        {
            var cantidadDeIntentosDeLogin =
                fromNewVersionDb ? _cantidadDeIntentosDeLoginV2 : _cantidadDeIntentosDeLogin;

            if ((usuario.GetLoginAttempts() ?? 0) < cantidadDeIntentosDeLogin) return false;

            usuario.SetUserStatusId((byte)UserStatus.Blocked);

            return true;
        }

        private async Task AnalizarEstadoDeUsuarioAsync(
            IUsuario usuario,
            AutenticacionModelOutput autenticacionModelOutput,
            FechaDbServer dateTime,
            bool fromNewVersionDb
        )
        {
            if (!fromNewVersionDb && usuario.GetPersonId() == null)
            {
                await CompletarPersonIdUsuarioAsync(usuario);
            }

            autenticacionModelOutput.IdPersona = usuario.GetPersonId();

            switch (usuario.GetUserStatusId())
            {
                case (byte)UserStatus.Active:
                    autenticacionModelOutput.Codigo = Codigo.Aceptado;
                    autenticacionModelOutput.Error = null;
                    usuario.SetLoginAttempts(0);
                    usuario.SetLastLogon(dateTime.Now);

                    if (!fromNewVersionDb)
                    {
                        await _auditoriaRepository.SaveLogOnAuditAsync(
                            usuario.GetUserId(),
                            AuditAction.LogOn,
                            AuditActionResult.LoggedOn,
                            dateTime.Now);
                    }

                    break;
                case (byte)UserStatus.Blocked:
                    autenticacionModelOutput.Codigo = Codigo.Bloqueado;
                    autenticacionModelOutput.Error = ErrorCode.UsuarioBloqueado;
                    break;
                case (byte)UserStatus.Inactive:
                    autenticacionModelOutput.Codigo = Codigo.Inactivo;
                    autenticacionModelOutput.Error = ErrorCode.UsuarioInactivo;
                    break;
                case (byte)UserStatus.Suspended:
                    autenticacionModelOutput.Codigo = Codigo.Suspendido;
                    autenticacionModelOutput.Error = ErrorCode.UsuarioSuspendido;
                    break;
                default:
                    autenticacionModelOutput.Codigo = Codigo.EstadoDeUsuarioNoControlado;
                    autenticacionModelOutput.Error = ErrorCode.EstadoDeUsuarioNoControlado;
                    break;
            }
        }

        private static bool ClaveVencidaBta(DateTime? passwordExpirationDate, DateTime now)
        {
            return now > passwordExpirationDate;
        }

        public async Task<bool> ClaveVencidaAsync(DateTime? lastPasswordChange, DateTime now)
        {
            var diasParaForzarCambioClave =
                await _configuracionesV2Service.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync();

            return (now - (lastPasswordChange ?? now)).TotalDays > diasParaForzarCambioClave;
        }

        private async Task<ErrorCode> ValidarResultadoNsbtAsync(
            PinFromNsbt resultNsbt,
            IRequestBody<AutenticacionClaveNumericaModelInput> autenticacionClaveNumericaModel
        )
        {
            if (resultNsbt != null && resultNsbt.Attempt >= AppConstants.IntentosLoginFallidosNsbtDefault)
            {
                return ErrorCode.UsuarioBloqueado;
            }

            if (ValidarPasswordPin(autenticacionClaveNumericaModel.Body.Password, resultNsbt?.Pin)) return null;

            if (resultNsbt == null || string.IsNullOrEmpty(resultNsbt.Pin)) return ErrorCode.UsuarioIncorrecto;

            var actualAttempt = resultNsbt.Attempt + 1;

            await _nsbtRepository.IncrementLoginAttemptsAsync(
                autenticacionClaveNumericaModel.Body.DocumentCountryId.ToString(),
                autenticacionClaveNumericaModel.Body.DocumentTypeId,
                autenticacionClaveNumericaModel.Body.DocumentNumber,
                resultNsbt.Pin,
                actualAttempt,
                resultNsbt.LastLogIn?.Replace("-", string.Empty));

            return actualAttempt >= AppConstants.IntentosLoginFallidosNsbtDefault
                ? ErrorCode.SeHaBloqueadoElUsuario
                : ErrorCode.UsuarioIncorrecto;
        }

        private async Task<int?> GetIdPersonaAsync(
            PinFromNsbt resultNsbt,
            IRequestBody<AutenticacionClaveNumericaModelInput> autenticacionClaveNumericaModel
        )
        {
            if (resultNsbt == null || string.IsNullOrEmpty(resultNsbt.Pin)) return null;

            var persona = await GetIdPersona(
                autenticacionClaveNumericaModel.Body.DocumentNumber,
                autenticacionClaveNumericaModel.Body.DocumentTypeId,
                autenticacionClaveNumericaModel.Body.DocumentCountryId);

            return persona.id;
        }

        private bool ValidarPasswordPin(string password, string pin)
        {
            if (string.IsNullOrEmpty(pin)) return false;

            var encryptedPassword = _tDesEncryption.Encrypt(password);

            return pin.ToUpper().Equals(encryptedPassword.ToUpper());
        }

        private async Task<AutenticacionModelOutput> ValidarPasswordAsync(
            string password,
            string userNameEncriptado,
            IUsuario usuario,
            bool fromNewVersionDb,
            FechaDbServer dateTime,
            FechaDbServerV2 dateTimeV2
        )
        {
            if (!fromNewVersionDb && usuario.GetPersonId() == null)
            {
                await CompletarPersonIdUsuarioAsync(usuario);
            }

            var autenticacionModelOutput = new AutenticacionModelOutput
            {
                Codigo = Codigo.Incorrecto,
                Error = ErrorCode.UsuarioIncorrecto,
                IdPersona = usuario.GetPersonId()
            };

            var claveEncriptada = _encryption.GetHash(password);

            if (!fromNewVersionDb && usuario.GetPassword().Equals(claveEncriptada)
                || fromNewVersionDb && usuario.GetPassword().Equals(claveEncriptada) &&
                usuario.GetUserName().Equals(userNameEncriptado))
            {
                await AnalizarEstadoDeUsuarioAsync(usuario, autenticacionModelOutput, dateTime, fromNewVersionDb);
            }
            else
            {
                ActualizarIntentoFallido(usuario, fromNewVersionDb ? dateTimeV2.Now : dateTime.Now);

                await SetupCantidadDeIntentosDeLoginAsync();

                if (ActualizarSiUsuarioBloqueado(usuario, fromNewVersionDb))
                {
                    autenticacionModelOutput.Codigo = Codigo.Bloqueado;
                    autenticacionModelOutput.Error = ErrorCode.SeHaBloqueadoElUsuario;
                }
            }

            return autenticacionModelOutput;
        }

        private async Task<AutenticacionModelOutput> ValidarPasswordAsync(
            string password,
            IUsuario usuario,
            bool fromNewVersionDb,
            FechaDbServer dateTime,
            FechaDbServerV2 dateTimeV2
        )
        {
            var autenticacionModelOutput = new AutenticacionModelOutput
            {
                Codigo = Codigo.Incorrecto,
                Error = ErrorCode.UsuarioIncorrecto,
                IdPersona = usuario.GetPersonId()
            };

            var claveEncriptada = _encryption.GetHash(password);

            if (usuario.GetPassword().Equals(claveEncriptada))
            {
                await AnalizarEstadoDeUsuarioAsync(usuario, autenticacionModelOutput, dateTime, fromNewVersionDb);
            }
            else
            {
                ActualizarIntentoFallido(usuario, dateTimeV2.Now);

                if (ActualizarSiUsuarioBloqueado(usuario, fromNewVersionDb))
                {
                    autenticacionModelOutput.Codigo = Codigo.Bloqueado;
                    autenticacionModelOutput.Error = ErrorCode.SeHaBloqueadoElUsuario;
                }
            }

            return autenticacionModelOutput;
        }

        private string ValidarUsuarioRegistracion(bool esNuevoModelo,
            IRequestBody<RegistracionModelInputV2> registracionModel
        )
        {
            var modelId = esNuevoModelo ? Model.BaseUnicaUsuarios : Model.HomeBankingIndividuo;
            var validaciones = new StringBuilder();

            TraerValidaciones(validaciones, modelId, Input.Usuario, registracionModel.Body.UserName,
                MessageConstants.ValidacionesUsuario);
            TraerValidaciones(validaciones, modelId, Input.Clave, registracionModel.Body.Password,
                MessageConstants.ValidacionesClave);

            return validaciones.ToString();
        }

        private string ValidarUsuarioMigracion(IRequestBody<MigracionModelInput> migracionModel)
        {
            var modelId = Model.BaseUnicaUsuarios;
            var validaciones = new StringBuilder();

            TraerValidaciones(validaciones, modelId, Input.Usuario, migracionModel.Body.UserName,
                MessageConstants.ValidacionesUsuario);
            TraerValidaciones(validaciones, modelId, Input.Clave, migracionModel.Body.Password,
                MessageConstants.ValidacionesClave);

            return validaciones.ToString();
        }

        private void TraerValidaciones(
            StringBuilder validaciones,
            Model modelId,
            Input inputId,
            string valorMatch,
            string inputText
        )
        {
            var validacionesMensaje = new List<string>();
            var regexInput =
                _reglaValidacionV2Repository.ObtenerReglasValidacionActivasByModelAndInputAsync((int)modelId,
                    (int)inputId);

            foreach (var item in regexInput.Result)
            {
                var exp = ObtenerRegex(item);
                if (!exp.IsMatch(valorMatch) && (item.IsRequired.HasValue && item.IsRequired.Value))
                {
                    validacionesMensaje.Add(item.ValidationRuleText);
                }
            }

            if (validacionesMensaje.Count > 0)
            {
                validaciones.Append(inputText);
                validacionesMensaje.ForEach(x => validaciones.Append(x.PadRight(x.Length + 1)));
            }
        }

        private static Regex ObtenerRegex(ReglaValidacionV2 item)
        {
            var regularExpressionNET = item.RegularExpression.Remove(0, 1);
            regularExpressionNET = regularExpressionNET.Remove(regularExpressionNET.Length - 1, 1);

            if (regularExpressionNET.Substring(regularExpressionNET.Length - 1, 1) == "/")
            {
                regularExpressionNET = regularExpressionNET.Remove(regularExpressionNET.Length - 1, 1);
            }

            var exp = new Regex(regularExpressionNET);
            return exp;
        }

        private async Task<string> CompletarPersonIdUsuarioAsync(IUsuario usuario)
        {
            var numeroDocumento = usuario.GetDocumentNumber();
            var tipoDocumento = usuario.GetDocumentType();
            var paisDocumento = usuario.GetDocumentCountryId();
            var result = await GetIdPersona(numeroDocumento, tipoDocumento, paisDocumento);
            var userData = usuario.GetDatosUsuario();

            if (!string.IsNullOrWhiteSpace(userData.PersonId))
                return userData.PersonId;

            userData.PersonId = result.id.ToString();
            _datosUsuarioRepository.Update(userData);
            await _datosUsuarioRepository.SaveChangesAsync();

            return userData.PersonId;
        }

        private async Task<PersonaModelResponse> GetIdPersona(
            string numeroDocumento,
            int tipoDocumento,
            int paisDocumento
        )
        {
            var result = await _personasRepository.ObtenerPersona(numeroDocumento, tipoDocumento, paisDocumento);

            if (result == null)
            {
                throw new BusinessException(
                    $"No se encontró a la Persona Física desde api-personas con PaisDoc: {paisDocumento}, " +
                    $"TipoDoc: {tipoDocumento} y NroDoc: {numeroDocumento}",
                    0
                );
            }

            return result;
        }

        private async Task SaveUsuario(IUsuario usuario, bool fromNewVersionDb)
        {
            if (!fromNewVersionDb)
            {
                _usuarioRepository.Update((Usuario)usuario);
                await _usuarioRepository.SaveChangesAsync();
            }
            else
            {
                _usuarioV2Repository.Update((UsuarioV2)usuario);
                await _usuarioV2Repository.SaveChangesAsync();
            }
        }

        private void ActualizarUsuarioPorIntentoFallido(IUsuario usuario, DateTime dateTimeV2)
        {
            ActualizarIntentoFallido(usuario, dateTimeV2);
            ActualizarSiUsuarioBloqueado(usuario, true);
        }

        private async Task<UserFromDbResult> GetUsuarioFromDbAsync(
            int documentCountryId,
            int documentTypeId,
            string documentNumber
        )
        {
            IUsuario usuario =
                await _usuarioV2Repository.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(documentCountryId,
                    documentTypeId, documentNumber);

            return new UserFromDbResult
            {
                Usuario = usuario,
                FromNewVersionDb = true,
                UsernameEncrypted = string.Empty
            };
        }

        private async Task<UserFromDbResult> GetUsuarioFromDbAsync(string userName, string documentNumber = null)
        {
            UserFromDbResult userFromDbResult;

            // Si no viene Nro. de Documento, asumimos que es un flujo legacy 
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                var usuario = await _usuarioRepository.ObtenerUsuarioAsync(userName);

                userFromDbResult = new UserFromDbResult
                {
                    Usuario = usuario,
                    FromNewVersionDb = false,
                    UsernameEncrypted = string.Empty
                };
            }
            else
            {
                userFromDbResult = await ObtenerUsuarioMultiModeloAsync(userName, documentNumber);
            }

            return userFromDbResult;
        }

        private async Task<UserFromDbResult> GetUsuarioFromDbAsync(long personId)
        {
            var fromNewVersionDb = false;

            IUsuario usuario = await _usuarioV2Repository.ObtenerUsuarioByPersonIdAsync(personId);

            if (usuario != null)
            {
                fromNewVersionDb = true;
            }
            else
            {
                usuario = await _usuarioRepository.ObtenerUsuarioByPersonIdAsync(personId);

                if (usuario == null)
                {
                    //En el legacy HBI pueden existir casos en los cuales no este el personId en la BD de usuarios
                    var persona = await _personasRepository.ObtenerInfoPersonaFisica(personId);

                    if (persona != null)
                    {
                        usuario = await _usuarioRepository.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                            GetCountryByBanTotal(persona.pais_documento),
                            persona.tipo_documento,
                            persona.numero_documento);
                        usuario.SetPersonId(persona.id.ToString());
                    }
                }
            }

            return new UserFromDbResult
            {
                Usuario = usuario,
                FromNewVersionDb = fromNewVersionDb,
            };
        }

        private async Task<UserFromDbResult> ObtenerUsuarioMultiModeloAsync(string userName, string documentNumber)
        {
            var fromNewVersionDb = false;
            var userNameEncriptado = string.Empty;
            IUsuario usuario = null;

            await SetupCantidadDeIntentosDeLoginAsync();

            // Buscando usuarios por NroDocumento en el modelo nuevo
            var usuarios = _usuarioV2Repository.ObtenerUsuarioByDocumentNumber(documentNumber);

            // Si no se encuentra, buscamos en el legacy
            if (!usuarios.Any())
            {
                usuario = await _usuarioRepository.ObtenerUsuarioAsync(userName, documentNumber);
            }
            else
            {
                fromNewVersionDb = true;

                userNameEncriptado = _encryption.GetHash(userName);

                // Está controlado por BD que no pueda existir Username y NroDoc repetidos 
                if (usuarios.Any(x => x.Username.Equals(userNameEncriptado)))
                {
                    usuario = usuarios.FirstOrDefault(x => x.Username.Equals(userNameEncriptado));
                }
                else
                {
                    // 10 Millones es el valor desde el cual asumimos que no hay duplicados
                    // por lo que buscamos en el legacy, algún registro, en el caso de que sea menor
                    if (usuarios.Count() > 1 ||
                        long.TryParse(documentNumber, out var nroDocumento) && nroDocumento > 10000000)
                    {
                        var dateTimeV2 = await _helperDbServerV2.ObtenerFechaAsync();

                        foreach (var usuarioV2 in usuarios)
                        {
                            ActualizarUsuarioPorIntentoFallido(usuarioV2, dateTimeV2.Now);

                            _usuarioV2Repository.Update(usuarioV2);
                        }

                        await _usuarioV2Repository.SaveChangesAsync();
                    }
                    else
                    {
                        usuario = await _usuarioRepository.ObtenerUsuarioAsync(userName, documentNumber);
                        fromNewVersionDb = false;
                    }
                }
            }

            return new UserFromDbResult
            {
                Usuario = usuario,
                FromNewVersionDb = fromNewVersionDb,
                UsernameEncrypted = userNameEncriptado
            };
        }

        private static string ObtenerEmailPersona(PersonaFisicaInfoModelResponse result)
        {
            var email = result.emails?.FirstOrDefault(e => (!e.dado_de_baja ?? false)
                                                           && (e.principal ?? false)
                                                           && (e.confiable ?? false));

            return email?.direccion ?? string.Empty;
        }

        private static Common.Dtos.PersonaService.Output.Telefono ObtenerTelefonoDobleFactorVerificadoPersona(
            PersonaFisicaInfoModelResponse result,
            string canal
        )
        {
            var telefonosVerificados = result.telefonos?.FindAll(t => t.ultima_verificacion_positiva != null);

            var telefonoDobleFactor = telefonosVerificados?.FirstOrDefault(t => (t.doble_factor ?? false)
                && (!t.dado_de_baja ?? false)
                && (t.principal ?? false)
                && (t.confiable ?? false)
                && (t.ultima_verificacion_positiva.canal_verificacion.ToUpper().Equals(canal.ToUpper())));

            return telefonoDobleFactor;
        }

        private static string PasswordStatusMessage(bool expired, bool fromBta = false)
        {
            if (expired)
            {
                return fromBta ? ErrorConstants.CodigoPasswordExpiradoBta : ErrorConstants.CodigoPasswordExpirado;
            }

            return ErrorConstants.CodigoPasswordNoExpirado;
        }

        private async Task<bool> ClaveYaUtilizadaSegunModeloAsync(
            IUsuario usuario,
            string nuevaClaveEncriptada,
            bool fromNewVersionDb
        )
        {
            bool claveYaUtilizada;
            var cantidadHistorialDeCambiosDeClave = fromNewVersionDb
                ? _cantidadHistorialDeCambiosDeClaveV2 ?? 0
                : _cantidadHistorialDeCambiosDeClave ?? 0;

            if (fromNewVersionDb)
            {
                var historialCambiosDeClaveUsuarioV2 =
                    await _historialClaveUsuariosV2Repository.ObtenerHistorialClavesUsuarioByUserIdAsync(
                        usuario.GetUserId());

                claveYaUtilizada = nuevaClaveEncriptada == usuario.GetPassword()
                                   || (historialCambiosDeClaveUsuarioV2 != null
                                       && historialCambiosDeClaveUsuarioV2
                                           .Take(cantidadHistorialDeCambiosDeClave - 1)
                                           .Where(h => h.GetPassword() == nuevaClaveEncriptada)
                                           .ToList().Count != 0);
            }
            else
            {
                var historialCambiosDeClaveUsuario =
                    await _historialClaveUsuariosRepository.ObtenerHistorialClavesUsuarioByUserIdAsync(
                        usuario.GetUserId());

                claveYaUtilizada = nuevaClaveEncriptada == usuario.GetPassword()
                                   || (historialCambiosDeClaveUsuario != null
                                       && historialCambiosDeClaveUsuario
                                           .Take(cantidadHistorialDeCambiosDeClave - 1)
                                           .Where(h => h.GetPassword() == nuevaClaveEncriptada)
                                           .ToList().Count != 0);
            }

            return claveYaUtilizada;
        }

        private async Task<bool> ClaveYaUtilizada(UsuarioV2 usuario, string nuevaClaveEncriptada)
        {
            // Obtenemos el historial de cambios de clave del usuario
            var cantidadDeHistorialDeCambiosDeClave = _cantidadHistorialDeCambiosDeClaveV2 ?? 0;

            var historialCambiosDeClaveUsuario =
                await _historialClaveUsuariosV2Repository.ObtenerHistorialClavesUsuarioByUserIdAsync(
                    usuario.GetUserId());

            // Tomamos la actual más las ultimas N-1 claves usadas por el usuario para verificar que no utilice la misma
            var claveYaUtilizada = nuevaClaveEncriptada == usuario.Password
                                   || (historialCambiosDeClaveUsuario != null
                                       && historialCambiosDeClaveUsuario
                                           .Take(cantidadDeHistorialDeCambiosDeClave - 1)
                                           .Where(h => h.Password == nuevaClaveEncriptada)
                                           .ToList().Count != 0);

            return claveYaUtilizada;
        }

        private async Task<bool> UsernameYaUtilizado(UsuarioV2 usuario, string nuevoUsuarioEncriptado)
        {
            // Obtenemos el historial de cambios de nombre de usuario
            var cantidadDeHistorialDeCambiosDeNombreUsuario =
                await _configuracionesV2Service.ObtenerConfiguracionCantidadDeHistorialDeCambiosDeNombreUsuarioAsync();

            var historialCambiosDeNombreUsuario =
                await _historialUsuarioUsuariosV2Repository.ObtenerHistorialNombresUsuarioByUserId(usuario.GetUserId());

            // Tomamos el actual más los últimos N-1 nombres de usuario usados por el usuario para verificar que no utilice el mismo
            var nombreUsuarioYaUtilizado = nuevoUsuarioEncriptado == usuario.Username
                                           || (historialCambiosDeNombreUsuario != null
                                               && historialCambiosDeNombreUsuario
                                                   .Take(cantidadDeHistorialDeCambiosDeNombreUsuario - 1)
                                                   .Where(h => h.Username == nuevoUsuarioEncriptado)
                                                   .ToList().Count != 0);

            return nombreUsuarioYaUtilizado;
        }

        private static string GetCustomerNumberByBanTotal(
            int documentCountryId,
            int documentTypeId,
            string documentNumber
        )
        {
            var country = GetCountryByBanTotal(documentCountryId);
            var documentType = documentTypeId.ToString().PadLeft(2, '0');

            return string.Concat(country[^3..], documentType[^2..], documentNumber);
        }

        private static string GetCountryByBanTotal(int documentCountryId)
        {
            return documentCountryId.ToString().PadLeft(3, '0');
        }

        private async Task<DateTime> GetPasswordExpiryDateAsync(DateTime? lastPasswordChange, bool fromNewVersionDb)
        {
            var diasParaForzarCambioClave = fromNewVersionDb
                ? await _configuracionesV2Service.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync()
                : await _configuracionesService.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync();

            var fechaAhora = fromNewVersionDb
                ? (await _helperDbServerV2.ObtenerFechaAsync()).Now
                : (await _helperDbServer.ObtenerFechaAsync()).Now;

            return (lastPasswordChange ?? fechaAhora).AddDays(diasParaForzarCambioClave);
        }

        private async Task SetupCantidadDeIntentosDeLoginAsync()
        {
            _cantidadDeIntentosDeLogin ??=
                await _configuracionesService.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync();
            _cantidadDeIntentosDeLoginV2 ??=
                await _configuracionesV2Service.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync();
        }

        private async Task SetupCantidadHistorialDeCambiosDeClaveAsync()
        {
            _cantidadHistorialDeCambiosDeClave ??= await _configuracionesService
                .ObtenerConfiguracionCantidadDeHistorialDeCambiosDeClaveAsync();
            _cantidadHistorialDeCambiosDeClaveV2 ??= await _configuracionesV2Service
                .ObtenerConfiguracionCantidadDeHistorialDeCambiosDeClaveAsync();
        }

        private static string VerificarReglasSegunModelo(string newPassword, bool fromNewVersionDb)
        {
            var inconsistencias = new StringBuilder();

            if (fromNewVersionDb)
            {
                foreach (var result in ResultsOfCredClave(newPassword))
                {
                    result
                        .OnOk(e => { })
                        .OnError(e => inconsistencias.Append(e.Message));
                }
            }
            else
            {
                foreach (var result in ResultsOfCredClaveOriginal(newPassword))
                {
                    result
                        .OnOk(e => { })
                        .OnError(e => inconsistencias.Append(e.Message));
                }
            }

            return inconsistencias.ToString();
        }

        private static IEnumerable<Result<CredClave>> ResultsOfCredClave(string newPassword)
        {
            return new[] { CredClave.TryParse(newPassword, ParameterNames.NuevaClave) };
        }

        private static IEnumerable<Result<CredClaveOriginal>> ResultsOfCredClaveOriginal(string newPassword)
        {
            return new[] { CredClaveOriginal.TryParse(newPassword, ParameterNames.NuevaClave) };
        }

        private async Task ActualizarFechaUltimoCambioDeClaveEnHbiAsync(long personId)
        {
            var usuarioHbi = await _usuarioRepository.ObtenerUsuarioByPersonIdAsync(personId);

            // Actualizamos LastPasswordChange del Legacy mientras siga el acoplamiento con HBI (CDIG-1903/CDIG-1913)
            if (usuarioHbi != null)
            {
                var fechaDbServerHbi = await _helperDbServer.ObtenerFechaAsync();

                usuarioHbi.SetLastPasswordChange(fechaDbServerHbi.Now);
                await _usuarioRepository.SaveChangesAsync();
            }
        }

        #endregion
    }
}
