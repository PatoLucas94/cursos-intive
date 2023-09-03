using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spv.Usuarios.Domain.Entities
{
    [Table("UsersData")]
    public class DatosUsuario
    {
        [Key]
        public int UserDataId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Mail { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string WorkPhone { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string CellPhone { get; set; }

        public bool? ReceiveMail { get; set; }

        public bool? ReceiveSMS { get; set; }

        public int? CellCompanyId { get; set; }

        public DateTime? WorkPhoneDate { get; set; }

        public DateTime? CellPhoneDate { get; set; }

        public int? OfficialHBId { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string OfficialHBDescription { get; set; }

        [Column(TypeName = "varchar(4)")]
        public string SucursalHBId { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string SucursalHBDescription { get; set; }

        public int? BlockCreditSituation { get; set; }

        public DateTime? MobileLastLogon { get; set; }

        public int? MobileLoginAttempts { get; set; }

        public DateTime? MobileLoginAttemptsDate { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string FacebookLink { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string TwitterLink { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string LinkedInLink { get; set; }

        public bool? ActiveKeySMS { get; set; }

        public DateTime? ActiveKeyDate { get; set; }

        public int? SMSAttemps { get; set; }

        public int? ForgotPasswordAttemps { get; set; }

        public DateTime? ForgotPasswordAttempsDate { get; set; }

        public int? CellCompanyIdInformation { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string CellPhoneInformation { get; set; }

        public DateTime? CellPhoneDateInformation { get; set; }

        public DateTime? SurveyAnswered { get; set; }

        public DateTime? SurveyNotAnswered { get; set; }

        public int? EmailEntryAttempts { get; set; }

        public int? TokenEntryAttempts { get; set; }

        public DateTime? DateTimeEntryAttempts { get; set; }

        public int? CellPhoneEntryAttempts { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PersonId { get; set; }

        // relations
        public Usuario User { get; set; }
    }
}