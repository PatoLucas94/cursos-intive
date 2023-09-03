using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// MigracionModelResponse
    /// </summary>
    public class MigracionModelResponse
    {
        /// <summary>
        /// Constructor MigracionModelResponse
        /// </summary>
        protected MigracionModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<MigracionModelResponse>> FromAsync(Task<IResponse<MigracionModelOutput>> task)
        {
            var response = await task;
            return response.Map(a => new MigracionModelResponse());
        }
    }
}
