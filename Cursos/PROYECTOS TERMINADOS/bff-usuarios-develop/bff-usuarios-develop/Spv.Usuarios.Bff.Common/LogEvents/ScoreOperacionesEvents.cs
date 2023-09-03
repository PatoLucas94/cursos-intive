using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Bff.Common.LogEvents
{
    [ExcludeFromCodeCoverage]
    public static class ScoreOperacionesEvents
    {
        public static EventId ExceptionCallingUpdateCredentials { get; }
            = new EventId(ServiceEventsRanges.ScoreOperacionesServiceEventsRange,
                "Excepción llamando a Spv.Usuarios.Bff.DataAccess.ExternalWebService.ApiScoreOperacionesRepository.UpdateCredentialsAsync.");

        public static EventId ExceptionCallingRegistracionAsync { get; }
            = new EventId(ServiceEventsRanges.ScoreOperacionesServiceEventsRange,
                "Excepción llamando a Spv.Usuarios.Bff.DataAccess.ExternalWebService.ApiScoreOperacionesRepository.ExceptionCallingRegistracionAsync.");

        public static EventId ExceptionCallingInicioSesionAsync { get; }
            = new EventId(ServiceEventsRanges.ScoreOperacionesServiceEventsRange,
                "Excepción llamando a Spv.Usuarios.Bff.DataAccess.ExternalWebService.ApiScoreOperacionesRepository.ExceptionCallingInicioSesionAsync.");
    }
}
