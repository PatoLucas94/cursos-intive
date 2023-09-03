using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Spv.Usuarios.Domain.Exceptions;
using Spv.Usuarios.Domain.Interfaces;

namespace Spv.Usuarios.Domain.Entities
{
    [Table("Users")]
    public class Usuario : IUsuario
    {
        [Key]
        public int UserId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string CustomerNumber { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string SessionId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string UserName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Password { get; set; }

        public byte UserStatusId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        public DateTime? LastPasswordChange { get; set; }

        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string DocumentCountryId { get; set; }

        public int? DocumentTypeId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string DocumentNumber { get; set; }

        public DateTime? LastLogon { get; set; }

        public int? LoginAttempts { get; set; }

        public DateTime? LoginAttemptsDate { get; set; }

        public bool MobileEnabled { get; set; }

        public bool? ReceiptExtract { get; set; }

        public DateTime? ReceiptExtractDate { get; set; }

        public bool IsEmployee { get; set; }

        [Column(TypeName = "char(20)")]
        public string CUIL { get; set; }

        public bool? IsResident { get; set; }

        public bool? FullControl { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string Culture { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string SecurityQuestion { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string SecurityAnswer { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string ChannelSource { get; set; }

        public short BenefitClubId { get; set; }

        public bool? MarcaUDF { get; set; }

        public DateTime? MarcaUDFDate { get; set; }

        // relations
        public DatosUsuario UserData { get; set; }

        public string GetName()
        {
            return Name;
        }

        public string GetLastName()
        {
            return LastName;
        }

        public DateTime? GetLoginAttemptsDate()
        {
            return LoginAttemptsDate;
        }

        public DatosUsuario GetDatosUsuario()
        {
            return UserData;
        }

        public DateTime? GetLastLogon()
        {
            return LastLogon;
        }

        public int? GetLoginAttempts()
        {
            return LoginAttempts;
        }

        public string GetPassword()
        {
            return Password;
        }

        public int GetUserId()
        {
            return UserId;
        }

        public int GetUserStatusId()
        {
            return UserStatusId;
        }

        public bool? GetIsEmployee() => IsEmployee;

        public void SetLoginAttemptsDate(DateTime? date)
        {
            LoginAttemptsDate = date;
        }

        public void SetLastLogon(DateTime? date)
        {
            LastLogon = date;
        }

        public void SetLoginAttempts(int? attempts)
        {
            LoginAttempts = attempts;
        }

        public void SetPassword(string password)
        {
            Password = password;
        }

        public void SetUserId(int id)
        {
            UserId = id;
        }

        public void SetUserStatusId(byte status)
        {
            UserStatusId = status;
        }

        public long? GetPersonId()
        {
            if (UserData == null)
            {
                throw new BusinessException(
                    "Los datos de usuario no se han cargado, por lo que no se puede obtener 'PersonId'",
                    0
                );
            }

            return long.TryParse(UserData.PersonId, out var result) ? (long?)result : null;
        }

        public string GetUserName()
        {
            return UserName;
        }

        public int GetDocumentCountryId()
        {
            return int.TryParse(DocumentCountryId, out var result) ? result : 0;
        }

        public int GetDocumentType()
        {
            return DocumentTypeId ?? 0;
        }

        public string GetDocumentNumber() => Convert.ToInt64(DocumentNumber).ToString();

        public DateTime GetCreatedDate()
        {
            return CreatedDate;
        }

        public DateTime? GetLastPasswordChange()
        {
            return LastPasswordChange;
        }

        public void SetPersonId(string personId)
        {
            if (UserData == null)
            {
                throw new BusinessException(
                    "Los datos de usuario no se han cargado, por lo que no se puede obtener 'PersonId'",
                    0
                );
            }

            UserData.PersonId = personId;
        }

        public void SetUserName(string userName)
        {
            UserName = userName;
        }

        public void SetDocumentType(int docType)
        {
            DocumentTypeId = docType;
        }

        public void SetDocumentNumber(string docNumber)
        {
            DocumentNumber = docNumber;
        }

        public void SetCreatedDate(DateTime date)
        {
            CreatedDate = date;
        }

        public void SetLastPasswordChange(DateTime date)
        {
            LastPasswordChange = date;
        }
    }
}
