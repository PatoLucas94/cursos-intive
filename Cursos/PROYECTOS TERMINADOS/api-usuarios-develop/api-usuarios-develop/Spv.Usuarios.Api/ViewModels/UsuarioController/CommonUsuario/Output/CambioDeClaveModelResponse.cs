using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// CambioDeClaveModelResponse
    /// </summary>
    public class CambioDeClaveModelResponse
    {
        /// <summary>
        /// Constructor CambioDeClaveModelResponse
        /// </summary>
        protected CambioDeClaveModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<CambioDeClaveModelResponse>> FromAsync(Task<IResponse<CambioDeClaveModelOutput>> task)
        {
            var response = await task;
            return response.Map(a => new CambioDeClaveModelResponse());
        }
    }
}
