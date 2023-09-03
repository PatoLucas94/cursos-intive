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
    public class DatosUsuarioEntityIntegrationTest
    {
        private readonly IDatosUsuarioRepository _datosUsuarioRepository;

        public DatosUsuarioEntityIntegrationTest(ServerFixture server)
        {
            _datosUsuarioRepository = server.HttpServer.TestServer.Services.GetRequiredService<IDatosUsuarioRepository>();
        }

        [Fact]
        public void DatosUsuarioEntity()
        {
            // Act
            var datosUsuario = _datosUsuarioRepository
                .Get(u => u.UserId == 5, o => o.OrderByDescending(u => u.UserId), nameof(DatosUsuario.User))
                .FirstOrDefault();

            // Assert
            datosUsuario.Should().NotBeNull();

            if (datosUsuario == null) return;

            datosUsuario.UserId.Should().Be(5);
            datosUsuario.UserDataId.Should().Be(5);
            datosUsuario.PersonId.Should().Be("10005");
            datosUsuario.Mail.Should().Be("test5@test.com");
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

            // relations
            datosUsuario.User.UserId.Should().Be(5);
            datosUsuario.User.CustomerNumber.Should().Be("0800412345678");
            datosUsuario.User.SessionId.Should().Be("Numero_de_session");
            datosUsuario.User.UserName.Should().Be("UsuarioTest5");
            datosUsuario.User.Password.Should().Be("+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8");
            datosUsuario.User.UserStatusId.Should().Be((byte)UserStatus.Active);
            datosUsuario.User.Name.Should().Be("Nombre_usuario");
            datosUsuario.User.LastName.Should().Be("Apellido_usuario");
            datosUsuario.User.LastPasswordChange.Should().Be(DateTime.Today.AddDays(-181));
            datosUsuario.User.CreatedDate.Should().Be(DateTime.Today.AddDays(-50));
            datosUsuario.User.DocumentCountryId.Should().Be("080");
            datosUsuario.User.DocumentTypeId.Should().Be(4);
            datosUsuario.User.DocumentNumber.Should().Be("12345678");
            datosUsuario.User.LastLogon.Should().Be(DateTime.Today.AddDays(-50));
            datosUsuario.User.LoginAttempts.Should().Be(0);
            datosUsuario.User.LoginAttemptsDate.Should().Be(DateTime.Today.AddDays(-50));
            datosUsuario.User.MobileEnabled.Should().Be(true);
            datosUsuario.User.ReceiptExtract.Should().Be(true);
            datosUsuario.User.ReceiptExtractDate.Should().Be(DateTime.Today.AddDays(-50));
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
            datosUsuario.User.MarcaUDFDate.Should().Be(DateTime.Today.AddDays(-50));
        }
    }
}
