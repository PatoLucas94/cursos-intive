using System;
using System.Collections.Generic;

namespace Spv.Usuarios.Bff.Common.Dtos.Service.TyCService.Output
{
    public class VigenteModelOutput
    {
        public string id { get; set; }

        public DateTime vigencia_desde { get; set; }

        public string contenido { get; set; }

        public List<CanalModelOutput> canales { get; set; }

        public List<ConceptoModelOutput> conceptos { get; set; }
    }
}
