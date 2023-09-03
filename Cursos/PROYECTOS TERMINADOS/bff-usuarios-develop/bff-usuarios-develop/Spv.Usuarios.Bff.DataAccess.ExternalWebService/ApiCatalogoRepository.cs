using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.CatalogoClient.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService
{
    public class ApiCatalogoRepository : IApiCatalogoRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiCatalogoHelper _apiCatalogoHelper;

        public ApiCatalogoRepository(IHttpClientFactory httpClientFactory, IApiCatalogoHelper apiCatalogoHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiCatalogoHelper = apiCatalogoHelper;
        }

        public async Task<List<ApiCatalogoPaisesModelOutput>> ObtenerPaisesAsync()
        {
            List<ApiCatalogoPaisesModelOutput> paises;

            var path = ObtenerPathConBase(_apiCatalogoHelper.PaisesPath());

            var response = await RequestApiCatalogoAsync(path, _apiCatalogoHelper.ApiCatalogoXCanal(), _apiCatalogoHelper.ApiCatalogoXUsuario());

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                paises = await JsonSerializer.DeserializeAsync<List<ApiCatalogoPaisesModelOutput>>(responseStream);
            }
            else
            {
                paises = null;
            }

            return paises;
        }

        public async Task<List<ApiCatalogoTiposDocumentoModelOutput>> ObtenerTiposDocumentoAsync()
        {
            List<ApiCatalogoTiposDocumentoModelOutput> tiposDoc;

            var path = ObtenerPathConBase(_apiCatalogoHelper.TiposDocumentoPath());

            var response = await RequestApiCatalogoAsync(path, _apiCatalogoHelper.ApiCatalogoXCanal(), _apiCatalogoHelper.ApiCatalogoXUsuario());

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                tiposDoc = await JsonSerializer.DeserializeAsync<List<ApiCatalogoTiposDocumentoModelOutput>>(responseStream);
            }
            else
            {
                tiposDoc = null;
            }

            return tiposDoc;
        }

        private string ObtenerPathConBase(string path)
        {
            return string.Concat(_apiCatalogoHelper.BasePath(), path);
        }

        private async Task<HttpResponseMessage> RequestApiCatalogoAsync(string path, string canal, string usuario)
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

            #endregion

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiCatalogo);

            return await client.SendAsync(request);
        }
    }
}
