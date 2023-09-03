using System;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class NroCliente : IEquatable<NroCliente>
    {
        public const int LongitudMinima = 1;
        public const int LongitudMaxima = 50;

        public NroCliente(string valor) : this(valor, nameof(valor))
        {
        }

        private NroCliente(string valor, string context)
        {
            Valor = Arg.LengthInRange(valor, LongitudMinima, LongitudMaxima, context);
        }

        public static Result<NroCliente> TryParse(string s, string context = null)
        {
            return Result.Of(() => new NroCliente(s, context ?? nameof(s)));
        }

        public string Valor { get; }

        public static implicit operator string(NroCliente nc)
        {
            return nc?.Valor ?? string.Empty;
        }

        public bool Equals(NroCliente other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NroCliente);
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
