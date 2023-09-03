using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Utils;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService
{
    public class ApiTyCRepository : IApiTyCRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiTyCHelper _apiTyCHelper;
        private readonly IMemoryCache _memoryCache;
        private readonly IApiUsuariosRepositoryV2 _apiUsuariosRepositoryV2;
        private readonly IMapper _mapper;

        public ApiTyCRepository(
            IHttpClientFactory httpClientFactory,
            IApiTyCHelper apiTyCHelper,
            IMemoryCache memoryCache,
            IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2,
            IMapper mapper
        )
        {
            _httpClientFactory = httpClientFactory;
            _apiTyCHelper = apiTyCHelper;
            _memoryCache = memoryCache;
            _apiUsuariosRepositoryV2 = apiUsuariosRepositoryV2;
            _mapper = mapper;
        }

        public async ValueTask<ApiTyCVigenteModelOutput> ObtenerVigenteAsync()
        {
            var canal = _apiTyCHelper.ApiTyCxCanal();
            var concepto = _apiTyCHelper.ConceptoRegistracion();
            var cacheKey = Cache.ApiTyC.ObtenerVigente(canal, concepto);

            _memoryCache.TryGetValue(cacheKey, out ApiTyCVigenteModelOutput tycVigente);

            if (tycVigente != null)
                return tycVigente;

            var path = ObtenerPathConBase(
                string.Format(_apiTyCHelper.TerminosYCondicionesVigentePath(), canal, concepto)
            );

            var response = await GetApiTyCAsync(path, canal, _apiTyCHelper.ApiTyCxUsuario());

            if (!response.IsSuccessStatusCode)
                return null;

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            tycVigente = await JsonSerializer.DeserializeAsync<ApiTyCVigenteModelOutput>(responseStream);
            _memoryCache.Set(cacheKey, tycVigente, _apiTyCHelper.ExpirationCache());

            return tycVigente;
        }

        public async ValueTask<ApiTyCAceptadosModelOutput> ObtenerAceptadosByPersonIdAsync(string personId)
        {
            var canal = _apiTyCHelper.ApiTyCxCanal();
            var concepto = _apiTyCHelper.ConceptoRegistracion();
            var cacheKey = Cache.ApiTyC.ObtenerAceptados(personId, canal, concepto);

            _memoryCache.TryGetValue(cacheKey, out ApiTyCAceptadosModelOutput tycAceptados);

            if (tycAceptados != null)
                return tycAceptados;

            var path = ObtenerPathConBase(
                $"{_apiTyCHelper.TerminosYCondicionesAceptadosPath()}?canal={canal}&concepto={concepto}&id_persona={personId}"
            );

            var response = await GetApiTyCAsync(path, canal, _apiTyCHelper.ApiTyCxUsuario());

            if (!response.IsSuccessStatusCode)
                return null;

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            tycAceptados = await JsonSerializer.DeserializeAsync<ApiTyCAceptadosModelOutput>(responseStream);
            _memoryCache.Set(cacheKey, tycAceptados, _apiTyCHelper.ExpirationCache());

            return tycAceptados;
        }

        public async ValueTask<ApiTyCAceptadosModelOutput> ObtenerAceptadosAsync(
            (string NumeroDocumento, string Usuario) info
        )
        {
            var obtenerUsuario = await _apiUsuariosRepositoryV2.ObtenerUsuarioAsync(info);

            if (obtenerUsuario is null)
                return null;

            return await ObtenerAceptadosByPersonIdAsync(obtenerUsuario.IdPersona);
        }

        public async ValueTask<ApiTyCAceptacionModelOutput> AceptarAsync(
            (int IdPersona, string IdTerminosCondiciones) info
        )
        {
            var body = new ApiTyCAceptacionModelInput
            {
                id_persona = info.IdPersona,
                id_terminos_condiciones = info.IdTerminosCondiciones,
                canal = _apiTyCHelper.ApiTyCxCanal(),
                concepto = _apiTyCHelper.ConceptoRegistracion()
            };

            var cacheKey = Cache.ApiTyC.ObtenerAceptados(body.id_persona.ToString(), body.canal, body.concepto);

            _memoryCache.TryGetValue(cacheKey, out ApiTyCAceptadosModelOutput tycAceptados);

            if (tycAceptados != null)
                return _mapper.Map<ApiTyCAceptacionModelOutput>(tycAceptados);

            var path = ObtenerPathConBase(_apiTyCHelper.TerminosYCondicionesAceptadosPath());

            var response = await PostApiTyCAsync(
                path,
                body,
                _apiTyCHelper.ApiTyCxCanal(),
                _apiTyCHelper.ApiTyCxUsuario()
            );

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<ApiTyCAceptacionModelOutput>(responseStream);

            result.status_code = response.StatusCode;

            var statusCode = (int)result.status_code;

            if (
                statusCode >= 200 &&
                statusCode <= 299 &&
                !string.IsNullOrWhiteSpace(result.id_terminos_condiciones)
            )
            {
                _memoryCache.Set(
                    cacheKey,
                    new ApiTyCAceptadosModelOutput
                    {
                        id_terminos_condiciones = result.id_terminos_condiciones,
                        fecha_aceptacion = result.fecha_aceptacion,
                        status_code = result.status_code
                    },
                    _apiTyCHelper.ExpirationCache()
                );
            }

            return result;
        }

        private string ObtenerPathConBase(string path)
        {
            return string.Concat(_apiTyCHelper.BasePath(), path);
        }

        private async Task<HttpResponseMessage> GetApiTyCAsync(string path, string canal, string usuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            return await RequestApiTyCAsync(request, canal, usuario);
        }

        private async Task<HttpResponseMessage> PostApiTyCAsync(string path, object body, string canal, string usuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);

            if (body != null)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body, new JsonSerializerOptions()),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json
                );
            }

            return await RequestApiTyCAsync(request, canal, usuario);
        }

        private async Task<HttpResponseMessage> RequestApiTyCAsync(
            HttpRequestMessage request,
            string canal,
            string usuario
        )
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

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiTyC);

            return await client.SendAsync(request);
        }
    }
}
