using System;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.ExternalWebService.Helpers
{
    public class ApiPersonasHelperTest
    {
        [Fact(DisplayName = "Cuando falta XCanal")]
        public void NoSeEncuentraXCanal()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var apiServicesConfigurationOptions = new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>();
            apiServicesConfigurationOptions.Setup(m => m.Value)
                .Returns(new ApiServicesHeadersConfigurationOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object, apiServicesConfigurationOptions.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ApiPersonasXCanal());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiServicesHeadersConfigurationOptions.xCanal)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta XUsuario")]
        public void NoSeEncuentraXUsuario()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var apiServicesConfigurationOptions = new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>();
            apiServicesConfigurationOptions.Setup(m => m.Value)
                .Returns(new ApiServicesHeadersConfigurationOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object, apiServicesConfigurationOptions.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ApiPersonasXUsuario());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiServicesHeadersConfigurationOptions.xUsuario)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta BasePath")]
        public void NoSeEncuentraBasePath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object, 
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.BasePath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.BasePath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta EmailsActualizacionPath")]
        public void NoSeEncuentraEmailsActualizacionPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.EmailsActualizacionPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.EmailsPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta EmailsCreacionPath")]
        public void NoSeEncuentraEmailsCreacionPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.EmailsCreacionPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.EmailsPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta PersonasFiltroPath")]
        public void NoSeEncuentraPersonasFiltroPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.PersonasFiltroPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.PersonasFiltroPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta PersonasFisicaInfoPath")]
        public void NoSeEncuentraPersonasFisicaInfoPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.PersonasFisicaInfoPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.PersonasFisicaInfoPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta PersonasInfoPath")]
        public void NoSeEncuentraPersonasInfoPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.PersonasInfoPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.PersonasInfoPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta PersonasPath")]
        public void NoSeEncuentraPersonasPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.PersonasPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.PersonasPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta TelefonosActualizacionPath")]
        public void NoSeEncuentraTelefonosActualizacionPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.TelefonosActualizacionPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.TelefonosPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta TelefonosCreacionPath")]
        public void NoSeEncuentraTelefonosCreacionPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.TelefonosCreacionPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.TelefonosPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta TelefonosDobleFactorActualizacionPath")]
        public void NoSeEncuentraTelefonosDobleFactorActualizacionPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.TelefonosDobleFactorActualizacionPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.TelefonosDobleFactorPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta TelefonosDobleFactorCreacionPath")]
        public void NoSeEncuentraTelefonosDobleFactorCreacionPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.TelefonosDobleFactorCreacionPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.TelefonosDobleFactorPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta TelefonosDobleFactorPath")]
        public void NoSeEncuentraTelefonosDobleFactorPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.TelefonosDobleFactorPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.TelefonosDobleFactorPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta TelefonosPath")]
        public void NoSeEncuentraTelefonosPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.TelefonosPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.TelefonosPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta TelefonosVerificacionPath")]
        public void NoSeEncuentraTelefonosVerificacionPath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.TelefonosVerificacionPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.TelefonosVerificacionPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta ProductosMarcaClientePath")]
        public void NoSeEncuentraProductosMarcaClientePath()
        {
            // Arrange
            var apiPersonasConfigurationsOptions = new Mock<IOptions<ApiPersonasConfigurationsOptions>>();
            apiPersonasConfigurationsOptions.Setup(m => m.Value)
                .Returns(new ApiPersonasConfigurationsOptions());

            var sut = new ApiPersonasHelper(apiPersonasConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ProductosMarcaClientePath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiPersonas}:{nameof(ApiPersonasConfigurationsOptions.ProductosMarcaClientePath)}"),
                    "Se esperaba error");
        }
    }
}
