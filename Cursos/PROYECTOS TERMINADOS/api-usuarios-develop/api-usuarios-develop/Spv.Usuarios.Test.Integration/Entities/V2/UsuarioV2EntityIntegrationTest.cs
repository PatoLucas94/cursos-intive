using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Entities.V2
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class UsuarioV2EntityIntegrationTest
    {
        private readonly IUsuarioV2Repository _usuarioV2Repository;
        public UsuarioV2EntityIntegrationTest(ServerFixture server)
        {
            _usuarioV2Repository = server.HttpServer.TestServer.Services.GetRequiredService<IUsuarioV2Repository>();
        }

        [Fact]
        public void UsuarioEntity()
        {
            // Act
            var usuarioV2 = _usuarioV2Repository.ObtenerUsuarioByDocumentNumber("61234567")
                .FirstOrDefault(x => x.Username.Equals("9Nzx6HQc6dZe19ctn0UokqNPQunC5on6TKdl72wW+Ys"));

            // Assert
            usuarioV2.Should().NotBeNull();

            if (usuarioV2 == null) return;
            usuarioV2.GetDocumentType().Should().Be(1);
            usuarioV2.GetDocumentNumber().Should().Be("61234567");
            usuarioV2.GetCreatedDate().Should().Be(DateTime.MinValue);
            usuarioV2.GetLastLogon().Should().Be(DateTime.MinValue);
            usuarioV2.GetLastPasswordChange().Should().Be(DateTime.MinValue);
            usuarioV2.GetLoginAttempts().Should().Be(0);
            usuarioV2.GetPassword().Should().Be("+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8");
            usuarioV2.GetPersonId().Should().Be(6);
            usuarioV2.GetUserName().Should().Be("9Nzx6HQc6dZe19ctn0UokqNPQunC5on6TKdl72wW+Ys");
            usuarioV2.GetUserStatusId().Should().Be(3);
        }

        [Fact]
        public void UsuarioConEstadoEntity()
        {
            // Act
            var usuarioV2 = _usuarioV2Repository
                .Get(u => u.Username == "9Nzx6HQc6dZe19ctn0UokqNPQunC5on6TKdl72wW+Ys", 
                    o => o.OrderByDescending(u => u.UserId), 
                    nameof(UsuarioV2.Status))
                .FirstOrDefault();

            // Assert
            usuarioV2.Should().NotBeNull();

            if (usuarioV2 == null) return;

            usuarioV2.DocumentTypeId.Should().Be(1);
            usuarioV2.DocumentNumber.Should().Be("61234567");
            usuarioV2.CreatedDate.Should().Be(DateTime.MinValue);
            usuarioV2.LastLogon.Should().Be(DateTime.MinValue);
            usuarioV2.LastPasswordChange.Should().Be(DateTime.MinValue);
            usuarioV2.LoginAttempts.Should().Be(0);
            usuarioV2.Password.Should().Be("+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8");
            usuarioV2.PersonId.Should().Be(6);
            usuarioV2.Username.Should().Be("9Nzx6HQc6dZe19ctn0UokqNPQunC5on6TKdl72wW+Ys");
            usuarioV2.UserStatusId.Should().Be(3);

            usuarioV2.Status.Should().NotBeNull();
            usuarioV2.Status.UserStatusId.Should().Be(3);
            usuarioV2.Status.Description.Should().Be("State 3");
        }

        [Fact]
        public void UsuarioConHistorialClavesEntity()
        {
            // Act
            var usuarioV2 = _usuarioV2Repository
                .Get(u => u.Username == "9Nzx6HQc6dZe19ctn0UokqNPQunC5on6TKdl72wW+Ys", 
                    o => o.OrderByDescending(u => u.UserId), 
                    nameof(UsuarioV2.UserPasswordHistory))
                .FirstOrDefault();

            // Assert
            usuarioV2.Should().NotBeNull();

            if (usuarioV2 == null) return;
           
            usuarioV2.DocumentTypeId.Should().Be(1);
            usuarioV2.DocumentNumber.Should().Be("61234567");
            usuarioV2.CreatedDate.Should().Be(DateTime.MinValue);
            usuarioV2.LastLogon.Should().Be(DateTime.MinValue);
            usuarioV2.LastPasswordChange.Should().Be(DateTime.MinValue);
            usuarioV2.LoginAttempts.Should().Be(0);
            usuarioV2.Password.Should().Be("+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8");
            usuarioV2.PersonId.Should().Be(6);
            usuarioV2.Username.Should().Be("9Nzx6HQc6dZe19ctn0UokqNPQunC5on6TKdl72wW+Ys");
            usuarioV2.UserStatusId.Should().Be(3);

            usuarioV2.UserPasswordHistory.Should().NotBeNull();
            usuarioV2.UserPasswordHistory.Count.Should().BeGreaterThan(0);

            usuarioV2.UserPasswordHistory.First().PasswordHistoryId.Should().Be(2);
            usuarioV2.UserPasswordHistory.First().AuditLogId.Should().Be(2);
            usuarioV2.UserPasswordHistory.First().UserId.Should().Be(6);
            usuarioV2.UserPasswordHistory.First().CreationDate.Should().Be(DateTime.MinValue);
            usuarioV2.UserPasswordHistory.First().Password.Should().Be("Password");
        }

        [Fact]
        public void UsuarioConHistorialUsernamesEntity()
        {
            // Act
            var usuarioV2 = _usuarioV2Repository
                .Get(u => u.Username == "9Nzx6HQc6dZe19ctn0UokqNPQunC5on6TKdl72wW+Ys",
                    o => o.OrderByDescending(u => u.UserId), 
                    nameof(UsuarioV2.UserUsernameHistory))
                .FirstOrDefault();

            // Assert
            usuarioV2.Should().NotBeNull();

            if (usuarioV2 == null) return;

            usuarioV2.DocumentTypeId.Should().Be(1);
            usuarioV2.DocumentNumber.Should().Be("61234567");
            usuarioV2.CreatedDate.Should().Be(DateTime.MinValue);
            usuarioV2.LastLogon.Should().Be(DateTime.MinValue);
            usuarioV2.LastPasswordChange.Should().Be(DateTime.MinValue);
            usuarioV2.LoginAttempts.Should().Be(0);
            usuarioV2.Password.Should().Be("+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8");
            usuarioV2.PersonId.Should().Be(6);
            usuarioV2.Username.Should().Be("9Nzx6HQc6dZe19ctn0UokqNPQunC5on6TKdl72wW+Ys");
            usuarioV2.UserStatusId.Should().Be(3);

            usuarioV2.UserUsernameHistory.Should().NotBeNull();
            usuarioV2.UserUsernameHistory.Count.Should().BeGreaterThan(0);

            usuarioV2.UserUsernameHistory.First().UsernameHistoryId.Should().Be(2);
            usuarioV2.UserUsernameHistory.First().AuditLogId.Should().Be(2);
            usuarioV2.UserUsernameHistory.First().UserId.Should().Be(6);
            usuarioV2.UserUsernameHistory.First().CreationDate.Should().Be(DateTime.MinValue);
            usuarioV2.UserUsernameHistory.First().Username.Should().Be("Username");
        }
    }
}
