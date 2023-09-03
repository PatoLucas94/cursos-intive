using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// RegistracionModelResponse
    /// </summary>
    public class RegistracionModelResponse
    {
        /// <summary>
        /// Constructor RegistracionModelResponse
        /// </summary>
        protected RegistracionModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<RegistracionModelResponse>> FromAsync(Task<IResponse<RegistracionModelOutput>> task)
        {
            var response = await task;
            return response.Map(a => new RegistracionModelResponse());
        }
    }
}
