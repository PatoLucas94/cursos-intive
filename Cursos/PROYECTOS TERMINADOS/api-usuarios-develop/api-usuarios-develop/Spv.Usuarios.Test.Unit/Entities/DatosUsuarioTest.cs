using System;
using FluentAssertions;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Enums;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities
{
    public class DatosUsuarioTest
    {
        [Fact]
        public void DatosUsuario()
        {
            //Arrange
            var datosUsuario = new DatosUsuario
            {
                UserDataId = 1,
                UserId = 1,
                PersonId = "PersonIdTest",
                Mail = "test@test.com",
                ActiveKeyDate = DateTime.MinValue,
                ActiveKeySMS = true,
                BlockCreditSituation = 1,
                CellCompanyId = 1,
                CellCompanyIdInformation = 1,
                CellPhone = "3512542094",
                CellPhoneDate = DateTime.MinValue,
                CellPhoneDateInformation = DateTime.MinValue,
                CellPhoneEntryAttempts = 1,
                CellPhoneInformation = "Cell Information Test",
                DateTimeEntryAttempts = DateTime.MinValue,
                EmailEntryAttempts = 1,
                FacebookLink = "Facebook link Test",
                ForgotPasswordAttemps = 1,
                ForgotPasswordAttempsDate = DateTime.MinValue,
                LinkedInLink = "LinkedIn Link Test",
                MobileLastLogon = DateTime.MinValue,
                MobileLoginAttempts = 1,
                MobileLoginAttemptsDate = DateTime.MinValue,
                OfficialHBDescription = "HBI description test",
                OfficialHBId = 1,
                ReceiveMail = true,
                ReceiveSMS = true,
                SMSAttemps = 1,
                SucursalHBDescription = "HBD description test",
                SucursalHBId = "HBI id test",
                SurveyAnswered = DateTime.MinValue,
                SurveyNotAnswered = DateTime.MinValue,
                TokenEntryAttempts = 1,
                TwitterLink = "Twitter link test",
                WorkPhone = "84731082794",
                WorkPhoneDate = DateTime.MinValue,
                User = new Usuario
                {
                    UserId = 5,
                    CustomerNumber = "0800412345678",
                    SessionId = "Numero_de_session",
                    UserName = "UsuarioTest5",
                    Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8",
                    UserStatusId = (byte)UserStatus.Active,
                    Name = "Nombre_usuario",
                    LastName = "Apellido_usuario",
                    LastPasswordChange = DateTime.Today.AddDays(-181),
                    CreatedDate = DateTime.MinValue,
                    DocumentCountryId = "080",
                    DocumentTypeId = 4,
                    DocumentNumber = "12345678",
                    LastLogon = DateTime.MinValue,
                    LoginAttempts = 0,
                    LoginAttemptsDate = DateTime.MinValue,
                    MobileEnabled = true,
                    ReceiptExtract = true,
                    ReceiptExtractDate = DateTime.MinValue,
                    IsEmployee = false,
                    CUIL = "20123456785",
                    IsResident = true,
                    FullControl = true,
                    Culture = "",
                    SecurityQuestion = "",
                    SecurityAnswer = "",
                    ChannelSource = "MOB",
                    BenefitClubId = 1,
                    MarcaUDF = true,
                    MarcaUDFDate = DateTime.MinValue
                }
            };

            //Assert
            datosUsuario.UserDataId.Should().Be(1);
            datosUsuario.UserId.Should().Be(1);
            datosUsuario.PersonId.Should().Be("PersonIdTest");
            datosUsuario.Mail.Should().Be("test@test.com");
            datosUsuario.ActiveKeyDate.Should().Be(DateTime.MinValue);
            datosUsuario.ActiveKeySMS.Should().Be(true);
            datosUsuario.BlockCreditSituation.Should().Be(1);
            datosUsuario.CellCompanyId.Should().Be(1);
            datosUsuario.CellCompanyIdInformation.Should().Be(1);
            datosUsuario.CellPhone.Should().Be("3512542094");
            datosUsuario.CellPhoneDate.Should().Be(DateTime.MinValue);
            datosUsuario.CellPhoneDateInformation.Should().Be(DateTime.MinValue);
            datosUsuario.CellPhoneEntryAttempts.Should().Be(1);
            datosUsuario.CellPhoneInformation.Should().Be("Cell Information Test");
            datosUsuario.DateTimeEntryAttempts.Should().Be(DateTime.MinValue);
            datosUsuario.EmailEntryAttempts.Should().Be(1);
            datosUsuario.FacebookLink.Should().Be("Facebook link Test");
            datosUsuario.ForgotPasswordAttemps.Should().Be(1);
            datosUsuario.ForgotPasswordAttempsDate.Should().Be(DateTime.MinValue);
            datosUsuario.LinkedInLink.Should().Be("LinkedIn Link Test");
            datosUsuario.MobileLastLogon.Should().Be(DateTime.MinValue);
            datosUsuario.MobileLoginAttempts.Should().Be(1);
            datosUsuario.MobileLoginAttemptsDate.Should().Be(DateTime.MinValue);
            datosUsuario.OfficialHBDescription.Should().Be("HBI description test");
            datosUsuario.OfficialHBId.Should().Be(1);
            datosUsuario.ReceiveMail.Should().Be(true);
            datosUsuario.ReceiveSMS.Should().Be(true);
            datosUsuario.SMSAttemps.Should().Be(1);
            datosUsuario.SucursalHBDescription.Should().Be("HBD description test");
            datosUsuario.SucursalHBId.Should().Be("HBI id test");
            datosUsuario.SurveyAnswered.Should().Be(DateTime.MinValue);
            datosUsuario.SurveyNotAnswered.Should().Be(DateTime.MinValue);
            datosUsuario.TokenEntryAttempts.Should().Be(1);
            datosUsuario.TwitterLink.Should().Be("Twitter link test");
            datosUsuario.WorkPhone.Should().Be("84731082794");
            datosUsuario.WorkPhoneDate.Should().Be(DateTime.MinValue);

            datosUsuario.User.UserId.Should().Be(5);
            datosUsuario.User.CustomerNumber.Should().Be("0800412345678");
            datosUsuario.User.SessionId.Should().Be("Numero_de_session");
            datosUsuario.User.UserName.Should().Be("UsuarioTest5");
            datosUsuario.User.Password.Should().Be("+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8");
            datosUsuario.User.UserStatusId.Should().Be((byte)UserStatus.Active);
            datosUsuario.User.Name.Should().Be("Nombre_usuario");
            datosUsuario.User.LastName.Should().Be("Apellido_usuario");
            datosUsuario.User.LastPasswordChange.Should().Be(DateTime.Today.AddDays(-181));
            datosUsuario.User.CreatedDate.Should().Be(DateTime.MinValue);
            datosUsuario.User.DocumentCountryId.Should().Be("080");
            datosUsuario.User.DocumentTypeId.Should().Be(4);
            datosUsuario.User.DocumentNumber.Should().Be("12345678");
            datosUsuario.User.LastLogon.Should().Be(DateTime.MinValue);
            datosUsuario.User.LoginAttempts.Should().Be(0);
            datosUsuario.User.LoginAttemptsDate.Should().Be(DateTime.MinValue);
            datosUsuario.User.MobileEnabled.Should().Be(true);
            datosUsuario.User.ReceiptExtract.Should().Be(true);
            datosUsuario.User.ReceiptExtractDate.Should().Be(DateTime.MinValue);
            datosUsuario.User.IsEmployee.Should().Be(false);
            datosUsuario.User.CUIL.Should().Be("20123456785");
            datosUsuario.User.IsResident.Should().Be(true);
            datosUsuario.User.FullControl.Should().Be(true);
            datosUsuario.User.Culture.Should().Be("");
            datosUsuario.User.SecurityQuestion.Should().Be("");
            datosUsuario.User.SecurityAnswer.Should().Be("");
            datosUsuario.User.ChannelSource.Should().Be("MOB");
            datosUsuario.User.BenefitClubId.Should().Be(1);
            datosUsuario.User.MarcaUDF.Should().Be(true);
            datosUsuario.User.MarcaUDFDate.Should().Be(DateTime.MinValue);
        }
    }
}
