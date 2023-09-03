using System;
using System.Linq;
using FluentAssertions;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.ApiEntities
{
    public class IdPersonaTest
    {
        [Fact]
        public void TryParse_las_instancias_existentes()
        {
            new[] { "" }
                .Select(s => IdPersona.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g.Valor, null)))
                .AssertAll(r => r
                    .OnOk(e => e.Should().Be($"Debió haber fallado pero devolvió '{e}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>()));

            var valor = new string('1', IdPersona.LongitudMaxima).Substring(0, IdPersona.LongitudMinima);

            new[] { valor }
                .Select(s => IdPersona.TryParse(s))
                .Select(r => r.Where(g => g.Valor == "1"))
                .AssertAll(r => r
                    .OnOk(cf => cf.Valor.Should().Be("1"))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            valor = new string('9', IdPersona.LongitudMaxima).Substring(0, IdPersona.LongitudMaxima);
            var valorEsperado = valor;

            new[] { valor }
                .Select(s => IdPersona.TryParse(s))
                .Select(r => r.Where(g => g.Valor == valorEsperado))
                .AssertAll(r => r
                    .OnOk(cf => cf.Valor.Should().Be(valorEsperado))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));


            valor = new string('9', IdPersona.LongitudMaxima + 1).Substring(0, IdPersona.LongitudMaxima + 1);

            new[] { valor }
                .Select(s => IdPersona.TryParse(s))
                .Select(r => r.Where(g => g.Valor == valorEsperado))
                .AssertAll(r => r
                    .OnOk(e => e.Should().Be($"Debió haber fallado pero devolvió '{e}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentOutOfRangeException>()));
        }

        [Fact]
        public void Equality_contract()
        {
            var self = new IdPersona("1");
            var same = self;

            var otherEqual = new IdPersona("1");
            var otherDifferent = new IdPersona("2");
            const string otherType = "false";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }

        [Fact]
        public void ToString_retorna_valor()
        {
            var sut = new IdPersona("1");
            sut.ToString().Should().Be(sut.Valor);
        }
    }
}
