using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.LogEvents
{
    [ExcludeFromCodeCoverage]
    public static class TyCServiceEvents
    {
        public const string MessageAceptadosByPersonId =
            "Excepción llamando a Spv.Usuarios.Bff.Service.TyCService.ObtenerAceptadosByPersonIdAsync.";

        public const string MessageObtenerVigente =
            "Excepción llamando a Spv.Usuarios.Bff.Service.TyCService.ObtenerVigenteAsync.";
    }
}
