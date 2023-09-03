using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService
{
    public class ApiNotificacionesRepository : IApiNotificacionesRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiNotificacionesHelper _apiNotificacionesHelper;

        public ApiNotificacionesRepository(
            IHttpClientFactory httpClientFactory,
            IApiNotificacionesHelper apiNotificacionesHelper
            )
        {
            _httpClientFactory = httpClientFactory;
            _apiNotificacionesHelper = apiNotificacionesHelper;
        }

        public async Task<HttpResponseMessage> CrearYEnviarTokenAsync(
            ApiNotificacionesCrearYEnviarTokenModelInput body)
        {
            var path = ObtenerPathConBase(_apiNotificacionesHelper.CrearYEnviarTokenPath());
            return await PostRequestApiNotificacionesAsync(
                path, 
                body, 
                _apiNotificacionesHelper.ApiNotificacionesXCanal(), 
                _apiNotificacionesHelper.ApiNotificacionesXUsuario());
        }

        public async Task<HttpResponseMessage> ValidarTokenAsync(
            ApiNotificacionesValidarTokenModelInput body)
        {
            var path = ObtenerPathConBase(_apiNotificacionesHelper.ValidarTokenPath());
            return await PostRequestApiNotificacionesAsync(
                path,
                body,
                _apiNotificacionesHelper.ApiNotificacionesXCanal(),
                _apiNotificacionesHelper.ApiNotificacionesXUsuario());
        }

        public async Task<HttpResponseMessage> EnviarEmailAsync(ApiNotificacionesEnviarEmailModelInput body)
        {
            var path = ObtenerPathConBase(_apiNotificacionesHelper.EnviarEmailPath());
            return await PostRequestApiNotificacionesAsync(
                path,
                body,
                _apiNotificacionesHelper.ApiNotificacionesXCanal(),
                _apiNotificacionesHelper.ApiNotificacionesXUsuario());
        }

        private string ObtenerPathConBase(string path)
        {
            return string.Concat(_apiNotificacionesHelper.BasePath(), path);
        }

        private async Task<HttpResponseMessage> PostRequestApiNotificacionesAsync(
            string path,
            object body,
            string canal = null,
            string usuario = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);

            if (body != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(body, new JsonSerializerOptions()), Encoding.UTF8, MediaTypeNames.Application.Json);
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

            #endregion

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiNotificaciones);
            var segundos = _apiNotificacionesHelper.TiempoRespuestaEnvioMails();
            client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(segundos));
            return await client.SendAsync(request);

    }
    }
}