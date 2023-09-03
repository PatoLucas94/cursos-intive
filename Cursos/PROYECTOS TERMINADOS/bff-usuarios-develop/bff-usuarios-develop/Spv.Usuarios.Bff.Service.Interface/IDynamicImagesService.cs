using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.DynamicImagesService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface IDynamicImagesService
    {
        Task<IResponse<List<ImagenLoginModelOutput>>> ObtenerImagenesLoginAsync(IRequestBody<string> LoginModel);

        Task<IResponse<List<ImagenLoginModelOutput>>> ObtenerImagenesLoginAsync();
    }
}
