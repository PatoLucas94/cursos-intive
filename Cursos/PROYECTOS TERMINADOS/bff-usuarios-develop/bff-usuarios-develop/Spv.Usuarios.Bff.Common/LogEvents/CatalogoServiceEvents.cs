using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Bff.Common.LogEvents
{
    public static class CatalogoServiceEvents
    {
        public static EventId ExceptionCallingObtenerPaises { get; } 
            = new EventId(ServiceEventsRanges.CatalogoServiceEventsRange, "Excepción llamando a Spv.Usuarios.Bff.Service.CatalogoService.ObtenerPaisesAsync.");

        public static EventId ExceptionCallingObtenerTiposDocumento { get; } 
            = new EventId(ServiceEventsRanges.CatalogoServiceEventsRange + 1, "Excepción llamando a Spv.Usuarios.Bff.Service.CatalogoService.ObtenerTiposDocumentoAsync.");
    }
}
