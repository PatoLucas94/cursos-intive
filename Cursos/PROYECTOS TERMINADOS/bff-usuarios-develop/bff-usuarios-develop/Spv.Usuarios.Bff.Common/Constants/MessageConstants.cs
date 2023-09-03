using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.Constants
{
    [ExcludeFromCodeCoverage]
    public static class MessageConstants
    {
        // Generales
        public const string CampoInvalido = "El valor enviado es inválido";
        public const string ErrorDeValidacion = "Se produjeron uno o más errores de validación.";
        public const string MensajeGenerico = "Ha ocurrido un error inesperado.";

        public const string ErrorInterno =
            "Ha ocurrido un error interno, por lo que no se pudo completar su solicitud.";

        public const string ErrorCliente = "Algo salió mal durante la solicitud.";

        // Catalogo
        public const string PaisesNoDisponibles = "No se encontraron paises.";
        public const string TiposDocumentoNoDisponibles = "No se encontraron tipos de documento.";

        // Clave SMS
        public const string ClaveSmsIncorrecta = "Clave SMS incorrecta.";

        //Perfil
        public const string PerfilInexistente = "Perfil inexistente.";

        // Personas
        public const string PersonaInexistente = "Persona inexistente.";
        public const string PersonaAmbigua = "Persona Ambigua.";

        public static string PersonaFisicaInexistente(int idPersona) =>
            $"No se encontró a la Persona Física desde api-personas con id {idPersona}";

        // Usuarios
        public const string DocumentoYaExiste = "El documento indicado ya se encuentra registrado.";
        public const string AutenticacionIncorrecta = "Credenciales invalidas.";

        // api-personas
        public const string ApiPersonasErrorCreacionEmail = "No se pudo crear el email desde api-personas.";
        public const string ApiPersonasErrorActualizacionEmail = "No se pudo actualizar el email desde api-personas.";
        public const string ApiPersonasErrorCreacionTelefono = "No se pudo crear el teléfono desde api-personas.";

        public const string ApiPersonasErrorCreacionTelefonoDobleFactor =
            "No se pudo crear el teléfono doble factor desde api-personas.";

        public const string ApiPersonasErrorVerificacionTelefono =
            "No se pudo verificar el teléfono desde api-personas.";

        // api-google
        public const string ApiGoogleErrorGenerico = "Ha ocurrido un error al consultar el servicio google.";
        public const string ApiGoogleActionInvalido = "El action indicado es inválido.";

        // api-tyc
        public const string TyCVigenteInexistente = "Términos y Condiciones vigente inexistente.";
        public const string TyCErrorAceptacion = "No se pudo aceptar los términos y condiciones desde api-tyc.";
        public const string TyCNoHabilitado = "Términos y Condiciones no estan habilitados.";

        // clave-canales
        public const string ClaveCanalesNoCorrespondiente =
            "La clave de canales no corresponde al usuario que quiere realizar la acción.";

        // api-softToken
        public const string SoftTokenBloqueado = "El token esta bloqueado.";
        public const string SoftTokenNoHabilitado = "El token no esta habilitado.";
        public const string SoftTokenInvalido = "El token no es valido.";
        public const string SoftTokenCanalInvalido = "El canal no es valido.";
        public const string SoftTokenUsuarioInvalido = "El usuario no es valido.";
        public const string SoftTokenIdentificadorInvalido = "El identificador no es valido.";

        // Configuracion
        public const string ConfiguracionLoginHabilitado = "El valor de LogInDisabled es nulo o vacio";
        public const string ConfiguracionLoginHabilitadoFueraDeRango = "El valor de LogInDisabled es distinto de 0, 1 o 2";
    }
}
