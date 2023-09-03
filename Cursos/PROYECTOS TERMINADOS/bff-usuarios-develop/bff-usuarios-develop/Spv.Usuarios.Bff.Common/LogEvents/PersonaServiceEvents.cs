using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Bff.Common.LogEvents
{
    public static class PersonaServiceEvents
    {
        public static EventId ExceptionCallingPersonaFisica { get; } 
            = new EventId(ServiceEventsRanges.PersonaServiceEventsRange, "Excepción llamando a Spv.Usuarios.Bff.Service.ObtenerPersonaFisicaAsync.");
        public static EventId ExceptionCallingPersonasFiltro { get; } 
            = new EventId(ServiceEventsRanges.PersonaServiceEventsRange + 1, "Excepción llamando a Spv.Usuarios.Bff.Service.ObtenerPersonaFiltroAsync.");
    }
}
