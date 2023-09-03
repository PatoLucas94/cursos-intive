using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Common.Dtos.ClaveCanalesService.Input;
using Spv.Usuarios.Common.Dtos.ClaveCanalesService.Output;
using Spv.Usuarios.Common.Errors;
using Spv.Usuarios.Common.LogEvents;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Domain.Services;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;

namespace Spv.Usuarios.Service
{
    public class ClaveCanalesService : IClaveCanalesService
    {
        private readonly ILogger<ClaveCanalesService> _logger;
        private readonly IEncryption _encryption;
        private readonly IHelperDbServer _helperDbServer;
        private readonly IUsuarioRegistradoRepository _usuarioRegistradoRepository;
        private readonly IConfiguracionesV2Service _configuracionesV2Service;
        private readonly IHelperDbServerV2 _helperDbServerV2;
        private readonly IAuditoriaLogV2Repository _auditoriaV2Repository;

        public ClaveCanalesService(
            ILogger<ClaveCanalesService> logger,
            IEncryption encryption,
            IHelperDbServer helperDbServer,
            IUsuarioRegistradoRepository usuarioRegistradoRepository,
            IConfiguracionesV2Service configuracionesV2Service,
            IHelperDbServerV2 helperDbServerV2,
            IAuditoriaLogV2Repository auditoriaV2Repository)
        {
            _logger = logger;
            _encryption = encryption;
            _helperDbServer = helperDbServer;
            _usuarioRegistradoRepository = usuarioRegistradoRepository;
            _configuracionesV2Service = configuracionesV2Service;
            _helperDbServerV2 = helperDbServerV2;
            _auditoriaV2Repository = auditoriaV2Repository;
        }

        public async Task<IResponse<ValidacionModelOutput>> ValidarAsync(IRequestBody<ValidacionModelInput> validacionModel)
        {
            try
            {
                var usuarioRegistrado = await _usuarioRegistradoRepository.ObtenerUsuarioRegistradoAsync(validacionModel.Body.DocumentTypeId, validacionModel.Body.DocumentNumber);
                var extendedInfoError = $"DocumentTypeId: {validacionModel.Body.DocumentTypeId} - DocumentNumber: {validacionModel.Body.DocumentNumber} - ";

                var inexistenteOInactiva = await ClaveCanalesInexistenteOInactiva(validacionModel.XCanal,
                                                                                              usuarioRegistrado,
                                                                                              extendedInfoError);

                if (inexistenteOInactiva != string.Empty)
                {
                    if(inexistenteOInactiva == ErrorCode.ClaveDeCanalesInexistente.Code)
                        return Responses.Unauthorized<ValidacionModelOutput>(ErrorCode.ClaveDeCanalesInexistente.Code, ErrorCode.ClaveDeCanalesInexistente.ErrorDescription);

                    return Responses.Unauthorized<ValidacionModelOutput>(ErrorCode.ClaveDeCanalesInactiva.Code, ErrorCode.ClaveDeCanalesInactiva.ErrorDescription);
                }

                var claveCanalesEncriptada = _encryption.EncryptChannelsKey(validacionModel.Body.ChannelKey, usuarioRegistrado.CardNumber);
                var claveCanalesValida = string.Equals(usuarioRegistrado.ChannelKey, claveCanalesEncriptada);
                var cantidadIntentosBloqueoDeClave = await _configuracionesV2Service.ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesAsync();

                // Clave Canales Incorrecta
                if (!claveCanalesValida)
                {
                    usuarioRegistrado.ForgotPasswordAttempts++;

                    _usuarioRegistradoRepository.Update(usuarioRegistrado);
                    await _usuarioRegistradoRepository.SaveChangesAsync();

                    // Clave Canales Incorrecta Sin Bloqueo de Clave o Con Bloqueo de Clave
                    if (usuarioRegistrado.ForgotPasswordAttempts <= cantidadIntentosBloqueoDeClave)
                    {
                        extendedInfoError += $"{JsonSerializer.Serialize(ErrorCode.ClaveDeCanalesIncorrecta.Code)} : { ErrorCode.ClaveDeCanalesIncorrecta.ErrorDescription} ";

                        await SaveAuditLogAsync(extendedInfoError, EventResults.Error, validacionModel.XCanal);
                        return Responses.Unauthorized<ValidacionModelOutput>(ErrorCode.ClaveDeCanalesIncorrecta.Code, ErrorCode.ClaveDeCanalesIncorrecta.ErrorDescription);
                    }
                    else
                    {
                        extendedInfoError += $"{JsonSerializer.Serialize(ErrorCode.ClaveDeCanalesBloqueada.Code)} : { ErrorCode.ClaveDeCanalesBloqueada.ErrorDescription} ";

                        await SaveAuditLogAsync(extendedInfoError, EventResults.Error, validacionModel.XCanal);
                        return Responses.Unauthorized<ValidacionModelOutput>(ErrorCode.ClaveDeCanalesBloqueada.Code, ErrorCode.ClaveDeCanalesBloqueada.ErrorDescription);
                    }
                }

                var bloqueadaoExpirada = await ClaveCanalesBloquedaOExpirada(validacionModel.XCanal,
                                                           validacionModel.Body.DocumentNumber,
                                                           validacionModel.Body.DocumentTypeId, 
                                                           usuarioRegistrado, 
                                                           extendedInfoError, 
                                                           cantidadIntentosBloqueoDeClave);

                if (bloqueadaoExpirada != string.Empty) { 
                    if(bloqueadaoExpirada == ErrorCode.ClaveDeCanalesBloqueada.Code)
                        return Responses.Unauthorized<ValidacionModelOutput>(ErrorCode.ClaveDeCanalesBloqueada.Code, ErrorCode.ClaveDeCanalesBloqueada.ErrorDescription);

                    return Responses.Unauthorized<ValidacionModelOutput>(ErrorCode.ClaveDeCanalesExpirada.Code, ErrorCode.ClaveDeCanalesExpirada.ErrorDescription);
                }

                return Responses.Accepted(new ValidacionModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(ClaveCanalesServiceEvents.ExceptionCallingValidacion, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<EstadoModelOutput>> ObtenerEstadoAsync(IRequestBody<EstadoModelInput> estadoModel)
        {
            try
            {
                var usuarioRegistrado = await _usuarioRegistradoRepository.ObtenerUsuarioRegistradoAsync(estadoModel.Body.DocumentTypeId, estadoModel.Body.DocumentNumber);
                var extendedInfoError = $"DocumentTypeId: {estadoModel.Body.DocumentTypeId} - DocumentNumber: {estadoModel.Body.DocumentNumber} - ";

                var claveCanalesInexistenteOInactiva = await ClaveCanalesInexistenteOInactiva(estadoModel.XCanal, usuarioRegistrado, extendedInfoError);

                if (claveCanalesInexistenteOInactiva != string.Empty)
                {
                    if (claveCanalesInexistenteOInactiva == ErrorCode.ClaveDeCanalesInexistente.Code)
                        return Responses.Unauthorized<EstadoModelOutput>(ErrorCode.ClaveDeCanalesInexistente.Code, ErrorCode.ClaveDeCanalesInexistente.ErrorDescription);

                    return Responses.Unauthorized<EstadoModelOutput>(ErrorCode.ClaveDeCanalesInactiva.Code, ErrorCode.ClaveDeCanalesInactiva.ErrorDescription);
                }
                var cantidadIntentosBloqueoDeClave = await _configuracionesV2Service.ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesAsync();

                var bloqueadaoExpirada = await ClaveCanalesBloquedaOExpirada(estadoModel.XCanal,
                                                           estadoModel.Body.DocumentNumber,
                                                           estadoModel.Body.DocumentTypeId,
                                                           usuarioRegistrado,
                                                           extendedInfoError,
                                                           cantidadIntentosBloqueoDeClave);

                if (bloqueadaoExpirada != string.Empty)
                {
                    if (bloqueadaoExpirada == ErrorCode.ClaveDeCanalesBloqueada.Code)
                        return Responses.Unauthorized<EstadoModelOutput>(ErrorCode.ClaveDeCanalesBloqueada.Code, ErrorCode.ClaveDeCanalesBloqueada.ErrorDescription);

                    return Responses.Unauthorized<EstadoModelOutput>(ErrorCode.ClaveDeCanalesExpirada.Code, ErrorCode.ClaveDeCanalesExpirada.ErrorDescription);
                }

                return Responses.Accepted(new EstadoModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(ClaveCanalesServiceEvents.ExceptionCallingObtenerEstado, ex.Message, ex);
                throw;
            }
        }

        private async Task<string> ClaveCanalesBloquedaOExpirada(
            string xCanal,
            string documentNumber,
            int documentTypeId,
            UsuarioRegistrado usuarioRegistrado, 
            string extendedInfoError, 
            int? cantidadIntentosBloqueoDeClave)
        {
            if (usuarioRegistrado.ForgotPasswordAttempts > cantidadIntentosBloqueoDeClave)
            {
                extendedInfoError += $"{JsonSerializer.Serialize(ErrorCode.ClaveDeCanalesBloqueada.Code)} : { ErrorCode.ClaveDeCanalesBloqueada.ErrorDescription} ";

                await SaveAuditLogAsync(extendedInfoError, EventResults.Error, xCanal);
                // Clave Canales Valida Bloqueada
                return ErrorCode.ClaveDeCanalesBloqueada.Code;
            }

            // Clave Canales valida Expirada o Correcta
            if (usuarioRegistrado.ExpiredDateTime < (await _helperDbServer.ObtenerFechaAsync()).Now)
            {
                extendedInfoError += $"{JsonSerializer.Serialize(ErrorCode.ClaveDeCanalesExpirada.Code)} : { ErrorCode.ClaveDeCanalesExpirada.ErrorDescription} ";
                await SaveAuditLogAsync(extendedInfoError, EventResults.Error, xCanal);

                return ErrorCode.ClaveDeCanalesExpirada.Code;
            }
            else
            {
                var extendedInfo = $"OK - DocumentTypeId: {documentTypeId} DocumentNumber: {documentNumber}";
                await SaveAuditLogAsync(JsonSerializer.Serialize(extendedInfo), EventResults.Ok, xCanal);

                return string.Empty;
            }
        }

        private async Task<string> ClaveCanalesInexistenteOInactiva(
            string xCanal, 
            UsuarioRegistrado usuarioRegistrado, 
            string extendedInfoError)
        {
            // Clave Canales Inexistente
            if (usuarioRegistrado == null)
            {
                extendedInfoError += $"{JsonSerializer.Serialize(ErrorCode.ClaveDeCanalesInexistente.Code)} : { ErrorCode.ClaveDeCanalesInexistente.ErrorDescription} ";

                await SaveAuditLogAsync(extendedInfoError, EventResults.Error, xCanal);

                return ErrorCode.ClaveDeCanalesInexistente.Code;
            }

            // Clave Canales Inactiva
            if (!usuarioRegistrado.Active)
            {
                extendedInfoError += $"{JsonSerializer.Serialize(ErrorCode.ClaveDeCanalesInactiva.Code)} : { ErrorCode.ClaveDeCanalesInactiva.ErrorDescription} ";

                await SaveAuditLogAsync(extendedInfoError, EventResults.Error, xCanal);

                return ErrorCode.ClaveDeCanalesInactiva.Code;
            }

            return string.Empty;
        }

        private async Task SaveAuditLogAsync(string extendedInfo, EventResults eventResults, string canal)
        {
            var dateTimeDbServerV2 = await _helperDbServerV2.ObtenerFechaAsync();
            await _auditoriaV2Repository.SaveAuditLogAsync(
                0,
                EventTypes.ChannelKey,
                eventResults,
                canal,
                dateTimeDbServerV2,
                extendedInfo
                );
        }

        public async Task<IResponse<InhabilitacionModelOutput>> InhabilitarAsync(IRequestBody<InhabilitacionModelInput> inhabilitacionModel)
        {
            try
            {
                var usuarioRegistrado = await _usuarioRegistradoRepository.ObtenerUsuarioRegistradoAsync(inhabilitacionModel.Body.DocumentTypeId, inhabilitacionModel.Body.DocumentNumber);

                // Clave Canales Inexistente
                if (usuarioRegistrado == null)
                {
                    return Responses.Unauthorized<InhabilitacionModelOutput>(ErrorCode.ClaveDeCanalesInexistente.Code, ErrorCode.ClaveDeCanalesInexistente.ErrorDescription);
                }

                // Clave Canales Inactiva
                if (!usuarioRegistrado.Active)
                {
                    return Responses.Unauthorized<InhabilitacionModelOutput>(ErrorCode.ClaveDeCanalesInactiva.Code, ErrorCode.ClaveDeCanalesInactiva.ErrorDescription);
                }

                var claveCanalesEncriptada = _encryption.EncryptChannelsKey(inhabilitacionModel.Body.ChannelKey, usuarioRegistrado.CardNumber);
                var claveCanalesValida = string.Equals(usuarioRegistrado.ChannelKey, claveCanalesEncriptada);

                // Clave Canales Incorrecta
                if (!claveCanalesValida)
                {
                    return Responses.Unauthorized<InhabilitacionModelOutput>(ErrorCode.ClaveDeCanalesIncorrecta.Code, ErrorCode.ClaveDeCanalesIncorrecta.ErrorDescription);
                }

                // procedemos a inhabilitar la clave canales
                usuarioRegistrado.Active = false;

                _usuarioRegistradoRepository.Update(usuarioRegistrado);
                _usuarioRegistradoRepository.SaveChanges();

                return Responses.Ok(new InhabilitacionModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(ClaveCanalesServiceEvents.ExceptionCallingInhabilitacion, ex.Message, ex);
                throw;
            }
        }
    }
}
