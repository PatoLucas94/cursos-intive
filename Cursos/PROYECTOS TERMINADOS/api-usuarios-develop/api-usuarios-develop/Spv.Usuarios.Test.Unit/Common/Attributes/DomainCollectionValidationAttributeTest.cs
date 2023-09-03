using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Spv.Usuarios.Api.Common.Attributes;
using Spv.Usuarios.Domain.ApiEntities;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Common.Attributes
{
    public class DomainCollectionValidationAttributeTest
    {
        [Fact]
        public void No_se_permite_null()
        {
            var sut = new DomainCollectionValidationAttribute(typeof(IdTipoDocumento)) { CantidadMinima = 1 };
            var value = new int[0];
            var result = sut.GetValidationResult(null, new ValidationContext(value) { MemberName = "id_tipo_documento" });
            result?.ErrorMessage.Should().Contain("'id_tipo_documento' no puede ser null");
        }

        [Fact]
        public void El_valor_debe_ser_array()
        {
            var sut = new DomainCollectionValidationAttribute(typeof(IdTipoDocumento)) { CantidadMinima = 1 };
            var value = new { bla = 1 };
            var result = sut.GetValidationResult(value, new ValidationContext(value) { MemberName = "id_tipo_documento" });
            result?.ErrorMessage.Should().Contain("'id_tipo_documento' debe ser un array.");
        }

        [Fact]
        public void La_coleccion_debe_tener_al_menos_la_cantidad_minima_de_elementos()
        {
            var sut = new DomainCollectionValidationAttribute(typeof(IdTipoDocumento)) { CantidadMinima = 1 };
            var value = new int[0];
            var result = sut.GetValidationResult(value, new ValidationContext(value) { MemberName = "id_tipo_documento" });
            result?.ErrorMessage.Should().Contain("'id_tipo_documento' debe contener como mínimo '1' elemento.");
        }

        [Fact]
        public void La_coleccion_debe_tener_como_máximo_la_cantidad_maxima_de_elementos()
        {
            var sut = new DomainCollectionValidationAttribute(typeof(IdTipoDocumento)) { CantidadMinima = 2, CantidadMaxima = 2 };
            var value = new int[3];
            var result = sut.GetValidationResult(value, new ValidationContext(value) { MemberName = "id_tipo_documento" });
            result?.ErrorMessage.Should().Contain("'id_tipo_documento' debe contener como máximo '2' elementos.");
        }

        [Fact]
        public void Si_contiene_valor_menor_que_minimo_lanza_error()
        {
            var sut = new DomainCollectionValidationAttribute(typeof(IdTipoDocumento)) { CantidadMinima = 1 };
            var value = new List<int> { IdTipoDocumento.ValorMinimo - 1 };
            var result = sut.GetValidationResult(value, new ValidationContext(value) { MemberName = "id_tipo_documento" });
            result?.ErrorMessage.Should().Contain("'id_tipo_documento[0]' debe estar entre");
        }

        [Fact]
        public void Si_contiene_valor_mayor_al_maximo_lanza_error()
        {
            var sut = new DomainCollectionValidationAttribute(typeof(IdTipoDocumento)) { CantidadMinima = 1 };
            var value = new List<int> { 1, 7, IdTipoDocumento.ValorMaximo + 1};
            var result = sut.GetValidationResult(value, new ValidationContext(value) { MemberName = "id_tipo_documento" });
            result?.ErrorMessage.Should().Contain("'id_tipo_documento[2]' debe estar entre");
        }

        [Fact]
        public void Si_contiene_valor_mayor_valido_es_caso_exitoso()
        {
            var sut = new DomainCollectionValidationAttribute(typeof(IdTipoDocumento)) { CantidadMinima = 1 };
            var value = new List<int>(1) { IdTipoDocumento.ValorMaximo };
            var result = sut.GetValidationResult(value, new ValidationContext(value) { MemberName = "id_tipo_documento" });
            result?.Equals(ValidationResult.Success).Should().BeTrue();
        }
    }
}
