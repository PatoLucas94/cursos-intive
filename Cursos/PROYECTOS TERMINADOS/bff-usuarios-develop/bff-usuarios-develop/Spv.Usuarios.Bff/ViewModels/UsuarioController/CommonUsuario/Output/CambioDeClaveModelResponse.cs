using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output
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
