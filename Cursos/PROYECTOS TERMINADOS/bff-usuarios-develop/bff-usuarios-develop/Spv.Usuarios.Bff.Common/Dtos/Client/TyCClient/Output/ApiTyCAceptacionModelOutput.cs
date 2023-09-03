using System;
using System.Net;
using System.Text.Json.Serialization;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output
{
    public class ApiTyCAceptacionModelOutput
    {
        [JsonIgnore]
        public HttpStatusCode status_code { get; set; }

        public string id_terminos_condiciones { get; set; }
        public DateTime fecha_aceptacion { get; set; }
    }
}
