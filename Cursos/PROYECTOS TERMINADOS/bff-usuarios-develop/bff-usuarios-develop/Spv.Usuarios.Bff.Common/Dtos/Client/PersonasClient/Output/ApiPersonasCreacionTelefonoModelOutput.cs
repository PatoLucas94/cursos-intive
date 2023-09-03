using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output
{
    public class ApiPersonasCreacionTelefonoModelOutput
    {
        [JsonIgnore]
        public HttpStatusCode status_code { get; set; }

        public int id { get; set; }
        public int? numero_local { get; set; }
        public int pais { get; set; }
        public int? prefijo_telefonico_pais { get; set; }
        public int? codigo_area { get; set; }
        public string numero { get; set; }
        public int? interno { get; set; }
        public string ddn { get; set; }
        public string ddi { get; set; }
        public string no_llame { get; set; }
        public string fecha_alta_no_llame { get; set; }
        public string fecha_baja_no_llame { get; set; }
        public bool es_geografico { get; set; }
        public string nombre_interlocutor { get; set; }
        public string cargo_interlocutor { get; set; }
        public decimal? score { get; set; }
        public bool normalizado { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_modificacion { get; set; }
        public string canal_creacion { get; set; }
        public string canal_modificacion { get; set; }
        public string usuario_creacion { get; set; }
        public string usuario_modificacion { get; set; }
        public string compania { get; set; }
        public string origen_contacto { get; set; }
        public bool principal { get; set; }
        public bool confiable { get; set; }
        public bool? doble_factor { get; set; }
        public List<Etiqueta> etiquetas { get; set; }
        public UltimaVerificacionPositiva ultima_verificacion_positiva { get; set; }
        public string tipo_telefono { get; set; }
        public bool dado_de_baja { get; set; }
    }
}
