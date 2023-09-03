using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Utils;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService
{
    public class ApiPersonasRepository : IApiPersonasRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiPersonasHelper _apiPersonaHelper;
        private readonly IMemoryCache _memoryCache;

        public ApiPersonasRepository(
            IHttpClientFactory httpClientFactory,
            IApiPersonasHelper apiPersonaHelper,
            IMemoryCache memoryCache
        )
        {
            _httpClientFactory = httpClientFactory;
            _apiPersonaHelper = apiPersonaHelper;
            _memoryCache = memoryCache;
        }

        public async Task<ApiPersonaModelOutput> ObtenerPersonaAsync(
            string numeroDocumento,
            int tipoDocumento,
            int paisDocumento
        )
        {
            ApiPersonaModelOutput persona;

            var path = ObtenerPathConBase(string.Format(
                _apiPersonaHelper.PersonasPath(),
                numeroDocumento,
                tipoDocumento.ToString(),
                paisDocumento.ToString()));

            var response = await GetApiPersonaAsync(path, _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario());

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                persona = await JsonSerializer.DeserializeAsync<ApiPersonaModelOutput>(responseStream);
            }
            else
            {
                persona = null;
            }

            return persona;
        }

        public async Task<ApiPersonaInfoModelOutput> ObtenerInfoPersonaAsync(string personId)
        {
            var cacheKey = Cache.ApiPersona.ObtenerInfoPersona(personId);

            _memoryCache.TryGetValue(cacheKey, out ApiPersonaInfoModelOutput personaInfo);

            if (personaInfo != null)
                return personaInfo;

            var path = ObtenerPathConBase(string.Format(_apiPersonaHelper.PersonasInfoPath(), personId));

            var response = await GetApiPersonaAsync(
                path,
                _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario()
            );

            if (!response.IsSuccessStatusCode)
                return null;

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            personaInfo = await JsonSerializer.DeserializeAsync<ApiPersonaInfoModelOutput>(responseStream);
            _memoryCache.Set(cacheKey, personaInfo, _apiPersonaHelper.ExpirationCache());

            return personaInfo;
        }

        public async Task<ApiPersonasFisicaInfoModelOutput> ObtenerInfoPersonaFisicaAsync(string personId)
        {
            ApiPersonasFisicaInfoModelOutput personaFisicaInfo;

            var path = ObtenerPathConBase(string.Format(_apiPersonaHelper.PersonasFisicaInfoPath(), personId));

            var response = await GetApiPersonaAsync(path, _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario());

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                personaFisicaInfo =
                    await JsonSerializer.DeserializeAsync<ApiPersonasFisicaInfoModelOutput>(responseStream);
            }
            else
            {
                personaFisicaInfo = null;
            }

            return personaFisicaInfo;
        }

        public async Task<List<ApiPersonasFiltroModelOutput>> ObtenerPersonaFiltroAsync(string numeroDocumento)
        {
            List<ApiPersonasFiltroModelOutput> personas;

            var path = ObtenerPathConBase(string.Format(_apiPersonaHelper.PersonasFiltroPath(), numeroDocumento));

            var response = await GetApiPersonaAsync(path, _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario());

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                personas = await JsonSerializer.DeserializeAsync<List<ApiPersonasFiltroModelOutput>>(responseStream);
            }
            else
            {
                personas = null;
            }

            return personas;
        }

        public async Task<ApiPersonasCreacionTelefonoModelOutput> CrearTelefonoAsync(
            string personId,
            ApiPersonasCreacionTelefonoModelInput body)
        {
            var path = ObtenerPathConBase(string.Format(_apiPersonaHelper.TelefonosCreacionPath(), personId));

            var response = await PostApiPersonaAsync(path, body, _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario());

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<ApiPersonasCreacionTelefonoModelOutput>(responseStream);

            result.status_code = response.StatusCode;

            return result;
        }

        public async Task<ApiPersonasCreacionTelefonoModelOutput> ActualizarTelefonoAsync(
            string personId,
            ApiPersonasActualizacionTelefonoModelInput body,
            string phoneId)
        {
            var path = ObtenerPathConBase(string.Format(_apiPersonaHelper.TelefonosActualizacionPath(), personId,
                phoneId));

            var response = await PatchApiPersonaAsync(path, body, _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario());

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<ApiPersonasCreacionTelefonoModelOutput>(responseStream);

            result.status_code = response.StatusCode;

            return result;
        }

        public async Task<ApiPersonasCreacionTelefonoModelOutput> CrearTelefonoDobleFactorAsync(
            string personId,
            ApiPersonasCreacionTelefonoModelInput body)
        {
            var path = ObtenerPathConBase(string.Format(_apiPersonaHelper.TelefonosDobleFactorCreacionPath(),
                personId));

            var response = await PostApiPersonaAsync(path, body, _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario());

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<ApiPersonasCreacionTelefonoModelOutput>(responseStream);

            result.status_code = response.StatusCode;

            return result;
        }

        public async Task<ApiPersonasCreacionTelefonoModelOutput> ActualizarTelefonoDobleFactorAsync(
            string personId,
            ApiPersonasActualizacionTelefonoModelInput body,
            string phoneId = null)
        {
            var path = ObtenerPathConBase(string.Format(_apiPersonaHelper.TelefonosDobleFactorActualizacionPath(),
                personId));

            var response = await PatchApiPersonaAsync(path, body, _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario());

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<ApiPersonasCreacionTelefonoModelOutput>(responseStream);

            result.status_code = response.StatusCode;

            return result;
        }

        public async Task<ApiPersonasCreacionEmailModelOutput> ActualizarEmailAsync(
            string personId,
            string emailId,
            ApiPersonasCreacionEmailModelInput body)
        {
            var path = ObtenerPathConBase(string.Format(_apiPersonaHelper.EmailsActualizacionPath(), personId,
                emailId));

            var response = await PatchApiPersonaAsync(path, body, _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario());

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<ApiPersonasCreacionEmailModelOutput>(responseStream);

            result.status_code = response.StatusCode;

            return result;
        }

        public async Task<ApiPersonasCreacionEmailModelOutput> CrearEmailAsync(
            string personId,
            ApiPersonasCreacionEmailModelInput body)
        {
            var path = ObtenerPathConBase(string.Format(_apiPersonaHelper.EmailsCreacionPath(), personId));

            var response = await PostApiPersonaAsync(path, body, _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario());

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<ApiPersonasCreacionEmailModelOutput>(responseStream);

            result.status_code = response.StatusCode;

            return result;
        }

        public async Task<ApiPersonasVerificacionTelefonoModelOutput> VerificarTelefonoAsync(
            long telefonoId,
            ApiPersonasVerificacionTelefonoModelInput body)
        {
            var path = ObtenerPathConBase(string.Format(_apiPersonaHelper.TelefonosVerificacionPath(), telefonoId));

            var response = await PostApiPersonaAsync(path, body, _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario());

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var result =
                await JsonSerializer.DeserializeAsync<ApiPersonasVerificacionTelefonoModelOutput>(responseStream);

            result.status_code = response.StatusCode;

            return result;
        }

        public async Task<ApiPersonasProductosMarcaClienteModelOutput> ObtenerProductosMarcaClienteAsync(
            string personId)
        {
            ApiPersonasProductosMarcaClienteModelOutput personasProductosMarcaCliente;

            var path = ObtenerPathConBase(string.Format(_apiPersonaHelper.ProductosMarcaClientePath(), personId));

            var response = await GetApiPersonaAsync(path, _apiPersonaHelper.ApiPersonasXCanal(),
                _apiPersonaHelper.ApiPersonasXUsuario());

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                personasProductosMarcaCliente =
                    await JsonSerializer.DeserializeAsync<ApiPersonasProductosMarcaClienteModelOutput>(responseStream);
            }
            else
            {
                personasProductosMarcaCliente = null;
            }

            return personasProductosMarcaCliente;
        }

        private string ObtenerPathConBase(string path)
        {
            return string.Concat(_apiPersonaHelper.BasePath(), path);
        }

        private async Task<HttpResponseMessage> GetApiPersonaAsync(string path, string canal, string usuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            return await RequestApiPersonaAsync(request, canal, usuario);
        }

        private async Task<HttpResponseMessage> PostApiPersonaAsync(string path, object body, string canal,
            string usuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);

            if (body != null)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body, new JsonSerializerOptions()), Encoding.UTF8,
                    MediaTypeNames.Application.Json);
            }

            return await RequestApiPersonaAsync(request, canal, usuario);
        }

        private async Task<HttpResponseMessage> PatchApiPersonaAsync(string path, object body, string canal,
            string usuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, path);

            if (body != null)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body, new JsonSerializerOptions()), Encoding.UTF8,
                    MediaTypeNames.Application.Json);
            }

            return await RequestApiPersonaAsync(request, canal, usuario);
        }

        private async Task<HttpResponseMessage> RequestApiPersonaAsync(HttpRequestMessage request, string canal,
            string usuario)
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

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiPersonas);

            return await client.SendAsync(request);
        }
    }
}
