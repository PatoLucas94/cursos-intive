using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spv.Usuarios.Domain.Entities
{
    [Table("UsersRegistrated")]
    public class UsuarioRegistrado
    {
        public int DocumentTypeId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string DocumentNumber { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ChannelKey { get; set; }

        public bool Active { get; set; }

        public bool Migrated { get; set; }

        public bool PasswordExpired { get; set; }

        public int ForgotPasswordAttempts { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string CardNumber { get; set; }

        public DateTime PostDateTime { get; set; }

        public DateTime ExpiredDateTime { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string ChannelSource { get; set; }
    }
}
