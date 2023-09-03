using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Output
{
    /// <summary>
    /// ValidacionCanalesModelResponse
    /// </summary>
    public class ValidacionClaveCanalesModelResponse
    {
        /// <summary>
        /// Constructor ValidacionCanalesModelResponse
        /// </summary>
        protected ValidacionClaveCanalesModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<ValidacionClaveCanalesModelResponse>> FromAsync(Task<IResponse<ValidacionClaveCanalesModelOutput>> task)
        {
            var response = await task;
            return response.Map(a => new ValidacionClaveCanalesModelResponse());
        }

    }
}