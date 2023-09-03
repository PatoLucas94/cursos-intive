using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Configurations;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.SSORepository.Output;
using Spv.Usuarios.DataAccess.ExternalWebService.Helpers;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;

namespace Spv.Usuarios.DataAccess.ExternalWebService
{
    [ExcludeFromCodeCoverage]
    public class SsoRepository : ISsoRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SsoConfigurationOptions _options;
        private readonly ILogger<SsoRepository> _logger;

        public SsoRepository(
            IHttpClientFactory httpClientFactory,
            IOptions<SsoConfigurationOptions> options,
            ILogger<SsoRepository> logger
        )
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<(TokenModelOutput Response, ErrorModel Error)> GetTokenAsync(
            string documentNumber,
            string userName,
            string password,
            string canal
        ) => await DeserializeResponseAsync<TokenModelOutput>(
            await TokenAsync(documentNumber, userName, password, canal)
        );

        public async Task<(TokenModelOutput Response, ErrorModel Error)> RefreshAccessTokenAsync(string refreshToken) =>
            await DeserializeResponseAsync<TokenModelOutput>(await TokenAsync(refreshToken));

        public async Task<(IReadOnlyDictionary<string, object> Response, ErrorModel Error)> GetIntrospectAsync(
            string accessToken
        ) => await DeserializeResponseAsync<IReadOnlyDictionary<string, object>>(await IntrospectAsync(accessToken));

        public async Task<(bool Response, ErrorModel Error)> GetLogoutAsync(string refreshToken) =>
            await DeserializeResponseAsync<bool>(await LogoutAsync(refreshToken));

        private async Task<HttpResponseMessage> TokenAsync(
            string documentNumber,
            string userName,
            string password,
            string canal
        ) => await PostAsync(
            new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", $"{documentNumber}|{userName}|{canal}" },
                { "password", password }
            },
            _options.TokenUrl
        );

        private async Task<HttpResponseMessage> TokenAsync(string refreshToken) => await PostAsync(
            new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            },
            _options.TokenUrl
        );

        private async Task<HttpResponseMessage> IntrospectAsync(string accessToken) => await PostAsync(
            new Dictionary<string, string> { { "token", accessToken } },
            _options.IntrospectUrl
        );

        private async Task<HttpResponseMessage> LogoutAsync(string refreshToken) => await PostAsync(
            new Dictionary<string, string> { { "refresh_token", refreshToken } },
            _options.LogoutUrl
        );

        private async Task<HttpResponseMessage> PostAsync(IDictionary<string, string> @params, string requestPath)
        {
            var client = _httpClientFactory.CreateClient(ExternalServicesNames.Sso);

            var parameters = new Dictionary<string, string>
            {
                { "client_id", _options.ClientId },
                { "client_secret", _options.ClientSecret }
            }.Concat(@params);

            var request = new HttpRequestMessage(HttpMethod.Post, requestPath);
            request.Content = new FormUrlEncodedContent(parameters);

            return await client.SendAsync(request);
        }

        private async Task<(T Response, ErrorModel Error)> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            await response.Content.ReadAsStreamAsync();

            var responseString = await response.Content.ReadAsStringAsync();
            var statusCode = (int)response.StatusCode;

            ExternalServicesCallsLog<SsoRepository>.LogDebug(
                _logger,
                ExternalServicesNames.Sso,
                statusCode.ToString(),
                response.RequestMessage.RequestUri.AbsoluteUri,
                response.RequestMessage.Method.ToString(),
                responseString
            );

            if (statusCode >= 200 && statusCode <= 299)
            {
                if (typeof(T) == typeof(bool) && string.IsNullOrWhiteSpace(responseString))
                    return (JsonConvert.DeserializeObject<T>("true"), null);

                return (JsonConvert.DeserializeObject<T>(responseString), null);
            }

            return (default, JsonConvert.DeserializeObject<ErrorModel>(responseString));
        }
    }
}
