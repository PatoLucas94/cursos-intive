using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Input;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.CatalogoController.CommonCatalogo.Input
{
    /// <summary>
    /// PaisesModelRequest
    /// </summary>
    public class PaisesModelRequest
    {
        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<PaisesModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new PaisesModelInput());
        }
    }
}
