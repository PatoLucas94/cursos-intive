using System;
using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Domain.Utils;
using Spv.Usuarios.Bff.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.ApiEntities
{
    public class TelefonoTest
    {
        [Fact]
        public void Se_puede_crear()
        {
            const string expected = "+12345678901";
            var sut = new Telefono(expected);
            sut.Valor.Should().Be(expected[1..]);
        }

        private static IEnumerable<Result<Telefono>> ResultsOf(string valor)
        {
            return new[] { Result.Of(() => new Telefono(valor)), Telefono.TryParse(valor) };
        }

        [Fact]
        public void Requiere_la_mínima_cantidad_de_caracteres()
        {
            var valor = new string('1', Telefono.LongitudMaxima).Substring(0, Telefono.LongitudMinima);

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(valor))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}"));
            }

            var valorMuyCorto = valor.Substring(0, Telefono.LongitudMinima - 1);

            foreach (var result in ResultsOf(valorMuyCorto))
            {
                result
                    .OnOk(e => e.Valor.Should().Be($"debió haber fallado para {e}"))
                    .OnError(e => e.Should().BeOfType<ArgumentOutOfRangeException>());
            }
        }

        [Fact]
        public void Restringe_a_la_máxima_cantidad_de_caracteres()
        {
            var valor = new string('1', Telefono.LongitudMaxima).Substring(0, Telefono.LongitudMaxima);

            foreach (var result in ResultsOf(valor))
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
                    .OnError(e => e.Should().BeOfType<ArgumentException>());
            }
        }

        [Fact]
        public void Debe_comenzar_con_caracter_valido()
        {
            var valor = new string('-', Telefono.LongitudMaxima).Substring(0, Telefono.LongitudMaxima);

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(null, $"debió haber fallado para {e}"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>());
            }
        }

        [Fact]
        public void Equality_contract()
        {
            var self = new Telefono("+1111111111");
            var same = self;
            var otherEqual = new Telefono("+1111111111");
            var otherDifferent = new Telefono("+1111111111");
            const string otherType = "+1111111111";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }

        [Fact]
        public void ToString_retorna_valor()
        {
            var sut = new Telefono("+1111111111");
            sut.ToString().Should().Be(sut.Valor);
        }

        [Fact]
        public void Se_puede_asignar_a_un_string()
        {
            const string expected = "1111111111";
            AsString(new Telefono(expected)).Should().Be(expected);
            AsString(null).Should().Be(string.Empty);
        }

        private static string AsString(Telefono e) => e;
    }
}
