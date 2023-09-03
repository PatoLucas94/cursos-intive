using FluentAssertions;
using Spv.Usuarios.Api.Common.Attributes;
using Spv.Usuarios.Domain.ApiEntities;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Common.Attributes
{
    public class DomainValidatorTests
    {
        [Theory]
        [InlineData(null, "CredUsuario", "no puede ser null")]
        [InlineData("", "CredUsuario", "no puede ser vacío")]
        [InlineData("   ", "CredUsuario", "no puede contener solo espacios")]
        public void Valida_basado_en_el_tipo_especificado(string valor, string fieldName, string mensajeEsperado)
        {
            var sut = new DomainValidator<CredUsuario>();

            var result = sut.Validate(valor, fieldName);
            result.ErrorMessage.Should().Contain(mensajeEsperado);
        }
    }
}
