using System;
using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Domain.Utils;
using Spv.Usuarios.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Test.Unit.ApiEntities
{
    public class NombreTest
    {
        [Fact]
        public void Se_puede_crear()
        {
            const string expected = "Nombre";
            var sut = new Nombre(expected);
            sut.Valor.Should().Be(expected);
        }

        private static IEnumerable<Result<Nombre>> ResultsOf(string valor)
        {
            return new[] { Result.Of(() => new Nombre(valor)), Nombre.TryParse(valor) };
        }

        [Fact]
        public void Requiere_la_mínima_cantidad_de_caracteres()
        {
            var valor = new string('u', Nombre.LongitudMaxima).Substring(0, Nombre.LongitudMinima);

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(valor))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}"));
            }

            var valorMuyCorto = valor.Substring(0, Nombre.LongitudMinima - 1);

            foreach (var result in ResultsOf(valorMuyCorto))
            {
                result
                    .OnOk(e => e.Valor.Should().Be($"debió haber fallado para {e}"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>());
            }
        }

        [Fact]
        public void Restringe_a_la_máxima_cantidad_de_caracteres()
        {
            var valor = new string('a', Nombre.LongitudMaxima).Substring(0, Nombre.LongitudMaxima);

            foreach(var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(valor))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}"));
            }

            var valorMuyLargo = valor + "e";

            foreach (var result in ResultsOf(valorMuyLargo))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(null, $"debió haber fallado para {e}"))
                    .OnError(e => e.Should().BeOfType<ArgumentOutOfRangeException>());
            }
        }

        [Fact]
        public void Equality_contract()
        {
            var self = new Nombre("Nombre");
            var same = self;
            var otherEqual = new Nombre("NOMBRE");
            var otherDifferent = new Nombre("NOMbre");
            const string otherType = "erbmon";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }

        [Fact]
        public void ToString_retorna_valor()
        {
            var sut = new Nombre("Nombre");
            sut.ToString().Should().Be(sut.Valor);
        }

        [Fact]
        public void Se_puede_asignar_a_un_string()
        {
            const string expected = "Nombre";
            AsString(new Nombre(expected)).Should().Be(expected);
            AsString(null).Should().Be(string.Empty);
        }

        private static string AsString(Nombre e) => e;
    }
}
