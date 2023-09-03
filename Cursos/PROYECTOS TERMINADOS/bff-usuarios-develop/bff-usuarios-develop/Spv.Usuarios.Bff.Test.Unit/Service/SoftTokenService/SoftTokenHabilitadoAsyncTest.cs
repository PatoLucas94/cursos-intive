using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Bff.Common.Dtos.Client.SofttokenClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.SofttokenClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.SoftToken.Input;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.SoftTokenService
{
    public class SoftTokenHabilitadoAsyncTest
    {
        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static Spv.Usuarios.Bff.Service.SoftTokenService CrearUsuarioService(IApiSoftTokenRepository apiSoftTokenRepository)
        {
            var loggerMock = new Mock<ILogger<Spv.Usuarios.Bff.Service.SoftTokenService>>();

            return new Spv.Usuarios.Bff.Service.SoftTokenService(
                loggerMock.Object,
                 apiSoftTokenRepository);
        }
        [Fact]
        public async Task HabilitadoAsyncOK()
        {
            // Arrange
            var apiSoftTokenRepository = new Mock<IApiSoftTokenRepository>();

            var result = new ApiSoftTokenModelOutput()
            {
                detalle = string.Empty,
                estado = "OK",
                bloqueado = false,
                identificador = "sts_11062858",
            };

            apiSoftTokenRepository
                .Setup(m => m.TokenHabilitadoAsync(It.IsAny<ApiSoftTokenModelInput>()))
                .ReturnsAsync(result);

            var datosRequestCorrectos = new SoftTokenModelInput
            {
                Canal = "OBI",
                Usuario = "OBI",
                Identificador = "sts_11062858",
            };

            var sut = CrearUsuarioService(apiSoftTokenRepository.Object);

            // Act
            var resultado = await sut.SoftTokenHabilitadoAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task HabilitadoAsyncInvalido()
        {
            // Arrange
            var apiSoftTokenRepository = new Mock<IApiSoftTokenRepository>();

            var result = new ApiSoftTokenModelOutput()
            {
                detalle = "No existe un token registrado para el identificador \"sts_111111111\"",
                estado = "Not Found",
                bloqueado = false,
                identificador = "sts_11062858",
            };

            var datosRequest= new ApiSoftTokenModelInput
            {
                Identificador = "sts_11462868",
            };

            apiSoftTokenRepository
                  .Setup(m => m.TokenHabilitadoAsync(datosRequest))
                  .ReturnsAsync(result);

            var datosRequestCorrectos = new SoftTokenModelInput
            {
                Canal = "OBI",
                Usuario = "OBI",
                Identificador = "sts_11062868",
            };

            var sut = CrearUsuarioService(apiSoftTokenRepository.Object);

            // Act
            var resultado = await sut.SoftTokenHabilitadoAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(false);
            resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        }
    }
}
