using System;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class ControlFull : IEquatable<ControlFull>
    {
        private const bool EsControlFull = true;
        private const bool NoEsControlFull = false;
        public static readonly ControlFull EsNull = new ControlFull(null);
        public static readonly ControlFull EsTrue = new ControlFull(EsControlFull);
        public static readonly ControlFull EsFalse = new ControlFull(NoEsControlFull);

        public ControlFull(bool? valor)
        {
            Valor = valor;
        }

        public bool? Valor { get; }

        public static Result<ControlFull> TryParse(string s, string context = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return Result.Ok(EsNull);
            }

            if (!bool.TryParse(s, out var valor))
            {
                return Result.Error<ControlFull>(new ArgumentException($"{context}: '{s}' debe ser un booleano.", context));
            }

            return valor ? Result.Ok(EsTrue) : Result.Ok(EsFalse);
        }

        public bool Equals(ControlFull other)
        {
            return Valor.Equals(other?.Valor);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is ControlFull other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }
    }
}
