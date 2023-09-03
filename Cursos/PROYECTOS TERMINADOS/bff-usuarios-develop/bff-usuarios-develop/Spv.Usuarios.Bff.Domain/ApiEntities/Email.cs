using System;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    public sealed class Email : IEquatable<Email>
    {
        public const int LongitudMinima = 5;
        public const int LongitudMaxima = 100;

        public Email(string valor) : this(valor, nameof(valor))
        {
        }

        public string Valor { get; }

        private Email(string valor, string context)
        {
            if (!string.IsNullOrWhiteSpace(valor))
            {
                Valor = new System.Net.Mail.MailAddress(Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context)).Address;
            }
        }

        public static Result<Email> TryParse(string s, string context = null)
        {
            return Result.Of(() => new Email(s, context ?? nameof(s)));
        }

        public static implicit operator string(Email e)
        {
            return e?.Valor ?? string.Empty;
        }

        public bool Equals(Email other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Email);
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
