using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Common.Configurations;
using Spv.Usuarios.Common.Constants;

namespace Spv.Usuarios.DataAccess.ExternalWebService.Helpers
{
    public class BtaHelper : IBtaHelper
    {
        private readonly IOptions<BtaConfigurationsOptions> _BtaConfigurations;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validChannelsConfigurationOptions"></param>
        public BtaHelper(IHttpClientFactory httpClientFactory, IOptions<BtaConfigurationsOptions> validChannelsConfigurationOptions)
        {
            _httpClientFactory = httpClientFactory;
            _BtaConfigurations = validChannelsConfigurationOptions;
        }

        /// <summary>
        /// Obtener el UserPassword
        /// </summary>
        /// <returns></returns>
        public string UserPassword()
        {
            return _BtaConfigurations.Value.UserPassword
                    ?? throw new Exception($"No se encontró '{nameof(_BtaConfigurations.Value.UserPassword)}' key.");
        }

        /// <summary>
        /// Obtener path base de BTA
        /// </summary>
        /// <returns></returns>
        public string BtaBasePath()
        {
            return _BtaConfigurations.Value.BtaBasePath
                   ?? throw new Exception($"No se encontró '{nameof(_BtaConfigurations.Value.BtaBasePath)}' key.");
        }

        /// <summary>
        /// Path del endpoint para obtener el Token
        /// </summary>
        /// <returns></returns>
        public string TokenBtaPath()
        {
            return _BtaConfigurations.Value.TokenBtaPath
                   ?? throw new Exception($"No se encontró '{nameof(_BtaConfigurations.Value.TokenBtaPath)}' key.");
        }

        /// <summary>
        /// Path del endpoint para obtener pin
        /// </summary>
        /// <returns></returns>
        public string ObtenerPinPath()
        {
            return _BtaConfigurations.Value.ObtenerPinPath
                   ?? throw new Exception($"No se encontró '{nameof(_BtaConfigurations.Value.ObtenerPinPath)}' key.");
        }

        public async Task<HttpResponseMessage> PostAsync(string requestPath, object body)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestPath);

            if (body != null)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body, new JsonSerializerOptions()),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json
                );
            }

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.BTA);

            return await client.SendAsync(request);
        }
    }
}
