using System;
using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Domain.Utils;
using Spv.Usuarios.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Test.Unit.ApiEntities
{
    public class CredUsuarioOriginalTest
    {
        [Fact]
        public void Se_puede_crear()
        {
            const string expected = "usu4r10s";
            var sut = new CredUsuarioOriginal(expected);
            sut.Valor.Should().Be(expected);
        }

        private static IEnumerable<Result<CredUsuarioOriginal>> ResultsOf(string valor)
        {
            return new[] { Result.Of(() => new CredUsuarioOriginal(valor)), CredUsuarioOriginal.TryParse(valor) };
        }

        [Fact]
        public void Requiere_la_mínima_cantidad_de_caracteres()
        {
            var valor = new string('u', CredUsuarioOriginal.LongitudMaxima).Substring(0, CredUsuarioOriginal.LongitudMinima);

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(valor))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}"));
            }

            var valorMuyCorto = valor.Substring(0, CredUsuarioOriginal.LongitudMinima - 1);

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
            var valor = new string('u', CredUsuarioOriginal.LongitudMaxima).Substring(0, CredUsuarioOriginal.LongitudMaxima);

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
            var self = new CredUsuarioOriginal("usu4r10s");
            var same = self;
            var otherEqual = new CredUsuarioOriginal("USU4R10S");
            var otherDifferent = new CredUsuarioOriginal("USU4r10s");
            const string otherType = "s01r4usu";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }

        [Fact]
        public void ToString_retorna_valor()
        {
            var sut = new CredUsuarioOriginal("usu4r10s");
            sut.ToString().Should().Be(sut.Valor);
        }

        [Fact]
        public void Se_puede_asignar_a_un_string()
        {
            const string expected = "usu4r10s";
            AsString(new CredUsuarioOriginal(expected)).Should().Be(expected);
            AsString(null).Should().Be(string.Empty);
        }

        private static string AsString(CredUsuarioOriginal e) => e;
    }
}
