using Microsoft.Extensions.Options;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    /// <summary>
    /// ApiNotificacionesHelper: servicio para obtener configuraciones necesarias para interactuar con api-notificaciones
    /// </summary>
    public class ApiNotificacionesHelper : BaseHelper, IApiNotificacionesHelper
    {
        public const string CreacionTokenSmsEstadoPendienteEnvio = "PENDIENTE_ENVIO";
        public const string CreacionTokenSmsEstadoEnviado = "ENVIADO";
        public const string ValidacionTokenSmsEstadoInvalido = "INVALIDO";
        public const string ValidacionTokenSmsEstadoUtilizado = "UTILIZADO";
        public const string EnvioEmailEstadoEnviado = "ENVIADO";
        
        private readonly IOptions<ApiNotificacionesConfigurationsOptions> _apiNotificacionesConfigurations;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiNotificacionesConfigurations"></param>
        /// <param name="apiServicesConfigurations"></param>
        public ApiNotificacionesHelper(
            IOptions<ApiNotificacionesConfigurationsOptions> apiNotificacionesConfigurations,
            IOptions<ApiServicesHeadersConfigurationOptions> apiServicesConfigurations) : base(apiServicesConfigurations)
        {
            _apiNotificacionesConfigurations = apiNotificacionesConfigurations;
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-notificaciones
        /// </summary>
        /// <returns></returns>
        public string ApiNotificacionesXCanal()
        {
            return XCanal(ExternalServicesNames.ApiNotificaciones);
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-notificaciones
        /// </summary>
        /// <returns></returns>
        public string ApiNotificacionesXUsuario()
        {
            return XUsuario(ExternalServicesNames.ApiNotificaciones);
        }

        /// <summary>
        /// Base Path del endpoint api-notificaciones
        /// </summary>
        /// <returns></returns>
        public string BasePath()
        {
            return _apiNotificacionesConfigurations.Value.BasePath ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiNotificacionesConfigurations.Value.BasePath),
                   ExternalServicesNames.ApiNotificaciones);
        }

        /// <summary>
        /// Obtener path de endpoint que crea y envía un token sms a la persona a partir de api-notificaciones
        /// </summary>
        /// <returns></returns>
        public string CrearYEnviarTokenPath()
        {
            return _apiNotificacionesConfigurations.Value.CrearYEnviarTokenPath ??
                ThrowNewExceptionPorFaltaDeKey(nameof(_apiNotificacionesConfigurations.Value.CrearYEnviarTokenPath),
                ExternalServicesNames.ApiNotificaciones);
        }

        /// <summary>
        /// Obtener path de endpoint que valida el token sms enviado a la persona a partir de api-notificaciones
        /// </summary>
        /// <returns></returns>
        public string ValidarTokenPath()
        {
            return _apiNotificacionesConfigurations.Value.ValidarTokenPath ??
                ThrowNewExceptionPorFaltaDeKey(nameof(_apiNotificacionesConfigurations.Value.ValidarTokenPath),
                ExternalServicesNames.ApiNotificaciones);
        }

        /// <summary>
        /// Obtener path de endpoint que envia un mail a la persona a partir de api-notificaciones
        /// </summary>
        /// <returns></returns>
        public string EnviarEmailPath()
        {
            return _apiNotificacionesConfigurations.Value.EnviarEmailPath ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiNotificacionesConfigurations.Value.EnviarEmailPath),
                       ExternalServicesNames.ApiNotificaciones);
        }

        /// <summary>
        /// Obtener Tiempo Respuesta Envio Mails
        /// </summary>
        /// <returns></returns>
        public string TiempoRespuestaEnvioMails()
        {
            return _apiNotificacionesConfigurations.Value.TiempoRespuestaEnvioMails ??
                   ThrowNewExceptionPorFaltaDeKey(nameof(_apiNotificacionesConfigurations.Value.TiempoRespuestaEnvioMails),
                       ExternalServicesNames.ApiNotificaciones);
        }
    }
}