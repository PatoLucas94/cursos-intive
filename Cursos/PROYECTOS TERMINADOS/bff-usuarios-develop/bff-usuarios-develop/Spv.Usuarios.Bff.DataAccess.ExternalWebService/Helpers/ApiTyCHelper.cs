using System;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    /// <summary>
    /// ApiTyCHelper: servicio para obtener configuraciones necesarias para interactuar con api-tyc
    /// </summary>
    public class ApiTyCHelper : BaseHelper, IApiTyCHelper
    {
        private readonly ApiTyCConfigurationsOptions _apiTyCConfigurations;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiTyCConfigurations"></param>
        /// <param name="apiServicesConfigurations"></param>
        public ApiTyCHelper(
            IOptions<ApiTyCConfigurationsOptions> apiTyCConfigurations,
            IOptions<ApiServicesHeadersConfigurationOptions> apiServicesConfigurations
        ) : base(apiServicesConfigurations)
        {
            _apiTyCConfigurations = apiTyCConfigurations.Value;
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-tyc
        /// </summary>
        /// <returns></returns>
        public string ApiTyCxCanal()
        {
            return XCanal(ExternalServicesNames.ApiTyC);
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-tyc
        /// </summary>
        /// <returns></returns>
        public string ApiTyCxUsuario()
        {
            return XUsuario(ExternalServicesNames.ApiTyC);
        }

        /// <summary>
        /// Base Path del endpoint api-tyc
        /// </summary>
        /// <returns></returns>
        public string BasePath()
        {
            return _apiTyCConfigurations.BasePath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiTyCConfigurations.BasePath),
                ExternalServicesNames.ApiTyC
            );
        }

        /// <summary>
        /// Obtener el concepto de registración para los términos y condiciones vigente de api-tyc
        /// </summary>
        /// <returns></returns>
        public string ConceptoRegistracion()
        {
            return _apiTyCConfigurations.ConceptoRegistracion ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiTyCConfigurations.ConceptoRegistracion),
                ExternalServicesNames.ApiTyC
            );
        }

        /// <summary>
        /// Obtener path de endpoint aceptar los términos y condiciones de api-tyc
        /// </summary>
        /// <returns></returns>
        public string TerminosYCondicionesAceptadosPath()
        {
            return _apiTyCConfigurations.TerminosYCondicionesAceptadosPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiTyCConfigurations.TerminosYCondicionesAceptadosPath),
                ExternalServicesNames.ApiTyC
            );
        }

        /// <summary>
        /// Obtener path de endpoint para obtener los términos y condiciones vigente a partir de api-tyc
        /// </summary>
        /// <returns></returns>
        public string TerminosYCondicionesVigentePath()
        {
            return _apiTyCConfigurations.TerminosYCondicionesVigentePath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiTyCConfigurations.TerminosYCondicionesVigentePath),
                ExternalServicesNames.ApiTyC
            );
        }

        public DateTimeOffset ExpirationCache() => DateTimeOffset.Now.AddMinutes(
            _apiTyCConfigurations.CacheExpiracionTyCMinutos
        );
    }
}
