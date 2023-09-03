using System;
using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Domain.Utils;
using Spv.Usuarios.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Test.Unit.ApiEntities
{
    public class CredClaveOriginalTest
    {
        [Fact]
        public void Se_puede_crear()
        {
            const string expected = "Info1212";
            var sut = new CredClaveOriginal(expected);
            sut.Valor.Should().Be(expected);
        }

        private static IEnumerable<Result<CredClaveOriginal>> ResultsOf(string valor)
        {
            return new[] { Result.Of(() => new CredClaveOriginal(valor)), CredClaveOriginal.TryParse(valor) };
        }

        [Fact]
        public void Requiere_la_minima_cantidad_de_caracteres()
        {
            const string valor = "Info1212";

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(valor))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}"));
            }

            var valorMuyCorto = valor.Substring(0, CredClaveOriginal.LongitudMinima - 1);

            foreach (var result in ResultsOf(valorMuyCorto))
            {
                result
                    .OnOk(e => e.Valor.Should().Be($"debió haber fallado para {e}"))
                    .OnError(e => e.Should().BeOfType<ArgumentOutOfRangeException>());
            }
        }

        [Fact]
        public void Restringe_a_la_maxima_cantidad_de_caracteres()
        {
            const string valor = "Info1212Info12";

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(valor))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}"));
            }

            const string valorMuyLargo = valor + "e";

            foreach (var result in ResultsOf(valorMuyLargo))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(null, $"debió haber fallado para {e}"))
                    .OnError(e => e.Should().BeOfType<ArgumentOutOfRangeException>());
            }
        }

        [Fact]
        public void Requiere_al_menos_2_digitos()
        {
            const string valor = "InfoInfo";

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(null, $"debió haber fallado para {e}"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>());
            }
        }

        [Fact]
        public void Restringe_caracteres_especiales_ok()
        {
            const string valor = "Info12_-.";

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(valor))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}"));
            }
        }

        [Fact]
        public void Restringe_caracteres_especiales_error()
        {
            const string valor = "Info1212*";

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(null, $"debió haber fallado para {e}"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>());
            }
        }

        [Fact]
        public void Requiere_al_menos_1_mayuscula()
        {
            const string valor = "info1212";

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
            var self = new CredClaveOriginal("Info1212");
            var same = self;
            var otherEqual = new CredClaveOriginal("INFO1212");
            var otherDifferent = new CredClaveOriginal("INfo1212");
            const string otherType = "2121ofni";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }

        [Fact]
        public void ToString_retorna_valor()
        {
            var sut = new CredClaveOriginal("Info1212");
            sut.ToString().Should().Be(sut.Valor);
        }

        [Fact]
        public void Se_puede_asignar_a_un_string()
        {
            const string expected = "Info1212";
            AsString(new CredClaveOriginal(expected)).Should().Be(expected);
            AsString(null).Should().Be(string.Empty);
        }

        private static string AsString(CredClaveOriginal e) => e;
    }
}
