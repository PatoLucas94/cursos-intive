using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Attributes;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Common.Attributes
{
    public class DomainValidationAttributeTests
    {
        [Theory]
        [InlineData(typeof(IdPersona), "1", nameof(IdPersona))]
        [InlineData(typeof(IdPersona), "99999999", nameof(IdPersona))]
        [InlineData(typeof(CredUsuario), "MyTestUser", nameof(CredUsuario))]
        [InlineData(typeof(CredUsuario), " Usu4r10S", nameof(CredUsuario))]
        public void Valida_OK_basado_en_el_tipo_especificado(Type type, object value, string memberName)
        {
            var sut = new DomainValidationAttribute(type);
            var result = sut.GetValidationResult(value, new ValidationContext(type) { MemberName = memberName });
            result.Should().Be(ValidationResult.Success);
        }

        [Theory]
        [InlineData(typeof(IdPersona), null, nameof(IdPersona), "no puede ser null")]
        [InlineData(typeof(IdPersona), "", nameof(IdPersona), "no puede ser vacío")]
        [InlineData(typeof(CredClave), "", nameof(CredClave), "no puede ser vacío")]
        [InlineData(typeof(CredClave), "    ", nameof(CredClave), "no puede contener solo espacios")]
        [InlineData(typeof(CredClave), "123", nameof(CredClave), "La longitud de")]
        [InlineData(typeof(CredClave), "123a", nameof(CredClave), "debe contener solo dígitos")]
        [InlineData(typeof(CredUsuario), "", nameof(CredUsuario), "no puede ser vacío")]
        [InlineData(typeof(CredUsuario), "    ", nameof(CredUsuario), "no puede contener solo espacios")]
        [InlineData(typeof(CredUsuario), "1234567", nameof(CredUsuario), "La longitud de")]
        public void Valida_basado_en_el_tipo_especificado(Type type, object value, string memberName, string expectedMessage)
        {
            var sut = new DomainValidationAttribute(type);
            var result = sut.GetValidationResult(value, new ValidationContext(type) { MemberName = memberName });
            result.Should().NotBeNull();
            result?.ErrorMessage.Should().Contain(expectedMessage);
        }
    }
}
