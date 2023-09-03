using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Common.LogEvents
{
    public static class ClaveCanalesServiceEvents
    {
        public static EventId ExceptionCallingValidacion { get; } = new EventId(2000, "Excepción llamando a Spv.ClaveCanales.Service.ValidacionAsync.");

        public static EventId ExceptionAlObtenerConfiguracionCantidadDeIntentosDeClaveDeCanales { get; } = new EventId(2001, "Excepción al Obtener Configuracion Cantidad De Intentos De Clave de Canales.");

        public static EventId ExceptionCallingInhabilitacion { get; } = new EventId(2002, "Excepción llamando a Spv.ClaveCanales.Service.InhabilitacionAsync.");

        public static EventId ExceptionCallingObtenerEstado { get; } = new EventId(2003, "Excepción llamando a Spv.ClaveCanales.Service.ObtenerEstadoAsync.");
    }
}