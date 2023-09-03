using System;
using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    [ExcludeFromCodeCoverage]
    public sealed class IdPersonaOriginal : IEquatable<IdPersonaOriginal>
    {
        public const int LongitudMinima = 1;
        public const int LongitudMaxima = 20;

        public IdPersonaOriginal(string valor) : this(valor, nameof(valor))
        {
        }

        private IdPersonaOriginal(string valor, string context)
        {
            Valor = !string.IsNullOrWhiteSpace(valor)
                ? Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context)
                : null;
        }

        public static Result<IdPersonaOriginal> TryParse(string s, string context = null)
        {
            return Result.Of(() => new IdPersonaOriginal(s, context ?? nameof(s)));
        }

        public string Valor { get; }

        public static implicit operator string(IdPersonaOriginal nc)
        {
            return nc?.Valor ?? string.Empty;
        }

        public bool Equals(IdPersonaOriginal other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IdPersonaOriginal);
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
