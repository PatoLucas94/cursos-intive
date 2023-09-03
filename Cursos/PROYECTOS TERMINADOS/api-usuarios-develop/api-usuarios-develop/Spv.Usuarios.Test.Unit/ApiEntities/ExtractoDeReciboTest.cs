using System;
using System.Linq;
using FluentAssertions;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Test.Unit.ApiEntities
{
    public class ExtractoDeReciboTest
    {
        [Fact]
        public void Se_corresponde_con_la_especificacion()
        {
            ExtractoDeRecibo.EsNull.Valor.Should().Be(null);
            ExtractoDeRecibo.EsTrue.Valor.Should().Be(true);
            ExtractoDeRecibo.EsFalse.Valor.Should().Be(false);
        }

        [Fact]
        public void TryParse_las_instancias_existentes()
        {
            new[] { "" }
                .Select(s => ExtractoDeRecibo.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, ExtractoDeRecibo.EsNull)))
                .AssertAll(r => r
                    .OnOk(edr => edr.Valor.Should().BeNull())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            new[] { "true" }
                .Select(s => ExtractoDeRecibo.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, ExtractoDeRecibo.EsTrue)))
                .AssertAll(r => r
                    .OnOk(edr => edr.Valor.Should().BeTrue())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            new[] { "false" }
                .Select(s => ExtractoDeRecibo.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g, ExtractoDeRecibo.EsFalse)))
                .AssertAll(r => r
                    .OnOk(edr => edr.Valor.Should().BeFalse())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));
        }

        [Fact]
        public void TryParse_retorna_error_para_valores_invalidos()
        {
            new[] { "-", "1", "2" }
                .Select(s => ExtractoDeRecibo.TryParse(s))
                .AssertAll(r => r
                    .OnOk(e => e.Should().Be($"Debió haber fallado pero devolvió '{e}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>()));
        }

        [Fact]
        public void Equality_contract()
        {
            var self = ExtractoDeRecibo.EsTrue;
            var same = self;

            var otherEqual = ExtractoDeRecibo.EsTrue;
            var otherDifferent = ExtractoDeRecibo.EsFalse;
            const string otherType = "false";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }
    }
}
