using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Common.Errors
{
    [ExcludeFromCodeCoverage]
    public class ErrorCode
    {
        public string Code { get; set; }
        public string ErrorDescription { get; set; }

        public ErrorCode()
        {
        }

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

        public static ErrorCode UsuarioInexistente => new ErrorCode(ErrorConstants.CodigoUsuarioInexistente,
            MessageConstants.UsuarioInexistente);

        public static ErrorCode UsuarioBloqueado =>
            new ErrorCode(ErrorConstants.CodigoUsuarioBloqueado, MessageConstants.UsuarioBloqueado);

        public static ErrorCode UsuarioInactivo =>
            new ErrorCode(ErrorConstants.CodigoUsuarioInactivo, MessageConstants.UsuarioInactivo);

        public static ErrorCode UsuarioSuspendido =>
            new ErrorCode(ErrorConstants.CodigoUsuarioSuspendido, MessageConstants.UsuarioSuspendido);

        public static ErrorCode EstadoDeUsuarioNoControlado => new ErrorCode(
            ErrorConstants.CodigoEstadoDeUsuarioNoControlado, MessageConstants.EstadoDeUsuarioNoControlado);

        public static ErrorCode SeHaBloqueadoElUsuario => new ErrorCode(ErrorConstants.CodigoUsuarioBloqueado,
            MessageConstants.SeHaBloqueadoElUsuario);

        public static ErrorCode ClaveDeCanalesInexistente => new ErrorCode(ErrorConstants.CodigoClaveCanalesInexistente,
            MessageConstants.ClaveDeCanalesInexistente);

        public static ErrorCode ClaveDeCanalesInactiva => new ErrorCode(ErrorConstants.CodigoClaveCanalesInactiva,
            MessageConstants.ClaveDeCanalesInactiva);

        public static ErrorCode ClaveDeCanalesIncorrecta => new ErrorCode(ErrorConstants.CodigoClaveCanalesIncorrecta,
            MessageConstants.ClaveDeCanalesIncorrecta);

        public static ErrorCode ClaveDeCanalesBloqueada => new ErrorCode(ErrorConstants.CodigoClaveCanalesBloqueada,
            MessageConstants.ClaveDeCanalesBloqueada);

        public static ErrorCode ClaveDeCanalesExpirada => new ErrorCode(ErrorConstants.CodigoClaveCanalesExpirada,
            MessageConstants.ClaveDeCanalesExpirada);

        public static ErrorCode ClaveYaUtilizada =>
            new ErrorCode(ErrorConstants.CodigoClaveYaUtilizada, MessageConstants.ClaveYaUtilizada);

        public static ErrorCode ClaveActualNoCoincide => new ErrorCode(ErrorConstants.CodigoPasswordDistinto,
            MessageConstants.ClaveActualNoCoincide);

        public static ErrorCode UsuarioYaUtilizado => new ErrorCode(ErrorConstants.CodigoUsuarioYaUtilizado,
            MessageConstants.UsuarioYaUtilizado);

        public static ErrorCode CredencialesYaUtilizadas => new ErrorCode(ErrorConstants.CodigoCredencialesYaUtilizadas,
            MessageConstants.CredencialesYaUtilizadas);

        public static ErrorCode UsuarioIncorrecto =>
            new ErrorCode(ErrorConstants.CodigoUsuarioIncorrecto, MessageConstants.UsuarioIncorrecto);

        public static ErrorCode OperacionNoHabilitada => new ErrorCode(ErrorConstants.CodigoOperacionNoHabilitada,
            MessageConstants.OperacionNoHabilitada);

        public static ErrorCode EstatusYaAsignado =>
            new ErrorCode(ErrorConstants.CodigoEstatusYaAsignado, MessageConstants.EstatusYaAsignado);
    }
}
