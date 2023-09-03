using Microsoft.Extensions.Options;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    public class ApiSoftTokenHelper : BaseHelper, IApiSoftTokenHelper
    {
        private readonly IOptions<ApiSoftTokenConfigurationsOptions> _apiSoftTokenConfigurations;
        public ApiSoftTokenHelper(
            IOptions<ApiSoftTokenConfigurationsOptions> apiSoftTokenConfigurations,
            IOptions<ApiServicesHeadersConfigurationOptions> apiServicesConfigurations) : base(apiServicesConfigurations)
        {
            _apiSoftTokenConfigurations = apiSoftTokenConfigurations;
        }

        public string ApiSoftTokenxCanal()
        {
            return XCanal(ExternalServicesNames.ApiSoftToken);
        }

        public string ApiSoftTokenxUsuario()
        {
            return XUsuario(ExternalServicesNames.ApiSoftToken);
        }

        public string BasePath()
        {
            return _apiSoftTokenConfigurations.Value.BasePath ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiSoftTokenConfigurations.Value.BasePath),
                   ExternalServicesNames.ApiSoftToken);
        }

        public string TokenHabilitado()
        {
            return _apiSoftTokenConfigurations.Value.TokenHabilitado ??
                 ThrowNewExceptionPorFaltaDeKey(nameof(_apiSoftTokenConfigurations.Value.TokenHabilitado),
                     ExternalServicesNames.ApiSoftToken);
        }

        public string TokenValido()
        {
            return _apiSoftTokenConfigurations.Value.TokenValido ??
                 ThrowNewExceptionPorFaltaDeKey(nameof(_apiSoftTokenConfigurations.Value.TokenValido),
                     ExternalServicesNames.ApiSoftToken);
        }

        public string TokenIdentificador()
        {
            return _apiSoftTokenConfigurations.Value.TokenIdentificador ??
                 ThrowNewExceptionPorFaltaDeKey(nameof(_apiSoftTokenConfigurations.Value.TokenIdentificador),
                     ExternalServicesNames.ApiSoftToken);
        }
    }
}
