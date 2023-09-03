using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Spv.Usuarios.Common.Configurations;
using Spv.Usuarios.Service.Helpers;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.Helpers
{
    public class EncryptionTest
    {
        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { null, "p1jBMmgS0axEVWOg/5tigLo0BBHWO/oACo1YNhSqtFI" },
                new object[] { "", "p1jBMmgS0axEVWOg/5tigLo0BBHWO/oACo1YNhSqtFI" },
                new object[] { "Info1212", "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8" }
            };

        public static IEnumerable<object[]> DatosChannelsKey =>
            new List<object[]>
            {
                new object[] { "12345678", "1234567890987654", "DFB80EEEB6158966" },
            };

        [Theory]
        [MemberData(nameof(Datos))]
        public void GetHash_Exitoso(string textToHash, string esperado)
        {
            // Arrange
            var sut = CrearEncryption();

            // Act
            var resultado = sut.GetHash(textToHash);

            // Assert
            resultado.Should().NotBeNullOrEmpty();
            resultado.Should().Be(esperado);
        }

        [Fact(DisplayName = "Cuando falta RetoResponseKey")]
        public void NoSeEncontroSecretKey()
        {
            // Arrange
            var apiUsuariosConfigurationOptions = new Mock<IOptions<ApiUsuariosConfigurationOptions>>();
            apiUsuariosConfigurationOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationOptions());

            var sut = new Encryption(apiUsuariosConfigurationOptions.Object);

            // Act
            var excepcion = Assert.Throws<Exception>(() => sut.GetHash("test"));

            // Assert
            excepcion.Message.Should().Be("No se encontró Secret Key.", "Se esperaba error");
        }

        [Theory]
        [MemberData(nameof(DatosChannelsKey))]
        public void EncryptChannelsKey_Exitoso(string channelsKey, string debitCardNumber, string esperado)
        {
            // Arrange
            var sut = CrearEncryption();

            // Act
            var resultado = sut.EncryptChannelsKey(channelsKey, debitCardNumber);

            // Assert
            resultado.Should().NotBeNullOrEmpty();
            resultado.Should().Be(esperado);
        }

        [Fact(DisplayName = "Cuando falta BanelcoExchangeKey")]
        public void NoSeEncontroBanelcoExchangeKey()
        {
            // Arrange
            var apiUsuariosConfigurationOptions = new Mock<IOptions<ApiUsuariosConfigurationOptions>>();
            apiUsuariosConfigurationOptions.Setup(m => m.Value).Returns(new ApiUsuariosConfigurationOptions());

            var sut = new Encryption(apiUsuariosConfigurationOptions.Object);

            // Act
            var excepcion = Assert.Throws<Exception>(() => sut.EncryptChannelsKey("12345678", "12345678912345678"));

            // Assert
            excepcion.Message.Should().Be("No se encontró Banelco Exchange key.", "Se esperaba error");
        }

        private Encryption CrearEncryption()
        {
            var apiUsuariosConfiguration = new ApiUsuariosConfigurationOptions
            {
                RetoResponseKey = "g7Hk.a9D3s",
                BanelcoExchangeKey = "0123456789ABCDEF"
            };

            var apiUsuariosConfigurationOptions = new Mock<IOptions<ApiUsuariosConfigurationOptions>>();
            apiUsuariosConfigurationOptions.Setup(m => m.Value).Returns(apiUsuariosConfiguration);

            return new Encryption(apiUsuariosConfigurationOptions.Object);
        }
    }
}
