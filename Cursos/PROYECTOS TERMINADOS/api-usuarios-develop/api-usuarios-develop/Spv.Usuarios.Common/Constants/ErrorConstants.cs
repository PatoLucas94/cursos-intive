using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Common.Constants
{
    [ExcludeFromCodeCoverage]
    public static class ErrorConstants
    {
        public const string CodigoCampoRequerido = "REQ";

        // Usuarios
        public const string CodigoUsuarioInexistente = "NXU";
        public const string CodigoUsuarioBloqueado = "BLQ";
        public const string CodigoEstadoDeUsuarioNoControlado = "NOC";
        public const string CodigoUsuarioInactivo = "INA";
        public const string CodigoUsuarioYaExiste = "UYE";
        public const string CodigoUsuarioYaUtilizado = "UYU";
        public const string CodigoUsuarioSuspendido = "SUS";
        public const string CodigoUsuarioIncorrecto = "INC";
        public const string CodigoEstatusYaAsignado = "EYA";

        // Password
        public const string CodigoPasswordNoExpirado = "NEXP";
        public const string CodigoPasswordExpirado = "EXP";
        public const string CodigoPasswordExpiradoBta = "EXPBTA";
        public const string CodigoPasswordDistinto = "CAD";

        // Clave de Canales
        public const string CodigoClaveCanalesInexistente = "NXC";
        public const string CodigoClaveCanalesInactiva = "INA";
        public const string CodigoClaveCanalesIncorrecta = "INC";
        public const string CodigoClaveCanalesBloqueada = "BLQ";
        public const string CodigoClaveCanalesExpirada = "EXP";

        // Cambio de Clave
        public const string CodigoClaveYaUtilizada = "CYU";

        // Cambio de Credenciales
        public const string CodigoCredencialesYaUtilizadas = "CREDYU";

        // Operación no habilitada
        public const string CodigoOperacionNoHabilitada = "OPNH";
    }
}
