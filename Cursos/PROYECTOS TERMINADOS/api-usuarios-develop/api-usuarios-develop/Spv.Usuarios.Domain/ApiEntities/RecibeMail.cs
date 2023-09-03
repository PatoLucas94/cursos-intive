using System;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class RecibeMail : IEquatable<RecibeMail>
    {
        private const bool SiRecibeMail = true;
        private const bool NoRecibeMail = false;
        public static readonly RecibeMail EsNull = new RecibeMail(null);
        public static readonly RecibeMail Recibe = new RecibeMail(SiRecibeMail);
        public static readonly RecibeMail NoRecibe = new RecibeMail(NoRecibeMail);

        public RecibeMail(bool? valor)
        {
            Valor = valor;
        }

        public bool? Valor { get; }

        public static Result<RecibeMail> TryParse(string s, string context = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return Result.Ok(EsNull);
            }

            if (!bool.TryParse(s, out var valor))
            {
                return Result.Error<RecibeMail>(new ArgumentException($"{context}: '{s}' debe ser un booleano.", context));
            }

            return valor ? Result.Ok(Recibe) : Result.Ok(NoRecibe);
        }

        public bool Equals(RecibeMail other)
        {
            return Valor.Equals(other?.Valor);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is RecibeMail other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }
    }
}
