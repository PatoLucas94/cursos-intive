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
    public class ApiCatalogoHelperTest
    {
        [Fact(DisplayName = "Cuando falta XCanal")]
        public void NoSeEncuentraXCanal()
        {
            // Arrange
            var apiCatalogoConfigurationsOptions = new Mock<IOptions<ApiCatalogoConfigurationsOptions>>();
            apiCatalogoConfigurationsOptions.Setup(m => m.Value).Returns(new ApiCatalogoConfigurationsOptions());

            var apiServicesConfigurationOptions = new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>();
            apiServicesConfigurationOptions.Setup(m => m.Value).Returns(new ApiServicesHeadersConfigurationOptions());

            var sut = new ApiCatalogoHelper(apiCatalogoConfigurationsOptions.Object, apiServicesConfigurationOptions.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ApiCatalogoXCanal());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiCatalogo}:{nameof(ApiServicesHeadersConfigurationOptions.xCanal)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta XUsuario")]
        public void NoSeEncuentraXUsuario()
        {
            // Arrange
            var apiCatalogoConfigurationsOptions = new Mock<IOptions<ApiCatalogoConfigurationsOptions>>();
            apiCatalogoConfigurationsOptions.Setup(m => m.Value).Returns(new ApiCatalogoConfigurationsOptions());

            var apiServicesConfigurationOptions = new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>();
            apiServicesConfigurationOptions.Setup(m => m.Value).Returns(new ApiServicesHeadersConfigurationOptions());

            var sut = new ApiCatalogoHelper(apiCatalogoConfigurationsOptions.Object, apiServicesConfigurationOptions.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ApiCatalogoXUsuario());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiCatalogo}:{nameof(ApiServicesHeadersConfigurationOptions.xUsuario)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta BasePath")]
        public void NoSeEncuentraBasePath()
        {
            // Arrange
            var apiCatalogoConfigurationsOptions = new Mock<IOptions<ApiCatalogoConfigurationsOptions>>();
            apiCatalogoConfigurationsOptions.Setup(m => m.Value).Returns(new ApiCatalogoConfigurationsOptions());

            var sut = new ApiCatalogoHelper(apiCatalogoConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.BasePath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiCatalogo}:{nameof(ApiCatalogoConfigurationsOptions.BasePath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta PaisesPath")]
        public void NoSeEncuentraPaisesPath()
        {
            // Arrange
            var apiCatalogoConfigurationsOptions = new Mock<IOptions<ApiCatalogoConfigurationsOptions>>();
            apiCatalogoConfigurationsOptions.Setup(m => m.Value).Returns(new ApiCatalogoConfigurationsOptions());

            var sut = new ApiCatalogoHelper(apiCatalogoConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.PaisesPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiCatalogo}:{nameof(ApiCatalogoConfigurationsOptions.PaisesPath)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta TiposDocumentoPath")]
        public void NoSeEncuentraTiposDocumentoPath()
        {
            // Arrange
            var apiCatalogoConfigurationsOptions = new Mock<IOptions<ApiCatalogoConfigurationsOptions>>();
            apiCatalogoConfigurationsOptions.Setup(m => m.Value).Returns(new ApiCatalogoConfigurationsOptions());

            var sut = new ApiCatalogoHelper(apiCatalogoConfigurationsOptions.Object,
                new Mock<IOptions<ApiServicesHeadersConfigurationOptions>>().Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.TiposDocumentoPath());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiCatalogo}:{nameof(ApiCatalogoConfigurationsOptions.TiposDocumentoPath)}"),
                    "Se esperaba error");
        }
    }
}
