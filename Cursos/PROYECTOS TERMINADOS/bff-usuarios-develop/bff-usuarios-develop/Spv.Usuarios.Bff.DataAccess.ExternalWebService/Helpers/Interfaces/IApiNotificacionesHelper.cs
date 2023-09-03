namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    /// <summary>
    /// IApiNotificacionesHelper
    /// </summary>
    public interface IApiNotificacionesHelper
    {
        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-notificaciones
        /// </summary>
        /// <returns></returns>
        string ApiNotificacionesXCanal();

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-notificaciones
        /// </summary>
        /// <returns></returns>
        string ApiNotificacionesXUsuario();

        /// <summary>
        /// BasePath
        /// </summary>
        /// <returns></returns>
        string BasePath();

        /// <summary>
        /// CrearYEnviarTokenPath
        /// </summary>
        /// <returns></returns>
        string CrearYEnviarTokenPath();

        /// <summary>
        /// ValidarTokenPath
        /// </summary>
        /// <returns></returns>
        string ValidarTokenPath();

        /// <summary>
        /// EnviarEmailPath
        /// </summary>
        /// <returns></returns>
        string EnviarEmailPath();

        /// <summary>
        /// TiempoRespuestaEnvioMails
        /// </summary>
        /// <returns></returns>
        string TiempoRespuestaEnvioMails();
    }
}
