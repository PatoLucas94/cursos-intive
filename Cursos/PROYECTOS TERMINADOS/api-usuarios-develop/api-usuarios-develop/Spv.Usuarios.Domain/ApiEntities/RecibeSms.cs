using System;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class RecibeSms : IEquatable<RecibeSms>
    {
        private const bool SiRecibeSms = true;
        private const bool NoRecibeSms = false;
        public static readonly RecibeSms EsNull = new RecibeSms(null);
        public static readonly RecibeSms Recibe = new RecibeSms(SiRecibeSms);
        public static readonly RecibeSms NoRecibe = new RecibeSms(NoRecibeSms);

        public RecibeSms(bool? valor)
        {
            Valor = valor;
        }

        public bool? Valor { get; }

        public static Result<RecibeSms> TryParse(string s, string context = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return Result.Ok(EsNull);
            }

            if (!bool.TryParse(s, out var valor))
            {
                return Result.Error<RecibeSms>(new ArgumentException($"{context}: '{s}' debe ser un booleano.", context));
            }

            return valor ? Result.Ok(Recibe) : Result.Ok(NoRecibe);
        }

        public bool Equals(RecibeSms other)
        {
            return Valor.Equals(other?.Valor);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is RecibeSms other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }
    }
}
