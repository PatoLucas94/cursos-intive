using FluentAssertions;
using Spv.Usuarios.Common.Errors;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Common.Errors
{
    public class ErrorCodeTest
    {
        [Fact]
        public void EqualsPositivoTest()
        {
            // Arrange
            const string code = "1";
            const string errorDescription = "ErrorDescription";
            var errorCode = new ErrorCode(code, errorDescription);

            // Act
            var sut = new ErrorCode(code, errorDescription);
            var resultado = sut.Equals(errorCode);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public void EqualsNegativoTest()
        {
            // Arrange
            const string code = "1";
            const string errorDescription = "ErrorDescription";
            var errorCode = new ErrorCode("2", errorDescription);

            // Act
            var sut = new ErrorCode(code, errorDescription);
            var resultado = sut.Equals(errorCode);

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            const string code = "1";
            const string errorDescription = "ErrorDescription";

            // Act
            var sut = new ErrorCode(code, errorDescription);
            var resultado = code.GetHashCode() ^ errorDescription.GetHashCode();

            // Assert
            sut.GetHashCode().Should().Be(resultado);
        }
    }
}
