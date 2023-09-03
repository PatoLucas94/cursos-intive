using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService
{
    public class ApiUsuariosRepository : IApiUsuariosRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiUsuariosHelper _apiUsuariosHelper;

        public ApiUsuariosRepository(IHttpClientFactory httpClientFactory, IApiUsuariosHelper apiUsuariosHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiUsuariosHelper = apiUsuariosHelper;
        }

        public async Task<ApiUsuariosPerfilModelOutput> ObtenerPerfilAsync(string nombreUsuario)
        {
            ApiUsuariosPerfilModelOutput usuarioPerfil;

            var path = ObtenerPathConBase(_apiUsuariosHelper.PerfilPath());

            path = path.Replace("{0}", nombreUsuario);

            var response = await GetRequestApiUsuarioAsync(
                path,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion());

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                usuarioPerfil = await JsonSerializer.DeserializeAsync<ApiUsuariosPerfilModelOutput>(responseStream);
            }
            else
            {
                usuarioPerfil = null;
            }

            return usuarioPerfil;
        }
        public async Task<HttpResponseMessage> ValidarExistenciaAsync(ApiUsuariosValidacionExistenciaHbiModelInput body)
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.ValidarExistenciaHbiPath());

            return await _apiUsuariosHelper.PostRequestApiUsuariosAsync(
                path,
                body,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion());
        }

        private async Task<HttpResponseMessage> GetRequestApiUsuarioAsync(
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

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiUsuarios);
            
            return await client.SendAsync(request);
        }

        private string ObtenerPathConBase(string path)
        {
            return string.Concat(_apiUsuariosHelper.BasePath(), path);
        }
    }
}
