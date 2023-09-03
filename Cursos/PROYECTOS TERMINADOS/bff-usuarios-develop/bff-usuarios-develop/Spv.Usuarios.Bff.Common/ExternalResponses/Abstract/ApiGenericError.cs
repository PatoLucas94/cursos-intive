using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Spv.Usuarios.Bff.Common.ExternalResponses.Abstract
{
    [ExcludeFromCodeCoverage]
    public abstract class ApiGenericError
    {
        /// <summary>
        /// ApiGenericError
        /// </summary>
        protected ApiGenericError()
        {
        }

        /// <summary>
        /// ApiGenericError
        /// </summary>
        protected ApiGenericError(string code, string title, string source, string detail, string spvTrackId)
        {
            Codigo = code;
            Titulo = title;
            Origen = source;
            Detalle = detail;
            SpvtrackId = spvTrackId;
        }

        /// <summary>
        /// Codigo
        /// </summary>
        [JsonProperty(PropertyName = "codigo")]
        public string Codigo { get; set; }

        /// <summary>
        /// Titulo
        /// </summary>
        [JsonProperty(PropertyName = "titulo")]
        public string Titulo { get; set; }

        /// <summary>
        /// Origen
        /// </summary>
        [JsonProperty(PropertyName = "origen")]
        public string Origen { get; set; }

        /// <summary>
        /// Detalle
        /// </summary>
        [JsonProperty(PropertyName = "detalle")]
        public string Detalle { get; set; }

        /// <summary>
        /// SpvtrackId
        /// </summary>
        [JsonProperty(PropertyName = "spvtrack_id")]
        public string SpvtrackId { get; set; }
    }
}
