using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Bff.Common.LogEvents
{
    public static class ConfiguracionServiceEvents
    {
        public static EventId ExceptionCallingLoginHabilitado { get; }
            = new EventId(ServiceEventsRanges.ConfiguracionEventsRange, "Excepción llamando a Spv.Usuarios.Bff.ConfiguracionService.LoginHabilitadoAsync.");
    }
}
