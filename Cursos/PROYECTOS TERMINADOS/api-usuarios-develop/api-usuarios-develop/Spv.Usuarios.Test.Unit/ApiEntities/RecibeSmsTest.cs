using System.Linq;
using FluentAssertions;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Test.Unit.ApiEntities
{
    public class RecibeSmsTest
    {
        [Fact]
        public void Se_corresponde_con_la_especificacion()
        {
            RecibeSms.Recibe.Valor.Should().Be(true);
            RecibeSms.NoRecibe.Valor.Should().Be(false);
        }

        [Fact]
        public void TryParse_las_instancias_existentes()
        {
            new[] { "" }
                .Select(s => RecibeSms.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, RecibeSms.EsNull)))
                .AssertAll(r => r
                    .OnOk(rs => rs.Valor.Should().BeNull())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            new[] { "true" }
                .Select(s => RecibeSms.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, RecibeSms.Recibe)))
                .AssertAll(r => r
                    .OnOk(rs => rs.Valor.Should().BeTrue())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            new[] { "false" }
                .Select(s => RecibeSms.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, RecibeSms.NoRecibe)))
                .AssertAll(r => r
                    .OnOk(e => e.Valor.Should().BeFalse())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));
        }

        [Fact]
        public void TryParse_retorna_error_para_valores_invalidos()
        {
            new[] { "-", "1", "2" }
                .Select(s => RecibeSms.TryParse(s))
                .AssertAll(r => r.OnOk(e => e.Should().Be($"Debió haber fallado pero devolvió '{e}'")));
        }

        [Fact]
        public void Equality_contract()
        {
            var self = RecibeSms.Recibe;
            var same = self;

            var otherEqual = RecibeSms.Recibe;
            var otherDifferent = RecibeSms.NoRecibe;
            const string otherType = "false";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }
    }
}
