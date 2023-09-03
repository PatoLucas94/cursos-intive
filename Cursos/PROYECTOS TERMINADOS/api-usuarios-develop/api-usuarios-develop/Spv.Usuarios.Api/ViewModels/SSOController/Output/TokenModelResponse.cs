using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.SSORepository.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.SSOController.Output
{
    /// <summary>
    /// TokenModelResponse
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TokenModelResponse
    {
        /// <summary>
        /// Constructor TokenModelResponse
        /// </summary>
        protected TokenModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// AccessToken
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// ExpiresIn
        /// </summary>
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// RefreshExpiresIn
        /// </summary>
        [JsonProperty(PropertyName = "refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }

        /// <summary>
        /// RefreshToken
        /// </summary>
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static async Task<IResponse<TokenModelResponse>> FromAsync(
            Task<IResponse<TokenModelOutput>> task,
            IMapper mapper
        )
        {
            var response = await task;
            return response.Map(mapper.Map<TokenModelResponse>);
        }
    }
}
