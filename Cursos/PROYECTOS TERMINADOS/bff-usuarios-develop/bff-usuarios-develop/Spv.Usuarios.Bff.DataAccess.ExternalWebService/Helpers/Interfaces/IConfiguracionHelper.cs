using System.Net.Http;
using System.Threading.Tasks;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    /// <summary>
    /// IConfiguracionHelper
    /// </summary>
    public interface IConfiguracionHelper
    {
        /// <summary>
        /// Obtener path de endpoint de T y C habilitado
        /// </summary>
        /// <returns></returns>
        string TYCHabilitadoPath();

        /// <summary>
        /// Obtener path de endpoint de login habilitado
        /// </summary>
        /// <returns></returns>
        string LoginHabilitadoPath();

        /// <summary>
        /// Obtener path de endpoint de Mensaje Default Login Deshabilitado
        /// </summary>
        /// <returns></returns>
        string MensajeDefaultLoginDeshabilitadoPath();

        /// <summary>
        /// Obtener path de endpoint Mensaje Login Deshabilitado
        /// </summary>
        /// <returns></returns>
        string MensajeLoginDeshabilitadoPath();

        /// <summary>
        /// Efectúa un GET en ApiUsuarios 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="canal"></param>
        /// <param name="usuario"></param>
        /// <param name="aplicacion"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetRequestApiUsuarioAsync(
            string path,
            string canal,
            string usuario,
            string aplicacion);
    }
}
