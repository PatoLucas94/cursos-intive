using System;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class CredClave : IEquatable<CredClave>
    {
        public const int Longitud = 4;

        public CredClave(string valor) : this(valor, nameof(valor))
        {
        }

        private CredClave(string valor, string context)
        {
            // Longitud
            Valor = Arg.InLength(valor, Longitud, context);

            // Solo dígitos
            for (var i = 0; i < Valor.Length; i++)
            {
                var c = Valor[i];

                if (!char.IsDigit(c))
                {
                    throw new ArgumentException($"'{context}' debe contener solo dígitos, pero en la posición '{i}' contiene '{c}'.", context);
                }
            }

            // Números consecutivos ascendentes y descendentes e iguales
            var nrosConsecAscen = true;
            var nrosConsecDesce = true;
            var numerosIguales = true;
            for (var i = 1; i <= Valor.Length - 1; i++)
            {
                // Si al menos un dígito no es consecutivo, ya no lo serán los demás y esta condición será false
                if (nrosConsecAscen) { nrosConsecAscen = Valor[i - 1] == (Valor[i] - 1); }
                if (nrosConsecDesce) { nrosConsecDesce = Valor[i - 1] == (Valor[i] + 1); }

                // Si al menos un dígito no es igual, ya no lo serán los demás y esta condición será false
                if (numerosIguales) { numerosIguales = Valor[i - 1] == Valor[i]; }
            }

            if (nrosConsecAscen)
            {
                throw new ArgumentException($"'{context}' no puede contener dígitos consecutivos ascendentes.", context);
            }

            if (nrosConsecDesce)
            {
                throw new ArgumentException($"'{context}' no puede contener dígitos consecutivos descendentes.", context);
            }

            if (numerosIguales)
            {
                throw new ArgumentException($"'{context}' no puede contener dígitos iguales.", context);
            }
        }

        public static Result<CredClave> TryParse(string s, string context = null)
        {
            return Result.Of(() => new CredClave(s, context ?? nameof(s)));
        }

        public string Valor { get; }

        public static implicit operator string(CredClave nc)
        {
            return nc?.Valor ?? string.Empty;
        }

        public bool Equals(CredClave other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CredClave);
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
