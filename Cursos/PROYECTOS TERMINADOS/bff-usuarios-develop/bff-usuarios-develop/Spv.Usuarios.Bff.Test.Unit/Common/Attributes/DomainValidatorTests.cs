using FluentAssertions;
using Spv.Usuarios.Bff.Common.Attributes;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Common.Attributes
{
    public class DomainValidatorTests
    {
        [Theory]
        [InlineData(null, "IdPersona", "no puede ser null")]
        [InlineData("", "IdPersona", "no puede ser vacío")]
        [InlineData("   ", "IdPersona", "no puede contener solo espacios")]
        public void Valida_basado_en_el_tipo_especificado(string valor, string fieldName, string mensajeEsperado)
        {
            var sut = new DomainValidator<IdPersona>();

            var result = sut.Validate(valor, fieldName);
            result.ErrorMessage.Should().Contain(mensajeEsperado);
        }
    }
}
