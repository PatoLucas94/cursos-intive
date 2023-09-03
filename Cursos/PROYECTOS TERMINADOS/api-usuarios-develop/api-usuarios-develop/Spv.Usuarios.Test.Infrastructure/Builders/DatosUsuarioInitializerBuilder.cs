using System;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.Test.Infrastructure.Builders
{
    public class DatosUsuarioInitializerBuilder
    {
        protected DatosUsuarioInitializerBuilder()
        {
        }

        private static DatosUsuario WithDefaultValues()
        {
            return new DatosUsuario
            {
                ActiveKeySMS = true,
                BlockCreditSituation = 1,
                CellCompanyId = 1,
                CellCompanyIdInformation = 1,
                CellPhone = "3512542094",
                CellPhoneEntryAttempts = 1,
                EmailEntryAttempts = 1,
                ForgotPasswordAttemps = 1,
                MobileLoginAttempts = 1,
                OfficialHBId = 1,
                ReceiveMail = true,
                ReceiveSMS = true,
                SMSAttemps = 1,
                TokenEntryAttempts = 1,
                WorkPhone = "84731082794",
                ActiveKeyDate = DateTime.Today.AddDays(-10),
                CellPhoneDate = DateTime.Today.AddDays(-100),
                CellPhoneDateInformation = DateTime.Today.AddDays(-10),
                DateTimeEntryAttempts = DateTime.Today.AddDays(-10),
                ForgotPasswordAttempsDate = DateTime.Today.AddDays(-10),
                MobileLastLogon = DateTime.Today.AddDays(-100),
                MobileLoginAttemptsDate = DateTime.Today.AddDays(-10),
                SurveyAnswered = DateTime.Today.AddDays(-10),
                SurveyNotAnswered = DateTime.Today.AddDays(-10),
                WorkPhoneDate = DateTime.Today.AddDays(-10),
                CellPhoneInformation = "Cell Information Test",
                FacebookLink = "Facebook link Test",
                LinkedInLink = "LinkedIn Link Test",
                OfficialHBDescription = "Official HBI description test",
                SucursalHBDescription = "HBD description test",
                SucursalHBId = "HBI id test",
                TwitterLink = "Twitter link test"
            };
        }

        public static DatosUsuario GetDatosUsuarioWithDefaultValues(
            int UserDataId,
            int UserId,
            string PersonId,
            string Email)
        {
            var datosUsuario = WithDefaultValues();

            datosUsuario.UserId = UserId;
            datosUsuario.UserDataId = UserDataId;
            datosUsuario.PersonId = PersonId;
            datosUsuario.Mail = Email;

            return datosUsuario;
        }

        public static DatosUsuario GetDatosUsuarioWithoutDefaultValues(
            int UserDataId,
            int UserId,
            string PersonId,
            string Email)
        {
            var datosUsuario = new DatosUsuario
            {
                UserId = UserId,
                UserDataId = UserDataId,
                PersonId = PersonId,
                Mail = Email
            };

            return datosUsuario;
        }
    }
}

