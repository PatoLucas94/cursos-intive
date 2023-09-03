using Newtonsoft.Json;

namespace Spv.Usuarios.Bff.Common.Dtos.Service.RhSsoService.Output
{
    public class TokenModelOutput
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }
    }
}
