using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Dtos.ConfiguracionesService.Input;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.ConfiguracionesController.Input
{
    /// <summary>
    /// TerminosYCondicionesHabilitadoModelRequest
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class TerminosYCondicionesHabilitadoModelRequest
    {
        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="allowedChannels"></param>
        /// <returns></returns>
        public static IRequestBody<TerminosYCondicionesHabilitadoModelInput> ToRequestBody(
            ApiHeaders headers,
            IAllowedChannels allowedChannels
        ) => headers?.ToRequestBody(new TerminosYCondicionesHabilitadoModelInput(), allowedChannels);
    }
}
