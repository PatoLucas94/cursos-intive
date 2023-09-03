using Microsoft.Extensions.Options;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    public class ApiScoreOperacionesHelper : BaseHelper, IApiScoreOperacionesHelper
    {

        private readonly IOptions<ApiScoreOperacionesConfigurationsOptions> _apiScoreOperacionesConfigurations;
        public ApiScoreOperacionesHelper(
            IOptions<ApiScoreOperacionesConfigurationsOptions> apiScoreOperacionesConfigurations,
            IOptions<ApiServicesHeadersConfigurationOptions> apiServicesConfigurations) : base(apiServicesConfigurations)
        {
            _apiScoreOperacionesConfigurations = apiScoreOperacionesConfigurations;
        }

        public string ApiScoreOperacionesxCanal()
        {
            return XCanal(ExternalServicesNames.ApiScoreOperaciones);
        }

        public string ApiScoreOperacionesxUsuario()
        {
            return XUsuario(ExternalServicesNames.ApiScoreOperaciones);
        }

        public string BasePath()
        {
            return _apiScoreOperacionesConfigurations.Value.BasePath ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiScoreOperacionesConfigurations.Value.BasePath),
                   ExternalServicesNames.ApiScoreOperaciones);
        }

        public string IniciarSesionScore()
        {
            return _apiScoreOperacionesConfigurations.Value.IniciarSesionScore ??
                    ThrowNewExceptionPorFaltaDeKey(nameof(_apiScoreOperacionesConfigurations.Value.IniciarSesionScore),
                        ExternalServicesNames.ApiScoreOperaciones);
        }

        public string RegistracionScore()
        {
            return _apiScoreOperacionesConfigurations.Value.RegistracionScore ??
                      ThrowNewExceptionPorFaltaDeKey(nameof(_apiScoreOperacionesConfigurations.Value.RegistracionScore),
                          ExternalServicesNames.ApiScoreOperaciones);
        }

        public string UpdateCredentials()
        {
            return _apiScoreOperacionesConfigurations.Value.UpdateCredentials ??
                      ThrowNewExceptionPorFaltaDeKey(nameof(_apiScoreOperacionesConfigurations.Value.UpdateCredentials),
                          ExternalServicesNames.ApiScoreOperaciones);
        }
    }
}
