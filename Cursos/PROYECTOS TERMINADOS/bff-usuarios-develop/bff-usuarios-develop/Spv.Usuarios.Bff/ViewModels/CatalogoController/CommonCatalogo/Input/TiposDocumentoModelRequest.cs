using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Input;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.CatalogoController.CommonCatalogo.Input
{
    /// <summary>
    /// TiposDocumentoModelRequest
    /// </summary>
    public class TiposDocumentoModelRequest
    {
        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<TiposDocumentoModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new TiposDocumentoModelInput());
        }
    }
}
