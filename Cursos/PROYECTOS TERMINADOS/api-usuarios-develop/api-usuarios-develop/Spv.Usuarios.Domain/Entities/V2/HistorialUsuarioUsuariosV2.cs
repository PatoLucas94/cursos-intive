using System;

namespace Spv.Usuarios.Domain.Entities.V2
{
    public class HistorialUsuarioUsuariosV2
    {
        public int UsernameHistoryId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime CreationDate { get; set; }
        public int AuditLogId { get; set; }

        public UsuarioV2 Usuario { get; set; }
    }
}
