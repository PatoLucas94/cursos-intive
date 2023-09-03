using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.LogEvents
{
    [ExcludeFromCodeCoverage]
    public static class BiometriaServiceEvents
    {
        public const string MessageExceptionAutenticar =
            "Excepción llamando a Spv.Usuarios.Bff.Service.BiometriaService.AutenticarAsync.";
    }
}
