using Newtonsoft.Json;

namespace Spv.Usuarios.Api.Exceptions
{
    /// <summary>
    /// Error
    /// </summary>
    [JsonObject(Title = "error")]
    public class ApiError
    {
        /// <summary>
        /// ApiError
        /// </summary>
        public ApiError(string code, string title, string source, string detail, string spvTrackId)
        {
            Code = code;
            Title = title;
            Source = source;
            Detail = detail;
            SpvTrackId = spvTrackId;
        }

        /// <summary>
        /// Codigo
        /// </summary>
        [JsonProperty(PropertyName = "codigo")]
        public string Code { get; set; }

        /// <summary>
        /// Titulo
        /// </summary>
        [JsonProperty(PropertyName = "titulo")]
        public string Title { get; set; }

        /// <summary>
        /// Origen
        /// </summary>
        [JsonProperty(PropertyName = "origen")]
        public string Source { get; set; }

        /// <summary>
        /// Detalle
        /// </summary>
        [JsonProperty(PropertyName = "detalle")]
        public string Detail { get; set; }

        /// <summary>
        /// TrackId
        /// </summary>
        [JsonProperty(PropertyName = "spvtrack_id")]
        public string SpvTrackId { get; set; }
    }
}
