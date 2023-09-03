using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.ScoreOperaciones.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Utils;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Domain.Exceptions;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Helpers;
using Spv.Usuarios.Bff.Service.Interface;

namespace Spv.Usuarios.Bff.Service
{
    public class UsuariosService : IUsuarioService
    {
        private readonly ILogger<UsuariosService> _logger;
        private readonly IApiUsuariosRepository _usuariosRepository;
        private readonly IApiPersonasRepository _personasRepository;
        private readonly IApiUsuariosRepositoryV2 _usuariosRepositoryV2;
        private readonly IApiCatalogoRepository _catalogoRepository;
        private readonly IApiNotificacionesRepository _notificacionesRepository;
        private readonly IMapper _mapper;
        private readonly IApiScoreOperacionesRepository _scoreOperacionesRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public UsuariosService(
            ILogger<UsuariosService> logger,
            IApiUsuariosRepository usuariosRepository,
            IApiUsuariosRepositoryV2 usuariosRepositoryV2,
            IApiPersonasRepository personasRepository,
            IApiCatalogoRepository catalogoRepository,
            IApiNotificacionesRepository notificacionesRepository,
            IMapper mapper,
            IApiScoreOperacionesRepository scoreOperacionesRepository,
            IMemoryCache memoryCache,
            IBackgroundJobClient backgroundJobClient
        )
        {
            _logger = logger;
            _usuariosRepository = usuariosRepository;
            _usuariosRepositoryV2 = usuariosRepositoryV2;
            _personasRepository = personasRepository;
            _catalogoRepository = catalogoRepository;
            _notificacionesRepository = notificacionesRepository;
            _mapper = mapper;
            _scoreOperacionesRepository = scoreOperacionesRepository;
            _memoryCache = memoryCache;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<IResponse<PerfilModelOutput>> ObtenerPerfilAsync(IRequestBody<PerfilModelInput> perfilModel)
        {
            try
            {
                var perfil = await _usuariosRepository.ObtenerPerfilAsync(perfilModel.Body.UserName);

                if (perfil == null)
                {
                    return Responses.NotFound<PerfilModelOutput>(ErrorCode.PersonaInexistente.ErrorDescription);
                }

                var result = new PerfilModelOutput
                {
                    PersonId = perfil.id_persona
                };

                return Responses.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(PersonaServiceEvents.ExceptionCallingPersonaFisica, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<PerfilModelOutputV2>> ObtenerPerfilAsync(
            IRequestBody<PerfilModelInputV2> perfilModel
        )
        {
            try
            {
                var resultPerfil = await _usuariosRepositoryV2.ObtenerPerfilAsync(perfilModel.Body.IdPersona);

                if (!resultPerfil.IsSuccessStatusCode)
                {
                    return Responses.NotFound<PerfilModelOutputV2>(ErrorCode.PerfilInexistente.ErrorDescription);
                }

                var perfil = await UsuariosHelper.DeserializarPerfilV2Async(resultPerfil);

                var result = new PerfilModelOutputV2
                {
                    UltimoLogin = perfil.ultimo_login,
                    IdPersona = perfil.id_persona,
                    NumeroDocumento = perfil.nro_documento,
                    TipoDocumento = perfil.tipo_documento,
                    Nombre = perfil.nombre,
                    Apellido = perfil.apellido,
                    Email = perfil.email,
                    Genero = perfil.genero,
                    Pais = perfil.pais,
                    FechaUltimoCambioClave = perfil.fecha_ultimo_cambio_clave,
                    FechaVencimientoClave = perfil.fecha_vencimiento_clave,
                    IdUsuario = perfil.id_usuario,
                    IdStatusUsuario = perfil.user_status_id
                };

                return Responses.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingPerfil, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<ValidacionExistenciaHbiModelOutput>> ValidarExistenciaHbiAsync(
            IRequestBody<ValidacionExistenciaHbiModelInput> validacionExistenciaHbiModel
        )
        {
            try
            {
                // Validamos la existencia del usuario
                var resultExistencia =
                    await UsuariosHelper.ValidarExistenciaUsuarioHbiAsync(
                        _usuariosRepository,
                        validacionExistenciaHbiModel.Body.UserName);

                return resultExistencia;
            }
            catch (Exception ex)
            {
                _logger.LogError(PersonaServiceEvents.ExceptionCallingPersonasFiltro, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<ValidacionExistenciaModelOutput>> ValidarExistenciaAsync(
            IRequestBody<ValidacionExistenciaModelInput> validacionExistenciaModel
        ) => await ValidarExistenciaAsync(validacionExistenciaModel.Body);

        private async Task<IResponse<ValidacionExistenciaModelOutput>> ValidarExistenciaAsync(
            ValidacionExistenciaModelInput validacionExistenciaModel
        )
        {
            try
            {
                // Obtenemos Personas
                var personas = await _personasRepository.ObtenerPersonaFiltroAsync(
                    validacionExistenciaModel.NumeroDocumento
                );

                if (personas == null || personas.Count == 0)
                {
                    return Responses.NotFound<ValidacionExistenciaModelOutput>(
                        ErrorCode.PersonaInexistente.ErrorDescription
                    );
                }

                if (
                    personas.Count == 1 ||
                    validacionExistenciaModel.TipoDocumento != null ||
                    validacionExistenciaModel.IdPais != null
                )
                {
                    var persona = personas.Count == 1
                        ? personas.First()
                        : personas.FirstOrDefault(x =>
                            x.tipo_documento == (validacionExistenciaModel.TipoDocumento ??
                                                 (int)TipoDocumento.Pasaporte)
                            && x.pais_documento == (validacionExistenciaModel.IdPais ??
                                                    AppConstants.ArgentinaCodigoBantotal));

                    return persona != null
                        ? await ApiUsuariosExistenciaAsync(persona)
                        : Responses.NotFound<ValidacionExistenciaModelOutput>(
                            ErrorCode.PersonaInexistente.ErrorDescription
                        );
                }

                if (
                    PersonasHelper.ExisteConflictoConDniVsLibretas(
                        validacionExistenciaModel.NumeroDocumento,
                        personas
                    )
                )
                {
                    var persona = personas.First(x => x.tipo_documento != (int)TipoDocumento.Dni);

                    var resultExistencia = await ApiUsuariosExistenciaAsync(persona);

                    return resultExistencia;
                }

                var resultsExistencia = new List<IResponse<ValidacionExistenciaModelOutput>>();
                var tiposDocumento = new List<TipoDocumentoModelOutput>();
                var paises = new List<PaisModelOutput>();

                await ComprobarConflictosValidacionExistenciaAsync(personas, resultsExistencia, tiposDocumento, paises);

                if (resultsExistencia.Count <= 1)
                {
                    return resultsExistencia.Count == 1
                        ? resultsExistencia.FirstOrDefault()
                        : Responses.NotFound<ValidacionExistenciaModelOutput>(
                            ErrorCode.PersonaInexistente.ErrorDescription
                        );
                }

                var conflicto = await PersonasHelper.RecopilarConflictosDePersonas(
                    _catalogoRepository,
                    tiposDocumento,
                    paises
                );

                var response = new ValidacionExistenciaModelOutput
                {
                    TiposDocumento = conflicto.TiposDocumento,
                    Paises = conflicto.Paises
                };

                return Responses.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(PersonaServiceEvents.ExceptionCallingPersonasFiltro, ex.Message, ex);
                throw;
            }
        }

        private async Task ComprobarConflictosValidacionExistenciaAsync(
            List<ApiPersonasFiltroModelOutput> personas,
            ICollection<IResponse<ValidacionExistenciaModelOutput>> resultsExistencia,
            ICollection<TipoDocumentoModelOutput> tiposDocumento,
            ICollection<PaisModelOutput> paises
        )
        {
            foreach (var persona in personas)
            {
                var resultExistencia = await ApiUsuariosExistenciaAsync(persona);

                if (!resultExistencia.IsOk)
                    continue;

                resultsExistencia.Add(resultExistencia);

                tiposDocumento.Add(new TipoDocumentoModelOutput
                {
                    codigo = persona.tipo_documento,
                    descripcion = AppConstants.GetTipoDocumentoDesc(persona.tipo_documento)
                });

                paises.Add(new PaisModelOutput
                {
                    codigo = persona.pais_documento
                });
            }
        }

        private async Task<IResponse<ValidacionExistenciaModelOutput>> ApiUsuariosExistenciaAsync(
            ApiPersonasFiltroModelOutput persona
        )
        {
            // Obtenemos Persona Física
            var personaFisica = await PersonasHelper.GetInfoPersonaFisicaAsync(_personasRepository, persona.id);

            // Validamos la existencia del usuario
            var resultExistencia = await UsuariosHelper.ValidarExistenciaUsuarioAsync(
                _usuariosRepositoryV2,
                personaFisica
            );

            return resultExistencia;
        }

        public async Task<IResponse<RegistracionModelOutput>> RegistrarAsync(
            IRequestBody<RegistracionModelInput> registracionModel
        )
        {
            try
            {
                string claveCanales = null;

                if (!string.IsNullOrWhiteSpace(registracionModel.Body.ChannelKey))
                {
                    var resultClaveCanales = ClaveCanales.TryParse(
                        registracionModel.Body.ChannelKey ?? string.Empty,
                        ParameterNames.ClaveCanales
                    );

                    resultClaveCanales
                        .OnOk(cc => claveCanales = cc.Valor)
                        .OnError(e => throw new BusinessException(e.Message, 0));
                }

                int.TryParse(registracionModel.Body.PersonId, out var idPersona);

                // Obtenemos Persona Física
                var personaFisica = await PersonasHelper.GetInfoPersonaFisicaAsync(_personasRepository, idPersona);

                // Validamos la existencia del usuario
                var resultExistenciaUsuario =
                    await UsuariosHelper.ValidarExistenciaUsuarioAsync(_usuariosRepositoryV2, personaFisica);

                if (resultExistenciaUsuario.IsOk)
                {
                    _logger.LogDebug(
                        "Conflicto al querer registrar un usuario que ya existe con id_persona: {0}",
                        idPersona
                    );

                    await ScoreOperacionesRegistracion(registracionModel, MessageConstants.DocumentoYaExiste);

                    return Responses.Conflict<RegistracionModelOutput>(MessageConstants.DocumentoYaExiste);
                }

                if (!string.IsNullOrWhiteSpace(registracionModel.Body.TyCId))
                {
                    _backgroundJobClient.Enqueue<IApiTyCRepository>(service =>
                        service.AceptarAsync(new ValueTuple<int, string>(idPersona, registracionModel.Body.TyCId))
                    );
                }

                await CrearEmailAsync(registracionModel);
                await CrearTelefonoAsync(registracionModel);

                var body = new ApiUsuariosRegistracionV2ModelInput
                {
                    clave = registracionModel.Body.Password,
                    id_pais = registracionModel.Body.DocumentCountryId,
                    id_persona = registracionModel.Body.PersonId,
                    id_tipo_documento = registracionModel.Body.DocumentTypeId,
                    nro_documento = registracionModel.Body.DocumentNumber,
                    usuario = registracionModel.Body.UserName
                };

                var validacionClaveResponse = await ClaveHelper.ValidarClaveCanales(
                    _usuariosRepositoryV2,
                    claveCanales,
                    registracionModel.Body.DocumentTypeId,
                    registracionModel.Body.DocumentNumber
                );

                if (!validacionClaveResponse.IsSuccessStatusCode)
                {
                    return Responses.Unauthorized<RegistracionModelOutput>(
                        ErrorCode.ClaveCanalesNoCorrespondiente.ErrorDescription,
                        ErrorCode.ClaveCanalesNoCorrespondiente.Code
                    );
                }

                var response = await _usuariosRepositoryV2.RegistrarUsuarioV2Async(body);

                var motivo = await ScoreOperacionesRegistracionMotivo(response.IsSuccessStatusCode, response);
                await ScoreOperacionesRegistracion(registracionModel, motivo);

                if (response.IsSuccessStatusCode)
                {
                    var inhabilitacionClaveCanalesBody = new ApiUsuariosInhabilitacionClaveCanalesModelInput
                    {
                        id_tipo_documento = registracionModel.Body.DocumentTypeId,
                        nro_documento = registracionModel.Body.DocumentNumber,
                        clave_canales = registracionModel.Body.ChannelKey
                    };

                    var responseInhabilitacion = await _usuariosRepositoryV2.InhabilitarClaveCanalesAsync(
                        inhabilitacionClaveCanalesBody
                    );

                    if (!responseInhabilitacion.IsSuccessStatusCode)
                    {
                        _logger.LogError(
                            "Falló en inhabilitación de clave de canales: {0}",
                            ProcessExternalError<InhabilitacionClaveCanalesModelOutput>.ProcessApiUsuariosErrorResponse(
                                responseInhabilitacion
                            )
                        );
                    }

                    var resultPerfil = await _usuariosRepositoryV2.ObtenerPerfilAsync(idPersona);

                    if (!resultPerfil.IsSuccessStatusCode)
                    {
                        _logger.LogWarning(
                            "Falló al obtener el perfil: {0}",
                            ProcessExternalError<ApiUsuariosPerfilModelOutputV2>.ProcessApiUsuariosErrorResponse(
                                resultPerfil
                            )
                        );
                    }
                    else
                    {
                        var perfil = await UsuariosHelper.DeserializarPerfilV2Async(resultPerfil);
                        await EnviarEmailConNombreAsync(perfil, AppConstants.EmailTemplateIdAltaCredenciales);
                    }
                }
                else
                {
                    _logger.LogError(
                        "Falló al Registrar el Usuario: {0}",
                        ProcessExternalError<InhabilitacionClaveCanalesModelOutput>.ProcessApiUsuariosErrorResponse(
                            response
                        )
                    );

                    return await ProcessExternalError<RegistracionModelOutput>.ProcessApiUsuariosErrorResponse(
                        response
                    );
                }

                return Responses.Created(new RegistracionModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingRegistro, ex.Message, ex);
                throw;
            }
        }

        private async Task ScoreOperacionesRegistracion(IRequestBody<RegistracionModelInput> registracionModel,
            string motivo)
        {
            await _scoreOperacionesRepository.RegistracionAsync(
                new ApiScoreOperacionesRegistracionModelInput
                {
                    IdPersona = registracionModel.Body.PersonId,
                    CBU = "Desconocido",
                    IdDispositivo = registracionModel.Body.DeviceId,
                    Motivo = motivo,
                    TipoRegistracion = "Desconocido"
                });
        }

        private async Task<string> ScoreOperacionesRegistracionMotivo(bool seRegistro, HttpResponseMessage response)
        {
            if (!seRegistro)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var error = await JsonSerializer.DeserializeAsync<ApiUsuariosErrorResponse>(responseStream);
                StringBuilder motivo = new StringBuilder();

                error.Errores.ForEach(error => { motivo.Append(error.Detalle); });
                return motivo.ToString();
            }

            return "Registrado";
        }

        private async Task CrearTelefonoAsync(IRequestBody<RegistracionModelInput> registracionModel)
        {
            var phoneCreationBody = new ApiPersonasCreacionTelefonoModelInput
            {
                confiable = true,
                principal = true,
                pais = AppConstants.ArgentinaCodigoBantotal,
                numero = registracionModel.Body.Phone
            };

            // Si el usuario completó la verificación por SMS lo guardamos como doble factor y verificado, en ApiPersonas
            if (registracionModel.Body.SmsValidated)
            {
                var phoneCreationResult = await _personasRepository.CrearTelefonoDobleFactorAsync(
                    registracionModel.Body.PersonId,
                    phoneCreationBody
                );

                await UsuariosHelper.ProcessPhoneCreationAsync(
                    _personasRepository.ActualizarTelefonoDobleFactorAsync,
                    registracionModel.Body.PersonId,
                    registracionModel.Body.Phone,
                    phoneCreationResult.status_code,
                    MessageConstants.ApiPersonasErrorCreacionTelefonoDobleFactor,
                    null
                );

                var verificacionBody = new ApiPersonasVerificacionTelefonoModelInput
                {
                    verificado = true
                };

                var verificacionResult = await _personasRepository.VerificarTelefonoAsync(
                    phoneCreationResult.id,
                    verificacionBody
                );

                if (verificacionResult.status_code != HttpStatusCode.OK)
                    throw new BusinessException(MessageConstants.ApiPersonasErrorVerificacionTelefono, 0);
            }
            else
            {
                var phoneCreationResult = await _personasRepository.CrearTelefonoAsync(
                    registracionModel.Body.PersonId,
                    phoneCreationBody
                );

                await UsuariosHelper.ProcessPhoneCreationAsync(
                    _personasRepository.ActualizarTelefonoAsync,
                    registracionModel.Body.PersonId,
                    registracionModel.Body.Phone,
                    phoneCreationResult.status_code,
                    MessageConstants.ApiPersonasErrorCreacionTelefono,
                    phoneCreationResult.id.ToString()
                );
            }
        }

        private async Task CrearEmailAsync(IRequestBody<RegistracionModelInput> registracionModel)
        {
            var emailCreationBody = new ApiPersonasCreacionEmailModelInput
            {
                confiable = true,
                principal = true,
                direccion = registracionModel.Body.Email
            };

            var emailCreationResult = await _personasRepository.CrearEmailAsync(
                registracionModel.Body.PersonId,
                emailCreationBody
            );

            if (emailCreationResult.status_code != HttpStatusCode.OK)
            {
                switch (emailCreationResult.status_code)
                {
                    case HttpStatusCode.Conflict:
                        var emailUpdateResult = await _personasRepository.ActualizarEmailAsync(
                            registracionModel.Body.PersonId,
                            emailCreationResult.id.ToString(),
                            emailCreationBody);

                        if (emailUpdateResult.status_code != HttpStatusCode.OK)
                            throw new BusinessException(MessageConstants.ApiPersonasErrorActualizacionEmail, 0);

                        break;

                    default:
                        throw new BusinessException(MessageConstants.ApiPersonasErrorCreacionEmail, 0);
                }
            }
        }

        public async Task<IResponse<CambioDeCredencialesModelOutput>> ModificarCredencialesAsync(
            IRequestBody<CambioDeCredencialesModelInput> cambioDeCredencialesModel
        )
        {
            try
            {
                string claveCanales = null;

                if (!string.IsNullOrWhiteSpace(cambioDeCredencialesModel.Body.ChannelKey))
                {
                    var resultClaveCanales = ClaveCanales.TryParse(
                        cambioDeCredencialesModel.Body.ChannelKey ?? string.Empty,
                        ParameterNames.ClaveCanales
                    );

                    resultClaveCanales
                        .OnOk(cc => claveCanales = cc.Valor)
                        .OnError(e => throw new BusinessException(e.Message, 0));
                }

                int.TryParse(cambioDeCredencialesModel.Body.PersonId, out var idPersona);

                var resultPerfil = await _usuariosRepositoryV2.ObtenerPerfilAsync(idPersona);

                if (!resultPerfil.IsSuccessStatusCode)
                {
                    return Responses.NotFound<CambioDeCredencialesModelOutput>(
                        ErrorCode.PerfilInexistente.ErrorDescription
                    );
                }

                var perfil = await UsuariosHelper.DeserializarPerfilV2Async(resultPerfil);

                if (cambioDeCredencialesModel.Body.IsChannelKey)
                {
                    var validacionClaveResponse = await ClaveHelper.ValidarClaveCanales(
                        _usuariosRepositoryV2,
                        claveCanales,
                        perfil.tipo_documento,
                        perfil.nro_documento
                    );

                    if (!validacionClaveResponse.IsSuccessStatusCode)
                    {
                        return Responses.Unauthorized<CambioDeCredencialesModelOutput>(
                            ErrorCode.ClaveCanalesNoCorrespondiente.ErrorDescription,
                            ErrorCode.ClaveCanalesNoCorrespondiente.Code
                        );
                    }
                }

                var newCredentialsBody = new ApiUsuariosCambioDeCredencialesModelInput
                {
                    id_persona = cambioDeCredencialesModel.Body.PersonId,
                    nueva_clave = cambioDeCredencialesModel.Body.NewPassword,
                    nuevo_usuario = cambioDeCredencialesModel.Body.NewUsername
                };

                var response = await _usuariosRepositoryV2.CambiarCredencialesAsync(newCredentialsBody);

                if (!response.IsSuccessStatusCode)
                {
                    return await ProcessExternalError<CambioDeCredencialesModelOutput>.ProcessApiUsuariosErrorResponse(
                        response
                    );
                }

                if (!string.IsNullOrWhiteSpace(claveCanales))
                {
                    var body = new ApiUsuariosInhabilitacionClaveCanalesModelInput
                    {
                        id_tipo_documento = perfil.tipo_documento,
                        nro_documento = perfil.nro_documento,
                        clave_canales = claveCanales
                    };

                    var responseInhabilitacion = await _usuariosRepositoryV2.InhabilitarClaveCanalesAsync(body);

                    if (!responseInhabilitacion.IsSuccessStatusCode)
                    {
                        _logger.LogError(
                            "Falló en inhabilitación de clave de canales: {0}",
                            ProcessExternalError<InhabilitacionClaveCanalesModelOutput>
                                .ProcessApiUsuariosErrorResponse(
                                    responseInhabilitacion
                                )
                        );
                    }
                }

                await EnviarEmailConNombreAsync(perfil, AppConstants.EmailTemplateIdCambioCredenciales);

                return Responses.Ok(new CambioDeCredencialesModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingModificarCredenciales, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<CambioDeClaveModelOutput>> ModificarClaveAsync(
            IRequestBody<CambioDeClaveModelInput> cambioDeClaveModel
        )
        {
            try
            {
                string claveCanales = null;
                var cambioDeClaveModelBody = cambioDeClaveModel.Body;

                if (!string.IsNullOrWhiteSpace(cambioDeClaveModelBody.ChannelKey))
                {
                    var resultClaveCanales = ClaveCanales.TryParse(
                        cambioDeClaveModelBody.ChannelKey,
                        ParameterNames.ClaveCanales
                    );

                    resultClaveCanales
                        .OnOk(cc => claveCanales = cc.Valor)
                        .OnError(e => throw new BusinessException(e.Message, 0));
                }

                int.TryParse(cambioDeClaveModelBody.PersonId, out var idPersona);

                var resultPerfil = await _usuariosRepositoryV2.ObtenerPerfilAsync(idPersona);

                if (!resultPerfil.IsSuccessStatusCode)
                {
                    return Responses.NotFound<CambioDeClaveModelOutput>(ErrorCode.PerfilInexistente.ErrorDescription);
                }

                var perfil = await UsuariosHelper.DeserializarPerfilV2Async(resultPerfil);

                if (cambioDeClaveModelBody.IsChannelKey)
                {
                    var validacionClaveResponse = await ClaveHelper.ValidarClaveCanales(
                        _usuariosRepositoryV2,
                        claveCanales,
                        perfil.tipo_documento,
                        perfil.nro_documento
                    );

                    if (!validacionClaveResponse.IsSuccessStatusCode)
                    {
                        return Responses.Unauthorized<CambioDeClaveModelOutput>(
                            ErrorCode.ClaveCanalesNoCorrespondiente.ErrorDescription,
                            ErrorCode.ClaveCanalesNoCorrespondiente.Code
                        );
                    }
                }

                var newClaveBody = new ApiUsuariosCambioDeClaveModelInput
                {
                    id_persona = cambioDeClaveModelBody.PersonId,
                    nueva_clave = cambioDeClaveModelBody.NewPassword
                };

                var response = await _usuariosRepositoryV2.CambiarClaveAsync(newClaveBody);

                if (!response.IsSuccessStatusCode)
                {
                    return await ProcessExternalError<CambioDeClaveModelOutput>.ProcessApiUsuariosErrorResponse(
                        response
                    );
                }

                if (!string.IsNullOrWhiteSpace(claveCanales))
                {
                    var body = new ApiUsuariosInhabilitacionClaveCanalesModelInput
                    {
                        id_tipo_documento = perfil.tipo_documento,
                        nro_documento = perfil.nro_documento,
                        clave_canales = claveCanales
                    };

                    var responseInhabilitacion = await _usuariosRepositoryV2.InhabilitarClaveCanalesAsync(body);

                    if (!responseInhabilitacion.IsSuccessStatusCode)
                    {
                        _logger.LogDebug(
                            "Fallo en inhabilitación de clave de canales: {0}",
                            ProcessExternalError<InhabilitacionClaveCanalesModelOutput>
                                .ProcessApiUsuariosErrorResponse(responseInhabilitacion)
                        );
                    }
                }

                await ScoreOperationUpdateCredentials(cambioDeClaveModelBody, perfil);
                await EnviarEmailConNombreAsync(perfil, AppConstants.EmailTemplateIdCambioClave);

                return Responses.Ok(new CambioDeClaveModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingModificarClave, ex.Message, ex);
                throw;
            }
        }

        private async Task ScoreOperationUpdateCredentials(
            CambioDeClaveModelInput cambioDeClaveModel,
            ApiUsuariosPerfilModelOutputV2 perfil
        )
        {
            var cacheKey = Cache.Usuario.ScoreOperationUpdateCredentials(
                cambioDeClaveModel.PersonId,
                cambioDeClaveModel.DeviceId,
                cambioDeClaveModel.IsChannelKey.ToString(),
                perfil.user_status_id.ToString()
            );

            _memoryCache.TryGetValue(cacheKey, out bool? sendRequest);

            if (sendRequest is null)
            {
                await _scoreOperacionesRepository.UpdateCredentialsAsync(
                    new ApiScoreOperacionesModelInput
                    {
                        IdPersona = cambioDeClaveModel.PersonId,
                        CBU = "Desconocido",
                        IdDispositivo = cambioDeClaveModel.DeviceId,
                        AccionDelEvento = "Password",
                        Motivo = cambioDeClaveModel.IsChannelKey ? "ClaveCanales" : "SoftToken",
                        TipoDeAccion = perfil.user_status_id == 5 ? "Desbloqueo" : "Olvido",
                        NumeroReferencia = "Desconocido",
                        ActualizarEntidad = "Desconocido",
                    }
                );

                _memoryCache.Set(cacheKey, true, DateTimeOffset.Now.AddSeconds(30));
            }
        }

        private async Task EnviarEmailConNombreAsync(
            ApiUsuariosPerfilModelOutputV2 perfil,
            string emailTemplateId,
            IReadOnlyCollection<VariablesTemplate> variablesOpcional = null
        )
        {
            if (string.IsNullOrWhiteSpace(perfil.email))
            {
                _logger.LogInformation(
                    $"No se envió email [{emailTemplateId}] porque la persona no tenía dirección. PersonId = {perfil.id_persona}"
                );

                return;
            }

            var variablesTemplate = new List<VariablesTemplate>
            {
                UsuariosHelper.NuevaVariableTemplate(AppConstants.EmailVariableTemplateNombre, perfil.nombre)
            };

            if (variablesOpcional != null)
                variablesTemplate.AddRange(variablesOpcional);

            var enviarEmailBody = UsuariosHelper.ArmarEmailBody(
                perfil.id_persona,
                emailTemplateId,
                UsuariosHelper.NuevoDestinatario(perfil.email),
                variablesTemplate
            );

            await EnviarEmailAsync(enviarEmailBody);
        }

        private async Task EnviarEmailAsync(ApiNotificacionesEnviarEmailModelInput body)
        {
            try
            {
                var responseEnvioMail = await _notificacionesRepository.EnviarEmailAsync(body);

                if (responseEnvioMail.IsSuccessStatusCode)
                {
                    await using var responseStream = await responseEnvioMail.Content.ReadAsStreamAsync();

                    var apiNotificacionesEnviarEmailModelOutput =
                        await JsonSerializer.DeserializeAsync<ApiNotificacionesEnviarEmailModelOutput>(responseStream);

                    _logger.LogDebug(
                        "Envío de email - Body: {0} - Respuesta: {1}",
                        JsonSerializer.Serialize(body),
                        JsonSerializer.Serialize(apiNotificacionesEnviarEmailModelOutput)
                    );
                }
                else
                {
                    _logger.LogError(
                        "Falló el envío de email: {0}",
                        ProcessExternalError<ApiNotificacionesEnviarEmailModelOutput>.ProcessApiUsuariosErrorResponse(
                            responseEnvioMail
                        )
                    );
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    $"No se envió email porque el servicio de notificaciones fallo. {0}", e.Message
                );
            }
        }

        public async Task<IResponse<MigracionModelOutput>> MigrarAsync(IRequestBody<MigracionModelInput> migracionModel)
        {
            try
            {
                var body = new ApiUsuariosMigracionModelInput()
                {
                    id_persona = migracionModel.Body.PersonId,
                    usuario = migracionModel.Body.UserName,
                    clave = migracionModel.Body.Password
                };

                var response = await _usuariosRepositoryV2.MigrarUsuarioAsync(body);

                return response.IsSuccessStatusCode
                    ? Responses.Created(new MigracionModelOutput())
                    : await ProcessExternalError<MigracionModelOutput>.ProcessApiUsuariosErrorResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingMigrar, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<ValidacionExistenciaModelOutput>> RecuperarUsuarioAsync(
            IRequestBody<RecuperarUsuarioModelInput> recuperarUsuario
        )
        {
            // Double check para verificar existencia del usuario
            var validacionExistenciaModel = _mapper.Map<ValidacionExistenciaModelInput>(recuperarUsuario.Body);
            var response = await ValidarExistenciaAsync(validacionExistenciaModel);

            if (!response.IsOk)
                return response;

            try
            {
                ValidacionExistenciaModelOutput validacionExistencia = null;
                response.Map(m => validacionExistencia = _mapper.Map<ValidacionExistenciaModelOutput>(m));

                var resultPerfil = await _usuariosRepositoryV2.ObtenerPerfilAsync(
                    validacionExistencia.IdPersona
                );

                if (!resultPerfil.IsSuccessStatusCode)
                {
                    return Responses.NotFound<ValidacionExistenciaModelOutput>(
                        ErrorCode.PerfilInexistente.ErrorDescription
                    );
                }

                var perfil = await UsuariosHelper.DeserializarPerfilV2Async(resultPerfil);

                // Obtiene el usuario
                var userName = await UsuariosHelper.RecuperarUsuarioAsync(_usuariosRepositoryV2, perfil);

                var variablesTemplate = new List<VariablesTemplate>
                {
                    UsuariosHelper.NuevaVariableTemplate(AppConstants.EmailVariableTemplateUsuario, userName)
                };

                await EnviarEmailConNombreAsync(
                    perfil,
                    AppConstants.EmailTemplateIdRecuperarUsuario,
                    variablesTemplate
                );

                var usuarioRecuperado = new ValidacionExistenciaModelOutput
                {
                    EmailSemiOfuscado = UsuariosHelper.OfuscarMail(perfil.email, _logger)
                };

                return Responses.Ok(usuarioRecuperado);
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingRecuperarUsuario, ex.Message, ex);
                throw;
            }
        }
    }
}
