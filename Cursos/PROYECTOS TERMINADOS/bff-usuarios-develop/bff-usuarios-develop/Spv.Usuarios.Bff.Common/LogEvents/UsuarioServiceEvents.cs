using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Bff.Common.LogEvents
{
    [ExcludeFromCodeCoverage]
    public static class UsuarioServiceEvents
    {
        public static EventId ExceptionCallingRegistro { get; }
            = new EventId(ServiceEventsRanges.UsuarioServiceEventsRange,
                "Excepción llamando a Spv.Usuarios.Bff.Service.RegistrarV2Async.");

        public static EventId ExceptionCallingModificarCredenciales { get; }
            = new EventId(ServiceEventsRanges.UsuarioServiceEventsRange + 1,
                "Excepción llamando a Spv.Usuarios.Bff.Service.ModificarCredencialesAsync.");

        public static EventId ExceptionCallingMigrar { get; }
            = new EventId(ServiceEventsRanges.UsuarioServiceEventsRange + 2,
                "Excepción llamando a Spv.Usuarios.Bff.Service.MigrarAsync.");

        public static EventId ExceptionCallingModificarClave { get; }
            = new EventId(ServiceEventsRanges.UsuarioServiceEventsRange + 3,
                "Excepción llamando a Spv.Usuarios.Bff.Service.ModificarClaveAsync.");

        public static EventId ExceptionCallingRecuperarUsuario { get; }
            = new EventId(ServiceEventsRanges.UsuarioServiceEventsRange + 4,
                "Excepción llamando a Spv.Usuarios.Bff.Service.RecuperarUsuarioAsync.");

        public static EventId ExceptionCallingPerfil { get; }
            = new EventId(ServiceEventsRanges.UsuarioServiceEventsRange,
                "Excepción llamando a Spv.Usuarios.Bff.Service.ObtenerPerfilAsync.");

        public static EventId ExceptionCallingObtenerImagenesLoginAsync { get; }
            = new EventId(ServiceEventsRanges.UsuarioServiceEventsRange,
                "Excepción llamando a Spv.Usuarios.Bff.Service.ObtenerImagenesLoginAsync");
    }
}
