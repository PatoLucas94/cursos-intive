using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Spv.Usuarios.Domain.Interfaces;

namespace Spv.Usuarios.Domain.Entities
{
    [Table("UsersPasswordHistory")]
    public class HistorialClaveUsuarios: IHistorialClaves
    {
        [Key]
        public int PasswordHistoryId { get; set; }
        
        public int UserId { get; set; }
        
        [Column(TypeName = "varchar(100)")]
        public string Password { get; set; }
        
        public DateTime CreationDate { get; set; }

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
    }
}
