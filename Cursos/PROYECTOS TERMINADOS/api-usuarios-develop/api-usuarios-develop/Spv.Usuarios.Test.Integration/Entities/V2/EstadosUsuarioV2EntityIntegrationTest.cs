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
    public class EstadosUsuarioV2EntityIntegrationTest
    {
        private readonly IEstadosUsuarioV2Repository _estadosUsuarioV2Repository;
        public EstadosUsuarioV2EntityIntegrationTest(ServerFixture server)
        {
            _estadosUsuarioV2Repository = server.HttpServer.TestServer.Services.GetRequiredService<IEstadosUsuarioV2Repository>();
        }

        [Fact]
        public void UsuarioEntity()
        {
            // Act
            var estadosUsuarioV2 = _estadosUsuarioV2Repository.Find(u => u.Description == "State 1");

            // Assert
            estadosUsuarioV2.Should().NotBeNull();

            estadosUsuarioV2.UserStatusId.Should().Be(1);
            estadosUsuarioV2.Description.Should().Be("State 1");
        }

        [Fact]
        public void EstadoConUsuariosEntity()
        {
            // Act
            var estadosUsuarioV2 = _estadosUsuarioV2Repository
                .Get(u => u.Description == "State 7", o => o.OrderByDescending(u => u.UserStatusId), nameof(EstadosUsuarioV2.Users))
                .FirstOrDefault();

            // Assert
            estadosUsuarioV2.Should().NotBeNull();

            estadosUsuarioV2.UserStatusId.Should().Be(7);
            estadosUsuarioV2.Description.Should().Be("State 7");
            estadosUsuarioV2.Users.Should().NotBeNull();
            estadosUsuarioV2.Users.Count.Should().Be(1);

        }
    }
}
