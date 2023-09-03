using System;
using System.Linq;
using FluentAssertions;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.ApiEntities
{
    public class IdPaisTest
    {
        [Fact]
        public void TryParse_las_instancias_existentes()
        {
            new[] { "" }
                .Select(s => IdPais.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g.Valor, null)))
                .AssertAll(r => r
                    .OnOk(p => p.Valor.ToString().Should().Be($"Debió haber fallado pero devolvió '{p.Valor}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>()));

            new[] { " " }
                .Select(s => IdPais.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g.Valor, null)))
                .AssertAll(r => r
                    .OnOk(p => p.Valor.ToString().Should().Be($"Debió haber fallado pero devolvió '{p.Valor}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>()));

            new[] { "valor" }
                .Select(s => IdPais.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g.Valor, null)))
                .AssertAll(r => r
                    .OnOk(p => p.Valor.ToString().Should().Be($"Debió haber fallado pero devolvió '{p.Valor}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>()));

            new[] { 0 }
                .Select(s => IdPais.TryParse(s.ToString()))
                .Select(r => r.Where(g => ReferenceEquals(g.Valor, null)))
                .AssertAll(r => r
                    .OnOk(p => p.Valor.ToString().Should().Be($"Debió haber fallado pero devolvió '{p.Valor}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentOutOfRangeException>()));

            new[] { 10 }
                .Select(s => IdPais.TryParse(s.ToString()))
                .Select(r => r.Where(g => g.Valor == 10))
                .AssertAll(r => r
                    .OnOk(cf => cf.Valor.Should().Be(10))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));
        }
    }
}
