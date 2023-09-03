using System;
using System.Collections.Generic;
using Spv.Usuarios.Domain.Exceptions;
using Spv.Usuarios.Domain.Interfaces;

namespace Spv.Usuarios.Domain.Entities.V2
{
    public class UsuarioV2 : IUsuario
    {
        public int UserId { get; set; }
        public long? PersonId { get; set; }
        public int DocumentCountryId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte UserStatusId { get; set; }
        public int? LoginAttempts { get; set; }
        public DateTime? LastPasswordChange { get; set; }
        public DateTime? LastLogon { get; set; }
        public DateTime CreatedDate { get; set; }

        // relations
        public EstadosUsuarioV2 Status { get; set; }
        public List<HistorialClaveUsuariosV2> UserPasswordHistory { get; set; }
        public List<HistorialUsuarioUsuariosV2> UserUsernameHistory { get; set; }

        public DateTime GetCreatedDate()
        {
            return CreatedDate;
        }

        public string GetDocumentNumber() => Convert.ToInt64(DocumentNumber).ToString();

        public int GetDocumentCountryId()
        {
            return DocumentCountryId;
        }

        public int GetDocumentType()
        {
            return DocumentTypeId;
        }

        public DateTime? GetLastLogon()
        {
            return LastLogon;
        }

        public DateTime? GetLastPasswordChange()
        {
            return LastPasswordChange;
        }

        public int? GetLoginAttempts()
        {
            return LoginAttempts;
        }

        public DateTime? GetLoginAttemptsDate()
        {
            return null;
        }

        public DatosUsuario GetDatosUsuario() => null;

        public string GetPassword()
        {
            return Password;
        }

        public long? GetPersonId()
        {
            return PersonId;
        }

        public int GetUserId()
        {
            return UserId;
        }

        public string GetUserName()
        {
            return Username;
        }

        public int GetUserStatusId()
        {
            return UserStatusId;
        }

        public void SetCreatedDate(DateTime date)
        {
            CreatedDate = date;
        }

        public void SetDocumentNumber(string docNumber)
        {
            DocumentNumber = docNumber;
        }

        public void SetDocumentType(int docType)
        {
            DocumentTypeId = docType;
        }

        public void SetLastLogon(DateTime? date)
        {
            LastLogon = date;
        }

        public void SetLastPasswordChange(DateTime date)
        {
            LastPasswordChange = date;
        }

        public void SetLoginAttempts(int? attempts)
        {
            LoginAttempts = attempts;
        }

        public void SetLoginAttemptsDate(DateTime? date)
        {
            // no se utiliza debido a que en la version 2 del modelo no se tiene esta propiedad en el Usuario
        }

        public void SetPassword(string password)
        {
            Password = password;
        }

        public void SetPersonId(string personId)
        {
            if (long.TryParse(personId, out var result))
            {
                PersonId = result;
            }
            else
            {
                throw new BusinessException($"No se pudo convertir 'PersonId' con el valor: {personId}", 0);
            }
        }

        public void SetUserId(int id)
        {
            UserId = id;
        }

        public void SetUserName(string userName)
        {
            Username = userName;
        }

        public void SetUserStatusId(byte status)
        {
            UserStatusId = status;
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public string GetLastName()
        {
            throw new NotImplementedException();
        }

        public bool? GetIsEmployee() => null;
    }
}
