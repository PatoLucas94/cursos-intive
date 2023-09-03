using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.ClaveCanalesService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.ClaveCanalesController.CommonClaveCanales.Output
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
