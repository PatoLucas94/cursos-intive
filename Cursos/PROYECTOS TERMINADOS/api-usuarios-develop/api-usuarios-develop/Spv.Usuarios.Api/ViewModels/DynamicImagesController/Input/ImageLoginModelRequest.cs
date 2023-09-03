using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.DynamicImagesController.Input
{
    /// <summary>
    /// ImageLoginModelRequest
    /// </summary>
    public static class ImageLoginModelRequest
    {
        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="allowedChannels"></param>
        /// <returns></returns>
        public static IRequestBody<bool> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels) =>
            headers?.ToRequestBody(true, allowedChannels);
    }
}
