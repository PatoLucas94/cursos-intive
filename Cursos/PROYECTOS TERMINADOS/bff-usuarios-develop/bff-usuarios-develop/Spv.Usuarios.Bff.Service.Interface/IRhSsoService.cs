using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.RhSsoService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.RhSsoService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface IRhSsoService
    {
        Task<IResponse<TokenModelOutput>> AutenticarAsync(IRequestBody<AutenticacionModelInput> autenticacionModel);
    }
}
