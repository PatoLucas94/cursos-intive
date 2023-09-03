using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.ExternalResponses.Abstract;

namespace Spv.Usuarios.Bff.Common.Errors
{
    /// <summary>
    /// ApiBiometriaError
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiBiometriaError : ApiGenericError
    {
        /// <summary>
        /// ApiBiometriaError
        /// </summary>
        public ApiBiometriaError() : base()
        {
        }

        /// <summary>
        /// ApiBiometriaError
        /// </summary>
        public ApiBiometriaError(string code, string title, string source, string detail, string spvTrackId)
            : base(code, title, source, detail, spvTrackId)
        {
        }

        [JsonProperty(PropertyName = "mensaje_largo")]
        public string MensajeLargo { get; set; }

        [JsonProperty(PropertyName = "mensaje_corto")]
        public string MensajeCorto { get; set; }
    }
}
