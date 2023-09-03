using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface ICatalogoService
    {
        Task<IResponse<List<PaisModelOutput>>> ObtenerPaisesAsync(IRequestBody<PaisesModelInput> paisesModel);
        Task<IResponse<List<TipoDocumentoModelOutput>>> ObtenerTiposDocumentoAsync(IRequestBody<TiposDocumentoModelInput> tiposDocumentoModel);
    }
}
