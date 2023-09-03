using System;
using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    [ExcludeFromCodeCoverage]
    public sealed class ClaveSms : IEquatable<ClaveSms>
    {
        public const int Longitud = 6;

        public ClaveSms(string valor) : this(valor, nameof(valor))
        {
        }

        private ClaveSms(string valor, string context)
        {
            Arg.InRange(valor.Length, Longitud, Longitud, context);
            Valor = valor;
        }

        public string Valor { get; }

        public static Result<ClaveSms> TryParse(string s, string context = null)
        {
            return Result.Of(() => new ClaveSms(s, context ?? nameof(s)));
        }

        public static implicit operator string(ClaveSms t)
        {
            return t?.Valor ?? "0";
        }

        public bool Equals(ClaveSms other)
        {
            return ReferenceEquals(this, other) || Valor.Equals(other?.Valor);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ClaveSms);
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
