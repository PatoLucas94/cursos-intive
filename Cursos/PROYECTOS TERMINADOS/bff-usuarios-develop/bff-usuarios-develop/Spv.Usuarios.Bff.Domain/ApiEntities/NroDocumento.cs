using System;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    public sealed class NroDocumento : IEquatable<NroDocumento>
    {
        public const int LongitudMinima = 1;
        public const int LongitudMaxima = 20;

        public NroDocumento(string valor) : this(valor, nameof(valor))
        {
        }

        private NroDocumento(string valor, string context)
        {
            Valor = Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context);
        }

        public static Result<NroDocumento> TryParse(string s, string context = null)
        {
            return Result.Of(() => new NroDocumento(s, context ?? nameof(s)));
        }

        public string Valor { get; }

        public static implicit operator string(NroDocumento nc)
        {
            return nc?.Valor ?? string.Empty;
        }

        public bool Equals(NroDocumento other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NroDocumento);
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
