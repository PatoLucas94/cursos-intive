using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Domain.Exceptions;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Domain.Exceptions
{
    public class BusinessExceptionTest
    {
        [Fact]
        public void ConstructorBusinessException()
        {
            // Arrange
            const string message = "Mensaje de BE";
            const int code = 1;

            // Act
            var be = new BusinessException(message, code);

            // Assert
            be.Message.Should().Be(message);
            be.Code.Should().Be(code);
        }

        [Fact]
        public void ConstructorBusinessExceptionConEventId()
        {
            // Arrange
            const int id = 1;
            const string name = "Test event";
            var eventId = new EventId(id, name);

            // Act
            var be = new BusinessException(eventId);

            // Assert
            be.Code.Should().Be(id);
            be.Errors.Count.Should().Be(1);
            be.Errors.ElementAt(0).Key.Should().Be(be.Code);
            be.Errors.ElementAt(0).Value[0].Should().Be(name);
        }
    }
}
