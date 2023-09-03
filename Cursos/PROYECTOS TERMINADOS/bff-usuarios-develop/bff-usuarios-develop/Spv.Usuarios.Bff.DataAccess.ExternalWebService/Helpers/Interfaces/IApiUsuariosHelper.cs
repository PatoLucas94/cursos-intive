using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    /// <summary>
    /// IApiUsuariosHelper
    /// </summary>
    public interface IApiUsuariosHelper
    {
        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-usuarios
        /// </summary>
        /// <returns></returns>
        string ApiUsuariosXCanal();

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-usuarios
        /// </summary>
        /// <returns></returns>
        string ApiUsuariosXUsuario();

        /// <summary>
        /// Obtener valor correspondiente al header X-Aplicacion utilizado para consumir api-usuarios
        /// </summary>
        /// <returns></returns>
        string ApiUsuariosXAplicacion();

        /// <summary>
        /// Obtener valor correspondiente al header X-Gateway utilizado para consumir api-usuarios
        /// </summary>
        /// <returns></returns>
        string ApiUsuariosXGateway();

        /// <summary>
        /// BasePath
        /// </summary>
        /// <returns></returns>
        string BasePath();

        /// <summary>
        /// Obtener path de endpoint que retorna perfil de un usuario
        /// </summary>
        /// <returns></returns>
        string PerfilPath();

        /// <summary>
        /// Obtener path de endpoint que retorna perfil de un usuarioV2
        /// </summary>
        /// <returns></returns>
        string PerfilPathV2();

        /// <summary>
        /// Obtener path de endpoint que valida clave de canales
        /// </summary>
        /// <returns></returns>
        string ValidarClaveCanalesPath();

        /// <summary>
        /// Obtener path de endpoint que inhabilita clave de canales
        /// </summary>
        /// <returns></returns>
        string InhabilitarClaveCanalesPath();

        /// <summary>
        /// Obtener path de endpoint que registra un usuario en el nuevo modelo
        /// </summary>
        /// <returns></returns>
        string RegistrarUsuarioPath();

        /// <summary>
        /// Obtener path de endpoint que verifica si un usuario posee credenciales en el sistema
        /// </summary>
        /// <returns></returns>
        string ValidarExistenciaPath();

        /// <summary>
        /// Obtener path de endpoint que verifica si un usuario (username) existe en el sistema HBI Legacy
        /// </summary>
        /// <returns></returns>
        string ValidarExistenciaHbiPath();

        /// <summary>
        /// Obtener path de endpoint que cambia las credenciales de un usuario del nuevo modelo
        /// </summary>
        /// <returns></returns>
        string CambiarCredencialesPath();

        /// <summary>
        /// Obtener path de endpoint que cambia la clave de un usuario
        /// </summary>
        /// <returns></returns>
        string CambiarClavePath();

        /// <summary>
        /// Obtener path de endpoint que migra un usuario al nuevo modelo
        /// </summary>
        /// <returns></returns>
        string MigrarUsuarioPath();

        /// <summary>
        /// Obtener path de endpoint que Actualiza el PersonId
        /// </summary>
        /// <returns></returns>
        string ActualizarPersonIdPath();

        /// <summary>
        /// Obtener path de endpoint de autenticación
        /// </summary>
        /// <returns></returns>
        string AutenticacionPath();
        
        /// <summary>
        /// Obtener path de endpoint de obtener usuario
        /// </summary>
        /// <returns></returns>
        string ObtenerUsuarioPath();

        /// <summary>
        /// Obtener path de endpoint de obtener imagenes login
        /// </summary>//
        /// <returns></returns>
        string ImagenesLoginPath();

        /// <summary>
        /// Obtener path de endpoint de Estado clave canales
        /// </summary>//
        /// <returns></returns>
        string ObtenerEstadoClaveCanalesPath();

        /// <summary>
        /// ExpirationCache
        /// </summary>
        /// <returns></returns>
        DateTimeOffset ExpirationCache();

        /// <summary>
        /// ExpirationCacheImagenesLogin
        /// </summary>
        /// <returns></returns>
        DateTimeOffset ExpirationCacheImagenesLogin();

        /// <summary>
        /// Efectúa un POST en ApiUsuarios del Body en el Path pasado por parámetro
        /// </summary>
        /// <param name="path">Path de endpoint de API</param>
        /// <param name="body">Body de petición</param>
        /// <param name="canal">Canal de petición</param>
        /// <param name="usuario">Usuario de petición</param>
        /// <param name="aplicacion">Aplicación de petición</param>
        /// <returns>HttpResponseMessage</returns>
        Task<HttpResponseMessage> PostRequestApiUsuariosAsync(
            string path,
            object body,
            string canal = null,
            string usuario = null,
            string aplicacion = null,
            string gateway = null);

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

        /// <summary>
        /// Efectúa un Patch en ApiUsuarios del Body en el Path pasado por parámetro
        /// </summary>
        /// <param name="path">Path de endpoint de API</param>
        /// <param name="body">Body de petición</param>
        /// <param name="canal">Canal de petición</param>
        /// <param name="usuario">Usuario de petición</param>
        /// <param name="aplicacion">Aplicación de petición</param>
        /// <returns>HttpResponseMessage</returns>
        Task<HttpResponseMessage> PatchRequestApiUsuariosAsync(
            string path,
            object body,
            string canal = null,
            string usuario = null,
            string aplicacion = null);
    }
}
