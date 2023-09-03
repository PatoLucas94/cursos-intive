using System;

namespace Spv.Usuarios.Domain.Entities.V2
{
    public class AuditoriaLogV2
    {
        public int AuditLogId { get; set; }
        public string Channel { get; set; }
        public DateTime DateTime { get; set; }
        public int? UserId { get; set; }
        public int EventTypeId { get; set; }
        public int EventResultId { get; set; }
        public string ExtendedInfo { get; set; }

        public TiposEventoV2 EventTypes { get; set; }
        public ResultadosEventoV2 EventResults { get; set; }
    }
}
