using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Bff.Common.LogEvents
{
    [ExcludeFromCodeCoverage]
    public static class SoftTokenServiceEvents
    {
        public static EventId ExceptionCallingSoftTokenHabilitado { get; }
            = new EventId(ServiceEventsRanges.SoftTokenServiceEventsRange,
                "Excepción llamando a Spv.Usuarios.Bff.Service.SoftTokenService.SoftTokenHabilitadoAsync.");

        public static EventId ExceptionCallingSoftTokenValido { get; }
            = new EventId(ServiceEventsRanges.SoftTokenServiceEventsRange,
                "Excepción llamando a Spv.Usuarios.Bff.Service.SoftTokenService.SoftTokenValidoAsync.");
    }
}
