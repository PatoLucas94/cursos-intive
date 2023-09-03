using System;
using System.Collections.Generic;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output
{
    public class ApiTyCVigenteModelOutput
    {
        public string id { get; set; }
        public DateTime vigencia_desde { get; set; }
        public string contenido { get; set; }
        public List<Canal> canales { get; set; }
        public List<Concepto> conceptos { get; set; }
    }

    public class Canal
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }
    }

    public class Concepto
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }
    }
}
