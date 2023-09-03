namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces
{
    public interface IApiScoreOperacionesHelper
    {
        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-ScoreOperaciones
        /// </summary>
        /// <returns></returns>
        string ApiScoreOperacionesxCanal();

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-ScoreOperaciones
        /// </summary>
        /// <returns></returns>
        string ApiScoreOperacionesxUsuario();

        /// <summary>
        /// BasePath
        /// </summary>
        /// <returns></returns>
        string BasePath();

        /// <summary>
        /// UPDATE CREDENTIALS
        /// </summary>
        /// <returns></returns>
        string UpdateCredentials();

        /// <summary>
        /// RegistracionScore
        /// </summary>
        /// <returns></returns>
        string RegistracionScore();

        /// <summary>
        /// IniciarSesionScore
        /// </summary>
        /// <returns></returns>
        string IniciarSesionScore();
    }
}
