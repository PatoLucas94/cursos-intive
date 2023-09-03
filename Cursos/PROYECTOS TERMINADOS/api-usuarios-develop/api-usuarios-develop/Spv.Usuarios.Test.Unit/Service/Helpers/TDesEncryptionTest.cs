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
    public class TDesEncryptionTest
    {
        public static readonly List<object[]> Data = new List<object[]>
        {
            new object[] { "lalala", "18f8318eb7fc9cc0" },
            new object[] { "Info1212", "bde05d0b5280e5ad4b088d3ce4ade517" },
            new object[] { "Documento4", "c0718633dc495eb6d5956dc9dd281eb5" },
            new object[] { "Andres13", "28bff29d4eb86b64cc7bbb67a1494989" },
            new object[] { "Andres131", "28bff29d4eb86b64d51f0405a4838b36" },
            new object[] { "Documento04", "c0718633dc495eb641e8076f6d7e9829" },
            new object[] { "Documento3", "c0718633dc495eb6dcb07c3f44126c9a" },
            new object[] { "AB02268", "83ce93c83295f06c" }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void TDesEncryptTest(string plainText, string expectedValue)
        {
            // Arrange
            var sut = CrearEncryption();

            // Act
            var expected = sut.Encrypt(plainText);

            // Assert
            expected.Should().Be(expectedValue);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void TDesDecryptTest(string expectedValue, string encryptedText)
        {
            // Arrange
            var sut = CrearEncryption();

            // Act
            var expected = sut.Decrypt(encryptedText);

            // Assert
            expected.Should().Be(expectedValue);
        }

        [Fact]
        public void TDesEncryptionExceptions()
        {
            // Arrange
            var apiUsuariosConfiguration = new ApiUsuariosConfigurationOptions
            {
                RetoResponseKey = "g7Hk.a9D3s",
                BanelcoExchangeKey = "0123456789ABCDEF"
            };

            var apiUsuariosConfigurationOptions = new Mock<IOptions<ApiUsuariosConfigurationOptions>>();
            apiUsuariosConfigurationOptions.Setup(m => m.Value).Returns(apiUsuariosConfiguration);

            var sut = new TDesEncryption(apiUsuariosConfigurationOptions.Object);

            // Act
            var resultDecrypt = Assert.Throws<Exception>(() => sut.Decrypt(""));
            resultDecrypt.Message.Should().Be("No se encontró 'NsbtEncryptionKey' key.");

            var resultEncrypt = Assert.Throws<Exception>(() => sut.Encrypt(""));
            resultEncrypt.Message.Should().Be("No se encontró 'NsbtEncryptionKey' key.");
        }

        private TDesEncryption CrearEncryption()
        {
            var apiUsuariosConfiguration = new ApiUsuariosConfigurationOptions
            {
                RetoResponseKey = "g7Hk.a9D3s",
                BanelcoExchangeKey = "0123456789ABCDEF",
                NsbtEncryptionKey = "F2CF12465544A012501A2280F2A7A0A50E02FD0AEBF3FBCF"
            };

            var apiUsuariosConfigurationOptions = new Mock<IOptions<ApiUsuariosConfigurationOptions>>();
            apiUsuariosConfigurationOptions.Setup(m => m.Value).Returns(apiUsuariosConfiguration);

            return new TDesEncryption(apiUsuariosConfigurationOptions.Object);
        }
    }
}
