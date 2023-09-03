using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Entities.V2
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ConfiguracionV2EntityIntegrationTest
    {
        private readonly IConfiguracionV2Repository _configuracionV2Repository;
        public ConfiguracionV2EntityIntegrationTest(ServerFixture server)
        {
            _configuracionV2Repository = server.HttpServer.TestServer.Services.GetRequiredService<IConfiguracionV2Repository>();
        }

        [Fact]
        public void ConfiguracionEntity()
        {
            // Act
            var configuracionV2 = _configuracionV2Repository.Find(u => 
                u.Description == "Cantidad de intentos de login en front.");

            // Assert
            configuracionV2.Should().NotBeNull();

            configuracionV2.ConfigurationId.Should().Be(2);
            configuracionV2.Description.Should().Be("Cantidad de intentos de login en front.");
            configuracionV2.IsSecurity.Should().Be(false);
            configuracionV2.Name.Should().Be(AppConstants.CantidadDeIntentosDeLoginKey);
            configuracionV2.Rol.Should().Be(AppConstants.ConfigurationRolConfiguration);
            configuracionV2.Type.Should().Be(AppConstants.ConfigurationTypeUsers);
            configuracionV2.Value.Should().Be("5");
        }

        [Fact]
        public void Configuracion_CantidadDeHistorialDeCambiosDeClave()
        {
            // Act
            var configuracionV2 = _configuracionV2Repository.Find(u =>
                u.Description == "Cantidad de contraseñas guardadas en el histórico.");

            // Assert
            configuracionV2.Should().NotBeNull();

            configuracionV2.ConfigurationId.Should().Be(3);
            configuracionV2.Description.Should().Be("Cantidad de contraseñas guardadas en el histórico.");
            configuracionV2.IsSecurity.Should().Be(false);
            configuracionV2.Name.Should().Be(AppConstants.CantidadDeHistorialDeCambiosDeClave);
            configuracionV2.Rol.Should().Be(AppConstants.ConfigurationRolConfiguration);
            configuracionV2.Type.Should().Be(AppConstants.ConfigurationTypeUsers);
            configuracionV2.Value.Should().Be("5");
        }
    }
}
