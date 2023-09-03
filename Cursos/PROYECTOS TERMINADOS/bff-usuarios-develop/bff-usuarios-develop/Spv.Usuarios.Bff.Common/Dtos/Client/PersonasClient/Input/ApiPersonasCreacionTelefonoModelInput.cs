using System.Collections.Generic;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Input
{
    public class ApiPersonasCreacionTelefonoModelInput
    {
        public string cargo_interlocutor { get; set; }
        public int? codigo_area { get; set; }
        public string compania { get; set; }
        public bool confiable { get; set; }
        public List<Etiqueta> etiquetas { get; set; }
        public int? interno { get; set; }
        public string nombre_interlocutor { get; set; }
        public string numero { get; set; }
        public int? numero_local { get; set; }
        public string origen_contacto { get; set; }
        public int pais { get; set; }
        public bool principal { get; set; }
        public string tipo_telefono { get; set; }
        public UltimaVerificacionPositiva ultima_verificacion_positiva { get; set; }
    }
}
