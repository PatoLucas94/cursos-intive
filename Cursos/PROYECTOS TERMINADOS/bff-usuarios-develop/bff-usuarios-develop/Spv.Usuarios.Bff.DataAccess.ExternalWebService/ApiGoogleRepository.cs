using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService
{
    public class ApiGoogleRepository : IApiGoogleRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiGoogleHelper _apiGoogleHelper;

        public ApiGoogleRepository(IHttpClientFactory httpClientFactory, IApiGoogleHelper apiGoogleHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiGoogleHelper = apiGoogleHelper;
        }

        public async Task<HttpResponseMessage> ReCaptchaV3ValidarTokenAsync(string token)
        {
            var path = _apiGoogleHelper.ValidacionTokenReCaptchaV3Path();

            var queryString = new Dictionary<string, string>()
            {
                { "secret", _apiGoogleHelper.SecretReCaptchaV3Key() },
                { "response", token }
            };

            var requestUri = QueryHelpers.AddQueryString(path, queryString);

            return await PostRequestApiGoogleAsync(requestUri);
        }

        private async Task<HttpResponseMessage> PostRequestApiGoogleAsync(
            string path,
            object body = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);

            if (body != null)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body, new JsonSerializerOptions()), Encoding.UTF8, MediaTypeNames.Application.Json);
            }

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiGoogle);

            return await client.SendAsync(request);
        }
    }
}
