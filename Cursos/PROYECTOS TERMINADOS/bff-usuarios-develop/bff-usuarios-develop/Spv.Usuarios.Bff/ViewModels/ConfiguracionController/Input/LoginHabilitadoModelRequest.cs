using Spv.Usuarios.Bff.Common.Dtos.Service.ConfiguracionService.Input;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.ConfiguracionController.Input
{
    /// <summary>
    /// LoginHabilitadoModelRequest
    /// </summary>
    public static class LoginHabilitadoModelRequest
    {
        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static IRequestBody<LoginHabilitadoModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(new LoginHabilitadoModelInput());
        }
    }
}
