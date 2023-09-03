using System.Net;
using System.Text.Json.Serialization;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output
{
    public class ApiPersonasVerificacionTelefonoModelOutput
    {
        [JsonIgnore]
        public HttpStatusCode status_code { get; set; }

        public string canal_creacion { get; set; }
        public string fecha_verificacion { get; set; }
        public int id { get; set; }
        public int id_telefono { get; set; }
        public string usuario_creacion { get; set; }
        public bool verificado { get; set; }
    }
}
