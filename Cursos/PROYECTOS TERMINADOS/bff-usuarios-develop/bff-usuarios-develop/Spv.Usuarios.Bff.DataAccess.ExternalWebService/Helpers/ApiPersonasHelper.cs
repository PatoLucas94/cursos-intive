using System;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    /// <summary>
    /// ApiPersonaHelper: servicio para obtener configuraciones necesarias para interactuar con api-personas
    /// </summary>
    public class ApiPersonasHelper : BaseHelper, IApiPersonasHelper
    {
        private readonly ApiPersonasConfigurationsOptions _apiPersonasConfigurations;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiPersonasConfigurations"></param>
        /// <param name="apiServicesConfigurations"></param>
        public ApiPersonasHelper(
            IOptions<ApiPersonasConfigurationsOptions> apiPersonasConfigurations,
            IOptions<ApiServicesHeadersConfigurationOptions> apiServicesConfigurations
        ) : base(apiServicesConfigurations)
        {
            _apiPersonasConfigurations = apiPersonasConfigurations.Value;
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-personas
        /// </summary>
        /// <returns></returns>
        public string ApiPersonasXCanal()
        {
            return XCanal(ExternalServicesNames.ApiPersonas);
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-personas
        /// </summary>
        /// <returns></returns>
        public string ApiPersonasXUsuario()
        {
            return XUsuario(ExternalServicesNames.ApiPersonas);
        }

        /// <summary>
        /// Base Path del endpoint api-personas
        /// </summary>
        /// <returns></returns>
        public string BasePath()
        {
            return _apiPersonasConfigurations.BasePath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiPersonasConfigurations.BasePath),
                ExternalServicesNames.ApiPersonas
            );
        }

        /// <summary>
        /// Path del endpoint para obtener el identificador de la persona
        /// </summary>
        /// <returns></returns>
        public string PersonasPath()
        {
            return _apiPersonasConfigurations.PersonasPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiPersonasConfigurations.PersonasPath),
                ExternalServicesNames.ApiPersonas
            );
        }

        /// <summary>
        /// Path del endpoint que retorna la información de la persona
        /// </summary>
        /// <returns></returns>
        public string PersonasInfoPath()
        {
            return _apiPersonasConfigurations.PersonasInfoPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiPersonasConfigurations.PersonasInfoPath),
                ExternalServicesNames.ApiPersonas
            );
        }

        /// <summary>
        /// Path del endpoint que retorna la información de la persona física
        /// </summary>
        /// <returns></returns>
        public string PersonasFisicaInfoPath()
        {
            return _apiPersonasConfigurations.PersonasFisicaInfoPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiPersonasConfigurations.PersonasFisicaInfoPath),
                ExternalServicesNames.ApiPersonas
            );
        }

        /// <summary>
        /// Path de endpoint para obtener el identificador de la persona filtrando por número de documento
        /// </summary>
        /// <returns></returns>
        public string PersonasFiltroPath()
        {
            return _apiPersonasConfigurations.PersonasFiltroPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiPersonasConfigurations.PersonasFiltroPath),
                ExternalServicesNames.ApiPersonas
            );
        }

        /// <summary>
        /// Obtener path de endpoint de teléfonos en api-personas
        /// </summary>
        /// <returns></returns>
        public string TelefonosPath()
        {
            return _apiPersonasConfigurations.TelefonosPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiPersonasConfigurations.TelefonosPath),
                ExternalServicesNames.ApiPersonas
            );
        }

        /// <summary>
        /// Obtener path de endpoint para creación de un teléfono nuevo en api-personas
        /// </summary>
        /// <returns></returns>
        public string TelefonosCreacionPath()
        {
            return TelefonosPath();
        }

        /// <summary>
        /// Obtener path de endpoint de actualización de teléfonos en api-personas
        /// </summary>
        /// <returns></returns>
        public string TelefonosActualizacionPath()
        {
            return TelefonosPath() + IdParam(1);
        }

        /// <summary>
        /// Path de endpoint que retorna los teléfonos con doble factor de la persona
        /// </summary>
        /// <returns></returns>
        public string TelefonosDobleFactorPath()
        {
            return _apiPersonasConfigurations.TelefonosDobleFactorPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiPersonasConfigurations.TelefonosDobleFactorPath),
                ExternalServicesNames.ApiPersonas
            );
        }

        /// <summary>
        /// Obtener path de endpoint para creación de un teléfono doble factor nuevo en api-personas
        /// </summary>
        /// <returns></returns>
        public string TelefonosDobleFactorCreacionPath()
        {
            return TelefonosDobleFactorPath();
        }

        /// <summary>
        /// Obtener path de endpoint para actualización de un teléfono doble factor en api-personas
        /// </summary>
        /// <returns></returns>
        public string TelefonosDobleFactorActualizacionPath()
        {
            return TelefonosDobleFactorPath();
        }

        /// <summary>
        /// Obtener path de endpoint para verificación de un teléfono en api-personas
        /// </summary>
        /// <returns></returns>
        public string TelefonosVerificacionPath()
        {
            return _apiPersonasConfigurations.TelefonosVerificacionPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiPersonasConfigurations.TelefonosVerificacionPath),
                ExternalServicesNames.ApiPersonas
            );
        }

        /// <summary>
        /// Obtener path de endpoint para actualización de un email nuevo en api-personas
        /// </summary>
        /// <returns></returns>
        public string EmailsActualizacionPath()
        {
            return EmailsCreacionPath() + IdParam(1);
        }

        /// <summary>
        /// Obtener path de endpoint para creación de un email nuevo en api-personas
        /// </summary>
        /// <returns></returns>
        public string EmailsCreacionPath()
        {
            return _apiPersonasConfigurations.EmailsPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiPersonasConfigurations.EmailsPath),
                ExternalServicesNames.ApiPersonas
            );
        }

        /// <summary>
        /// Obtener path de endpoint para marca cliente en api-personas-productos
        /// </summary>
        /// <returns></returns>
        public string ProductosMarcaClientePath()
        {
            return _apiPersonasConfigurations.ProductosMarcaClientePath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiPersonasConfigurations.ProductosMarcaClientePath),
                ExternalServicesNames.ApiPersonas
            );
        }

        public DateTimeOffset ExpirationCache() => DateTimeOffset.Now.AddMinutes(
            _apiPersonasConfigurations.CacheExpiracionTyCMinutos
        );

        /// <summary>
        /// Agregar parámetro a path
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string IdParam(int index)
        {
            return "/{" + index + "}";
        }
    }
}
