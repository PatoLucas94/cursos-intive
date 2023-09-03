using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spv.Usuarios.Domain.Entities
{
    [Table("AuditLog")]
    public class AuditoriaLog
    {
        [Key]
        public int AuditLogId { get; set; }

        public int ChannelId { get; set; }

        public DateTime DateTime { get; set; }

        [Column(TypeName = "char(24)")]
        public string SessionId { get; set; }

        [Column(TypeName = "char(20)")]
        public string UserIP { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string UserAgent { get; set; }

        public int UserId { get; set; }

        public int OrganizationId { get; set; }

        public int ActionId { get; set; }

        public int ActionResultId { get; set; }

        /// <summary>
        ///  XML Column
        /// </summary>
        public string ExtendedInfo { get; set; }
    }
}
