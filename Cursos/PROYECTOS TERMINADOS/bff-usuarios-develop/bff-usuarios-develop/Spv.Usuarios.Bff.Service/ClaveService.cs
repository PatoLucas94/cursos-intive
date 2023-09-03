using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.ClaveCliente.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Helpers;
using Spv.Usuarios.Bff.Service.Interface;

namespace Spv.Usuarios.Bff.Service
{
    public class ClaveService : IClaveService
    {
        private readonly ILogger<ClaveService> _logger;
        private readonly IApiUsuariosRepositoryV2 _usuariosRepositoryV2;
        private readonly IApiNotificacionesRepository _notificacionesRepository;
        private readonly IApiPersonasRepository _personasRepository;

        public ClaveService(
            ILogger<ClaveService> logger,
            IApiUsuariosRepositoryV2 usuariosRepositoryV2,
            IApiPersonasRepository personasRepository,
            IApiNotificacionesRepository notificacionesRepository)
        {
            _logger = logger;
            _usuariosRepositoryV2 = usuariosRepositoryV2;
            _notificacionesRepository = notificacionesRepository;
            _personasRepository = personasRepository;
        }

        public async Task<IResponse<ValidacionClaveCanalesModelOutput>> ValidarClaveCanalesAsync(
            IRequestBody<ValidacionClaveCanalesModelInput> validacionModel)
        {
            try
            {
                var body = new ApiUsuariosValidacionClaveCanalesModelInput
                {
                    clave_canales = validacionModel.Body.ChannelKey,
                    id_tipo_documento = validacionModel.Body.DocumentTypeId,
                    nro_documento = validacionModel.Body.DocumentNumber
                };

                var response = await _usuariosRepositoryV2.ValidarClaveCanalesAsync(body);

                return response.IsSuccessStatusCode
                    ? Responses.Accepted(new ValidacionClaveCanalesModelOutput())
                    : await ProcessExternalError<ValidacionClaveCanalesModelOutput>.ProcessApiUsuariosErrorResponse(
                        response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ClaveServiceEvents.ExceptionCallingValidacionClaveCanales, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<ValidacionClaveCanalesModelOutput>> ValidarClaveCanalesIdPersonaAsync(
            IRequestBody<ValidacionClaveCanalesIdPersonaModelInput> validacionModel)
        {
            try
            {
                var idPersona = 0;
                int.TryParse(validacionModel.Body.PersonId, out idPersona);
                var persona = await PersonasHelper.GetInfoPersonaFisicaAsync(_personasRepository, idPersona);

                var body = new ApiUsuariosValidacionClaveCanalesModelInput
                {
                    clave_canales = validacionModel.Body.ChannelKey,
                    id_tipo_documento = persona.TipoDocumento,
                    nro_documento = persona.NumeroDocumento
                };

                var response = await _usuariosRepositoryV2.ValidarClaveCanalesAsync(body);

                return response.IsSuccessStatusCode
                    ? Responses.Accepted(new ValidacionClaveCanalesModelOutput())
                    : await ProcessExternalError<ValidacionClaveCanalesModelOutput>.ProcessApiUsuariosErrorResponse(
                        response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ClaveServiceEvents.ExceptionCallingValidacionClaveCanales, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<GeneracionClaveSmsModelOutput>> GenerarClaveSmsAsync(
            IRequestBody<GeneracionClaveSmsModelInput> generacionModel)
        {
            try
            {
                var guid = Guid.NewGuid().ToString();
                var body = new ApiNotificacionesCrearYEnviarTokenModelInput
                {
                    id_persona = long.Parse(generacionModel.Body.PersonId),
                    identificador = guid,
                    destinatario = new Destinatario
                    {
                        medio = AppConstants.ClaveSmsDestinatarioMedio,
                        numero = generacionModel.Body.Telefono
                    },
                    template_id = AppConstants.ClaveSmsTemplateId
                };

                var creacionTokenResponse = await _notificacionesRepository.CrearYEnviarTokenAsync(body);

                if (!creacionTokenResponse.IsSuccessStatusCode)
                {
                    return await ProcessExternalError<GeneracionClaveSmsModelOutput>
                        .ProcessApiNotificacionesErrorResponse(creacionTokenResponse);
                }

                var responseStream = await creacionTokenResponse.Content.ReadAsStreamAsync();
                var crearYEnviarTokenOutput =
                    await JsonSerializer
                        .DeserializeAsync<ApiNotificacionesCrearYEnviarTokenModelOutput>(responseStream);

                if (!(crearYEnviarTokenOutput.estado == ApiNotificacionesHelper.CreacionTokenSmsEstadoEnviado ||
                      crearYEnviarTokenOutput.estado == ApiNotificacionesHelper.CreacionTokenSmsEstadoPendienteEnvio)
                   )
                {
                    return await ProcessExternalError<GeneracionClaveSmsModelOutput>
                        .ProcessApiNotificacionesErrorResponse(creacionTokenResponse);
                }

                // Retornamos el guid generado como identificador ya que es la referencia al token generado
                return Responses.Accepted(new GeneracionClaveSmsModelOutput
                {
                    Identificador = guid
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ClaveServiceEvents.ExceptionCallingGeneracionClaveSms, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<ValidacionClaveSmsModelOutput>> ValidarClaveSmsAsync(
            IRequestBody<ValidacionClaveSmsModelInput> validacionModel)
        {
            try
            {
                var body = new ApiNotificacionesValidarTokenModelInput
                {
                    id_persona = long.Parse(validacionModel.Body.PersonId),
                    identificador = validacionModel.Body.Identificador,
                    token = validacionModel.Body.ClaveSms
                };

                var validarTokenResponse = await _notificacionesRepository.ValidarTokenAsync(body);

                if (!validarTokenResponse.IsSuccessStatusCode)
                {
                    return await ProcessExternalError<ValidacionClaveSmsModelOutput>
                        .ProcessApiNotificacionesErrorResponse(validarTokenResponse);
                }

                await using var responseStream = await validarTokenResponse.Content.ReadAsStreamAsync();
                var validarTokenOutput =
                    await JsonSerializer.DeserializeAsync<ApiNotificacionesValidarTokenModelOutput>(responseStream);

                return !(validarTokenOutput is { estado: ApiNotificacionesHelper.ValidacionTokenSmsEstadoUtilizado })
                    ? Responses.Unauthorized<ValidacionClaveSmsModelOutput>(ErrorCode.ClaveSmsIncorrecta.Code,
                        ErrorCode.ClaveSmsIncorrecta.ErrorDescription)
                    : Responses.Accepted(new ValidacionClaveSmsModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(ClaveServiceEvents.ExceptionCallingValidacionClaveSms, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<EstadoModelOutput>> ObtenerEstadoAsync(IRequestBody<EstadoModelInput> estadoModel)
        {
            try
            {
                var body = new ApiUsuariosObtenerEstadoClaveCanalesModelInput
                {
                    id_tipo_documento = estadoModel.Body.DocumentTypeId,
                    nro_documento = estadoModel.Body.DocumentNumber
                };

                var response = await _usuariosRepositoryV2.ObtenerEstadoClaveCanalesAsync(body);

                return response.IsSuccessStatusCode
                    ? Responses.Accepted(new EstadoModelOutput())
                    : await ProcessExternalError<EstadoModelOutput>.ProcessApiUsuariosErrorResponse(
                        response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ClaveServiceEvents.ExceptionCallingObtenerEstado, ex.Message, ex);
                throw;
            }
        }
    }
}
