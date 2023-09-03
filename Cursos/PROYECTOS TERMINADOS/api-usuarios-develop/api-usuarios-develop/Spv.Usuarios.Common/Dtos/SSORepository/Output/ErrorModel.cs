using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Spv.Usuarios.Common.Dtos.SSORepository.Output
{
    [ExcludeFromCodeCoverage]
    public class ErrorModel
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }
    }
}
