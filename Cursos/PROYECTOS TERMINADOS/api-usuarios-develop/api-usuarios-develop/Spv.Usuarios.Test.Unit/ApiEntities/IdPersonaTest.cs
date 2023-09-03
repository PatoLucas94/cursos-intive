using System;
using System.Linq;
using FluentAssertions;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Test.Unit.ApiEntities
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
                    .OnOk(p => p.Valor.ToString().Should().Be($"Debió haber fallado pero devolvió '{p.Valor}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>()));

            new[] { " " }
                .Select(s => IdPersona.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g.Valor, null)))
                .AssertAll(r => r
                    .OnOk(p => p.Valor.ToString().Should().Be($"Debió haber fallado pero devolvió '{p.Valor}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>()));

            new[] { "valor" }
                .Select(s => IdPersona.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g.Valor, null)))
                .AssertAll(r => r
                    .OnOk(p => p.Valor.ToString().Should().Be($"Debió haber fallado pero devolvió '{p.Valor}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>()));

            new[] { 0 }
                .Select(s => IdPersona.TryParse(s.ToString()))
                .Select(r => r.Where(g => ReferenceEquals(g.Valor, null)))
                .AssertAll(r => r
                    .OnOk(p => p.Valor.ToString().Should().Be($"Debió haber fallado pero devolvió '{p.Valor}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentOutOfRangeException>()));

            new[] { 10 }
                .Select(s => IdPersona.TryParse(s.ToString()))
                .Select(r => r.Where(g => g.Valor == 10))
                .AssertAll(r => r
                    .OnOk(cf => cf.Valor.Should().Be(10))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));
        }

        [Fact]
        public void Equality_contract()
        {
            var self = new IdPersona(1);
            var same = self;

            var otherEqual = new IdPersona(1);
            var otherDifferent = new IdPersona(2);
            const string otherType = "false";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }

        [Fact]
        public void Se_puede_asignar_a_un_string()
        {
            const int expected = 1;
            AsString(new IdPersona(expected)).Should().Be(expected.ToString());
        }

        private static string AsString(IdPersona e) => e.ToString();
    }
}
