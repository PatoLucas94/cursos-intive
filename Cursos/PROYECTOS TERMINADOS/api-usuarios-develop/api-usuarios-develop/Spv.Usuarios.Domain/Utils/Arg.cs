using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Domain.Utils
{
    [ExcludeFromCodeCoverage]
    public static class Arg
    {
        [DebuggerStepThrough]
        public static T NonNull<T>(T value, string parameterName) where T : class
        {
            return value ?? throw new ArgumentNullException(parameterName, $"'{parameterName}' no puede ser null.");
        }

        [DebuggerStepThrough]
        public static T NonNull<T>(T? value, string parameterName) where T : struct
        {
            return value ?? throw new ArgumentNullException(parameterName, $"'{parameterName}' no puede ser null.");
        }

        [DebuggerStepThrough]
        public static string NonNullNorEmpty(string value, string parameterName)
        {
            var s = NonNull(value, parameterName);
            return s.Length > 0
                ? s
                : throw new ArgumentException($"'{parameterName}' no puede ser vacío.", parameterName);
        }

        [DebuggerStepThrough]
        public static string NonNullNorSpaces(string value, string parameterName)
        {
            var s = NonNullNorEmpty(value, parameterName);
            return !string.IsNullOrWhiteSpace(s)
                ? s
                : throw new ArgumentException($"'{parameterName}' no puede contener solo espacios.", parameterName);
        }

        [DebuggerStepThrough]
        public static int InRange(int value, int lower, int upper, string parameterName)
        {
            return value >= lower && value <= upper
                ? value
                : throw new ArgumentOutOfRangeException(parameterName, value,
                    $"'{parameterName}' debe estar entre {lower} y {upper}");
        }

        [DebuggerStepThrough]
        public static long InRange(long value, long lower, long upper, string parameterName)
        {
            return value >= lower && value <= upper
                ? value
                : throw new ArgumentOutOfRangeException(parameterName, value,
                    $"'{parameterName}' debe estar entre {lower} y {upper}");
        }

        [DebuggerStepThrough]
        public static string LengthInRange(string value, int lower, int upper, string parameterName)
        {
            var s = NonNullNorSpaces(value, parameterName);
            return s.Length >= lower && s.Length <= upper
                ? value
                : throw new ArgumentOutOfRangeException(parameterName, value,
                    $"La longitud de '{parameterName}' debe estar entre {lower} y {upper}");
        }

        [DebuggerStepThrough]
        public static string InLength(string value, int length, string parameterName)
        {
            var s = NonNullNorSpaces(value, parameterName);
            return s.Length == length
                ? value
                : throw new ArgumentOutOfRangeException(parameterName, value,
                    $"La longitud de '{parameterName}' debe ser de {length}");
        }
    }
}
