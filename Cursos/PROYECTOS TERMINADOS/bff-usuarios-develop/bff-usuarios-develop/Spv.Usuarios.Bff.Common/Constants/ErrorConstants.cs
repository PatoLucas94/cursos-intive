using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.Constants
{
    [ExcludeFromCodeCoverage]
    public static class ErrorConstants
    {
        public const string CodigoCampoRequerido = "REQ";
        public const string CodigoPersonaInexistente = "NXP";
        public const string CodigoPaisesInexistentes = "NXPA";
        public const string CodigoTiposDocumentoInexistentes = "NXTD";

        // Usuarios
        public const string CodigoUsuarioYaUtilizado = "UYU";
        public const string Autenticacion = "KeyCk";

        // clave sms
        public const string CodigoClaveSMSIncorrecta = "INC";

        // Clave canales
        public const string CodigoClaveCanalesIncorrecta = "INC";

        // TyC
        public const string CodigoTyCVigenteInexistente = "NXTyCV";
        public const string CodigoTyCNoHabilitado = "TyCNH";

        // Generales
        public const string NoSeEncontroKey = "No se encontró '{0}' key.";
        public const string CodigoGenericoError = "ERR";

        // Captcha V3
        public const string CodigoReCaptchaActionInvalido = "INV";

        // Token
        public const string CodigoSoftTokenInvalido = "STINV";
        public const string CodigoSoftTokenNoHabilitado = "STNH";
        public const string CodigoSoftTokenBloqueado = "STB";

        //Configuracion
        public const string CodigoLoginHabilitado = "CLH";
        public const string CodigoLoginHabilitadoFueraDeRango = "CLHFR";
    }
}
