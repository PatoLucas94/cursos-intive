using System;

namespace Spv.Usuarios.Bff.Common.Dtos.Service.TyCService.Output
{
    public class AceptadosModelOutput
    {
        public string id { get; set; }

        public DateTime vigencia_desde { get; set; }

        public string contenido { get; set; }

        public bool aceptados { get; set; }
    }
}
