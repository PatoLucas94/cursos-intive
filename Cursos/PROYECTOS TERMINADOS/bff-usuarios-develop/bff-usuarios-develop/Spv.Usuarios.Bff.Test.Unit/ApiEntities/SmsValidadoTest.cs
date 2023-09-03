using System.Linq;
using FluentAssertions;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.ApiEntities
{
    public class SmsValidadoTest
    {
        [Fact]
        public void Se_corresponde_con_la_especificacion()
        {
            SmsValidado.Si.Valor.Should().Be(true);
            SmsValidado.No.Valor.Should().Be(false);
        }

        [Fact]
        public void TryParse_las_instancias_existentes()
        {
            new[] { "" }
                .Select(s => SmsValidado.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, SmsValidado.EsNull)))
                .AssertAll(r => r
                    .OnOk(rs => rs.Valor.Should().BeNull())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            new[] { "true" }
                .Select(s => SmsValidado.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, SmsValidado.Si)))
                .AssertAll(r => r
                    .OnOk(rs => rs.Valor.Should().BeTrue())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            new[] { "false" }
                .Select(s => SmsValidado.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, SmsValidado.No)))
                .AssertAll(r => r
                    .OnOk(e => e.Valor.Should().BeFalse())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));
        }

        [Fact]
        public void TryParse_retorna_error_para_valores_invalidos()
        {
            new[] { "-", "1", "2" }
                .Select(s => SmsValidado.TryParse(s))
                .AssertAll(r => r.OnOk(e => e.Should().Be($"Debió haber fallado pero devolvió '{e}'")));
        }

        [Fact]
        public void Equality_contract()
        {
            var self = SmsValidado.Si;
            var same = self;

            var otherEqual = SmsValidado.Si;
            var otherDifferent = SmsValidado.No;
            const string otherType = "false";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }
    }
}
