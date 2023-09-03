using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Bff.Common.LogEvents
{
    [ExcludeFromCodeCoverage]
    public static class ClaveServiceEvents
    {
        public static EventId ExceptionCallingValidacionClaveCanales { get; } = new EventId(
            ServiceEventsRanges.ClaveServiceEventsRange,
            "Excepción llamando a Spv.Usuarios.Bff.ClaveService.ValidarClaveCanalesAsync."
        );

        public static EventId ExceptionCallingGeneracionClaveSms { get; } = new EventId(
            ServiceEventsRanges.ClaveServiceEventsRange + 1,
            "Excepción llamando a Spv.Usuarios.Bff.ClaveService.GenerarClaveSmsAsync."
        );

        public static EventId ExceptionCallingValidacionClaveSms { get; } = new EventId(
            ServiceEventsRanges.ClaveServiceEventsRange + 2,
            "Excepción llamando a Spv.Usuarios.Bff.ClaveService.ValidarClaveSmsAsync."
        );

        public static EventId ExceptionCallingObtenerEstado { get; } = new EventId(
            ServiceEventsRanges.ClaveServiceEventsRange + 3,
            "Excepción llamando a Spv.Usuarios.Bff.ClaveService.ObtenerEstadoAsync."
        );
    }
}
