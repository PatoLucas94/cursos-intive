using System;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class Apellido : IEquatable<Apellido>
    {
        public const int LongitudMinima = 1;
        public const int LongitudMaxima = 50;

        public Apellido(string valor) : this(valor, nameof(valor))
        {
        }

        private Apellido(string valor, string context)
        {
            Valor = Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context);
        }

        public static Result<Apellido> TryParse(string s, string context = null)
        {
            return Result.Of(() => new Apellido(s, context ?? nameof(s)));
        }

        public string Valor { get; }

        public static implicit operator string(Apellido nc)
        {
            return nc?.Valor ?? string.Empty;
        }

        public bool Equals(Apellido other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Apellido);
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
