using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Spv.Usuarios.Service.Utils
{
    [ExcludeFromCodeCoverage]
    internal static class DistributedCache
    {
        public static class Sso
        {
            private const string Key = nameof(SsoService);

            public static string Autenticacion(string segment) => $"{Key}_{nameof(Autenticacion)}_{segment}";
        }

        public static class Usuario
        {
            private const string Key = nameof(UsuariosService);

            public static string Error(string segment) => $"{Key}_{nameof(Error)}_{segment}";

            public static string ObtenerUsuario(string segment) => $"{Key}_{nameof(ObtenerUsuario)}_{segment}";

            public static string ObtenerInfoPersonaFisica(string segment) =>
                $"{Key}_{nameof(ObtenerInfoPersonaFisica)}_{segment}";
        }

        public static class Bta
        {
            private const string Key = "BTA";

            public static string Error(string segment) => $"{Key}_{nameof(Error)}_{segment}";

            public static string ObtenerTokenBta(string segment) =>
                $"{Key}_{nameof(ObtenerTokenBta)}_{segment}";
        }

        public static byte[] Serialize(string value) => Encoding.UTF8.GetBytes(value);

        public static byte[] Serialize<T>(T value) where T : class => JsonSerializer.SerializeToUtf8Bytes(value);

        public static T Deserialize<T>(byte[] value) where T : class
        {
            if (value == null)
                return default;

            var jsonToDeserialize = Deserialize(value);

            return string.IsNullOrWhiteSpace(jsonToDeserialize)
                ? default
                : JsonSerializer.Deserialize<T>(jsonToDeserialize);
        }

        public static string Deserialize(byte[] value) => Encoding.UTF8.GetString(value);

        public static DistributedCacheEntryOptions SlidingExpirationMinutes(int minutes) =>
            new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(minutes));

        public static DistributedCacheEntryOptions SlidingExpirationDays(int days) =>
            new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(days));

        public static DistributedCacheEntryOptions SlidingExpirationHours(int hours) =>
            new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(hours));
    }
}
