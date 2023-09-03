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
    public class ApiNotificacionesHelperTest
    {
        [Fact(DisplayName = "Cuando falta XCanal")]
        public void NoSeEncuentraXCanal()
        {
            // Arrange
            var apiNotificacionesConfigurationsOptions = new Mock<IOptions<ApiNotificacionesConfigurationsOptions>>();
            apiNotificacionesConfigurationsOptions.Setup(m => m.Value).Returns(new ApiNotificacionesConfigurationsOptions());

            var apiServicesConfigurationOptions = new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>();
            apiServicesConfigurationOptions.Setup(m => m.Value).Returns(new ApiServicesHeadersConfigurationOptions());

            var sut = new ApiNotificacionesHelper(apiNotificacionesConfigurationsOptions.Object, apiServicesConfigurationOptions.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ApiNotificacionesXCanal());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiNotificaciones}:{nameof(ApiServicesHeadersConfigurationOptions.xCanal)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta XUsuario")]
        public void NoSeEncuentraXUsuario()
        {
            // Arrange
            var apiNotificacionesConfigurationsOptions = new Mock<IOptions<ApiNotificacionesConfigurationsOptions>>();
            apiNotificacionesConfigurationsOptions.Setup(m => m.Value).Returns(new ApiNotificacionesConfigurationsOptions());

            var apiServicesConfigurationOptions = new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>();
            apiServicesConfigurationOptions.Setup(m => m.Value).Returns(new ApiServicesHeadersConfigurationOptions());

            var sut = new ApiNotificacionesHelper(apiNotificacionesConfigurationsOptions.Object, apiServicesConfigurationOptions.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ApiNotificacionesXUsuario());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiNotificaciones}:{nameof(ApiServicesHeadersConfigurationOptions.xUsuario)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta BasePath")]
        public void NoSeEncuentraBasePath()
        {
            // Arrange
            var apiNotificacionesConfigurationsOptions = new Mock<IOptions<ApiNotificacionesConfigurationsOptions>>();
            apiNotificacionesConfigurationsOptions.Setup(m => m.Value).Returns(new ApiNotificacionesConfigurationsOptions());

            var sut = new ApiNotificacionesHelper(apiNotificacionesConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.BasePath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiNotificaciones}:{nameof(ApiNotificacionesConfigurationsOptions.BasePath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta CrearYEnviarTokenPath")]
        public void NoSeEncuentraCrearYEnviarTokenPath()
        {
            // Arrange
            var apiNotificacionesConfigurationsOptions = new Mock<IOptions<ApiNotificacionesConfigurationsOptions>>();
            apiNotificacionesConfigurationsOptions.Setup(m => m.Value).Returns(new ApiNotificacionesConfigurationsOptions());

            var sut = new ApiNotificacionesHelper(apiNotificacionesConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.CrearYEnviarTokenPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiNotificaciones}:{nameof(ApiNotificacionesConfigurationsOptions.CrearYEnviarTokenPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta ValidarTokenPath")]
        public void NoSeEncuentraValidarTokenPath()
        {
            // Arrange
            var apiNotificacionesConfigurationsOptions = new Mock<IOptions<ApiNotificacionesConfigurationsOptions>>();
            apiNotificacionesConfigurationsOptions.Setup(m => m.Value).Returns(new ApiNotificacionesConfigurationsOptions());

            var sut = new ApiNotificacionesHelper(apiNotificacionesConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ValidarTokenPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiNotificaciones}:{nameof(ApiNotificacionesConfigurationsOptions.ValidarTokenPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta EnviarEmailPath")]
        public void NoSeEncuentraEnviarEmailPath()
        {
            // Arrange
            var apiNotificacionesConfigurationsOptions = new Mock<IOptions<ApiNotificacionesConfigurationsOptions>>();
            apiNotificacionesConfigurationsOptions.Setup(m => m.Value).Returns(new ApiNotificacionesConfigurationsOptions());

            var sut = new ApiNotificacionesHelper(apiNotificacionesConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.EnviarEmailPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiNotificaciones}:{nameof(ApiNotificacionesConfigurationsOptions.EnviarEmailPath)}"),
                    "Se esperaba error");
        }
    }
}
