namespace Spv.Usuarios.DataAccess.ExternalWebService.Helpers
{
    /// <summary>
    /// IApiPersonaHelper
    /// </summary>
    public interface IApiPersonasHelper
    {
        /// <summary>
        /// Obtener path base
        /// </summary>
        /// <returns></returns>
        string PersonaBasePath();

        /// <summary>
        /// Obtener path de endpoint que retorna id de persona
        /// </summary>
        /// <returns></returns>
        string PersonaPath();

        /// <summary>
        /// Obtener path de endpoint que retorna la información de la persona
        /// </summary>
        /// <returns></returns>
        string PersonaInfoPath();

        /// <summary>
        /// Obtener path de endpoint que retorna la información de la persona física
        /// </summary>
        /// <returns></returns>
        string PersonaFisicaInfoPath();
    }
}
