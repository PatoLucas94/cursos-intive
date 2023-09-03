using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Dtos.SSORepository.Output;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.SsoService
{
    public class AutenticarAsyncTest
    {
        private const string UsuarioTest1 = "Usuario1";
        private const string ClaveCorrecta = "ClaveCorrecta";

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "HBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        public static IEnumerable<object[]> DatosCorrecto =>
            new List<object[]>
            {
                new object[] { UsuarioTest1, ClaveCorrecta, true, StatusCodes.Status200OK },
            };

        public static IEnumerable<object[]> DatosIncorrecto =>
            new List<object[]>
            {
                new object[] { UsuarioTest1, ClaveCorrecta, false, StatusCodes.Status401Unauthorized },
            };

        private static Spv.Usuarios.Service.SsoService CrearUsuarioService(ISsoRepository ssoRepository)
        {
            var loggerMock = new Mock<ILogger<Spv.Usuarios.Service.SsoService>>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();

            helperDbServerMockV2.Setup(m => m.ObtenerFechaAsync())
                .ReturnsAsync(new FechaDbServerV2 { Now = DateTime.Now });

            return new Spv.Usuarios.Service.SsoService(
                loggerMock.Object,
                ssoRepository,
                auditoriaV2Repository.Object,
                helperDbServerMockV2.Object,
                new Mock<IDistributedCache>().Object
            );
        }

        [Theory]
        [MemberData(nameof(DatosCorrecto))]
        public async Task AutenticarAsyncOK(string usuario, string clave, bool isOk, int statusCode)
        {
            // Arrange
            var ssoRepository = new Mock<ISsoRepository>();
            ssoRepository.Setup(m =>
                    m.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((new TokenModelOutput(), null));

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = usuario,
                Password = clave
            };

            var sut = CrearUsuarioService(ssoRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Theory]
        [MemberData(nameof(DatosIncorrecto))]
        public async Task AutenticarAsyncError(string usuario, string clave, bool isOk, int statusCode)
        {
            // Arrange
            var ssoRepository = new Mock<ISsoRepository>();
            ssoRepository.Setup(m =>
                    m.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((null, new ErrorModel()));

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = usuario,
                Password = clave
            };

            var sut = CrearUsuarioService(ssoRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Fact]
        public async Task AutenticarAsyncThrows()
        {
            // Arrange
            var ssoRepository = new Mock<ISsoRepository>();

            ssoRepository.Setup(m =>
                    m.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("Exception"));

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = "Usuario1",
                Password = "ClaveCorrecta"
            };

            var sut = CrearUsuarioService(ssoRepository.Object);

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.AutenticarAsync(
                    Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels())));

            // Assert
            resultado.Message.Should().Be("Exception");
        }
    }
}
