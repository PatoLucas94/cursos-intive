namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    /// <summary>
    /// IApiGoogleHelper
    /// </summary>
    public interface IApiGoogleHelper
    {
        /// <summary>
        /// Url del endpoint para verificar token generado por recaptcha V3
        /// </summary>
        /// <returns></returns>
        string ValidacionTokenReCaptchaV3Path();

        /// <summary>
        /// Clave privada necesaria para validar token generado por recaptcha V3
        /// </summary>
        /// <returns></returns>
        string SecretReCaptchaV3Key();
    }
}
