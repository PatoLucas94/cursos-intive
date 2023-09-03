using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    [ExcludeFromCodeCoverage]
    public abstract class Id<T> : IComparable<Id<T>>
    {
        protected Id(long valor)
        {
            Valor = valor;
        }

        public long Valor { get; }

        protected static Result<long> TryParseValue(string s, long minValue, long maxValue, string context = null)
        {
            context ??= nameof(s);
            if (s is null)
            {
                return Result.Error<long>(new ArgumentNullException(context, $"'{context}' no puede ser null."));
            }

            if (string.IsNullOrWhiteSpace(s))
            {
                return Result.Error<long>(new ArgumentException($"'{context}' no puede ser espacios o vacío.",
                    context));
            }

            if (!decimal.TryParse(s, out var valor))
            {
                return Result.Error<long>(new ArgumentException($"'{context}' debe ser un entero.", context));
            }

            if (valor < minValue || valor > maxValue)
            {
                return Result.Error<long>(new ArgumentOutOfRangeException(context, valor,
                    $"'{context}' debe estar entre {minValue} y {maxValue}."));
            }

            return Result.Ok((long)valor);
        }

        public int CompareTo(Id<T> other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            return other is null ? 1 : Valor.CompareTo(other.Valor);
        }

        public sealed override bool Equals(object obj)
        {
            return Equals(obj as Id<T>);
        }

        public bool Equals(Id<T> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other is null)
            {
                return false;
            }

            return Valor == other.Valor;
        }

        public sealed override int GetHashCode() => (int)Valor;

        public static bool operator ==(Id<T> a, Id<T> b) => a?.Equals(b) ?? b is null;

        public static bool operator !=(Id<T> a, Id<T> b) => !(a == b);

        public static bool operator <(Id<T> a, Id<T> b)
        {
            if (!(a is null || b is null))
            {
                return a.Valor < b.Valor;
            }

            return a is null;
        }

        public static bool operator <=(Id<T> a, Id<T> b) => !(a > b);

        public static bool operator >(Id<T> a, Id<T> b)
        {
            if (a is null)
            {
                return false;
            }

            return b is null || a.Valor > b.Valor;
        }

        public static bool operator >=(Id<T> a, Id<T> b) => !(a < b);

        public override string ToString() => Valor.ToString(CultureInfo.InvariantCulture);
    }
}
