using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Entities
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class UsuarioEntityIntegrationTest
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioEntityIntegrationTest(ServerFixture server)
        {
            _usuarioRepository = server.HttpServer.TestServer.Services.GetRequiredService<IUsuarioRepository>();
        }

        [Fact]
        public void UsuarioEntity()
        {
            // Act
            var usuario = _usuarioRepository.Find(u => u.UserId == 5);

            // Assert
            usuario.Should().NotBeNull();

            usuario.UserId.Should().Be(5);
            usuario.CustomerNumber.Should().Be("0800412345678");
            usuario.SessionId.Should().Be("Numero_de_session");
            usuario.UserName.Should().Be("UsuarioTest5");
            usuario.Password.Should().Be("+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8");
            usuario.UserStatusId.Should().Be((byte)UserStatus.Active);
            usuario.Name.Should().Be("Nombre_usuario");
            usuario.LastName.Should().Be("Apellido_usuario");
            usuario.LastPasswordChange.Should().Be(DateTime.Today.AddDays(-181));
            usuario.CreatedDate.Should().Be(DateTime.Today.AddDays(-50));
            usuario.DocumentCountryId.Should().Be("080");
            usuario.DocumentTypeId.Should().Be(4);
            usuario.DocumentNumber.Should().Be("12345678");
            usuario.LastLogon.Should().Be(DateTime.Today.AddDays(-50));
            usuario.LoginAttempts.Should().Be(0);
            usuario.LoginAttemptsDate.Should().Be(DateTime.Today.AddDays(-50));
            usuario.MobileEnabled.Should().Be(true);
            usuario.ReceiptExtract.Should().Be(true);
            usuario.ReceiptExtractDate.Should().Be(DateTime.Today.AddDays(-50));
            usuario.IsEmployee.Should().Be(false);
            usuario.CUIL.Should().Be("20123456785");
            usuario.IsResident.Should().Be(true);
            usuario.FullControl.Should().Be(true);
            usuario.Culture.Should().Be("");
            usuario.SecurityQuestion.Should().Be("");
            usuario.SecurityAnswer.Should().Be("");
            usuario.ChannelSource.Should().Be("MOB");
            usuario.BenefitClubId.Should().Be(1);
            usuario.MarcaUDF.Should().Be(true);
            usuario.MarcaUDFDate.Should().Be(DateTime.Today.AddDays(-50));
        }

        [Fact]
        public void UsuarioConDatosUsuarioEntity()
        {
            // Act
            var usuario = _usuarioRepository
                .Get(u => u.UserName == "UsuarioTest5", o => o.OrderByDescending(u => u.UserId), nameof(Usuario.UserData))
                .FirstOrDefault();

            // Assert
            usuario.Should().NotBeNull();

            if (usuario == null) return;

            usuario.UserId.Should().Be(5);
            usuario.CustomerNumber.Should().Be("0800412345678");
            usuario.SessionId.Should().Be("Numero_de_session");
            usuario.UserName.Should().Be("UsuarioTest5");
            usuario.Password.Should().Be("+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8");
            usuario.UserStatusId.Should().Be((byte)UserStatus.Active);
            usuario.Name.Should().Be("Nombre_usuario");
            usuario.LastName.Should().Be("Apellido_usuario");
            usuario.LastPasswordChange.Should().Be(DateTime.Today.AddDays(-181));
            usuario.CreatedDate.Should().Be(DateTime.Today.AddDays(-50));
            usuario.DocumentCountryId.Should().Be("080");
            usuario.DocumentTypeId.Should().Be(4);
            usuario.DocumentNumber.Should().Be("12345678");
            usuario.LastLogon.Should().Be(DateTime.Today.AddDays(-50));
            usuario.LoginAttempts.Should().Be(0);
            usuario.LoginAttemptsDate.Should().Be(DateTime.Today.AddDays(-50));
            usuario.MobileEnabled.Should().Be(true);
            usuario.ReceiptExtract.Should().Be(true);
            usuario.ReceiptExtractDate.Should().Be(DateTime.Today.AddDays(-50));
            usuario.IsEmployee.Should().Be(false);
            usuario.CUIL.Should().Be("20123456785");
            usuario.IsResident.Should().Be(true);
            usuario.FullControl.Should().Be(true);
            usuario.Culture.Should().Be("");
            usuario.SecurityQuestion.Should().Be("");
            usuario.SecurityAnswer.Should().Be("");
            usuario.ChannelSource.Should().Be("MOB");
            usuario.BenefitClubId.Should().Be(1);
            usuario.MarcaUDF.Should().Be(true);
            usuario.MarcaUDFDate.Should().Be(DateTime.Today.AddDays(-50));

            // relations
            usuario.UserData.UserId.Should().Be(5);
            usuario.UserData.UserDataId.Should().Be(5);
            usuario.UserData.PersonId.Should().Be("10005");
            usuario.UserData.Mail.Should().Be("test5@test.com");
            usuario.UserData.ActiveKeyDate.Should().Be(DateTime.Today.AddDays(-10));
            usuario.UserData.ActiveKeySMS.Should().Be(true);
            usuario.UserData.BlockCreditSituation.Should().Be(1);
            usuario.UserData.CellCompanyId.Should().Be(1);
            usuario.UserData.CellCompanyIdInformation.Should().Be(1);
            usuario.UserData.CellPhone.Should().Be("3512542094");
            usuario.UserData.CellPhoneDate.Should().Be(DateTime.Today.AddDays(-100));
            usuario.UserData.CellPhoneDateInformation.Should().Be(DateTime.Today.AddDays(-10));
            usuario.UserData.CellPhoneEntryAttempts.Should().Be(1);
            usuario.UserData.CellPhoneInformation.Should().Be("Cell Information Test");
            usuario.UserData.DateTimeEntryAttempts.Should().Be(DateTime.Today.AddDays(-10));
            usuario.UserData.EmailEntryAttempts.Should().Be(1);
            usuario.UserData.FacebookLink.Should().Be("Facebook link Test");
            usuario.UserData.ForgotPasswordAttemps.Should().Be(1);
            usuario.UserData.ForgotPasswordAttempsDate.Should().Be(DateTime.Today.AddDays(-10));
            usuario.UserData.LinkedInLink.Should().Be("LinkedIn Link Test");
            usuario.UserData.MobileLastLogon.Should().Be(DateTime.Today.AddDays(-100));
            usuario.UserData.MobileLoginAttempts.Should().Be(1);
            usuario.UserData.MobileLoginAttemptsDate.Should().Be(DateTime.Today.AddDays(-10));
            usuario.UserData.OfficialHBDescription.Should().Be("Official HBI description test");
            usuario.UserData.OfficialHBId.Should().Be(1);
            usuario.UserData.ReceiveMail.Should().Be(true);
            usuario.UserData.ReceiveSMS.Should().Be(true);
            usuario.UserData.SMSAttemps.Should().Be(1);
            usuario.UserData.SucursalHBDescription.Should().Be("HBD description test");
            usuario.UserData.SucursalHBId.Should().Be("HBI id test");
            usuario.UserData.SurveyAnswered.Should().Be(DateTime.Today.AddDays(-10));
            usuario.UserData.SurveyNotAnswered.Should().Be(DateTime.Today.AddDays(-10));
            usuario.UserData.TokenEntryAttempts.Should().Be(1);
            usuario.UserData.TwitterLink.Should().Be("Twitter link test");
            usuario.UserData.WorkPhone.Should().Be("84731082794");
            usuario.UserData.WorkPhoneDate.Should().Be(DateTime.Today.AddDays(-10));
        }
    }
}
