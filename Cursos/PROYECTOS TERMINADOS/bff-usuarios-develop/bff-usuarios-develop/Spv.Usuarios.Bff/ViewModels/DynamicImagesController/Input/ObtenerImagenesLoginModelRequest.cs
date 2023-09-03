using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.DynamicImagesController.Input
{
    /// <summary>
    /// ObtenerImagenesLoginModelRequest
    /// </summary>
    public static class ObtenerImagenesLoginModelRequest
    {
        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static IRequestBody<string> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(string.Empty);
        }
    }
}
