using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.ClaveCanalesService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.ClaveCanalesController.CommonClaveCanales.Output
{
    /// <summary>
    /// ValidacionModelResponse
    /// </summary>
    public class ValidacionModelResponse
    {
        /// <summary>
        /// Constructor ValidacionModelResponse
        /// </summary>
        protected ValidacionModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<ValidacionModelResponse>> FromAsync(Task<IResponse<ValidacionModelOutput>> task)
        {
            var response = await task;
            return response.Map(a => new ValidacionModelResponse());
        }
    }
}
