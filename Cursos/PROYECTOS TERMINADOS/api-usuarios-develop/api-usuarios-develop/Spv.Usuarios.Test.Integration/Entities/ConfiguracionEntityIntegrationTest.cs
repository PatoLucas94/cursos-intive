using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Entities
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ConfiguracionEntityIntegrationTest
    {
        private readonly IConfiguracionRepository _configuracionRepository;
        public ConfiguracionEntityIntegrationTest(ServerFixture server)
        {
            _configuracionRepository = server.HttpServer.TestServer.Services.GetRequiredService<IConfiguracionRepository>();
        }

        [Fact]
        public void ConfiguracionEntity()
        {
            // Act
            var configuracion = _configuracionRepository.Find(u => 
                u.ConfigurationId == 2);

            // Assert
            configuracion.Should().NotBeNull();

            configuracion.ConfigurationId.Should().Be(2);
            configuracion.Name.Should().Be(AppConstants.CantidadDeIntentosDeLoginKey);
            configuracion.Type.Should().Be(AppConstants.ConfigurationTypeUsers);
            configuracion.Value.Should().Be("3");
        }

        [Fact]
        public void Configuracion_CantidadDeHistorialDeCambiosDeClave()
        {
            // Act
            var configuracion = _configuracionRepository.Find(u =>
                u.ConfigurationId == 4);

            // Assert
            configuracion.Should().NotBeNull();

            configuracion.ConfigurationId.Should().Be(4);
            configuracion.Name.Should().Be(AppConstants.CantidadDeHistorialDeCambiosDeClave);
            configuracion.Type.Should().Be(AppConstants.ConfigurationTypeUsers);
            configuracion.Value.Should().Be("3");
        }
    }
}
