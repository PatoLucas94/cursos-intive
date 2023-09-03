using System.Collections.Generic;

namespace Spv.Usuarios.Domain.Entities.V2
{
    public class ResultadosEventoV2
    {
        public int EventResultId { get; set; }
        public string Description { get; set; }

        public List<AuditoriaLogV2> Audits { get; set; }
    }
}
