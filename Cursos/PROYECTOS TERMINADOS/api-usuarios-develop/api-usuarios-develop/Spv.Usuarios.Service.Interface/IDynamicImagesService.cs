using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.DynamicImagesService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Service.Interface
{
    public interface IDynamicImagesService
    {
        Task<IResponse<List<ImageOutput>>> ObtenerImagesLoginAsync(IRequestBody<bool> habilitado);
    }
}
