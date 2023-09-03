using System.Linq;
using FluentAssertions;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Test.Unit.ApiEntities
{
    public class ControlFullTest
    {
        [Fact]
        public void Se_corresponde_con_la_especificacion()
        {
            ControlFull.EsNull.Valor.Should().Be(null);
            ControlFull.EsTrue.Valor.Should().Be(true);
            ControlFull.EsFalse.Valor.Should().Be(false);
        }

        [Fact]
        public void TryParse_las_instancias_existentes()
        {
            new[] { "" }
                .Select(s => ControlFull.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, ControlFull.EsNull)))
                .AssertAll(r => r
                    .OnOk(cf => cf.Valor.Should().BeNull())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            new[] { "true" }
                .Select(s => ControlFull.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, ControlFull.EsTrue)))
                .AssertAll(r => r
                    .OnOk(cf => cf.Valor.Should().BeTrue())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            new[] { "false" }
                .Select(s => ControlFull.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, ControlFull.EsFalse)))
                .AssertAll(r => r
                    .OnOk(cf => cf.Valor.Should().BeFalse())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));
        }

        [Fact]
        public void TryParse_retorna_error_para_valores_invalidos()
        {
            new[] { "-", "1", "2" }
                .Select(s => ControlFull.TryParse(s))
                .AssertAll(r => r.OnOk(e => e.Should().Be($"Debió haber fallado pero devolvió '{e}'")));
        }

        [Fact]
        public void Equality_contract()
        {
            var self = ControlFull.EsTrue;
            var same = self;

            var otherEqual = ControlFull.EsTrue;
            var otherDifferent = ControlFull.EsFalse;
            const string otherType = "false";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }
    }
}
