using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Output
{
    /// <summary>
    /// ObtenerEstadoModelResponse
    /// </summary>
    public class ObtenerEstadoModelResponse
    {
        /// <summary>
        /// Constructor ObtenerEstadoModelResponse
        /// </summary>
        protected ObtenerEstadoModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<ObtenerEstadoModelResponse>> FromAsync(Task<IResponse<EstadoModelOutput>> task)
        {
            var response = await task;
            return response.Map(a => new ObtenerEstadoModelResponse());
        }
    }
}
