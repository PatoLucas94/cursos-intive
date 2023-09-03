namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    /// <summary>
    /// IApiCatalogoHelper
    /// </summary>
    public interface IApiCatalogoHelper
    {
        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-catalogo
        /// </summary>
        /// <returns></returns>
        string ApiCatalogoXCanal();

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-catalogo
        /// </summary>
        /// <returns></returns>
        string ApiCatalogoXUsuario();

        /// <summary>
        /// BasePath
        /// </summary>
        /// <returns></returns>
        string BasePath();

        /// <summary>
        /// PaisesPath
        /// </summary>
        /// <returns></returns>
        string PaisesPath();

        /// <summary>
        /// TiposDocumentoPath
        /// </summary>
        /// <returns></returns>
        string TiposDocumentoPath();
    }
}
