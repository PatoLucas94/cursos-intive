using System.Net.Http;
using System.Threading.Tasks;

namespace Spv.Usuarios.DataAccess.ExternalWebService.Helpers
{
    public interface IBtaHelper
    {
        /// <summary>
        /// Retorna el UserPassword
        /// </summary>
        /// <returns></returns>
        string UserPassword();

        /// <summary>
        /// Obtener path base
        /// </summary>
        /// <returns></returns>
        string BtaBasePath();

        /// <summary>
        /// Obtener path de endpoint del token bta
        /// </summary>
        /// <returns></returns>
        string TokenBtaPath();

        /// <summary>
        /// Obtener path de endpoint pin
        /// </summary>
        /// <returns></returns>
        string ObtenerPinPath();


        /// <summary>
        /// Efectúa un POST en BtaHelper del Body en el Path pasado por parámetro
        /// </summary>
        /// <param name="path">Path de endpoint de API</param>
        /// <param name="body">Body de petición</param>
        /// <returns>HttpResponseMessage</returns>
        Task<HttpResponseMessage> PostAsync(
         string requestPath,
         object body);
    }
}
