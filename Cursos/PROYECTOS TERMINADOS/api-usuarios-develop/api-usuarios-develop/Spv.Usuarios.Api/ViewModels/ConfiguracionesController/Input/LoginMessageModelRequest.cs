using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Dtos.ConfiguracionesService.Input;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.ConfiguracionesController.Input
{
    /// <summary>
    /// LoginMessageModelRequest
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class LoginMessageModelRequest
    {
        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="allowedChannels"></param>
        /// <returns></returns>
        public static IRequestBody<LoginMessageModelInput> ToRequestBody(
            ApiHeaders headers,
            IAllowedChannels allowedChannels
        ) => headers?.ToRequestBody(new LoginMessageModelInput(), allowedChannels);
    }
}
