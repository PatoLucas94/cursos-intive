using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output
{
    public class ApiPersonasCreacionEmailModelOutput
    {
        [JsonIgnore]
        public HttpStatusCode status_code { get; set; }

        public int id { get; set; }
        public string direccion { get; set; }
        public string nombre_interlocutor { get; set; }
        public string cargo_interlocutor { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_modificacion { get; set; }
        public string canal_creacion { get; set; }
        public string canal_modificacion { get; set; }
        public string usuario_creacion { get; set; }
        public string usuario_modificacion { get; set; }
        public string origen_contacto { get; set; }
        public bool principal { get; set; }
        public bool confiable { get; set; }
        public List<Etiqueta> etiquetas { get; set; }
        public decimal? score { get; set; }
        public UltimaVerificacionPositiva ultima_verificacion_positiva { get; set; }
        public bool dado_de_baja { get; set; }
    }
}
