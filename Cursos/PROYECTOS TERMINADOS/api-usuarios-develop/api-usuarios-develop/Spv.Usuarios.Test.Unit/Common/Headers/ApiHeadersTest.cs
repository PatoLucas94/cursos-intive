using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Domain.Exceptions;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Common.Headers
{
    public class ApiHeadersTest
    {
        private static readonly ApiHeaders HeadersInvalidChannel = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "CanalInvalido",
            XUsuario = "user",
            XRequestId = "1"
        };

        [Fact]
        public void IsValidThrowsException()
        {
            //Arrange
            var allowedChannelsService = AllowedChannelsBuilder.CrearAllowedChannels();

            //Act
            var exception = Assert.Throws<BusinessException>(() => 
                HeadersInvalidChannel.ToRequestBody(
                    new AutenticacionModelInput
                    {
                        UserName = "Prueba", 
                        Password = "Prueba"
                    }, 
                    allowedChannelsService));

            //Assert
            exception.Code.Should().Be(StatusCodes.Status401Unauthorized);
            exception.Message.Should().Be(MessageConstants.ChannelHeaderInvalidMessage);
        }
    }
}
