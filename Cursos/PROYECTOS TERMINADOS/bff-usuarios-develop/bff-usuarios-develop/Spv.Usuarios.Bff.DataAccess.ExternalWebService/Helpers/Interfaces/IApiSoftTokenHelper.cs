namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    public interface IApiSoftTokenHelper
    {
        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-tyc
        /// </summary>
        /// <returns></returns>
        string ApiSoftTokenxCanal();

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-tyc
        /// </summary>
        /// <returns></returns>
        string ApiSoftTokenxUsuario();

        /// <summary>
        /// BasePath
        /// </summary>
        /// <returns></returns>
        string BasePath();

        /// <summary>
        /// TokenHabilitado
        /// </summary>
        /// <returns></returns>
        string TokenHabilitado();

        /// <summary>
        /// TokenValido
        /// </summary>
        /// <returns></returns>
        string TokenValido();

        /// <summary>
        /// TokenIdentificador
        /// </summary>
        /// <returns></returns>
        string TokenIdentificador();
    }
}
