using System;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    public sealed class SmsValidado : IEquatable<SmsValidado>
    {
        private const bool SiSmsValidado = true;
        private const bool NoSmsValidado = false;
        public static readonly SmsValidado EsNull = new SmsValidado(null);
        public static readonly SmsValidado Si = new SmsValidado(SiSmsValidado);
        public static readonly SmsValidado No = new SmsValidado(NoSmsValidado);

        public SmsValidado(bool? valor)
        {
            Valor = valor;
        }

        public bool? Valor { get; }

        public static Result<SmsValidado> TryParse(string s, string context = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return Result.Ok(EsNull);
            }

            if (!bool.TryParse(s, out var valor))
            {
                return Result.Error<SmsValidado>(new ArgumentException($"{context}: '{s}' debe ser un booleano.", context));
            }

            return valor ? Result.Ok(Si) : Result.Ok(No);
        }

        public bool Equals(SmsValidado other)
        {
            return Valor.Equals(other?.Valor);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is SmsValidado other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }
    }
}
