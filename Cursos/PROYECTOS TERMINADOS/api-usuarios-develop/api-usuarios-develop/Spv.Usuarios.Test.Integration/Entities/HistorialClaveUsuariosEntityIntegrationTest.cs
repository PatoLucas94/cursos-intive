using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Entities
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class HistorialClaveUsuariosEntityIntegrationTest
    {
        private readonly IHistorialClaveUsuariosRepository _historialClaveUsuariosRepository;
        public HistorialClaveUsuariosEntityIntegrationTest(ServerFixture server)
        {
            _historialClaveUsuariosRepository = server.HttpServer.TestServer.Services.GetRequiredService<IHistorialClaveUsuariosRepository>();
        }

        [Fact]
        public void HistorialClavesEntity()
        {
            // Act
            var historialClaveUsuarios = _historialClaveUsuariosRepository.Find(h => h.PasswordHistoryId == 1);

            // Assert
            historialClaveUsuarios.Should().NotBeNull();

            historialClaveUsuarios.PasswordHistoryId.Should().Be(1);
            historialClaveUsuarios.GetUserId().Should().Be(1);
            historialClaveUsuarios.GetCreationDate().Should().Be(DateTime.MinValue);
            historialClaveUsuarios.GetPassword().Should().Be("DzvVZ1a7J+SUZ/nI83o/OQXzKOhmAbiGSEu7LtNaXWc");
        }
    }
}
