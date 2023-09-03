using FluentAssertions;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.Helpers
{
    public class AllowedChannelsTest
    {
        [Theory]
        [InlineData("obi")]
        [InlineData("obI")]
        [InlineData("oBI")]
        [InlineData("OBI")]
        [InlineData("hbi")]
        [InlineData("hbI")]
        [InlineData("hBI")]
        [InlineData("HBI")]
        public void CanalValido(string canal)
        {
            //Arrange
            var allowedChannelsService = AllowedChannelsBuilder.CrearAllowedChannels();

            //Act
            var valido = allowedChannelsService.IsValidChannel(canal);

            //Assert
            valido.Should().BeTrue();
        }

        [Fact]
        public void CanalInvalido()
        {
            //Arrange
            var allowedChannelsService = AllowedChannelsBuilder.CrearAllowedChannels();

            //Act
            var valido = allowedChannelsService.IsValidChannel("Invalido");

            //Assert
            valido.Should().BeFalse();
        }

        [Fact]
        public void CanalesNoDefinidos()
        {
            //Arrange
            var allowedChannelsService = AllowedChannelsBuilder.CrearEmptyAllowedChannels();

            //Act
            var valido = allowedChannelsService.IsValidChannel("DebeAceptarCualquiera");

            //Assert
            valido.Should().BeTrue();
        }
    }
}
