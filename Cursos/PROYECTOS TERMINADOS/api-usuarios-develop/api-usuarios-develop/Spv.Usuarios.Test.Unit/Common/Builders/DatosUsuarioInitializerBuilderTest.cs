using System;
using FluentAssertions;
using Spv.Usuarios.Test.Infrastructure.Builders;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Common.Builders
{
    public class DatosUsuarioInitializerBuilderTest : DatosUsuarioInitializerBuilder
    {
        int UserDataId = 123;
        int UserId = 456;
        string PersonId = "123543567";
        string Email = "Emailtest@test.com";

        public DatosUsuarioInitializerBuilderTest() : base()
        {

        }

        [Fact]
        public void WithoutDefaultValuesTest()
        {
            var datosUsuario = GetDatosUsuarioWithoutDefaultValues(UserDataId, UserId, PersonId, Email);

            datosUsuario.UserId.Should().Be(UserId);
            datosUsuario.UserDataId.Should().Be(UserDataId);
            datosUsuario.PersonId.Should().Be(PersonId);
            datosUsuario.Mail.Should().Be(Email);
            datosUsuario.ActiveKeyDate.Should().Be(null);
            datosUsuario.ActiveKeySMS.Should().Be(null);
            datosUsuario.BlockCreditSituation.Should().Be(null);
            datosUsuario.CellCompanyId.Should().Be(null);
            datosUsuario.CellCompanyIdInformation.Should().Be(null);
            datosUsuario.CellPhone.Should().Be(null);
            datosUsuario.CellPhoneDate.Should().Be(null);
            datosUsuario.CellPhoneDateInformation.Should().Be(null);
            datosUsuario.CellPhoneEntryAttempts.Should().Be(null);
            datosUsuario.CellPhoneInformation.Should().Be(null);
            datosUsuario.DateTimeEntryAttempts.Should().Be(null);
            datosUsuario.EmailEntryAttempts.Should().Be(null);
            datosUsuario.FacebookLink.Should().Be(null);
            datosUsuario.ForgotPasswordAttemps.Should().Be(null);
            datosUsuario.ForgotPasswordAttempsDate.Should().Be(null);
            datosUsuario.LinkedInLink.Should().Be(null);
            datosUsuario.MobileLastLogon.Should().Be(null);
            datosUsuario.MobileLoginAttempts.Should().Be(null);
            datosUsuario.MobileLoginAttemptsDate.Should().Be(null);
            datosUsuario.OfficialHBDescription.Should().Be(null);
            datosUsuario.OfficialHBId.Should().Be(null);
            datosUsuario.ReceiveMail.Should().Be(null);
            datosUsuario.ReceiveSMS.Should().Be(null);
            datosUsuario.SMSAttemps.Should().Be(null);
            datosUsuario.SucursalHBDescription.Should().Be(null);
            datosUsuario.SucursalHBId.Should().Be(null);
            datosUsuario.SurveyAnswered.Should().Be(null);
            datosUsuario.SurveyNotAnswered.Should().Be(null);
            datosUsuario.TokenEntryAttempts.Should().Be(null);
            datosUsuario.TwitterLink.Should().Be(null);
            datosUsuario.WorkPhone.Should().Be(null);
            datosUsuario.WorkPhoneDate.Should().Be(null);
        }

        [Fact]
        public void WithDefaultValuesTest()
        {
            var datosUsuario = GetDatosUsuarioWithDefaultValues(UserDataId, UserId, PersonId, Email);

            datosUsuario.UserId.Should().Be(UserId);
            datosUsuario.UserDataId.Should().Be(UserDataId);
            datosUsuario.PersonId.Should().Be(PersonId);
            datosUsuario.Mail.Should().Be(Email);

            datosUsuario.ActiveKeyDate.Should().Be(DateTime.Today.AddDays(-10));
            datosUsuario.ActiveKeySMS.Should().Be(true);
            datosUsuario.BlockCreditSituation.Should().Be(1);
            datosUsuario.CellCompanyId.Should().Be(1);
            datosUsuario.CellCompanyIdInformation.Should().Be(1);
            datosUsuario.CellPhone.Should().Be("3512542094");
            datosUsuario.CellPhoneDate.Should().Be(DateTime.Today.AddDays(-100));
            datosUsuario.CellPhoneDateInformation.Should().Be(DateTime.Today.AddDays(-10));
            datosUsuario.CellPhoneEntryAttempts.Should().Be(1);
            datosUsuario.CellPhoneInformation.Should().Be("Cell Information Test");
            datosUsuario.DateTimeEntryAttempts.Should().Be(DateTime.Today.AddDays(-10));
            datosUsuario.EmailEntryAttempts.Should().Be(1);
            datosUsuario.FacebookLink.Should().Be("Facebook link Test");
            datosUsuario.ForgotPasswordAttemps.Should().Be(1);
            datosUsuario.ForgotPasswordAttempsDate.Should().Be(DateTime.Today.AddDays(-10));
            datosUsuario.LinkedInLink.Should().Be("LinkedIn Link Test");
            datosUsuario.MobileLastLogon.Should().Be(DateTime.Today.AddDays(-100));
            datosUsuario.MobileLoginAttempts.Should().Be(1);
            datosUsuario.MobileLoginAttemptsDate.Should().Be(DateTime.Today.AddDays(-10));
            datosUsuario.OfficialHBDescription.Should().Be("Official HBI description test");
            datosUsuario.OfficialHBId.Should().Be(1);
            datosUsuario.ReceiveMail.Should().Be(true);
            datosUsuario.ReceiveSMS.Should().Be(true);
            datosUsuario.SMSAttemps.Should().Be(1);
            datosUsuario.SucursalHBDescription.Should().Be("HBD description test");
            datosUsuario.SucursalHBId.Should().Be("HBI id test");
            datosUsuario.SurveyAnswered.Should().Be(DateTime.Today.AddDays(-10));
            datosUsuario.SurveyNotAnswered.Should().Be(DateTime.Today.AddDays(-10));
            datosUsuario.TokenEntryAttempts.Should().Be(1);
            datosUsuario.TwitterLink.Should().Be("Twitter link test");
            datosUsuario.WorkPhone.Should().Be("84731082794");
            datosUsuario.WorkPhoneDate.Should().Be(DateTime.Today.AddDays(-10));
        }
    }
}
