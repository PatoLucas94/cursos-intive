using System;
using Spv.Usuarios.Domain.Interfaces;

namespace Spv.Usuarios.Domain.Entities.V2
{
    public class HistorialClaveUsuariosV2 : IHistorialClaves
    {
        public int PasswordHistoryId { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public int AuditLogId { get; set; }

        public UsuarioV2 Usuario { get; set; }

        public int GetUserId()
        {
            return UserId;
        }

        public string GetPassword()
        {
            return Password;
        }

        public DateTime GetCreationDate()
        {
            return CreationDate;
        }

        public int GetAuditLogId()
        {
            return AuditLogId;
        }
    }
}
