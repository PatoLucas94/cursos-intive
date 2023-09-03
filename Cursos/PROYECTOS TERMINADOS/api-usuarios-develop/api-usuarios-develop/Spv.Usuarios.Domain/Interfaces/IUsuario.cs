using System;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.Domain.Interfaces
{
    public interface IUsuario
    {
        string GetName();
        string GetLastName();
        int GetUserId();
        long? GetPersonId();
        string GetUserName();
        int GetDocumentCountryId();
        int GetDocumentType();
        string GetDocumentNumber();
        DateTime GetCreatedDate();
        DateTime? GetLastPasswordChange();
        string GetPassword();
        int GetUserStatusId();
        int? GetLoginAttempts();
        DateTime? GetLastLogon();
        DateTime? GetLoginAttemptsDate();
        DatosUsuario GetDatosUsuario();
        bool? GetIsEmployee();

        void SetUserId(int id);
        void SetPersonId(string personId);
        void SetUserName(string userName);
        void SetDocumentType(int docType);
        void SetDocumentNumber(string docNumber);
        void SetCreatedDate(DateTime date);
        void SetLastPasswordChange(DateTime date);
        void SetPassword(string password);
        void SetUserStatusId(byte status);
        void SetLoginAttempts(int? attempts);
        void SetLastLogon(DateTime? date);
        void SetLoginAttemptsDate(DateTime? date);
    }
}
