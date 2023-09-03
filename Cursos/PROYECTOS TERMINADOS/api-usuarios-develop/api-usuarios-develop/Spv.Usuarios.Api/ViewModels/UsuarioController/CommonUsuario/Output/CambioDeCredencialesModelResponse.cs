using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// CambioDeCredencialesModelResponse
    /// </summary>
    public class CambioDeCredencialesModelResponse
    {
        /// <summary>
        /// Constructor CambioDeCredencialesModelResponse
        /// </summary>
        protected CambioDeCredencialesModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<CambioDeCredencialesModelResponse>> FromAsync(Task<IResponse<CambioDeCredencialesModelOutput>> task)
        {
            var response = await task;
            return response.Map(a => new CambioDeCredencialesModelResponse());
        }
    }
}
