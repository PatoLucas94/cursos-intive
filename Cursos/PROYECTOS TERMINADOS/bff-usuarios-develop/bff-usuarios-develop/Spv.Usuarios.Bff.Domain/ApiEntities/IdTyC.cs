using System;
using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    [ExcludeFromCodeCoverage]
    public sealed class IdTyC : IEquatable<IdTyC>
    {
        public const int LongitudMaxima = 36;
        public const int LongitudMinima = 36;

        public IdTyC(string valor) : this(valor, nameof(valor))
        {
        }

        private IdTyC(string valor, string context)
        {
            if (!string.IsNullOrWhiteSpace(valor))
            {
                Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context);

                if (!System.Guid.TryParse(valor, out var result) || result == System.Guid.Empty)
                {
                    throw new ArgumentException($"'{context}' es inválido.");
                }
            }

            Valor = valor;
        }

        public string Valor { get; }

        public static Result<IdTyC> TryParse(string s, string context = null)
        {
            return Result.Of(() => new IdTyC(s, context ?? nameof(s)));
        }

        public static implicit operator string(IdTyC t)
        {
            return t?.Valor ?? "0";
        }

        public bool Equals(IdTyC other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IdTyC);
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }

        public override string ToString()
        {
            return Valor;
        }
    }
}
