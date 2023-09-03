using System;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    public sealed class Guid : IEquatable<Guid>
    {
        public const int LongitudMaxima = 36;
        public const int LongitudMinima = 36;

        public Guid(string valor) : this(valor, nameof(valor))
        {
        }

        private Guid(string valor, string context)
        {
            Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context);

            if (!System.Guid.TryParse(valor, out var result) || result == System.Guid.Empty)
            {
                throw new ArgumentException($"'{context}' es inválido o vacío.");
            }

            Valor = valor;
        }

        public string Valor { get; }

        public static Result<Guid> TryParse(string s, string context = null)
        {
            return Result.Of(() => new Guid(s, context ?? nameof(s)));
        }

        public static implicit operator string(Guid t)
        {
            return t?.Valor ?? "0";
        }

        public bool Equals(Guid other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Guid);
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
