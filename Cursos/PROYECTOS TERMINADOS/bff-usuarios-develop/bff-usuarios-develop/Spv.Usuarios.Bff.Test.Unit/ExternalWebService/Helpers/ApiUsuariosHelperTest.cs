using System;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.ExternalWebService.Helpers
{
    public class ApiUsuariosHelperTest
    {
        [Fact(DisplayName = "Cuando falta XCanal")]
        public void NoSeEncuentraXCanal()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var apiServicesConfigurationOptions = new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>();
            apiServicesConfigurationOptions.Setup(m => m.Value).Returns(new ApiServicesHeadersConfigurationOptions());

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object, apiServicesConfigurationOptions.Object, httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ApiUsuariosXCanal());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiServicesHeadersConfigurationOptions.xCanal)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta XUsuario")]
        public void NoSeEncuentraXUsuario()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var apiServicesConfigurationOptions = new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>();
            apiServicesConfigurationOptions.Setup(m => m.Value).Returns(new ApiServicesHeadersConfigurationOptions());

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object, apiServicesConfigurationOptions.Object, httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ApiUsuariosXUsuario());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiServicesHeadersConfigurationOptions.xUsuario)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta XAplicacion")]
        public void NoSeEncuentraXAplicacion()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var apiServicesConfigurationOptions = new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>();
            apiServicesConfigurationOptions.Setup(m => m.Value).Returns(new ApiServicesHeadersConfigurationOptions());

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object, apiServicesConfigurationOptions.Object, httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ApiUsuariosXAplicacion());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiServicesHeadersConfigurationOptions.xAplicacion)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta BasePath")]
        public void NoSeEncuentraBasePath()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object,
                httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.BasePath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiUsuariosConfigurationsOptions.BasePath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta PerfilPath")]
        public void NoSeEncuentraPerfilPath()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object,
                httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.PerfilPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiUsuariosConfigurationsOptions.PerfilPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta PerfilPathV2")]
        public void NoSeEncuentraPerfilV2Path()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object,
                httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.PerfilPathV2());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiUsuariosConfigurationsOptions.PerfilPathV2)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta RegistrarUsuarioPath")]
        public void NoSeEncuentraRegistrarUsuarioPath()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object,
                httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.RegistrarUsuarioPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiUsuariosConfigurationsOptions.RegistrarUsuarioPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta ValidarClaveCanalesPath")]
        public void NoSeEncuentraValidarClaveCanalesPath()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object,
                httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ValidarClaveCanalesPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiUsuariosConfigurationsOptions.ValidarClaveCanalesPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta ValidarClaveCanalesPath")]
        public void NoSeEncuentraInhabilitarClaveCanalesPath()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object,
                httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.InhabilitarClaveCanalesPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiUsuariosConfigurationsOptions.InhabilitarClaveCanalesPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta ValidarExistenciaPath")]
        public void NoSeEncuentraValidarExistenciaPath()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object,
                httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ValidarExistenciaPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiUsuariosConfigurationsOptions.ValidarExistenciaPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta ValidarExistenciaPath")]
        public void NoSeEncuentraValidarExistenciaHbiPath()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object,
                httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ValidarExistenciaHbiPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiUsuariosConfigurationsOptions.ValidarExistenciaHbiPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta CambiarClavePath")]
        public void NoSeEncuentraCambiarClavePath()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object,
                httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.CambiarClavePath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiUsuariosConfigurationsOptions.CambiarClavePath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta CambiarCredencialesPath")]
        public void NoSeEncuentraCambiarCredencialesPath()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object,
                httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.CambiarCredencialesPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiUsuariosConfigurationsOptions.CambiarCredencialesPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta MigrarUsuarioPath")]
        public void NoSeEncuentraMigrarUsuarioPath()
        {
            // Arrange
            var apiUsuariosConfigurationsOptions = new Mock<IOptions<ApiUsuariosConfigurationsOptions>>();
            apiUsuariosConfigurationsOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationsOptions());
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var sut = new ApiUsuariosHelper(apiUsuariosConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object,
                httpClientFactory.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.MigrarUsuarioPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiUsuarios}:{nameof(ApiUsuariosConfigurationsOptions.MigrarUsuarioPath)}"),
                    "Se esperaba error");
        }
    }
}
