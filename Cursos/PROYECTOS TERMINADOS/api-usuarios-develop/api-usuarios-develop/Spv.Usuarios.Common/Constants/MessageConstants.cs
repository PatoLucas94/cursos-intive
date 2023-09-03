namespace Spv.Usuarios.Common.Constants
{
    public static class MessageConstants
    {
        public const string NoSeEncontroSecretKey = "No se encontró secret key.";
        public const string CampoInvalido = "El valor enviado es inválido";
        public const string ErrorDeValidacion = "Se produjeron uno o más errores de validación.";

        // Usuario
        public const string UsuarioBloqueado = "El usuario se encuentra bloqueado.";
        public const string UsuarioInexistente = "Usuario inexistente.";
        public const string UsuarioInactivo = "El usuario se encuentra inactivo.";
        public const string UsuarioSuspendido = "El usuario se encuentra suspendido.";
        public const string EstadoDeUsuarioNoControlado = "Estado de usuario no controlado.";
        public const string SeHaBloqueadoElUsuario = "Datos incorrectos, se ha bloqueado el usuario.";
        public const string UsuarioYaExiste = "El usuario ya existe.";
        public const string UsuarioYaUtilizado = "El usuario ya fue utilizado.";
        public const string CredencialesYaUtilizadas = "Las credenciales ya fueron utilizadas.";
        public const string UsuarioIncorrecto = "Datos incorrectos.";
        public const string EstatusYaAsignado = "El estatus actual ya se encuentra asignado.";

        public const string ClaveNumericaErrorWebService = "Error al consumir WS.";

        // Clave de Canales
        public const string ClaveDeCanalesInexistente = "No se encontró clave de canales.";
        public const string ClaveDeCanalesInactiva = "Clave de canales inactiva.";
        public const string ClaveDeCanalesIncorrecta = "Clave de canales incorrecta.";
        public const string ClaveDeCanalesBloqueada = "Clave de canales bloqueada.";
        public const string ClaveDeCanalesExpirada = "Clave de canales expirada.";

        public const string ClaveYaUtilizada = "La clave ya fue utilizada anteriormente.";

        public const string OperacionNoHabilitada = "Operación no habilitada para el canal.";

        public const string ClaveActualNoCoincide = "La clave ingresada no coincide con la clave actual.";
        public const string ClaveActualVacia = "La clave actual es nula o vacia";

        // Persona Física
        public const string NoSeEncontroPersonaFisica = "No se encontró a la Persona Física desde api-personas con id {0}";
        public const string NroDocumentoInvalido = "El valor de entrada [{0}] no es un valor válido para Número de Documento.";
        public const string NroDocumentoInvalidoDePersonaFisica = "El valor [{0}] no es un valor válido para Número de Documento de la consulta a Api-Persona. ";
        public const string NroDocumentoNoCoincide = "El Número de Documento ingresado '{0}' no es coherente con el valor '{1}' de Api-Personas. ";
        public const string TipoDocumentoNoCoincide = "El Tipo de Documento ingresado '{0}' no es coherente con el valor '{1}' de Api-Personas. ";
        public const string PaisNoCoincide = "El País ingresado '{0}' no es coherente con el valor '{1}' de Api-Personas. ";

        public const string ValidacionesUsuario = "El campo usuario debe cumplir las siguientes validaciones: ";
        public const string ValidacionesClave = "El campo clave debe cumplir las siguientes validaciones: ";

        #region Headers

        public const string ChannelHeaderRequiredMessage = "El header X-Canal es requerido.";

        public const string ChannelHeaderInvalidMessage = "El header X-Canal es inválido.";

        public const string UserHeaderRequiredMessage = "El header X-Usuario es requerido.";

        public const string ApplicationHeaderRequiredMessage = "El header X-Aplicacion es requerido.";

        public const string GateWayHeaderRequiredMessage = "El header X-GateWay es requerido.";

        #endregion
    }
}
