using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.ConfiguracionesService.Input;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Service;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.ConfiguracioService
{
    public class ObtenerConfiguracionLoginHabilitadoAsyncTest
    {
        [Fact]
        public void ObtenerConfiguracionLoginHabilitadoAsyncVacio()
        {
            var configuracionRepository = new Mock<IConfiguracionRepository>();

            ApiHeaders Headers = new ApiHeaders
            {
                XAplicacion = "app",
                XCanal = "HBI",
                XUsuario = "user",
                XRequestId = "1"
            };

            var sut = new ConfiguracionesService(configuracionRepository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionLoginHabilitadoAsync(Headers.ToRequestBody(new LoginHabilitadoModelInput(),
                    AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.Should().NotBeNull();
            resultado.Status.Should().Be(TaskStatus.RanToCompletion);
        }

        [Fact]
        public void ObtenerConfiguracionLoginHabilitadoAsync()
        {
            // Arrange
            var configuracionMock = new Configuracion
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "true"
            };

            var configuracionRepository = new Mock<IConfiguracionRepository>();
            configuracionRepository.Setup(m =>
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionMock);

            ApiHeaders Headers = new ApiHeaders
            {
                XAplicacion = "app",
                XCanal = "HBI",
                XUsuario = "user",
                XRequestId = "1"
            };

            var sut = new ConfiguracionesService(configuracionRepository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionLoginHabilitadoAsync(Headers.ToRequestBody(new LoginHabilitadoModelInput(),
                    AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.Should().NotBeNull();
            resultado.Status.Should().Be(TaskStatus.RanToCompletion);
        }
    }
}
