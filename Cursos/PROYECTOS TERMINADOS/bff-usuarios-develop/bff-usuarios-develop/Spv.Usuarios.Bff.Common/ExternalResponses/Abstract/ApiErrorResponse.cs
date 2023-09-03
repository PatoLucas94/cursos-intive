using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Spv.Usuarios.Bff.Common.ExternalResponses.Abstract
{
    [ExcludeFromCodeCoverage]
    public abstract class ApiErrorResponse<T> where T : ApiGenericError
    {
        /// <summary>
        /// ApiUsuariosErrorResponse
        /// </summary>
        protected ApiErrorResponse()
        {
            Errores = new List<T>();
        }

        /// <summary>
        /// constructor
        /// </summary>
        protected ApiErrorResponse(string state, int code, string type, string detail, List<T> errors)
        {
            Estado = state;
            Codigo = code;
            Tipo = type;
            Detalle = detail;
            Errores = errors;
        }

        /// <summary>
        /// Descripción del código de error Http
        /// </summary>
        [JsonProperty(PropertyName = "estado")]
        public string Estado { get; set; }

        /// <summary>
        /// codigo HTTP
        /// </summary>
        [JsonProperty(PropertyName = "codigo")]
        public int Codigo { get; set; }

        /// <summary>
        /// Tipo de error, valores posibles: negocio|tecnico
        /// </summary>
        [JsonProperty(PropertyName = "tipo")]
        public string Tipo { get; set; }

        /// <summary>
        /// Detalle del error
        /// </summary>
        [JsonProperty(PropertyName = "detalle")]
        public string Detalle { get; set; }

        /// <summary>
        /// Lista de errores
        /// </summary>
        [JsonProperty(PropertyName = "errores")]
        public List<T> Errores { get; set; }
    }
}
