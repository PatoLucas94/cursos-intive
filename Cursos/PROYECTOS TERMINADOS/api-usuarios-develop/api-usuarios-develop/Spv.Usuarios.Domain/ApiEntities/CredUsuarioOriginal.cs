using System;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class CredUsuarioOriginal : IEquatable<CredUsuarioOriginal>
    {
        public const int LongitudMinima = 8;
        public const int LongitudMaxima = 15;

        public CredUsuarioOriginal(string valor) : this(valor, nameof(valor))
        {
        }

        private CredUsuarioOriginal(string valor, string context)
        {
            Valor = Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context);
        }

        public static Result<CredUsuarioOriginal> TryParse(string s, string context = null)
        {
            return Result.Of(() => new CredUsuarioOriginal(s, context ?? nameof(s)));
        }

        public string Valor { get; }

        public static implicit operator string(CredUsuarioOriginal nc)
        {
            return nc?.Valor ?? string.Empty;
        }

        public bool Equals(CredUsuarioOriginal other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CredUsuarioOriginal);
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
