namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    /// <summary>
    /// IRhSsoHelper
    /// </summary>
    public interface IRhSsoHelper
    {
        /// <summary>
        /// Obtener valor correspondiente al header X-Aplicacion
        /// </summary>
        /// <returns></returns>
        string XAplicacion();
    }
}
