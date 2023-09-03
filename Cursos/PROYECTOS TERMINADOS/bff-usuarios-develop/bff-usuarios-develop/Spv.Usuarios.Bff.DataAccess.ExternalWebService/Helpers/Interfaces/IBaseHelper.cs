namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    public interface IBaseHelper
    {
        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir servicios externos
        /// </summary>
        /// <returns></returns>
        string XCanal(string serviceName);

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir servicios externos
        /// </summary>
        /// <returns></returns>
        string XUsuario(string serviceName);

        /// <summary>
        /// Obtener valor correspondiente al header X-Aplicacion utilizado para consumir servicios externos
        /// </summary>
        /// <returns></returns>
        string XAplicacion(string serviceName);
    }
}
