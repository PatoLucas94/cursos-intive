using System;
using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Domain.Utils;
using Spv.Usuarios.Test.Unit.Util;
using Xunit;

namespace Spv.Usuarios.Test.Unit.ApiEntities
{
    public class CredClaveTest
    {
        [Fact]
        public void Se_puede_crear()
        {
            const string expected = "4132";
            var sut = new CredClave(expected);
            sut.Valor.Should().Be(expected);
        }

        private static IEnumerable<Result<CredClave>> ResultsOf(string valor)
        {
            return new[] { Result.Of(() => new CredClave(valor)), CredClave.TryParse(valor) };
        }

        [Fact]
        public void Requiere_la_minima_cantidad_de_caracteres()
        {
            const string valor = "4132";

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(valor))
                    .OnError(e => e.Message.Should().Be($"debió haber funcionado pero falló con {e}"));
            }

            var valorMuyCorto = valor.Substring(0, CredClave.Longitud - 1);

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
            const string valor = "4132";

            foreach(var result in ResultsOf(valor))
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
        public void Restringe_numeros_consecutivos_ascendentes()
        {
            const string valor = "1234";

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(null, $"debió haber fallado para {e}"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>());
            }
        }

        [Fact]
        public void Restringe_numeros_consecutivos_descendentes()
        {
            const string valor = "4321";

            foreach (var result in ResultsOf(valor))
            {
                result
                    .OnOk(e => e.Valor.Should().Be(null, $"debió haber fallado para {e}"))
                    .OnError(e => e.Should().BeOfType<ArgumentException>());
            }
        }

        [Fact]
        public void Restringe_numeros_iguales()
        {
            const string valor = "1111";

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
            var self = new CredClave("4132");
            var same = self;
            var otherEqual = new CredClave("4132");
            var otherDifferent = new CredClave("5142");
            const string otherType = "1234";

            Assertions.Equality(self, same, otherEqual, otherDifferent, null, otherType);
        }

        [Fact]
        public void ToString_retorna_valor()
        {
            var sut = new CredClave("4132");
            sut.ToString().Should().Be(sut.Valor);
        }

        [Fact]
        public void Se_puede_asignar_a_un_string()
        {
            const string expected = "4132";
            AsString(new CredClave(expected)).Should().Be(expected);
            AsString(null).Should().Be(string.Empty);
        }

        private static string AsString(CredClave e) => e;
    }
}
