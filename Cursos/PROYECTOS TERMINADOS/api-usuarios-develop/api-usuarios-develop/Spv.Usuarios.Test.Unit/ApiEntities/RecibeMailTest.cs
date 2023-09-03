using System.Linq;
using FluentAssertions;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Test.Unit.ApiEntities
{
    public class RecibeMailTest
    {
        [Fact]
        public void Se_corresponde_con_la_especificacion()
        {
            RecibeMail.Recibe.Valor.Should().Be(true);
            RecibeMail.NoRecibe.Valor.Should().Be(false);
        }

        [Fact]
        public void TryParse_las_instancias_existentes()
        {
            new[] { "" }
                .Select(s => RecibeMail.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, RecibeMail.EsNull)))
                .AssertAll(r => r
                    .OnOk(rm => rm.Valor.Should().BeNull())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            new[] { "True" }
                .Select(s => RecibeMail.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, RecibeMail.Recibe)))
                .AssertAll(r => r
                    .OnOk(rm => rm.Valor.Should().BeTrue())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));
                
            new[] { "False" }
                .Select(s => RecibeMail.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, RecibeMail.NoRecibe)))
                .AssertAll(r => r
                    .OnOk(e => e.Valor.Should().BeFalse())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));
        }

        [Fact]
        public void TryParse_retorna_error_para_valores_invalidos()
        {
            new[] { "-", "1", "2" }
                .Select(s => RecibeMail.TryParse(s))
                .AssertAll(r => r.OnOk(e => e.Should().Be($"Debió haber fallado pero devolvió '{e}'")));
        }

        [Fact]
        public void Equality_contract()
        {
            var self = RecibeMail.Recibe;
            var same = self;

            var otherEqual = RecibeMail.Recibe;
            var otherDifferent = RecibeMail.NoRecibe;
            const string otherType = "false";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }
    }
}
