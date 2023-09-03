using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Dtos.ClaveCanalesService.Input;
using Spv.Usuarios.Common.Errors;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Service;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;
using Spv.Usuarios.Test.Unit.Common.Builders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.ClaveCanalService
{
    public class ValidarAsyncTest
    {
        [Fact]
        public async Task ClaveCanalesInexistenteAsync()
        {
            // Arrange
            var usuarioRegistradoRepository = new Mock<IUsuarioRegistradoRepository>();
            var auditoriaLog = new Mock<IAuditoriaLogV2Repository>();

            var sut = CrearClaveCanalesService(usuarioRegistradoRepository.Object, auditoriaLog.Object);

            var body = new ValidacionModelInput
            {
                DocumentTypeId = 80,
                DocumentNumber = "",
                ChannelKey = ""
            };

            // Act
            var resultado = await sut.ValidarAsync(Headers.ToRequestBody(body, AllowedChannelsBuilder.CrearAllowedChannels()));


            // Assert
            auditoriaLog.Verify(x => x.SaveAuditLogAsync(It.IsAny<int>(),
                                             EventTypes.ChannelKey,
                                             EventResults.Error,
                                             It.IsAny<string>(),
                                             It.IsAny<FechaDbServerV2>(),
                                             It.IsAny<string>()), Times.Once);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var message = resultado.Match(a => a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            message.Should().Be(ErrorCode.ClaveDeCanalesInexistente.ErrorDescription);
        }

        [Fact]
        public async Task ClaveCanalesInactivaAsync()
        {
            // Arrange
            var usuarioRegistradoRepository = new Mock<IUsuarioRegistradoRepository>();
            var auditoriaLog = new Mock<IAuditoriaLogV2Repository>();

            usuarioRegistradoRepository.Setup(m => m.ObtenerUsuarioRegistradoAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new UsuarioRegistrado { Active = false });

            var sut = CrearClaveCanalesService(usuarioRegistradoRepository.Object, auditoriaLog.Object);

            var body = new ValidacionModelInput
            {
                DocumentTypeId = 80,
                DocumentNumber = "",
                ChannelKey = ""
            };

            // Act
            var resultado = await sut.ValidarAsync(Headers.ToRequestBody(body, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            auditoriaLog.Verify(x => x.SaveAuditLogAsync(It.IsAny<int>(),
                                             EventTypes.ChannelKey,
                                             EventResults.Error,
                                             It.IsAny<string>(),
                                             It.IsAny<FechaDbServerV2>(),
                                             It.IsAny<string>()), Times.Once);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var message = resultado.Match(a => a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            message.Should().Be(ErrorCode.ClaveDeCanalesInactiva.ErrorDescription);
        }

        [Fact]
        public async Task ClaveCanalesInvalidaAsync()
        {
            // Arrange
            var usuarioClaveDeCanalesIncorrecta = new UsuarioRegistrado { Active = true, CardNumber = NroTarjetaIncorrecto, ForgotPasswordAttempts = 0 };
            var usuarioClaveDeCanalesBloqueada = new UsuarioRegistrado { Active = true, CardNumber = NroTarjetaIncorrecto, ForgotPasswordAttempts = 6 };

            var usuarioRegistradoRepository = new Mock<IUsuarioRegistradoRepository>();
            var auditoriaLog = new Mock<IAuditoriaLogV2Repository>();

            usuarioRegistradoRepository.Setup(m => m.ObtenerUsuarioRegistradoAsync(80, It.IsAny<string>())).ReturnsAsync(usuarioClaveDeCanalesIncorrecta);
            usuarioRegistradoRepository.Setup(m => m.ObtenerUsuarioRegistradoAsync(81, It.IsAny<string>())).ReturnsAsync(usuarioClaveDeCanalesBloqueada);

            var sut = CrearClaveCanalesService(usuarioRegistradoRepository.Object, auditoriaLog.Object);

            var bodyClaveDeCanalesIncorrecta = new ValidacionModelInput
            {
                DocumentTypeId = 80,
                DocumentNumber = "",
                ChannelKey = CanalIncorrecto
            };

            var bodyClaveDeCanalesBloqueada = new ValidacionModelInput
            {
                DocumentTypeId = 81,
                DocumentNumber = "",
                ChannelKey = CanalIncorrecto
            };

            // Act
            var resultadoClaveDeCanalesIncorrecta = await sut.ValidarAsync(Headers.ToRequestBody(bodyClaveDeCanalesIncorrecta, AllowedChannelsBuilder.CrearAllowedChannels()));
            
            var resultadoClaveDeCanalesBloqueada = await sut.ValidarAsync(Headers.ToRequestBody(bodyClaveDeCanalesBloqueada, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            auditoriaLog.Verify(x => x.SaveAuditLogAsync(It.IsAny<int>(),
                                             EventTypes.ChannelKey,
                                             EventResults.Error,
                                             It.IsAny<string>(),
                                             It.IsAny<FechaDbServerV2>(),
                                             It.IsAny<string>()), Times.Exactly(2));

            resultadoClaveDeCanalesIncorrecta.IsOk.Should().BeFalse();
            resultadoClaveDeCanalesIncorrecta.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            resultadoClaveDeCanalesBloqueada.IsOk.Should().BeFalse();
            resultadoClaveDeCanalesBloqueada.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            usuarioClaveDeCanalesIncorrecta.ForgotPasswordAttempts.Should().Be(1);
            usuarioClaveDeCanalesBloqueada.ForgotPasswordAttempts.Should().Be(7);

            var messageClaveDeCanalesIncorrecta = resultadoClaveDeCanalesIncorrecta.Match(a => a.Payload.ToString(), a => a.Message, a => a.Exception.Message);
            var messageClaveDeCanalesBloqueada = resultadoClaveDeCanalesBloqueada.Match(a => a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            messageClaveDeCanalesIncorrecta.Should().Be(ErrorCode.ClaveDeCanalesIncorrecta.ErrorDescription);
            messageClaveDeCanalesBloqueada.Should().Be(ErrorCode.ClaveDeCanalesBloqueada.ErrorDescription);
        }

        [Fact]
        public async Task ClaveCanalesValidaExpiradaAsync()
        {
            // Arrange
            var usuarioClaveDeCanalesCorrectaExpirada = new UsuarioRegistrado { Active = true, CardNumber = NroTarjetaCorrecto, ForgotPasswordAttempts = 0, ExpiredDateTime = DateTime.MinValue, ChannelKey = ClaveCanalesEncriptadaValida };

            var usuarioRegistradoRepository = new Mock<IUsuarioRegistradoRepository>();
            var auditoriaLog = new Mock<IAuditoriaLogV2Repository>();

            usuarioRegistradoRepository.Setup(m => m.ObtenerUsuarioRegistradoAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(usuarioClaveDeCanalesCorrectaExpirada);

            var sut = CrearClaveCanalesService(usuarioRegistradoRepository.Object, auditoriaLog.Object);

            var bodyClaveDeCanalesCorrectaExpirada = new ValidacionModelInput
            {
                DocumentTypeId = 80,
                DocumentNumber = "",
                ChannelKey = CanalCorrecto
            };

            // Act
            var resultadoClaveDeCanalesCorrectaExpirada = await sut.ValidarAsync(Headers.ToRequestBody(bodyClaveDeCanalesCorrectaExpirada, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            auditoriaLog.Verify(x => x.SaveAuditLogAsync(It.IsAny<int>(),
                                             EventTypes.ChannelKey,
                                             EventResults.Error,
                                             It.IsAny<string>(),
                                             It.IsAny<FechaDbServerV2>(),
                                             It.IsAny<string>()), Times.Once);

            resultadoClaveDeCanalesCorrectaExpirada.IsOk.Should().BeFalse();
            resultadoClaveDeCanalesCorrectaExpirada.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var messageClaveDeCanalesCorrectaExpirada = resultadoClaveDeCanalesCorrectaExpirada.Match(a => a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            messageClaveDeCanalesCorrectaExpirada.Should().Be(ErrorCode.ClaveDeCanalesExpirada.ErrorDescription);
        }

        [Fact]
        public async Task ClaveCanalesValidaThrowsExceptionAsync()
        {
            // Arrange
            var usuarioClaveDeCanalesCorrecta = new UsuarioRegistrado { Active = true, CardNumber = NroTarjetaCorrecto, ForgotPasswordAttempts = 0, ExpiredDateTime = DateTime.MaxValue, ChannelKey = ClaveCanalesEncriptadaValida };

            var usuarioRegistradoRepository = new Mock<IUsuarioRegistradoRepository>();
            var auditoriaLog = new Mock<IAuditoriaLogV2Repository>();

            usuarioRegistradoRepository.Setup(m => m.ObtenerUsuarioRegistradoAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(usuarioClaveDeCanalesCorrecta);

            var sut = CrearClaveCanalesService(usuarioRegistradoRepository.Object, auditoriaLog.Object, true);

            var bodyClaveDeCanalesCorrecta = new ValidacionModelInput
            {
                DocumentTypeId = 80,
                DocumentNumber = "",
                ChannelKey = CanalCorrecto
            };

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() => sut.ValidarAsync(Headers.ToRequestBody(bodyClaveDeCanalesCorrecta, AllowedChannelsBuilder.CrearAllowedChannels())));

            // Assert
            auditoriaLog.Verify(x => x.SaveAuditLogAsync(It.IsAny<int>(),
                                             EventTypes.ChannelKey,
                                             EventResults.Error,
                                             It.IsAny<string>(),
                                             It.IsAny<FechaDbServerV2>(),
                                             It.IsAny<string>()), Times.Never);

            resultado.Message.Should().Be("Test Exception");
        }

        [Fact]
        public async Task ClaveCanalesValidaAsync()
        {
            // Arrange
            var usuarioClaveDeCanalesCorrecta = new UsuarioRegistrado { Active = true, CardNumber = NroTarjetaCorrecto, ForgotPasswordAttempts = 0, ExpiredDateTime = DateTime.MaxValue, ChannelKey = ClaveCanalesEncriptadaValida };

            var usuarioRegistradoRepository = new Mock<IUsuarioRegistradoRepository>();
            var auditoriaLog = new Mock<IAuditoriaLogV2Repository>();

            usuarioRegistradoRepository.Setup(m => m.ObtenerUsuarioRegistradoAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(usuarioClaveDeCanalesCorrecta);

            var sut = CrearClaveCanalesService(usuarioRegistradoRepository.Object, auditoriaLog.Object);

            var bodyClaveDeCanalesCorrecta = new ValidacionModelInput
            {
                DocumentTypeId = 80,
                DocumentNumber = "",
                ChannelKey = CanalCorrecto
            };

            // Act
            var resultadoClaveDeCanalesCorrecta = await sut.ValidarAsync(Headers.ToRequestBody(bodyClaveDeCanalesCorrecta, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            auditoriaLog.Verify(x => x.SaveAuditLogAsync(It.IsAny<int>(),
                                               EventTypes.ChannelKey,
                                               EventResults.Ok,
                                               It.IsAny<string>(),
                                               It.IsAny<FechaDbServerV2>(),
                                               It.IsAny<string>()), Times.Once);

            resultadoClaveDeCanalesCorrecta.IsOk.Should().BeTrue();
            resultadoClaveDeCanalesCorrecta.StatusCode.Should().Be(StatusCodes.Status202Accepted);
        }

        private ClaveCanalesService CrearClaveCanalesService(IUsuarioRegistradoRepository usuarioRegistradoRepository, IAuditoriaLogV2Repository auditoriaLog, bool throwsException = false)
        {
            var loggerMock = new Mock<ILogger<ClaveCanalesService>>();
            var encryption = new Mock<IEncryption>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var helpDbServerV2 = new Mock<IHelperDbServerV2>();

            configuracionesV2Service.Setup(m => m.ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesAsync()).ReturnsAsync(3);

            if (!throwsException)
            {
                encryption.Setup(m => m.EncryptChannelsKey(CanalCorrecto, NroTarjetaCorrecto)).Returns(ClaveCanalesEncriptadaValida);
                encryption.Setup(m => m.EncryptChannelsKey(CanalIncorrecto, NroTarjetaIncorrecto)).Returns(ClaveCanalesEncriptadaInvalida);
            }
            else
            {
                encryption.Setup(m => m.EncryptChannelsKey(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("Test Exception"));
            }

            var helperDbServerMock = new Mock<IHelperDbServer>();
            helperDbServerMock.Setup(m => m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServer { Now = DateTime.Now });

            return new ClaveCanalesService(
                loggerMock.Object,
                encryption.Object,
                helperDbServerMock.Object,
                usuarioRegistradoRepository,
                configuracionesV2Service.Object,
                helpDbServerV2.Object,
                auditoriaLog);
        }

        private const string CanalCorrecto = "CanalCorrecto";
        private const string CanalIncorrecto = "CanalIncorrecto";
        private const string NroTarjetaCorrecto = "NroTarjetaCorrecto";
        private const string NroTarjetaIncorrecto = "NroTarjetaIncorrecto";
        private const string ClaveCanalesEncriptadaValida = "ClaveCanalesEncriptadaValida";
        private const string ClaveCanalesEncriptadaInvalida = "ClaveCanalesEncriptadaInvalida";

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "HBI",
            XUsuario = "user",
            XRequestId = "1"
        };
    }
}
