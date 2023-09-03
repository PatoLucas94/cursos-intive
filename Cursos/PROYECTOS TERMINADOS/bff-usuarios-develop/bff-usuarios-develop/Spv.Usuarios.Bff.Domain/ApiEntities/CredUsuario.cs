using System;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    public sealed class CredUsuario : IEquatable<CredUsuario>
    {
        public const int LongitudMinima = 8;
        public const int LongitudMaxima = 15;

        public CredUsuario(string valor) : this(valor, nameof(valor))
        {
        }

        private CredUsuario(string valor, string context)
        {
            Valor = Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context);
        }

        public static Result<CredUsuario> TryParse(string s, string context = null)
        {
            return Result.Of(() => new CredUsuario(s, context ?? nameof(s)));
        }

        public string Valor { get; }

        public static implicit operator string(CredUsuario nc)
        {
            return nc?.Valor ?? string.Empty;
        }

        public bool Equals(CredUsuario other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CredUsuario);
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
