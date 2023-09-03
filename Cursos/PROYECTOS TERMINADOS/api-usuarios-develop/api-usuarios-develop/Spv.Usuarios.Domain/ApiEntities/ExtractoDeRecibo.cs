using System;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class ExtractoDeRecibo : IEquatable<ExtractoDeRecibo>
    {
        private const bool EsExtractoDeRecibo = true;
        private const bool NoEsExtractoDeRecibo = false;
        public static readonly ExtractoDeRecibo EsNull = new ExtractoDeRecibo(null);
        public static readonly ExtractoDeRecibo EsTrue = new ExtractoDeRecibo(EsExtractoDeRecibo);
        public static readonly ExtractoDeRecibo EsFalse = new ExtractoDeRecibo(NoEsExtractoDeRecibo);

        public ExtractoDeRecibo(bool? valor)
        {
            Valor = valor;
        }

        public bool? Valor { get; }

        public static Result<ExtractoDeRecibo> TryParse(string s, string context = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return Result.Ok(EsNull);
            }

            if (!bool.TryParse(s, out var valor))
            {
                return Result.Error<ExtractoDeRecibo>(new ArgumentException($"{context}: '{s}' debe ser un booleano.", context));
            }

            return valor ? Result.Ok(EsTrue) : Result.Ok(EsFalse);
        }

        public bool Equals(ExtractoDeRecibo other)
        {
            return Valor.Equals(other?.Valor);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is ExtractoDeRecibo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }
    }
}
