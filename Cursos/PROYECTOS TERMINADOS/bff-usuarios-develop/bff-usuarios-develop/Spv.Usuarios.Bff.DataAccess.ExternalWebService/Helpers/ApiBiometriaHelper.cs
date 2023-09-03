using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    /// <summary>
    /// ApiBiometriaHelper
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiBiometriaHelper : BaseHelper, IApiBiometriaHelper
    {
        private readonly ApiBiometriaConfigurationsOptions _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// ApiBiometriaHelper
        /// </summary>
        /// <param name="configurationOptions"></param>
        /// <param name="apiServicesConfigurations"></param>
        public ApiBiometriaHelper(
            IOptions<ApiBiometriaConfigurationsOptions> configurationOptions,
            IOptions<ApiServicesHeadersConfigurationOptions> apiServicesConfigurations,
            IHttpClientFactory httpClientFactory
        ) : base(apiServicesConfigurations)
        {
            _configuration = configurationOptions.Value;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Canal
        /// </summary>
        /// <returns></returns>
        public string XCanal() => base.XCanal(ExternalServicesNames.ApiBiometria);

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario 
        /// </summary>
        /// <returns></returns>
        public string XUsuario() => base.XUsuario(ExternalServicesNames.ApiBiometria);

        /// <summary>
        /// Obtener valor correspondiente al header X-Aplicacion 
        /// </summary>
        /// <returns></returns>
        public string XAplicacion() => base.XAplicacion(ExternalServicesNames.ApiBiometria);

        /// <summary>
        /// Base Path
        /// </summary>
        /// <returns></returns>
        public string BasePath() => _configuration.BasePath ?? ThrowNewExceptionPorFaltaDeKey(
            nameof(_configuration.BasePath),
            ExternalServicesNames.ApiBiometria
        );

        /// <summary>
        /// Obtener path de endpoint de autenticación
        /// </summary>
        /// <returns></returns>
        public string AutenticacionPath() => _configuration.Autenticacion ?? ThrowNewExceptionPorFaltaDeKey(
            nameof(_configuration.Autenticacion),
            ExternalServicesNames.ApiBiometria
        );

        public async Task<HttpResponseMessage> PostRequestAsync<T>(
            string path,
            T body,
            string canal = null,
            string usuario = null,
            string aplicacion = null
        ) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);

            if (body != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json
                );
            }

            #region Requerido para UTs

            if (!string.IsNullOrWhiteSpace(canal))
            {
                request.Headers.Add(HeaderNames.ChannelHeaderName, canal);
            }

            if (!string.IsNullOrWhiteSpace(usuario))
            {
                request.Headers.Add(HeaderNames.UserHeaderName, usuario);
            }

            if (!string.IsNullOrWhiteSpace(aplicacion))
            {
                request.Headers.Add(HeaderNames.ApplicationHeaderName, aplicacion);
            }

            #endregion

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiBiometria);

            return await client.SendAsync(request);
        }
    }
}
