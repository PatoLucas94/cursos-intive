using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// RegistracionModelResponse
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RegistracionModelResponse
    {
        /// <summary>
        /// Constructor RegistracionModelResponse
        /// </summary>
        private RegistracionModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<RegistracionModelResponse>> FromAsync(
            Task<IResponse<RegistracionModelOutput>> task)
        {
            var response = await task;
            return response.Map(a => new RegistracionModelResponse());
        }
    }
}
