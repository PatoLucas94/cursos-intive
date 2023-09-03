using System;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Common.Configurations;

namespace Spv.Usuarios.DataAccess.ExternalWebService.Helpers
{
    /// <summary>
    /// ApiPersonaHelper: servicio para obtener configuraciones necesarias para interactuar con api-personas
    /// </summary>
    public class ApiPersonasHelper : IApiPersonasHelper
    {
        private readonly IOptions<ApiPersonasConfigurationsOptions> _apiPersonasConfigurations;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validChannelsConfigurationOptions"></param>
        public ApiPersonasHelper(IOptions<ApiPersonasConfigurationsOptions> validChannelsConfigurationOptions)
        {
            _apiPersonasConfigurations = validChannelsConfigurationOptions;
        }

        /// <summary>
        /// Obtener path base de recursos de Personas
        /// </summary>
        /// <returns></returns>
        public string PersonaBasePath()
        {
            return _apiPersonasConfigurations.Value.PersonaBasePath 
                   ?? throw new Exception($"No se encontró '{nameof(_apiPersonasConfigurations.Value.PersonaBasePath)}' key.");
        }

        /// <summary>
        /// Path del endpoint para obtener el identificador de la persona
        /// </summary>
        /// <returns></returns>
        public string PersonaPath()
        {
            return _apiPersonasConfigurations.Value.PersonaPath 
                   ?? throw new Exception($"No se encontró '{nameof(_apiPersonasConfigurations.Value.PersonaPath)}' key.");
        }

        /// <summary>
        /// Path del endpoint que retorna la información de la persona
        /// </summary>
        /// <returns></returns>
        public string PersonaInfoPath()
        {
            return _apiPersonasConfigurations.Value.PersonaInfoPath 
                   ?? throw new Exception($"No se encontró '{nameof(_apiPersonasConfigurations.Value.PersonaInfoPath)}' key.");
        }

        /// <summary>
        /// Path del endpoint que retorna la información de la persona física
        /// </summary>
        /// <returns></returns>
        public string PersonaFisicaInfoPath()
        {
            return _apiPersonasConfigurations.Value.PersonaFisicaInfoPath 
                   ?? throw new Exception($"No se encontró '{nameof(_apiPersonasConfigurations.Value.PersonaFisicaInfoPath)}' key.");
        }
    }
}
