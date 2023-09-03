using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface IClaveService
    {
        Task<IResponse<ValidacionClaveCanalesModelOutput>> ValidarClaveCanalesIdPersonaAsync(IRequestBody<ValidacionClaveCanalesIdPersonaModelInput> validacionModel);
        Task<IResponse<ValidacionClaveCanalesModelOutput>> ValidarClaveCanalesAsync(IRequestBody<ValidacionClaveCanalesModelInput> validacionModel);
        Task<IResponse<ValidacionClaveSmsModelOutput>> ValidarClaveSmsAsync(IRequestBody<ValidacionClaveSmsModelInput> validacionModel);
        Task<IResponse<GeneracionClaveSmsModelOutput>> GenerarClaveSmsAsync(IRequestBody<GeneracionClaveSmsModelInput> generacionModel);
        Task<IResponse<EstadoModelOutput>> ObtenerEstadoAsync(IRequestBody<EstadoModelInput> estadoModel);
    }
}
