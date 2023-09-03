using System;

namespace Spv.Usuarios.Common.Dtos.UsuariosService.Output
{
    public class PerfilMigradoModelOutput
    {
        public int UserId { get; set; }

        public long? PersonId { get; set; }

        public int DocumentCountryId { get; set; }

        public int DocumentTypeId { get; set; }

        public string DocumentNumber { get; set; }

        public byte UserStatusId { get; set; }

        public int? LoginAttempts { get; set; }

        public DateTime? LastPasswordChange { get; set; }

        public DateTime? LastLogon { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}