using System;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    public class ApiGoogleHelper : IApiGoogleHelper
    {
        private readonly IOptions<ApiGoogleConfigurationOptions> _apiGoogleConfigurations;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiGoogleConfigurations"></param>
        public ApiGoogleHelper(
            IOptions<ApiGoogleConfigurationOptions> apiGoogleConfigurations)
        {
            _apiGoogleConfigurations = apiGoogleConfigurations;
        }

        public string ValidacionTokenReCaptchaV3Path()
        {
            return _apiGoogleConfigurations.Value.ValidacionTokenCaptchaV3Path ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiGoogleConfigurations.Value.ValidacionTokenCaptchaV3Path),
                   ExternalServicesNames.ApiGoogle);
        }

        public string SecretReCaptchaV3Key()
        {
            return _apiGoogleConfigurations.Value.SecretCaptchaV3Key ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiGoogleConfigurations.Value.SecretCaptchaV3Key),
                   ExternalServicesNames.ApiGoogle);
        }

        private static string ThrowNewExceptionPorFaltaDeKey(string key, string serviceName)
        {
            var message = string.Format(ErrorConstants.NoSeEncontroKey, $"{serviceName}:{key}");
            throw new ArgumentNullException(message, new Exception(message));
        }
    }
}
