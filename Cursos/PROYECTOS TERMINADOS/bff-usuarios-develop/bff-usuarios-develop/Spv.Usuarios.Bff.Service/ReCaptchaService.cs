using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.GoogleClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.ReCaptchaService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.ReCaptchaService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Interface;

namespace Spv.Usuarios.Bff.Service
{
    public class ReCaptchaService : IReCaptchaService
    {
        private readonly ILogger<ReCaptchaService> _logger;
        private readonly IApiGoogleRepository _googleRepository;

        public ReCaptchaService(ILogger<ReCaptchaService> logger, IApiGoogleRepository googleRepository)
        {
            _logger = logger;
            _googleRepository = googleRepository;
        }

        public async Task<IResponse<ReCaptchaValidacionModelOutput>> ValidarAsync(IRequestBody<ReCaptchaValidacionModelInput> validacionCaptchaModel)
        {
            try
            {
                var response = await _googleRepository.ReCaptchaV3ValidarTokenAsync(validacionCaptchaModel.Body.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return Responses.InternalServerError<ReCaptchaValidacionModelOutput>(new Exception(MessageConstants.ApiGoogleErrorGenerico));
                }

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var validationOutput = await JsonSerializer.DeserializeAsync<ApiGoogleValidarTokenCaptchaV3ModelOutput>(responseStream);

                if (!validationOutput.success)
                {
                    return Responses.Unauthorized<ReCaptchaValidacionModelOutput>(
                        ErrorCode.ReCaptchaValidacionFallida.ErrorDescription,
                        ErrorCode.ReCaptchaValidacionFallida.Code);
                }

                if(validationOutput.action != validacionCaptchaModel.Body.Action)
                {
                    return Responses.Unauthorized<ReCaptchaValidacionModelOutput>(
                        ErrorCode.ReCaptchaActionInvalido.ErrorDescription,
                        ErrorCode.ReCaptchaActionInvalido.Code);
                }

                var finalResponse = new ReCaptchaValidacionModelOutput() { Score = validationOutput.score, Success = validationOutput.success };

                return Responses.Ok(finalResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(CaptchaServiceEvents.ExceptionCallingValidacionCaptcha, ex.Message, ex);
                throw;
            }
        }
    }
}
