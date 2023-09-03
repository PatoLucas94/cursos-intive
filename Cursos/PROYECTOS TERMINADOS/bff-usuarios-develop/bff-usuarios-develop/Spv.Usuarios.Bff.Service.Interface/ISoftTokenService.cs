using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.SoftToken.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.SoftToken.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface ISoftTokenService
    {
        Task<IResponse<SoftTokenModelOutput>> SoftTokenHabilitadoAsync(IRequestBody<SoftTokenModelInput> softTokenModel);

        Task<IResponse<SoftTokenModelOutput>> SoftTokenValidoAsync(IRequestBody<SoftTokenValidoModelInput> softTokenModel);
    }
}
