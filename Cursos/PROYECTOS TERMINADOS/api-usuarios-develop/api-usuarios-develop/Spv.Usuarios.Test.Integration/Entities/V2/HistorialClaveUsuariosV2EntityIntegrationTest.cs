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
    public class HistorialClaveUsuariosV2EntityIntegrationTest
    {
        private readonly IHistorialClaveUsuariosV2Repository _historialClaveUsuariosV2Repository;
        public HistorialClaveUsuariosV2EntityIntegrationTest(ServerFixture server)
        {
            _historialClaveUsuariosV2Repository = server.HttpServer.TestServer.Services.GetRequiredService<IHistorialClaveUsuariosV2Repository>();
        }

        [Fact]
        public void AuditoriaLogEntity()
        {
            // Act
            var historialClaveUsuarios = _historialClaveUsuariosV2Repository.Find(h => h.PasswordHistoryId == 1);

            // Assert
            historialClaveUsuarios.Should().NotBeNull();

            historialClaveUsuarios.PasswordHistoryId.Should().Be(1);
            historialClaveUsuarios.AuditLogId.Should().Be(1);
            historialClaveUsuarios.UserId.Should().Be(1);
            historialClaveUsuarios.CreationDate.Should().Be(DateTime.MinValue);
            historialClaveUsuarios.Password.Should().Be("Password");
        }

        [Fact]
        public void AuditoriaLogConUsuarioEntity()
        {
            // Act
            var historialClaveUsuarios = _historialClaveUsuariosV2Repository
                .Get(h => h.PasswordHistoryId == 2, o => o.OrderByDescending(h => h.PasswordHistoryId), nameof(HistorialClaveUsuariosV2.Usuario))
                .FirstOrDefault();

            // Assert
            historialClaveUsuarios.Should().NotBeNull();

            if (historialClaveUsuarios == null) return;

            historialClaveUsuarios.PasswordHistoryId.Should().Be(2);
            historialClaveUsuarios.GetAuditLogId().Should().Be(2);
            historialClaveUsuarios.GetUserId().Should().Be(6);
            historialClaveUsuarios.GetCreationDate().Should().Be(DateTime.MinValue);
            historialClaveUsuarios.GetPassword().Should().Be("Password");

            historialClaveUsuarios.Usuario.Should().NotBeNull();
            historialClaveUsuarios.Usuario.DocumentTypeId.Should().Be(1);
            historialClaveUsuarios.Usuario.DocumentNumber.Should().Be("61234567");
            historialClaveUsuarios.Usuario.CreatedDate.Should().Be(DateTime.MinValue);
            historialClaveUsuarios.Usuario.LastLogon.Should().Be(DateTime.MinValue);
            historialClaveUsuarios.Usuario.LastPasswordChange.Should().Be(DateTime.MinValue);

            historialClaveUsuarios.Usuario.LoginAttempts.Should().Be(0);
            historialClaveUsuarios.Usuario.Password.Should().Be("+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8");
            historialClaveUsuarios.Usuario.PersonId.Should().Be(6);
            historialClaveUsuarios.Usuario.Username.Should().Be("9Nzx6HQc6dZe19ctn0UokqNPQunC5on6TKdl72wW+Ys");
            historialClaveUsuarios.Usuario.UserStatusId.Should().Be(3);
        }
    }
}
