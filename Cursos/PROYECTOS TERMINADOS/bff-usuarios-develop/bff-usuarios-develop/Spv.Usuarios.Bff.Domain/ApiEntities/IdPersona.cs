using System;
using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    [ExcludeFromCodeCoverage]
    public sealed class IdPersona : IEquatable<IdPersona>
    {
        public const int LongitudMinima = 1;
        public const int LongitudMaxima = 20;

        public IdPersona(string valor) : this(valor, nameof(valor))
        {
        }

        private IdPersona(string valor, string context)
        {
            Valor = Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context);
        }

        public static Result<IdPersona> TryParse(string s, string context = null)
        {
            return Result.Of(() => new IdPersona(s, context ?? nameof(s)));
        }

        public string Valor { get; }

        public static implicit operator string(IdPersona nc)
        {
            return nc?.Valor ?? string.Empty;
        }

        public bool Equals(IdPersona other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IdPersona);
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
