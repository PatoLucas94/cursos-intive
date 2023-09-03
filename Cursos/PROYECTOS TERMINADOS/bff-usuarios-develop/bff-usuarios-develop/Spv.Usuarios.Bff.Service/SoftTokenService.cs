using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Dtos.Client.SofttokenClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.SoftToken.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.SoftToken.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Interface;

namespace Spv.Usuarios.Bff.Service
{
    public class SoftTokenService : ISoftTokenService
    {
        private readonly ILogger<SoftTokenService> _logger;
        private readonly IApiSoftTokenRepository _softTokenRepository;

        public SoftTokenService(ILogger<SoftTokenService> logger, IApiSoftTokenRepository softTokenRepository)
        {
            _logger = logger;
            _softTokenRepository = softTokenRepository;
        }

        public async Task<IResponse<SoftTokenModelOutput>> SoftTokenHabilitadoAsync(IRequestBody<SoftTokenModelInput> softTokenModel)
        {
            try
            {
                var body = new ApiSoftTokenModelInput
                {
                    Identificador = softTokenModel.Body.Identificador
                };

                var softTokenHabilitado = await _softTokenRepository.TokenHabilitadoAsync(body);

                if (softTokenHabilitado == null)
                {
                    return Responses.NotFound<SoftTokenModelOutput>(ErrorCode.SoftTokenNoHabilitado.ErrorDescription);
                }

                var response = new SoftTokenModelOutput
                {
                  Bloqueado = softTokenHabilitado.bloqueado,
                  Identificador = softTokenHabilitado.identificador,
                  Detalle = softTokenHabilitado.detalle,
                  Estado = softTokenHabilitado.estado,
                };
              

                return Responses.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(SoftTokenServiceEvents.ExceptionCallingSoftTokenHabilitado, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<SoftTokenModelOutput>> SoftTokenValidoAsync(IRequestBody<SoftTokenValidoModelInput> softTokenModel)
        {
            try
            {
                var body = new ApiSoftTokenValidoModelInput
                {
                    Identificador = softTokenModel.Body.Identificador,
                    Token = softTokenModel.Body.Token
                };

                var softTokenValido = await _softTokenRepository.SoftTokenValidoAsync(body);

                if (softTokenValido.estado == "INVALIDO")
                {
                    return Responses.NotFound<SoftTokenModelOutput>(ErrorCode.SoftTokenInvalido.ErrorDescription);
                }

                if (softTokenValido.estado == "BLOQUEADO")
                {
                    return Responses.NotFound<SoftTokenModelOutput>(ErrorCode.SoftTokenBloqueado.ErrorDescription);
                }

                var response = new SoftTokenModelOutput
                {
                    Bloqueado = softTokenValido.bloqueado,
                    Identificador = softTokenValido.identificador,
                    Detalle = softTokenValido.detalle,
                    Estado = softTokenValido.estado,
                };


                return Responses.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(SoftTokenServiceEvents.ExceptionCallingSoftTokenValido, ex.Message, ex);
                throw;
            }
        }
    }
}
