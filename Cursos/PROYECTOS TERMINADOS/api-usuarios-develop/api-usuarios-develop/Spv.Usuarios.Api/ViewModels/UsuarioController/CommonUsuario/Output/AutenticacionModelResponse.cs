using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// AutenticacionModelResponse
    /// </summary>
    public class AutenticacionModelResponse
    {
        /// <summary>
        /// Constructor AutenticacionModelResponse
        /// </summary>
        protected AutenticacionModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<AutenticacionModelResponse>> FromAsync(Task<IResponse<AutenticacionModelOutput>> task)
        {
            var response = await task;
            return response.Map(a => new AutenticacionModelResponse());
        }
    }
}
