using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Dtos.Service.ConfiguracionService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.ConfiguracionService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Interface;

namespace Spv.Usuarios.Bff.Service
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly ILogger<ConfiguracionService> _logger;
        private readonly IApiUsuariosRepositoryV2 _apiUsuariosRepositoryV2;
        public ConfiguracionService(
             ILogger<ConfiguracionService> logger,
             IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2
            )
        {
            _logger = logger;
            _apiUsuariosRepositoryV2 = apiUsuariosRepositoryV2;
        }

        public async Task<IResponse<LoginHabilitadoModelOutput>> LoginHabilitadoAsync(IRequestBody<LoginHabilitadoModelInput> LoginModel)
        {
            try
            {
                var LoginHabilitado = await _apiUsuariosRepositoryV2.ObtenerLoginHabilitadoAsync();

                if (LoginHabilitado == null)
                {
                    return Responses.NotFound<LoginHabilitadoModelOutput>(ErrorCode.LoginHabilitado.ErrorDescription);
                }
                var result = new LoginHabilitadoModelOutput();

                if (LoginHabilitado.habilitado == "0")
                {
                    result.Habilitado = LoginHabilitado.habilitado;
                    return Responses.Ok(result);
                }
                else if (LoginHabilitado.habilitado == "1" || LoginHabilitado.habilitado == "2")
                {
                    var LoginDefaultMessaje = await _apiUsuariosRepositoryV2.ObtenerMensajeLoginDeshabilitadoAsync();
                    if (string.IsNullOrEmpty(LoginDefaultMessaje.mensaje))
                    {
                        LoginDefaultMessaje = await _apiUsuariosRepositoryV2.ObtenerMensajeDefaultLoginDeshabilitadoAsync();
                    }
                    result.Mensaje = LoginDefaultMessaje.mensaje;
                    result.Habilitado = LoginHabilitado.habilitado;
                }
                else
                {
                    return Responses.NotFound<LoginHabilitadoModelOutput>(ErrorCode.LoginHabilitadoFueraDeRango.ErrorDescription);
                }

                return Responses.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ConfiguracionServiceEvents.ExceptionCallingLoginHabilitado, ex.Message, ex);
                throw;
            }
        }
    }
}
