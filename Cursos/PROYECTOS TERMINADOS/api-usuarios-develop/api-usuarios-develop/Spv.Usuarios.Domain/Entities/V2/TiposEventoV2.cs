using System.Collections.Generic;

namespace Spv.Usuarios.Domain.Entities.V2
{
    public class TiposEventoV2
    {
        public int EventTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<AuditoriaLogV2> Audits { get; set; }
    }
}
