using FluentAssertions;
using Spv.Usuarios.Bff.Domain.Services;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Domain.Services
{
    public class RequestTest
    {
        [Fact]
        public void UnRequestSePuedeCrear()
        {
            // Act
            var sut = new Request("34b2b562-a9b4-4091-83ac-9cf42b88d35f");

            // Assert
            sut.XRequestId.Should().Be("34b2b562-a9b4-4091-83ac-9cf42b88d35f");
        }

        [Fact]
        public void UnRequestSePuedeCrearConBody()
        {
            // Arrange
            const string body = "test";

            // Act
            var sut = new RequestBody<string>("e5a06bdc-71fc-4e76-9e66-33751ac801df", body);

            // Assert
            sut.XRequestId.Should().Be("e5a06bdc-71fc-4e76-9e66-33751ac801df");
            sut.Body.Should().Be(body);
        }

        [Fact]
        public void SePuedeCrearRequestRelacionada()
        {
            // Arrange
            const string body = "test";
            
            // Act
            var sut = new RequestBody<string>("aeb0b3aa-3579-4141-945e-97b7383d87fb", body);

            // Assert
            sut.XRequestId.Should().Be("aeb0b3aa-3579-4141-945e-97b7383d87fb");
            sut.Body.Should().Be(body);

            // Arrange
            const string relatedBody = "test1";

            // Act
            var related = sut.MakeRelated(relatedBody);

            // Assert
            related.XRequestId.Should().Be("aeb0b3aa-3579-4141-945e-97b7383d87fb");
            related.Body.Should().Be(relatedBody);
        }
    }
}
