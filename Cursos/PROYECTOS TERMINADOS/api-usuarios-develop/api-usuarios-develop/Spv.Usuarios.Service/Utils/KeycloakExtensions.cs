using System;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Service.Utils
{
    public static class KeycloakExtensions
    {
        public static bool RequestFromKeycloak<T>(this IRequestBody<T> request) =>
            !string.IsNullOrWhiteSpace(request.XAplicacion) &&
            request.XAplicacion.Equals("keycloak", StringComparison.OrdinalIgnoreCase);
    }
}
