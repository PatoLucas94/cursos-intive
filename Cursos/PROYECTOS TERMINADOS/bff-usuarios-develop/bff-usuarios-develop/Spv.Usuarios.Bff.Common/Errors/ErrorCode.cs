using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Common.Errors
{
    [ExcludeFromCodeCoverage]
    public class ErrorCode
    {
        public string Code { get; }
        public string ErrorDescription { get; }

        public ErrorCode(string code, string errorDescription)
        {
            Code = Arg.NonNullNorSpaces(code, nameof(code));
            ErrorDescription = Arg.NonNullNorSpaces(errorDescription, nameof(errorDescription));
        }

        public override bool Equals(object obj) => obj is ErrorCode other
                                                   && Equals(Code, other.Code)
                                                   && Equals(ErrorDescription, other.ErrorDescription);

        public override int GetHashCode()
        {
            return Code.GetHashCode() ^ ErrorDescription.GetHashCode();
        }

        // Catalogo
        public static ErrorCode PaisesInexistentes { get; }
            = new ErrorCode(ErrorConstants.CodigoPaisesInexistentes, MessageConstants.PaisesNoDisponibles);

        public static ErrorCode TiposDocumentoInexistentes { get; } = new ErrorCode(
            ErrorConstants.CodigoTiposDocumentoInexistentes,
            MessageConstants.TiposDocumentoNoDisponibles
        );

        // Claves
        public static ErrorCode ClaveSmsIncorrecta { get; } = new ErrorCode(
            ErrorConstants.CodigoClaveSMSIncorrecta,
            MessageConstants.ClaveSmsIncorrecta
        );

        public static ErrorCode ClaveCanalesNoCorrespondiente { get; } = new ErrorCode(
            ErrorConstants.CodigoClaveCanalesIncorrecta,
            MessageConstants.ClaveCanalesNoCorrespondiente
        );

        // Personas
        public static ErrorCode PersonaInexistente { get; }
            = new ErrorCode(ErrorConstants.CodigoPersonaInexistente, MessageConstants.PersonaInexistente);

        public static ErrorCode PersonaAmbigua { get; }
            = new ErrorCode(ErrorConstants.CodigoGenericoError, MessageConstants.PersonaAmbigua);

        //Perfil
        public static ErrorCode PerfilInexistente { get; }
            = new ErrorCode(ErrorConstants.CodigoGenericoError, MessageConstants.PerfilInexistente);

        // Api TyC
        public static ErrorCode TyCVigenteInexistente { get; }
            = new ErrorCode(ErrorConstants.CodigoTyCVigenteInexistente, MessageConstants.TyCVigenteInexistente);

        public static ErrorCode TyCNoHabilitado { get; }
            = new ErrorCode(ErrorConstants.CodigoTyCNoHabilitado, MessageConstants.TyCNoHabilitado);

        // Api google
        public static ErrorCode ReCaptchaValidacionFallida { get; }
            = new ErrorCode(ErrorConstants.CodigoGenericoError, MessageConstants.ErrorDeValidacion);

        public static ErrorCode ReCaptchaActionInvalido { get; }
            = new ErrorCode(ErrorConstants.CodigoReCaptchaActionInvalido, MessageConstants.ApiGoogleActionInvalido);

        // Api Softtoken
        public static ErrorCode SoftTokenInvalido { get; }
            = new ErrorCode(ErrorConstants.CodigoSoftTokenInvalido, MessageConstants.SoftTokenInvalido);

        public static ErrorCode SoftTokenNoHabilitado { get; }
            = new ErrorCode(ErrorConstants.CodigoSoftTokenNoHabilitado, MessageConstants.SoftTokenNoHabilitado);

        public static ErrorCode SoftTokenBloqueado { get; }
            = new ErrorCode(ErrorConstants.CodigoSoftTokenNoHabilitado, MessageConstants.SoftTokenBloqueado);

        // Autenticacion
        public static ErrorCode AutenticacionIncorrecta { get; }
            = new ErrorCode(ErrorConstants.Autenticacion, MessageConstants.AutenticacionIncorrecta);

        // Configuracion
        public static ErrorCode LoginHabilitado { get; }
            = new ErrorCode(ErrorConstants.CodigoLoginHabilitado, MessageConstants.ConfiguracionLoginHabilitado);

        public static ErrorCode LoginHabilitadoFueraDeRango { get; } = new ErrorCode(
            ErrorConstants.CodigoLoginHabilitadoFueraDeRango,
            MessageConstants.ConfiguracionLoginHabilitadoFueraDeRango
        );
    }
}
