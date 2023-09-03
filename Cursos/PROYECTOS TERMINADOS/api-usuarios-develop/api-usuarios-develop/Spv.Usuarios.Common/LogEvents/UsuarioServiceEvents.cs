using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Common.LogEvents
{
    [ExcludeFromCodeCoverage]
    public static class UsuarioServiceEvents
    {
        #region TemplatesMessages

        public const string CallingAutenticacionMessage = "Excepción llamando a Spv.Usuarios.Service.AutenticarAsync.";

        public const string CallingVerificarTokenMessage =
            "Excepción llamando a Spv.Usuarios.Service.VerificarTokenAsync.";

        public const string CallingCerrarSesionMessage = "Excepción llamando a Spv.Usuarios.Service.CerrarSesionAsync.";

        public const string CallingObtenerUsuarioMessage =
            "Excepción llamando a Spv.Usuarios.Service.ObtenerUsuarioAsync.";

        #endregion

        public static EventId ExceptionCallingAutenticacion { get; } = new EventId(1000, CallingAutenticacionMessage);

        public static EventId ExceptionAlObtenerConfiguracionCantidadDeIntentosDeLogin { get; } = new EventId(
            1001,
            "Excepción al Obtener Configuración Cantidad De Intentos De Login."
        );

        public static EventId ExceptionCallingPerfil { get; } = new EventId(
            1002,
            "Excepción llamando a Spv.Usuarios.Service.ObtenerPerfilAsync."
        );

        public static EventId ExceptionCallingRegistro { get; } = new EventId(
            1003,
            "Excepción llamando a Spv.Usuarios.Service.RegistrarAsync."
        );

        public static EventId ExceptionCallingValidacionExistencia { get; } = new EventId(
            1004,
            "Excepción llamando a Spv.Usuarios.Service.ValidarExistenciaAsync."
        );

        public static EventId ExceptionCallingRegistroV1 { get; } = new EventId(
            1005,
            "Excepción llamando a Spv.Usuarios.Service.RegistrarV1Async."
        );

        public static EventId ExceptionCallingAutenticacionConClaveNumerica { get; } = new EventId(
            1006,
            "Excepción llamando a Spv.Usuarios.Service.AutenticarConClaveNumericaAsync."
        );

        public static EventId ExceptionCallingCambioDeClaveV2 { get; } = new EventId(
            1007,
            "Excepción llamando a Spv.Usuarios.Service.CambioDeClaveAsync."
        );

        public static EventId ExceptionAlObtenerConfiguracionDeHistorialDeCambiosDeClave { get; } = new EventId(
            1008,
            "Excepción al Obtener Configuración de Historial de Cambios de Clave."
        );

        public static EventId ExceptionCallingCambioDeCredencialesV2 { get; } = new EventId(
            1009,
            "Excepción llamando a Spv.Usuarios.Service.CambioDeCredencialesAsync."
        );

        public static EventId ExceptionAlObtenerConfiguracionDeHistorialDeCambiosDeNombreUsuario { get; } = new EventId(
            1010,
            "Excepción al Obtener Configuración de Historial de Cambios de Nombre de usuario."
        );

        public static EventId ExceptionAlObtenerConfiguracionDeCantidadDiasParaForzarCambioDeClave { get; } =
            new EventId(
                1011,
                "Excepción al Obtener Configuración de cantidad dias para forzar cambio de clave."
            );

        public static EventId ExceptionAlObtenerConfiguracionRegistracionNuevoModeloHabilitado { get; } = new EventId(
            1012,
            "Excepción al Obtener Configuración Registración nuevo modelo habilitado."
        );

        public static EventId ExceptionCallingValidacionExistenciaHbi { get; } = new EventId(
            1013,
            "Excepción llamando a Spv.Usuarios.Service.ValidarExistenciaHbiAsync."
        );

        public static EventId InformationCallingValidarClaveActual { get; } = new EventId(
            1014,
            "Infomacion llamando a Spv.Usuarios.Service.ValidarClaveActual."
        );

        public static EventId ExceptionCambiandoEstatus { get; } = new EventId(
            1004,
            "Excepción llamando a Spv.Usuarios.Service.CambiarEstadoAsync."
        );

        public static EventId ExceptionCallingActualizarPersonId { get; } = new EventId(
            1004,
            "Excepción llamando a Spv.Usuarios.Service.ActualizarPersonIdAsync."
        );
    }
}
