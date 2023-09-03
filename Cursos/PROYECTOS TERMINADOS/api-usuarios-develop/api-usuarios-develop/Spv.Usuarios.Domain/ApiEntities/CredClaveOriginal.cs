using System;
using System.Linq;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class CredClaveOriginal : IEquatable<CredClaveOriginal>
    {
        public const int LongitudMinima = 8;
        public const int LongitudMaxima = 14;
        public const int CantidadDeNumeros = 2;

        public CredClaveOriginal(string valor) : this(valor, nameof(valor))
        {
        }

        private CredClaveOriginal(string valor, string context)
        {
            Valor = Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context);

            var alMenosXNumeros = false;
            var cantidad = 0;
            // Al menos "CantidadDeNumeros" digitos
            foreach (var c in Valor)
            {
                if (char.IsDigit(c))
                {
                    cantidad += 1;
                }

                if (cantidad < CantidadDeNumeros) continue;

                alMenosXNumeros = true;
                break;
            }

            if (!alMenosXNumeros)
            {
                throw new ArgumentException($"'{context}' debe contener al menos '{CantidadDeNumeros}' dígitos.", context);
            }

            //Al menos una Mayúscula
            var alMenos1Mayuscula = Valor.Any(char.IsUpper);

            if (!alMenos1Mayuscula)
            {
                throw new ArgumentException($"'{context}' debe contener como mínimo 1 letra mayúscula.", context);
            }

            //Caracteres especiales permitidos
            var simbolospermitidos = Valor.All(c => char.IsDigit(c) || char.IsLetter(c) 
                                                                    || c.Equals('.') || c.Equals('-') || c.Equals('_'));

            if (!simbolospermitidos)
            {
                throw new ArgumentException($"'{context}' los únicos caracteres permitidos son: punto (.), guion medio (-), y guion inferior (_).", context);
            }
        }

        public static Result<CredClaveOriginal> TryParse(string s, string context = null)
        {
            return Result.Of(() => new CredClaveOriginal(s, context ?? nameof(s)));
        }

        public string Valor { get; }

        public static implicit operator string(CredClaveOriginal nc)
        {
            return nc?.Valor ?? string.Empty;
        }

        public bool Equals(CredClaveOriginal other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CredClaveOriginal);
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return Valor;
        }
    }
}
