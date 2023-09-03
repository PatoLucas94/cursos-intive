using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Output
{
    /// <summary>
    /// ValidacionClaveSmsModelResponse
    /// </summary>
    public class ValidacionClaveSmsModelResponse
    {
        /// <summary>
        /// Constructor ValidacionClaveSmsModelResponse
        /// </summary>
        protected ValidacionClaveSmsModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<ValidacionClaveSmsModelResponse>> FromAsync(Task<IResponse<ValidacionClaveSmsModelOutput>> task)
        {
            var response = await task;
            return response.Map(a => new ValidacionClaveSmsModelResponse());
        }
    }
}