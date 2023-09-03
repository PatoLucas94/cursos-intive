using Microsoft.Extensions.Options;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    public class ApiCatalogoHelper : BaseHelper, IApiCatalogoHelper
    {
        private readonly IOptions<ApiCatalogoConfigurationsOptions> _apiCatalogoConfigurations;
        

        public ApiCatalogoHelper(
            IOptions<ApiCatalogoConfigurationsOptions> apiCatalogoConfigurations,
            IOptions<ApiServicesHeadersConfigurationOptions> apiServicesConfigurations) : base(apiServicesConfigurations)
        {
            _apiCatalogoConfigurations = apiCatalogoConfigurations;
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-catalogo
        /// </summary>
        /// <returns></returns>
        public string ApiCatalogoXCanal()
        {
            return XCanal(ExternalServicesNames.ApiCatalogo);
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-catalogo
        /// </summary>
        /// <returns></returns>
        public string ApiCatalogoXUsuario()
        {
            return XUsuario(ExternalServicesNames.ApiCatalogo);
        }

        public string BasePath()
        {
            return _apiCatalogoConfigurations.Value.BasePath ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiCatalogoConfigurations.Value.BasePath),
                   ExternalServicesNames.ApiCatalogo);
        }

        public string PaisesPath()
        {
            return _apiCatalogoConfigurations.Value.PaisesPath ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiCatalogoConfigurations.Value.PaisesPath),
                   ExternalServicesNames.ApiCatalogo);
        }

        public string TiposDocumentoPath()
        {
            return _apiCatalogoConfigurations.Value.TiposDocumentoPath ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiCatalogoConfigurations.Value.TiposDocumentoPath),
                   ExternalServicesNames.ApiCatalogo);
        }
    }
}
