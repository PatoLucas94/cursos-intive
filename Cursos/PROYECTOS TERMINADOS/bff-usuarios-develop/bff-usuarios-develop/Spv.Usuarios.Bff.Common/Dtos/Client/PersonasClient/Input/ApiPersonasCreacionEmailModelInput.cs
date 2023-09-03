using System.Collections.Generic;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Input
{
    public class ApiPersonasCreacionEmailModelInput
    {
        public string cargo_interlocutor { get; set; }
        public bool confiable { get; set; }
        public string direccion { get; set; }
        public List<Etiqueta> etiquetas { get; set; }
        public string nombre_interlocutor { get; set; }
        public string origen_contacto { get; set; }
        public bool principal { get; set; }
        public UltimaVerificacionPositiva ultima_verificacion_positiva { get; set; }
    }
}
