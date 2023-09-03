using FluentAssertions;
using Spv.Usuarios.Common.Information;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Common.Information
{
    public class InformationCodeTest
    {
        [Fact]
        public void EqualsPositivoTest()
        {
            // Arrange
            const string code = "CAV";
            const string InformationDescription = "InformationDescription";
            var InformationCode = new InformationCode(code, InformationDescription);

            // Act
            var sut = new InformationCode(code, InformationDescription);
            var resultado = sut.Equals(InformationCode);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public void EqualsNegativoTest()
        {
            // Arrange
            const string code = "1";
            const string InformationDescription = "InformationDescription";
            var errorCode = new InformationCode("2", InformationDescription);

            // Act
            var sut = new InformationCode(code, InformationDescription);
            var resultado = sut.Equals(errorCode);

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            const string code = "1";
            const string InformationDescription = "InformationDescription";

            // Act
            var sut = new InformationCode(code, InformationDescription);
            var resultado = code.GetHashCode() ^ InformationDescription.GetHashCode();

            // Assert
            sut.GetHashCode().Should().Be(resultado);
        }
    }
}
