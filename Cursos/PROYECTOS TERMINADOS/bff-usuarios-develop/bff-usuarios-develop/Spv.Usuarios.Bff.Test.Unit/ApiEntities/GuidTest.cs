using System;
using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Bff.Domain.Utils;
using Spv.Usuarios.Bff.Test.Unit.Util;
using Xunit;
using ApiEntityGuid = Spv.Usuarios.Bff.Domain.ApiEntities.Guid;

namespace Spv.Usuarios.Bff.Test.Unit.ApiEntities
{
    public class GuidTest
    {
        [Fact]
        public void Se_puede_crear()
        {
            const string expected = "d544ad9a-8e1e-4cef-b8ca-25e024d3ac52";
            var sut = new ApiEntityGuid(expected);
            sut.Valor.Should().Be(expected);
        }

        private static IEnumerable<Result<ApiEntityGuid>> ResultsOf(string valor)
        {
            return new[] { Result.Of(() => new ApiEntityGuid(valor)), ApiEntityGuid.TryParse(valor) };
        }

        [Fact]
        public void Requiere_ser_GUID()
        {
            const string valor = "d544ad9a-8e1e-4cef-b8ca-25e024d3ac52";

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(valor))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}"));
            }
        }

        [Fact]
        public void No_puede_ser_Guid_Empty()
        {
            const string valor = "00000000-0000-0000-0000-000000000000";

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be($"debió haber fallado para {e}"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>());
            }
        }

        [Fact]
        public void Equality_contract()
        {
            var self = new ApiEntityGuid("d544ad9a-8e1e-4cef-b8ca-25e024d3ac52");
            var same = self;
            var otherEqual = new ApiEntityGuid("d544ad9a-8e1e-4cef-b8ca-25e024d3ac52");
            var otherDifferent = new ApiEntityGuid("ce56fe4e-73ba-4833-b45c-4f1547c305d6");
            const string otherType = "208637d1-1d8c-4972-bc01-a904069895f1";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }

        [Fact]
        public void ToString_retorna_valor()
        {
            var sut = new ApiEntityGuid("d544ad9a-8e1e-4cef-b8ca-25e024d3ac52");
            sut.ToString().Should().Be(sut.Valor);
        }

        [Fact]
        public void Se_puede_asignar_a_un_string()
        {
            const string expected = "d544ad9a-8e1e-4cef-b8ca-25e024d3ac52";
            AsString(new ApiEntityGuid(expected)).Should().Be(expected);
            AsString(null).Should().Be("0");
        }

        private static string AsString(ApiEntityGuid e) => e;
    }
}
