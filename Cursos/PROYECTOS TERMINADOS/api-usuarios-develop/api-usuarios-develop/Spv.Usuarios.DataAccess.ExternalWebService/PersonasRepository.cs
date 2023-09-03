using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.PersonaService.Output;
using Spv.Usuarios.DataAccess.ExternalWebService.Helpers;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;

namespace Spv.Usuarios.DataAccess.ExternalWebService
{
    public class PersonasRepository : IPersonasRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiPersonasHelper _apiPersonaHelper;
        private readonly ILogger<PersonasRepository> _logger;

        public PersonasRepository(
            IHttpClientFactory httpClientFactory, 
            IApiPersonasHelper apiPersonaHelper,
            ILogger<PersonasRepository> logger)
        {
            _httpClientFactory = httpClientFactory;
            _apiPersonaHelper = apiPersonaHelper;
            _logger = logger;
        }

        public async Task<PersonaModelResponse> ObtenerPersona(string numeroDocumento, int tipoDocumento, int paisDocumento, string canal = null, string usuario = null)
        {
            PersonaModelResponse persona;

            var path = string.Format(
                ObtenerPathConBase(_apiPersonaHelper.PersonaPath()),
                numeroDocumento,
                tipoDocumento.ToString(),
                paisDocumento.ToString());

            var response = await RequestApiPersonaAsync(path, canal, usuario);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                persona = await JsonSerializer.DeserializeAsync<PersonaModelResponse>(responseStream);
            }
            else
            {
                persona = null;
            }

            var responseString = await response.Content.ReadAsStringAsync();
            ExternalServicesCallsLog<PersonasRepository>.LogDebug(
                _logger,
                ExternalServicesNames.ApiPersonas,
                response.StatusCode.ToString(),
                response.RequestMessage.RequestUri.AbsoluteUri,
                response.RequestMessage.Method.ToString(),
                responseString);

            return persona;
        }

        public async Task<PersonaInfoModelResponse> ObtenerInfoPersona(long personId, string canal = null, string usuario = null)
        {
            PersonaInfoModelResponse personaInfo;

            var path = string.Format(
                ObtenerPathConBase(_apiPersonaHelper.PersonaInfoPath()),
                personId);

            var response = await RequestApiPersonaAsync(path, canal, usuario);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                personaInfo = await JsonSerializer.DeserializeAsync<PersonaInfoModelResponse>(responseStream);
            }
            else
            {
                personaInfo = null;
            }

            var responseString = await response.Content.ReadAsStringAsync();
            ExternalServicesCallsLog<PersonasRepository>.LogDebug(
                _logger,
                ExternalServicesNames.ApiPersonas,
                response.StatusCode.ToString(),
                response.RequestMessage.RequestUri.AbsoluteUri,
                response.RequestMessage.Method.ToString(),
                responseString);

            return personaInfo;
        }

        public async Task<PersonaFisicaInfoModelResponse> ObtenerInfoPersonaFisica(long personId, string canal = null, string usuario = null)
        {
            PersonaFisicaInfoModelResponse personaFisicaInfo;

            var path = string.Format(
                ObtenerPathConBase(_apiPersonaHelper.PersonaFisicaInfoPath()),
                personId);

            var response = await RequestApiPersonaAsync(path, canal, usuario);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                personaFisicaInfo = await JsonSerializer.DeserializeAsync<PersonaFisicaInfoModelResponse>(responseStream);
            }
            else
            {
                personaFisicaInfo = null;
            }
            
            var responseString = await response.Content.ReadAsStringAsync();
            ExternalServicesCallsLog<PersonasRepository>.LogDebug(
                _logger,
                ExternalServicesNames.ApiPersonas,
                response.StatusCode.ToString(),
                response.RequestMessage.RequestUri.AbsoluteUri,
                response.RequestMessage.Method.ToString(),
                responseString);

            return personaFisicaInfo;
        }

        private string ObtenerPathConBase(string path)
        {
            return string.Concat(_apiPersonaHelper.PersonaBasePath(), path);
        }

        private async Task<HttpResponseMessage> RequestApiPersonaAsync(string path, string canal, string usuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            #region Requerido para UTs

            // Como este servicio se consume principalmente dentro de otro servicio que ya cuenta con los headers
            // que se propagan automáticamente, es que necesitamos verificar si necesitamos pasar estos headers
            // en particular, de lo contrario el servicio devuelve un error por duplicidad
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
