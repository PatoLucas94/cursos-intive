using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output
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
