using System;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    /// <summary>
    /// IApiTyCHelper
    /// </summary>
    public interface IApiTyCHelper
    {
        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-tyc
        /// </summary>
        /// <returns></returns>
        string ApiTyCxCanal();

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-tyc
        /// </summary>
        /// <returns></returns>
        string ApiTyCxUsuario();

        /// <summary>
        /// BasePath
        /// </summary>
        /// <returns></returns>
        string BasePath();

        /// <summary>
        /// TerminosYCondicionesVigentePath
        /// </summary>
        /// <returns></returns>
        string TerminosYCondicionesVigentePath();

        /// <summary>
        /// TerminosYCondicionesAceptadosPath
        /// </summary>
        /// <returns></returns>
        string TerminosYCondicionesAceptadosPath();

        /// <summary>
        /// ConceptoRegistracion
        /// </summary>
        /// <returns></returns>
        string ConceptoRegistracion();

        /// <summary>
        /// ExpirationCache
        /// </summary>
        /// <returns></returns>
        public DateTimeOffset ExpirationCache();
    }
}
