using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.LogEvents;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Service;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service
{
    public class ConfiguracionesServiceTest
    {
        // Cantidad de Intentos de Login Legacy
        [Fact]
        public void ObtenerConfiguracionCantidadDeIntentosDeLogin()
        {
            // Arrange
            var configuracionMock = new Configuracion
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.CantidadDeIntentosDeLoginKey,
                Value = "3"
            };

            var configuracionRepository = new Mock<IConfiguracionRepository>();
            configuracionRepository.Setup(m => 
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionMock);

            var sut = new ConfiguracionesService(configuracionRepository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);
        }

        // Cantidad de Intentos de Login Nuevo Modelo
        [Fact]
        public void ObtenerConfiguracionCantidadDeIntentosDeLoginNuevoModelo()
        {
            // Arrange
            var configuracionV2Mock = new ConfiguracionV2
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.CantidadDeIntentosDeLoginKey,
                Value = "5"
            };

            var configuracionV2Repository = new Mock<IConfiguracionV2Repository>();
            configuracionV2Repository.Setup(m =>
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionV2Mock);

            var sut = new ConfiguracionesV2Service(configuracionV2Repository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(5);
        }

        [Fact]
        public void ObtenerConfiguracionCantidadDeIntentosDeLoginDevuelveValorEnMemoria()
        {
            // Arrange
            var configuracionMock1 = new Configuracion
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.CantidadDeIntentosDeLoginKey,
                Value = "3"
            };

            // Arrange
            var configuracionMock2 = new ConfiguracionV2
            {
                ConfigurationId = 2,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.CantidadDeIntentosDeLoginKey,
                Value = "3"
            };

            var configuracionRepository = new Mock<IConfiguracionRepository>();
            configuracionRepository.Setup(m => 
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionMock1);

            var configuracionRepository2 = new Mock<IConfiguracionV2Repository>();
            configuracionRepository2.Setup(m =>
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionMock2);

            var sut = new ConfiguracionesService(configuracionRepository.Object);
            var sut2 = new ConfiguracionesV2Service(configuracionRepository2.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);

            // Act
            resultado = sut2.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);
        }

        [Fact]
        public void ObtenerConfiguracionCantidadDeIntentosDeLoginDevuelveException()
        {
            // Arrange
            var configuracionMock1 = new Configuracion
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.CantidadDeIntentosDeLoginKey,
                Value = "No Numérico"
            };

            var configuracionRepository = new Mock<IConfiguracionRepository>();
            configuracionRepository.Setup(m => 
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionMock1);

            var sut = new ConfiguracionesService(configuracionRepository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Status.Should().Be(TaskStatus.Faulted);
            resultado.Exception?.InnerException?.Message.Should()
                .Be(UsuarioServiceEvents.ExceptionAlObtenerConfiguracionCantidadDeIntentosDeLogin.Name);
        }

        // Cantidad de Intentos de Clave de Canales
        [Fact]
        public void ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanales()
        {
            // Arrange
            var configuracionV2Mock = new ConfiguracionV2
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeChannelsKey,
                Name = AppConstants.CantidadDeIntentosDeClaveDeCanalesKey,
                Value = "3"
            };

            var configuracionV2Repository = new Mock<IConfiguracionV2Repository>();
            configuracionV2Repository.Setup(m => 
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionV2Mock);

            var sut = new ConfiguracionesV2Service(configuracionV2Repository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);
        }

        [Fact]
        public void ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesDevuelveValorEnMemoria()
        {
            // Arrange
            var configuracionV2Mock1 = new ConfiguracionV2
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeChannelsKey,
                Name = AppConstants.CantidadDeIntentosDeClaveDeCanalesKey,
                Value = "3"
            };

            var configuracionV2Repository = new Mock<IConfiguracionV2Repository>();
            configuracionV2Repository.Setup(m => 
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionV2Mock1);

            var sut = new ConfiguracionesV2Service(configuracionV2Repository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);

            // Arrange
            var configuracionV2Mock2 = new ConfiguracionV2
            {
                ConfigurationId = 2,
                Type = AppConstants.ConfigurationTypeChannelsKey,
                Name = AppConstants.CantidadDeIntentosDeClaveDeCanalesKey,
                Value = "2"
            };

            var configuracionV2Repository2 = new Mock<IConfiguracionV2Repository>();
            configuracionV2Repository2.Setup(m => 
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionV2Mock2);

            // Act
            resultado = sut.ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);
        }

        [Fact]
        public void ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesDevuelveException()
        {
            // Arrange
            var configuracionV2Mock1 = new ConfiguracionV2
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeChannelsKey,
                Name = AppConstants.CantidadDeIntentosDeClaveDeCanalesKey,
                Value = "No Numérico"
            };

            var configuracionV2Repository = new Mock<IConfiguracionV2Repository>();
            configuracionV2Repository.Setup(m =>
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionV2Mock1);

            var sut = new ConfiguracionesV2Service(configuracionV2Repository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Status.Should().Be(TaskStatus.Faulted);
            resultado.Exception?.InnerException?.Message.Should()
                .Be(ClaveCanalesServiceEvents.ExceptionAlObtenerConfiguracionCantidadDeIntentosDeClaveDeCanales.Name);
        }

        // Cantidad dias para forzar cambio de clave
        [Fact]
        public void ObtenerConfiguracionCantidadDiasParaForzarCambioDeClave()
        {
            // Arrange
            var configuracionMock = new Configuracion
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "3"
            };

            var configuracionRepository = new Mock<IConfiguracionRepository>();
            configuracionRepository.Setup(m =>
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionMock);

            var sut = new ConfiguracionesService(configuracionRepository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);
        }

        // Cantidad dias para forzar cambio de clave V2
        [Fact]
        public void ObtenerConfiguracionCantidadDiasParaForzarCambioDeClaveV2()
        {
            // Arrange
            var configuracionV2Mock = new ConfiguracionV2
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "3"
            };

            var configuracionV2Repository = new Mock<IConfiguracionV2Repository>();
            configuracionV2Repository.Setup(m => 
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionV2Mock);

            var sut = new ConfiguracionesV2Service(configuracionV2Repository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);
        }

        [Fact]
        public void ObtenerConfiguracionCantidadDiasParaForzarCambioDeClaveEnMemoria()
        {
            // Arrange
            var configuracionMock = new Configuracion
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "3"
            };

            var configuracionRepository = new Mock<IConfiguracionRepository>();
            configuracionRepository.Setup(m => 
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionMock);

            var sut = new ConfiguracionesService(configuracionRepository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);

            // Arrange
            var configuracionMock2 = new Configuracion
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "10"
            };

            var configuracionRepository2 = new Mock<IConfiguracionRepository>();
            configuracionRepository2.Setup(m => 
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionMock2);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);
        }

        [Fact]
        public void ObtenerConfiguracionCantidadDiasParaForzarCambioDeClaveEnMemoriaV2()
        {
            // Arrange
            var configuracionV2Mock = new ConfiguracionV2
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "3"
            };

            var configuracionV2Repository = new Mock<IConfiguracionV2Repository>();
            configuracionV2Repository.Setup(m =>
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionV2Mock);

            var sut = new ConfiguracionesV2Service(configuracionV2Repository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);

            // Arrange
            var configuracionV2Mock2 = new ConfiguracionV2
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "10"
            };

            var configuracionV2Repository2 = new Mock<IConfiguracionV2Repository>();
            configuracionV2Repository2.Setup(m =>
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionV2Mock2);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Result.Should().Be(3);
        }

        [Fact]
        public void ObtenerConfiguracionCantidadDiasParaForzarCambioDeClaveDevuelveException()
        {
            // Arrange
            var configuracionMock = new Configuracion
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "No numerico"
            };

            var configuracionRepository = new Mock<IConfiguracionRepository>();
            configuracionRepository.Setup(m => 
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionMock);

            var sut = new ConfiguracionesService(configuracionRepository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Status.Should().Be(TaskStatus.Faulted);
            resultado.Exception?.InnerException?.Message.Should()
                .Be(UsuarioServiceEvents.ExceptionAlObtenerConfiguracionDeCantidadDiasParaForzarCambioDeClave.Name);
        }

        [Fact]
        public void ObtenerConfiguracionCantidadDiasParaForzarCambioDeClaveDevuelveExceptionV2()
        {
            // Arrange
            var configuracionV2Mock = new ConfiguracionV2
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "No numerico"
            };

            var configuracionV2Repository = new Mock<IConfiguracionV2Repository>();
            configuracionV2Repository.Setup(m =>
                    m.ObtenerConfiguracion(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(configuracionV2Mock);

            var sut = new ConfiguracionesV2Service(configuracionV2Repository.Object);

            // Act
            var resultado = sut.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Status.Should().Be(TaskStatus.Faulted);
            resultado.Exception?.InnerException?.Message.Should()
                .Be(UsuarioServiceEvents.ExceptionAlObtenerConfiguracionDeCantidadDiasParaForzarCambioDeClave.Name);
        }
    }
}
