using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.LogEvents
{
    [ExcludeFromCodeCoverage]
    public static class RhSsoServiceEvents
    {
        public const string MessageExceptionAutenticar =
            "Excepción llamando a Spv.Usuarios.Bff.Service.RhSsoService.AutenticarAsync.";
    }
}
