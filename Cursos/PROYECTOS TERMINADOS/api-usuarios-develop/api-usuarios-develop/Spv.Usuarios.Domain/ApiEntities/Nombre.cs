using System;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class Nombre : IEquatable<Nombre>
    {
        public const int LongitudMinima = 1;
        public const int LongitudMaxima = 50;

        public Nombre(string valor) : this(valor, nameof(valor))
        {
        }

        private Nombre(string valor, string context)
        {
            Valor = Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context);
        }

        public static Result<Nombre> TryParse(string s, string context = null)
        {
            return Result.Of(() => new Nombre(s, context ?? nameof(s)));
        }

        public string Valor { get; }

        public static implicit operator string(Nombre nc)
        {
            return nc?.Valor ?? string.Empty;
        }

        public bool Equals(Nombre other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Nombre);
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
