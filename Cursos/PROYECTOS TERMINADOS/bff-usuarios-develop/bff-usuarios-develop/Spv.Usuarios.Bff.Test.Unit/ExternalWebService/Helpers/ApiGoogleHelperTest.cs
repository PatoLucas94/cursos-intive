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
    public class ApiGoogleHelperTest
    {
        [Fact(DisplayName = "Cuando falta ValidacionTokenCaptchaV3Path")]
        public void NoSeEncuentraValidacionTokenCaptchaV3Path()
        {
            // Arrange
            var apiCatalogoConfigurationsOptions = new Mock<IOptions<ApiGoogleConfigurationOptions>>();
            apiCatalogoConfigurationsOptions.Setup(m => m.Value).Returns(new ApiGoogleConfigurationOptions());

            var sut = new ApiGoogleHelper(apiCatalogoConfigurationsOptions.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.ValidacionTokenReCaptchaV3Path());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiGoogle}:{nameof(ApiGoogleConfigurationOptions.ValidacionTokenCaptchaV3Path)}"),
                    "Se esperaba error");
        }

        [Fact(DisplayName = "Cuando falta SecretCaptchaV3Key")]
        public void NoSeEncuentraSecretCaptchaV3Key()
        {
            // Arrange
            var apiCatalogoConfigurationsOptions = new Mock<IOptions<ApiGoogleConfigurationOptions>>();
            apiCatalogoConfigurationsOptions.Setup(m => m.Value).Returns(new ApiGoogleConfigurationOptions());

            var sut = new ApiGoogleHelper(apiCatalogoConfigurationsOptions.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.SecretReCaptchaV3Key());

            // Assert
            exception.Message.Should()
                .Be(
                    string.Format(ErrorConstants.NoSeEncontroKey,
                        $"{ExternalServicesNames.ApiGoogle}:{nameof(ApiGoogleConfigurationOptions.SecretCaptchaV3Key)}"),
                    "Se esperaba error");
        }
    }
}
