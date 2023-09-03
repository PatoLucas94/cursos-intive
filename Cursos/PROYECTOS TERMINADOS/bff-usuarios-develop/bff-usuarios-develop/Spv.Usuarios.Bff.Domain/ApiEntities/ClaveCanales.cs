using System;
using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    [ExcludeFromCodeCoverage]
    public sealed class ClaveCanales : IEquatable<ClaveCanales>
    {
        private const int Longitud = 8;

        private ClaveCanales(string valor, string context)
        {
            Arg.InRange(valor.Length, Longitud, Longitud, context);
            Valor = valor;
        }

        public string Valor { get; }

        public static Result<ClaveCanales> TryParse(string s, string context = null)
        {
            return Result.Of(() => new ClaveCanales(s, context ?? nameof(s)));
        }

        public static implicit operator string(ClaveCanales t)
        {
            return t?.Valor ?? "0";
        }

        public bool Equals(ClaveCanales other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ClaveCanales);
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
