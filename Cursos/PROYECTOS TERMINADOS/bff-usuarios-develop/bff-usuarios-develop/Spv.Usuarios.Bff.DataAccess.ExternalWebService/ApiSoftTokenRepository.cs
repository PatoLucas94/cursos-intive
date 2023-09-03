using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.SofttokenClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.SofttokenClient.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService
{
    public class ApiSoftTokenRepository : IApiSoftTokenRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiSoftTokenHelper _apiSoftTokenHelper;
        public ApiSoftTokenRepository(IHttpClientFactory httpClientFactory, IApiSoftTokenHelper apiSoftTokenHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiSoftTokenHelper = apiSoftTokenHelper;
        }

        private string ObtenerPathConBase(string path)
        {
            return string.Concat(_apiSoftTokenHelper.BasePath(), path);
        }

        public async Task<ApiSoftTokenModelOutput> TokenHabilitadoAsync(ApiSoftTokenModelInput body)
        {
            ApiSoftTokenModelOutput SoftToken;

            var identificador = _apiSoftTokenHelper.TokenIdentificador() != string.Empty ? _apiSoftTokenHelper.TokenIdentificador() : body.Identificador;

            var path = ObtenerPathConBase(string.Format(
                _apiSoftTokenHelper.TokenHabilitado()+
                identificador));

            var response = await GetApiSoftTokenAsync(path,_apiSoftTokenHelper.ApiSoftTokenxCanal(), _apiSoftTokenHelper.ApiSoftTokenxUsuario());

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                SoftToken = await JsonSerializer.DeserializeAsync<ApiSoftTokenModelOutput>(responseStream);
            }
            else
            {
                SoftToken = null;
            }

            return SoftToken;
        }

        public async Task<ApiSoftTokenModelOutput> SoftTokenValidoAsync(ApiSoftTokenValidoModelInput body)
        {
            var path = ObtenerPathConBase(_apiSoftTokenHelper.TokenValido());

            if (_apiSoftTokenHelper.TokenIdentificador() != string.Empty)
            {
                path = path.Replace("{0}", _apiSoftTokenHelper.TokenIdentificador());
            }
            else
            {
                path = path.Replace("{0}", body.Identificador);
            }

            var bodyPost = new { otp = body.Token };

            var response = await PostApiTyCAsync(path, bodyPost, _apiSoftTokenHelper.ApiSoftTokenxCanal(), _apiSoftTokenHelper.ApiSoftTokenxUsuario());

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<ApiSoftTokenModelOutput>(responseStream);

            return result;
        }

        private async Task<HttpResponseMessage> PostApiTyCAsync(string path, object body, string canal, string usuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);

            if (body != null)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body, new JsonSerializerOptions()), Encoding.UTF8, MediaTypeNames.Application.Json);
            }

            return await RequestApiSoftTokenAsync(request, canal, usuario);
        }

        private async Task<HttpResponseMessage> GetApiSoftTokenAsync(string path, string canal, string usuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            return await RequestApiSoftTokenAsync(request, canal, usuario);
        }

        private async Task<HttpResponseMessage> RequestApiSoftTokenAsync(HttpRequestMessage request, string canal, string usuario)
        {
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

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiSoftToken);

            return await client.SendAsync(request);
        }
    }
}
