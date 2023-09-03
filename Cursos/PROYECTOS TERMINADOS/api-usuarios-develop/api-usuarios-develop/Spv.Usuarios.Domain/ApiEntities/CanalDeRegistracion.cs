using System;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class CanalDeRegistracion : IEquatable<CanalDeRegistracion>
    {
        public const int LongitudMinima = 2;
        public const int LongitudMaxima = 50;

        public CanalDeRegistracion(string valor) : this(valor, nameof(valor))
        {
        }

        private CanalDeRegistracion(string valor, string context)
        {
            Valor = Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context);
        }

        public static Result<CanalDeRegistracion> TryParse(string s, string context = null)
        {
            return Result.Of(() => new CanalDeRegistracion(s, context ?? nameof(s)));
        }

        public string Valor { get; }

        public static implicit operator string(CanalDeRegistracion nc)
        {
            return nc?.Valor ?? string.Empty;
        }

        public bool Equals(CanalDeRegistracion other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CanalDeRegistracion);
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
