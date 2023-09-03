using System;
using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Domain.Utils;
using Spv.Usuarios.Bff.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.ApiEntities
{
    public class EmailTest
    {
        [Fact]
        public void Se_puede_crear()
        {
            const string expected = "a@a.co";
            var sut = new Email(expected);
            sut.Valor.Should().Be(expected);
        }

        private static IEnumerable<Result<Email>> ResultsOf(string valor)
        {
            return new[] { Result.Of(() => new Email(valor)), Email.TryParse(valor) };
        }

        [Fact]
        public void Requiere_la_mínima_cantidad_de_caracteres()
        {
            var charRepeater = new string('m', Email.LongitudMaxima);
            var valor = string.Concat("a@a.co", charRepeater).Substring(0, Email.LongitudMinima);

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(valor))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}"));
            }

            var valorMuyCorto = valor.Substring(0, Email.LongitudMinima - 1);

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
            var charRepeater = new string('m', Email.LongitudMaxima);
            var valor = string.Concat("a@a.co", charRepeater).Substring(0, Email.LongitudMaxima);

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
                    .OnError(e => e.Should().BeOfType<ArgumentOutOfRangeException>());
            }
        }

        [Fact]
        public void Equality_contract()
        {
            var self = new Email("a@a.co");
            var same = self;
            var otherEqual = new Email("A@a.co");
            var otherDifferent = new Email("A@A.co");
            const string otherType = "a@a.com";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }

        [Fact]
        public void ToString_retorna_valor()
        {
            var sut = new Email("a@a.co");
            sut.ToString().Should().Be(sut.Valor);
        }

        [Fact]
        public void Se_puede_asignar_a_un_string()
        {
            const string expected = "a@a.co";
            AsString(new Email(expected)).Should().Be(expected);
            AsString(null).Should().Be(string.Empty);
        }

        private static string AsString(Email e) => e;
    }
}
