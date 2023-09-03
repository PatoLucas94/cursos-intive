using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.ConfiguracionService.Input;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.ConfiguracionService
{
    public class LoginHabilitadoAsyncTest
    {
        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static Spv.Usuarios.Bff.Service.ConfiguracionService CrearUsuarioService(IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2)
        {
            var loggerMock = new Mock<ILogger<Spv.Usuarios.Bff.Service.ConfiguracionService>>();

            return new Spv.Usuarios.Bff.Service.ConfiguracionService(
                loggerMock.Object,
                apiUsuariosRepositoryV2
            );
        }

        [Fact]
        public async Task LoginHabilitadoAsyncHabilitadoCero()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            var result = new ApiUsuariosLoginHabilitadoModelOutput();
            result.habilitado = "0";
            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerLoginHabilitadoAsync()).ReturnsAsync(result);

            var datosRequestCorrectos = new LoginHabilitadoModelInput();

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object);

            var resultado = await sut.LoginHabilitadoAsync(Headers.ToRequestBody(datosRequestCorrectos));

            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);

            apiUsuarioRepositoryV2.Verify(x =>
                  x.ObtenerLoginHabilitadoAsync(), Times.Once()
            );
        }

        [Fact]
        public async Task LoginHabilitadoAsyncHabilitadoUno()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            var result = new ApiUsuariosLoginHabilitadoModelOutput();
            result.habilitado = "1";
            var loginMensaje = new ApiUsuariosLoginMessageModelOutput();
            loginMensaje.mensaje = "Mensaje1";

            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerLoginHabilitadoAsync()).ReturnsAsync(result);

            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerMensajeLoginDeshabilitadoAsync()).ReturnsAsync(loginMensaje);

            var datosRequestCorrectos = new LoginHabilitadoModelInput();

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object);

            var resultado = await sut.LoginHabilitadoAsync(Headers.ToRequestBody(datosRequestCorrectos));

            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);

            apiUsuarioRepositoryV2.Verify(x =>
                  x.ObtenerLoginHabilitadoAsync(), Times.Once()
            );

            apiUsuarioRepositoryV2.Verify(x =>
                  x.ObtenerMensajeLoginDeshabilitadoAsync(), Times.Once()
            );
        }

        [Fact]
        public async Task LoginHabilitadoAsyncHabilitadoDos()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            var result = new ApiUsuariosLoginHabilitadoModelOutput();
            result.habilitado = "2";
            var loginMensaje = new ApiUsuariosLoginMessageModelOutput();
            loginMensaje.mensaje = "Mensaje1";

            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerLoginHabilitadoAsync()).ReturnsAsync(result);

            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerMensajeLoginDeshabilitadoAsync()).ReturnsAsync(loginMensaje);

            var datosRequestCorrectos = new LoginHabilitadoModelInput();

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object);

            var resultado = await sut.LoginHabilitadoAsync(Headers.ToRequestBody(datosRequestCorrectos));

            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);

            apiUsuarioRepositoryV2.Verify(x =>
                  x.ObtenerLoginHabilitadoAsync(), Times.Once()
            );

            apiUsuarioRepositoryV2.Verify(x =>
                  x.ObtenerMensajeLoginDeshabilitadoAsync(), Times.Once()
            );
        }

        [Fact]
        public async Task LoginHabilitadoAsyncHabilitadoUnoVacio()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            var result = new ApiUsuariosLoginHabilitadoModelOutput();
            result.habilitado = "1";
            var loginMensaje = new ApiUsuariosLoginMessageModelOutput();
            loginMensaje.mensaje = "";
            var loginMensaje2 = new ApiUsuariosLoginMessageModelOutput();
            loginMensaje2.mensaje = "mensaje2";

            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerLoginHabilitadoAsync()).ReturnsAsync(result);

            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerMensajeLoginDeshabilitadoAsync()).ReturnsAsync(loginMensaje);

            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerMensajeDefaultLoginDeshabilitadoAsync()).ReturnsAsync(loginMensaje2);

            var datosRequestCorrectos = new LoginHabilitadoModelInput();

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object);

            var resultado = await sut.LoginHabilitadoAsync(Headers.ToRequestBody(datosRequestCorrectos));

            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);

            apiUsuarioRepositoryV2.Verify(x =>
                  x.ObtenerLoginHabilitadoAsync(), Times.Once()
            );

            apiUsuarioRepositoryV2.Verify(x =>
                  x.ObtenerMensajeLoginDeshabilitadoAsync(), Times.Once()
            );

            apiUsuarioRepositoryV2.Verify(x =>
                  x.ObtenerMensajeDefaultLoginDeshabilitadoAsync(), Times.Once()
            );
        }

        [Fact]
        public async Task LoginHabilitadoAsyncHabilitadoDosVacio()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            var result = new ApiUsuariosLoginHabilitadoModelOutput();
            result.habilitado = "2";
            var loginMensaje = new ApiUsuariosLoginMessageModelOutput();
            loginMensaje.mensaje = "";
            var loginMensaje2 = new ApiUsuariosLoginMessageModelOutput();
            loginMensaje2.mensaje = "mensaje2";

            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerLoginHabilitadoAsync()).ReturnsAsync(result);

            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerMensajeLoginDeshabilitadoAsync()).ReturnsAsync(loginMensaje);

            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerMensajeDefaultLoginDeshabilitadoAsync()).ReturnsAsync(loginMensaje2);

            var datosRequestCorrectos = new LoginHabilitadoModelInput();

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object);

            var resultado = await sut.LoginHabilitadoAsync(Headers.ToRequestBody(datosRequestCorrectos));

            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);

            apiUsuarioRepositoryV2.Verify(x =>
                  x.ObtenerLoginHabilitadoAsync(), Times.Once()
            );

            apiUsuarioRepositoryV2.Verify(x =>
                  x.ObtenerMensajeLoginDeshabilitadoAsync(), Times.Once()
            );

            apiUsuarioRepositoryV2.Verify(x =>
                  x.ObtenerMensajeDefaultLoginDeshabilitadoAsync(), Times.Once()
            );
        }

        [Fact]
        public async Task LoginHabilitadoAsyncHabilitadoNull()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            var result = new ApiUsuariosLoginHabilitadoModelOutput();
            result.habilitado = null;
            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerLoginHabilitadoAsync()).ReturnsAsync(result);

            var datosRequestCorrectos = new LoginHabilitadoModelInput();

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object);

            var resultado = await sut.LoginHabilitadoAsync(Headers.ToRequestBody(datosRequestCorrectos));

            resultado.IsOk.Should().Be(false);
            resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = resultado.Match(a => a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            message.Should().Be(ErrorCode.LoginHabilitadoFueraDeRango.ErrorDescription);
        }

        [Fact]
        public async Task LoginHabilitadoAsyncNull()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            var datosRequestCorrectos = new LoginHabilitadoModelInput();

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object);

            var resultado = await sut.LoginHabilitadoAsync(Headers.ToRequestBody(datosRequestCorrectos));

            resultado.IsOk.Should().Be(false);
            resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = resultado.Match(a => a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            message.Should().Be(ErrorCode.LoginHabilitado.ErrorDescription);
        }

        [Fact]
        public async Task LoginHabilitadoAsyncHabilitadoThrows()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            var result = new ApiUsuariosLoginHabilitadoModelOutput();
            result.habilitado = null;

            apiUsuarioRepositoryV2.Setup(m =>
                   m.ObtenerLoginHabilitadoAsync())
               .Throws(new Exception("Test Exception"));

            var datosRequestCorrectos = new LoginHabilitadoModelInput();

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object);

           var resultado = await Assert.ThrowsAsync<Exception>(() =>
               sut.LoginHabilitadoAsync(Headers.ToRequestBody(datosRequestCorrectos))
           );

             resultado.Message.Should().Be("Test Exception");
        }
    }
}
