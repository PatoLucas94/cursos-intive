using System;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    /// <summary>
    /// IApiPersonaHelper
    /// </summary>
    public interface IApiPersonasHelper
    {
        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-personas
        /// </summary>
        /// <returns></returns>
        string ApiPersonasXCanal();

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-personas
        /// </summary>
        /// <returns></returns>
        string ApiPersonasXUsuario();

        /// <summary>
        /// BasePath
        /// </summary>
        /// <returns></returns>
        string BasePath();

        /// <summary>
        /// Obtener path de endpoint que retorna id de persona
        /// </summary>
        /// <returns></returns>
        string PersonasPath();

        /// <summary>
        /// Obtener path de endpoint que retorna id de persona filtrando por numero de documento
        /// </summary>
        /// <returns></returns>
        string PersonasFiltroPath();

        /// <summary>
        /// Obtener path de endpoint que retorna la información de la persona
        /// </summary>
        /// <returns></returns>
        string PersonasInfoPath();

        /// <summary>
        /// Obtener path de endpoint que retorna la información de la persona física
        /// </summary>
        /// <returns></returns>
        string PersonasFisicaInfoPath();

        /// <summary>
        /// Obtener path de endpoint que retorna los teléfonos de la persona
        /// </summary>
        /// <returns></returns>
        string TelefonosPath();

        /// <summary>
        /// Obtener path de endpoint para creación de un teléfono en api-personas
        /// </summary>
        /// <returns></returns>
        string TelefonosCreacionPath();

        /// <summary>
        /// Obtener path de endpoint para actualización de un teléfono en api-personas
        /// </summary>
        /// <returns></returns>
        string TelefonosActualizacionPath();

        /// <summary>
        /// Obtener path de endpoint que retorna los teléfonos con doble factor de la persona
        /// </summary>
        /// <returns></returns>
        string TelefonosDobleFactorPath();

        /// <summary>
        /// Obtener path de endpoint para creación de un teléfono nuevo en api-personas
        /// </summary>
        /// <returns></returns>
        string TelefonosDobleFactorCreacionPath();

        /// <summary>
        /// Obtener path de endpoint para verificación de un teléfono nuevo en api-personas
        /// </summary>
        /// <returns></returns>
        string TelefonosDobleFactorActualizacionPath();

        /// <summary>
        /// Obtener path de endpoint para verificación de un teléfono en api-personas
        /// </summary>
        /// <returns></returns>
        string TelefonosVerificacionPath();

        /// <summary>
        /// Obtener path de endpoint para actualización de un email en api-personas
        /// </summary>
        /// <returns></returns>
        string EmailsActualizacionPath();

        /// <summary>
        /// Obtener path de endpoint para creación de un email nuevo en api-personas
        /// </summary>
        /// <returns></returns>
        string EmailsCreacionPath();

        /// <summary>
        /// Obtener path de endpoint para marca cliente en api-personas-productos
        /// </summary>
        /// <returns></returns>
        string ProductosMarcaClientePath();

        /// <summary>
        /// ExpirationCache
        /// </summary>
        /// <returns></returns>
        DateTimeOffset ExpirationCache();
    }
}
