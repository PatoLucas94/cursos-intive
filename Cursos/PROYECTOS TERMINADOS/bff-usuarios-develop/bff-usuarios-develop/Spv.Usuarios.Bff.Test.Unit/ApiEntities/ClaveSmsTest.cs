using System;
using System.Linq;
using FluentAssertions;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.ApiEntities
{
    public class ClaveSmsTest
    {
        [Fact]
        public void TryParse_las_instancias_existentes()
        {
            new[] { "" }
                .Select(s => ClaveSms.TryParse(s))
                .Select(r => r.Where(g => ReferenceEquals(g.Valor, null)))
                .AssertAll(r => r
                    .OnOk(e => e.Should().Be($"Debió haber fallado pero devolvió '{e}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentOutOfRangeException>()));

            var valor = new string('9', ClaveSms.Longitud);
            var valorEsperado = valor;

            new[] { valor }
                .Select(s => ClaveSms.TryParse(s))
                .Select(r => r.Where(g => g.Valor == valorEsperado))
                .AssertAll(r => r
                    .OnOk(cf => cf.Valor.Should().Be(valorEsperado))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}")));


            valor = new string('9', ClaveSms.Longitud + 1);

            new[] { valor }
                .Select(s => ClaveSms.TryParse(s))
                .Select(r => r.Where(g => g.Valor == valorEsperado))
                .AssertAll(r => r
                    .OnOk(e => e.Should().Be($"Debió haber fallado pero devolvió '{e}'"))
                    .OnError(e => e.Should().BeOfType<ArgumentOutOfRangeException>()));
        }

        [Fact]
        public void Equality_contract()
        {
            var self = new ClaveSms("111111");
            var same = self;

            var otherEqual = new ClaveSms("111111");
            var otherDifferent = new ClaveSms("222222");
            const string otherType = "false";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }

        [Fact]
        public void ToString_retorna_valor()
        {
            var sut = new ClaveSms("111111");
            sut.ToString().Should().Be(sut.Valor);
        }
    }
}
