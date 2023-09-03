using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output
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
