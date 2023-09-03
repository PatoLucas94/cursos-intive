using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    public class ConfiguracionHelper : IConfiguracionHelper
    {
        private readonly ConfiguracionConfigurationOptions _apiUsuariosConfigurations;
        private readonly IHttpClientFactory _httpClientFactory;
        public ConfiguracionHelper(IOptions<ConfiguracionConfigurationOptions> apiUsuariosConfigurationOptions,
            IHttpClientFactory httpClientFactory
        )
        {
            _apiUsuariosConfigurations = apiUsuariosConfigurationOptions.Value;
            _httpClientFactory = httpClientFactory;
        }

        public string TYCHabilitadoPath()
        {
            return _apiUsuariosConfigurations.TYCHabilitadoPath ??
               ThrowNewExceptionPorFaltaDeKey(
                   nameof(_apiUsuariosConfigurations.TYCHabilitadoPath),
                   ExternalServicesNames.Configuraciones
               );
        }

        public string LoginHabilitadoPath()
        {
            return _apiUsuariosConfigurations.LoginHabilitadoPath ??
               ThrowNewExceptionPorFaltaDeKey(
                   nameof(_apiUsuariosConfigurations.LoginHabilitadoPath),
                   ExternalServicesNames.Configuraciones
               );
        }

        public string MensajeDefaultLoginDeshabilitadoPath()
        {
            return _apiUsuariosConfigurations.MensajeDefaultLoginDeshabilitadoPath ??
               ThrowNewExceptionPorFaltaDeKey(
                   nameof(_apiUsuariosConfigurations.MensajeDefaultLoginDeshabilitadoPath),
                   ExternalServicesNames.Configuraciones
               );
        }

        public string MensajeLoginDeshabilitadoPath()
        {
            return _apiUsuariosConfigurations.MensajeLoginDeshabilitadoPath ??
               ThrowNewExceptionPorFaltaDeKey(
                   nameof(_apiUsuariosConfigurations.MensajeLoginDeshabilitadoPath),
                   ExternalServicesNames.Configuraciones
               );
        }

        public async Task<HttpResponseMessage> GetRequestApiUsuarioAsync(
            string path,
            string canal,
            string usuario,
            string aplicacion)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);

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

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.Configuraciones);

            return await client.SendAsync(request);
        }

        private static string ThrowNewExceptionPorFaltaDeKey(string key, string serviceName)
        {
            var message = string.Format(ErrorConstants.NoSeEncontroKey, $"{serviceName}:{key}");
            throw new ArgumentNullException(message, new Exception(message));
        }
    }
}
