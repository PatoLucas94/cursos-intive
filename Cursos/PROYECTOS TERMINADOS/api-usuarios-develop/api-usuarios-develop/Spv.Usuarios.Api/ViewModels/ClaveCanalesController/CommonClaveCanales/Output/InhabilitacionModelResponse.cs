using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.ClaveCanalesService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.ClaveCanalesController.CommonClaveCanales.Output
{
    /// <summary>
    /// InhabilitacionModelResponse
    /// </summary>
    public class InhabilitacionModelResponse
    {
        /// <summary>
        /// Constructor InhabilitacionModelResponse
        /// </summary>
        protected InhabilitacionModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<InhabilitacionModelResponse>> FromAsync(Task<IResponse<InhabilitacionModelOutput>> task)
        {
            var response = await task;
            return response.Map(a => new InhabilitacionModelResponse());
        }
    }
}
