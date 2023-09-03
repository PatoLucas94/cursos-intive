using System.Collections.Generic;
using Newtonsoft.Json;

namespace Spv.Usuarios.Api.Exceptions
{
    /// <summary>
    /// Detalle de error model
    /// </summary>
    [JsonObject(Title = "detalle_error_model")]
    public class ErrorDetailModel
    {
        /// <summary>
        /// ErrorDetailModel
        /// </summary>
        public ErrorDetailModel()
        {
            Errors = new List<ApiError>();
        }

        /// <summary>
        /// ErrorDetailModel
        /// </summary>
        public ErrorDetailModel(string state, int code, string type, string detail, List<ApiError> errors)
        {
            State = state;
            Code = code;
            Type = type;
            Detail = detail;
            Errors = errors;
        }

        /// <summary>
        /// Descripción del código de error Http
        /// </summary>
        [JsonProperty(PropertyName = "estado")]
        public string State { get; set; }

        /// <summary>
        /// Codigo HTTP
        /// </summary>
        [JsonProperty(PropertyName = "codigo")]
        public int Code { get; set; }

        /// <summary>
        /// Tipo de error, valores posibles: negocio|tecnico
        /// </summary>
        [JsonProperty(PropertyName = "tipo")]
        public string Type { get; set; }

        /// <summary>
        /// Detalle del error
        /// </summary>
        [JsonProperty(PropertyName = "detalle")]
        public string Detail { get; set; }

        /// <summary>
        /// Lista de errores
        /// </summary>
        [JsonProperty(PropertyName = "errores")]
        public List<ApiError> Errors { get; set; }
    }
}
