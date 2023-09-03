using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Dtos.ClaveCanalesService.Input;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Service;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.ClaveCanalService
{
    public class InhabilitarAsyncTest
    {
        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "HBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        [Fact]
        public async Task InhabilitarClaveCanalesThrowsExceptionAsync()
        {
            // Arrange
            var usuarioClaveDeCanalesCorrecta = new UsuarioRegistrado { Active = true, CardNumber = "NroTarjetaCorrecto", ForgotPasswordAttempts = 0, ExpiredDateTime = DateTime.MaxValue, ChannelKey = "ClaveCanalesEncriptadaValida" };


            var usuarioRegistradoRepository = new Mock<IUsuarioRegistradoRepository>();

            usuarioRegistradoRepository.Setup(m => m.ObtenerUsuarioRegistradoAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(usuarioClaveDeCanalesCorrecta);

            var sut = CrearClaveCanalesService(usuarioRegistradoRepository.Object);

            var bodyClaveDeCanalesCorrecta = new InhabilitacionModelInput
            {
                DocumentTypeId = 80,
                DocumentNumber = "",
                ChannelKey = ""
            };

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() => sut.InhabilitarAsync(Headers.ToRequestBody(bodyClaveDeCanalesCorrecta, AllowedChannelsBuilder.CrearAllowedChannels())));

            // Assert
            resultado.Message.Should().Be("Test Exception");
        }

        private ClaveCanalesService CrearClaveCanalesService(IUsuarioRegistradoRepository usuarioRegistradoRepository)
        {
            var loggerMock = new Mock<ILogger<ClaveCanalesService>>();
            var encryption = new Mock<IEncryption>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var helpDbServerV2 = new Mock<IHelperDbServerV2>();
            var auditoriaLog = new Mock<IAuditoriaLogV2Repository>();

            configuracionesV2Service.Setup(m => m.ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesAsync());

            encryption.Setup(m => m.EncryptChannelsKey(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("Test Exception"));

            var helperDbServerMock = new Mock<IHelperDbServer>();
            helperDbServerMock.Setup(m => m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServer { Now = DateTime.Now });

            return new ClaveCanalesService(
                loggerMock.Object,
                encryption.Object,
                helperDbServerMock.Object,
                usuarioRegistradoRepository,
                configuracionesV2Service.Object,
                helpDbServerV2.Object,
                auditoriaLog.Object);
        }
    }
}
