using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.ClaveCanalesService.Input;
using Spv.Usuarios.Common.Dtos.ClaveCanalesService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Service.Interface
{
    public interface IClaveCanalesService
    {
        Task<IResponse<ValidacionModelOutput>> ValidarAsync(IRequestBody<ValidacionModelInput> validacionModel);
        Task<IResponse<InhabilitacionModelOutput>> InhabilitarAsync(IRequestBody<InhabilitacionModelInput> inhabilitacionModel);
        Task<IResponse<EstadoModelOutput>> ObtenerEstadoAsync(IRequestBody<EstadoModelInput> estadoModel);
    }
}
