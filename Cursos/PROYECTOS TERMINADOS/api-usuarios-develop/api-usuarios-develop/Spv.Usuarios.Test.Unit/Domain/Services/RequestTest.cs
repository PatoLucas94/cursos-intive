using FluentAssertions;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Domain.Services;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Domain.Services
{
    public class RequestTest
    {
        [Fact]
        public void UnRequestSePuedeCrear()
        {
            // Act
            var sut = new Request("34b2b562-a9b4-4091-83ac-9cf42b88d35f", "C", "U", "A");

            // Assert
            sut.XRequestId.Should().Be("34b2b562-a9b4-4091-83ac-9cf42b88d35f");
            sut.XCanal.Should().Be("C");
            sut.XUsuario.Should().Be("U");
            sut.XAplicacion.Should().Be("A");
        }

        [Fact]
        public void UnRequestSePuedeCrearConBody()
        {
            // Arrange
            var body = new AutenticacionModelInput
            {
                UserName = "usuario_test",
                Password = "clave_test"
            };

            // Act
            var sut = new RequestBody<AutenticacionModelInput>("e5a06bdc-71fc-4e76-9e66-33751ac801df", "C", "U", "A", body);

            // Assert
            sut.XRequestId.Should().Be("e5a06bdc-71fc-4e76-9e66-33751ac801df");
            sut.XCanal.Should().Be("C");
            sut.XUsuario.Should().Be("U");
            sut.XAplicacion.Should().Be("A");
            sut.Body.Should().Be(body);
        }

        [Fact]
        public void SePuedeCrearRequestRelacionada()
        {
            // Arrange
            var body = new AutenticacionModelInput
            {
                UserName = "usuario_test",
                Password = "clave_test"
            };

            // Act
            var sut = new RequestBody<AutenticacionModelInput>("aeb0b3aa-3579-4141-945e-97b7383d87fb", "C", "U", "A", body);

            // Assert
            sut.XRequestId.Should().Be("aeb0b3aa-3579-4141-945e-97b7383d87fb");
            sut.XCanal.Should().Be("C");
            sut.XUsuario.Should().Be("U");
            sut.XAplicacion.Should().Be("A");
            sut.Body.Should().Be(body);

            // Arrange
            var relatedBody = new AutenticacionModelInput
            {
                UserName = "usuario_test_1",
                Password = "clave_test_1"
            };

            // Act
            var related = sut.MakeRelated(relatedBody);

            // Assert
            related.XRequestId.Should().Be("aeb0b3aa-3579-4141-945e-97b7383d87fb");
            related.XCanal.Should().Be("C");
            related.XUsuario.Should().Be("U");
            related.XAplicacion.Should().Be("A");
            related.Body.Should().Be(relatedBody);
        }

        [Fact]
        public void SePuedeCrearRequestRelacionadaPartiendoDelBody()
        {
            // Arrange
            var body = new AutenticacionModelInput
            {
                UserName = "usuario_test",
                Password = "clave_test"
            };

            // Act
            var sut = new RequestBody<AutenticacionModelInput>("648bf4a1-53f7-4266-a32e-3df96cfd9772", "C", "U", "A", body);

            // Assert
            sut.XRequestId.Should().Be("648bf4a1-53f7-4266-a32e-3df96cfd9772");
            sut.XCanal.Should().Be("C");
            sut.XUsuario.Should().Be("U");
            sut.XAplicacion.Should().Be("A");
            sut.Body.Should().Be(body);

            // Arrange - Act
            var related = sut.Map(idc => new AutenticacionModelInput
            {
                UserName = idc.UserName,
                Password = idc.Password
            });

            // Assert
            related.XRequestId.Should().Be("648bf4a1-53f7-4266-a32e-3df96cfd9772");
            related.XCanal.Should().Be("C");
            related.XUsuario.Should().Be("U");
            related.XAplicacion.Should().Be("A");
            related.Body.UserName.Should().Be(body.UserName);
            related.Body.Password.Should().Be(body.Password);
        }
    }
}
