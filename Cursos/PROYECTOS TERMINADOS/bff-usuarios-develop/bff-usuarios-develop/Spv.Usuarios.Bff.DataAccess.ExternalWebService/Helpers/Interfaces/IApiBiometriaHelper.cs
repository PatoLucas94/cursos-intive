using System.Net.Http;
using System.Threading.Tasks;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    /// <summary>
    /// IApiBiometriaHelper
    /// </summary>
    public interface IApiBiometriaHelper
    {
        /// <summary>
        /// Obtener valor correspondiente al header X-Canal
        /// </summary>
        /// <returns></returns>
        string XCanal();

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario 
        /// </summary>
        /// <returns></returns>
        string XUsuario();

        /// <summary>
        /// Obtener valor correspondiente al header X-Aplicacion 
        /// </summary>
        /// <returns></returns>
        string XAplicacion();

        /// <summary>
        /// BasePath
        /// </summary>
        /// <returns></returns>
        string BasePath();

        /// <summary>
        /// Obtener path de endpoint de autenticación
        /// </summary>
        /// <returns></returns>
        string AutenticacionPath();

        /// <summary>
        /// Efectúa un POST
        /// </summary>
        /// <param name="path">Path de endpoint de API</param>
        /// <param name="body">Body de petición</param>
        /// <param name="canal">Canal de petición</param>
        /// <param name="usuario">Usuario de petición</param>
        /// <param name="aplicacion">Aplicación de petición</param>
        /// <returns>HttpResponseMessage</returns>
        Task<HttpResponseMessage> PostRequestAsync<T>(
            string path,
            T body,
            string canal = null,
            string usuario = null,
            string aplicacion = null
        ) where T : class;
    }
}
