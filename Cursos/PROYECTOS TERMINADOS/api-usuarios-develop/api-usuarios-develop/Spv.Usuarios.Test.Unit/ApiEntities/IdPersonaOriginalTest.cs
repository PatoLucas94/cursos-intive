using System.Linq;
using FluentAssertions;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Test.Unit.ApiEntities
{
    public class IdPersonaOriginalTest
    {
        [Fact]
        public void TryParse_las_instancias_existentes()
        {
            new[] { "" }
                .Select(s => IdPersonaOriginal.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g.Valor, null)))
                .AssertAll(r => r
                    .OnOk(cf => cf.Valor.Should().BeNull())
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            var valor = new string('1', IdPersonaOriginal.LongitudMaxima).Substring(0, IdPersonaOriginal.LongitudMinima);

            new[] { valor }
                .Select(s => IdPersonaOriginal.TryParse(s))
                .Select(r => r.Where(g => g.Valor == "1"))
                .AssertAll(r => r
                    .OnOk(cf => cf.Valor.Should().Be("1"))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));

            valor = new string('9', IdPersonaOriginal.LongitudMaxima).Substring(0, IdPersonaOriginal.LongitudMaxima);
            var valorEsperado = valor;

            new[] { valor }
                .Select(s => IdPersonaOriginal.TryParse(s))
                .Select(r => r.Where(g => g.Valor == valorEsperado))
                .AssertAll(r => r
                    .OnOk(cf => cf.Valor.Should().Be(valorEsperado))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));
        }
    }
}
