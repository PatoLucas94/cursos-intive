using System;
using System.Text;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    public sealed class Telefono : IEquatable<Telefono>
    {
        public const int LongitudMinima = 9;
        public const int LongitudMaxima = 15;

        public Telefono(string valor) : this(valor, nameof(valor))
        {
        }

        private Telefono(string valor, string context)
        {
            if (string.IsNullOrWhiteSpace(valor)) return;

            var s = Arg.NonNullNorEmpty(valor, context).Trim();
            var c = s[0];
            var isDigit = char.IsDigit(c);
            if (c != '+' && c != '(' && !isDigit)
            {
                throw new ArgumentException($"'{context}' debe comenzar con '+', '(' o dígito, pero comienza con '{c}'.", context);
            }
            var result = new StringBuilder(s.Length);
            if (isDigit)
            {
                result.Append(c);
            }

            for (var i = 1; i < s.Length; i++)
            {
                c = s[i];
                isDigit = char.IsDigit(c);
                if (c != ' ' && c != '-' && c != '(' && c != ')' && !isDigit)
                {
                    throw new ArgumentException($"'{context}' debe contener '(', ')', '-' o dígitos, pero en la posición '{i}' contiene '{c}'.", nameof(valor));
                }
                if (isDigit)
                {
                    result.Append(c);
                }
            }

            Arg.InRange(result.Length, LongitudMinima, LongitudMaxima, context);
            Valor = result.ToString();
        }

        public string Valor { get; }

        public static Result<Telefono> TryParse(string s, string context = null)
        {
            return Result.Of((() => new Telefono(s, context ?? nameof(s))));
        }

        public static implicit operator string(Telefono t)
        {
            return t?.Valor ?? string.Empty;
        }

        public bool Equals(Telefono other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Telefono);
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode(StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return Valor;
        }
    }
}
