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
    public class HistorialUsuarioUsuariosV2EntityIntegrationTest
    {
        private readonly IHistorialUsuarioUsuariosV2Repository _historialUsuarioUsuariosV2Repository;
        public HistorialUsuarioUsuariosV2EntityIntegrationTest(ServerFixture server)
        {
            _historialUsuarioUsuariosV2Repository = server.HttpServer.TestServer.Services.GetRequiredService<IHistorialUsuarioUsuariosV2Repository>();
        }

        [Fact]
        public void AuditoriaLogEntity()
        {
            // Act
            var historialUsuarioUsuarios = _historialUsuarioUsuariosV2Repository.Find(h => h.UsernameHistoryId == 1);

            // Assert
            historialUsuarioUsuarios.Should().NotBeNull();

            historialUsuarioUsuarios.UsernameHistoryId.Should().Be(1);
            historialUsuarioUsuarios.AuditLogId.Should().Be(1);
            historialUsuarioUsuarios.UserId.Should().Be(1);
            historialUsuarioUsuarios.CreationDate.Should().Be(DateTime.MinValue);
            historialUsuarioUsuarios.Username.Should().Be("Username");
        }

        [Fact]
        public void AuditoriaLogConUsuarioEntity()
        {
            // Act
            var historialUsuarioUsuarios = _historialUsuarioUsuariosV2Repository
                .Get(h => h.UsernameHistoryId == 2, o => o.OrderByDescending(h => h.UsernameHistoryId), nameof(HistorialClaveUsuariosV2.Usuario))
                .FirstOrDefault();

            // Assert
            historialUsuarioUsuarios.Should().NotBeNull();

            if (historialUsuarioUsuarios == null) return;

            historialUsuarioUsuarios.UsernameHistoryId.Should().Be(2);
            historialUsuarioUsuarios.AuditLogId.Should().Be(2);
            historialUsuarioUsuarios.UserId.Should().Be(6);
            historialUsuarioUsuarios.CreationDate.Should().Be(DateTime.MinValue);
            historialUsuarioUsuarios.Username.Should().Be("Username");

            historialUsuarioUsuarios.Usuario.Should().NotBeNull();
            historialUsuarioUsuarios.Usuario.DocumentTypeId.Should().Be(1);
            historialUsuarioUsuarios.Usuario.DocumentNumber.Should().Be("61234567");
            historialUsuarioUsuarios.Usuario.CreatedDate.Should().Be(DateTime.MinValue);
            historialUsuarioUsuarios.Usuario.LastLogon.Should().Be(DateTime.MinValue);
            historialUsuarioUsuarios.Usuario.LastPasswordChange.Should().Be(DateTime.MinValue);

            historialUsuarioUsuarios.Usuario.LoginAttempts.Should().Be(0);
            historialUsuarioUsuarios.Usuario.Password.Should().Be("+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8");
            historialUsuarioUsuarios.Usuario.PersonId.Should().Be(6);
            historialUsuarioUsuarios.Usuario.Username.Should().Be("9Nzx6HQc6dZe19ctn0UokqNPQunC5on6TKdl72wW+Ys");
            historialUsuarioUsuarios.Usuario.UserStatusId.Should().Be(3);
        }
    }
}
