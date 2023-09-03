using System;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    public class BaseHelper : IBaseHelper
    {
        private readonly IOptions<ApiServicesHeadersConfigurationOptions> _apiServicesConfigurations;

        public BaseHelper(IOptions<ApiServicesHeadersConfigurationOptions> apiServicesConfigurations)
        {
            _apiServicesConfigurations = apiServicesConfigurations;
        }

        public string XCanal(string serviceName)
        {
            return _apiServicesConfigurations.Value.xCanal ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiServicesConfigurations.Value.xCanal), serviceName);
        }

        public string XUsuario(string serviceName)
        {
            return _apiServicesConfigurations.Value.xUsuario ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiServicesConfigurations.Value.xUsuario), serviceName);
        }

        public string XAplicacion(string serviceName)
        {
            return _apiServicesConfigurations.Value.xAplicacion ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiServicesConfigurations.Value.xAplicacion), serviceName);
        }

        public string XGateway(string serviceName)
        {
            return _apiServicesConfigurations.Value.xGateway ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiServicesConfigurations.Value.xGateway), serviceName);
        }

        protected static string ThrowNewExceptionPorFaltaDeKey(string key, string serviceName)
        {
            var message = string.Format(ErrorConstants.NoSeEncontroKey, $"{serviceName}:{key}");
            throw new ArgumentNullException(message, new Exception(message));
        }
    }
}
