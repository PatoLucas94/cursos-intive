using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.SSORepository.Output;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Service.Interface
{
    public interface ISsoService
    {
        Task<IResponse<TokenModelOutput>> AutenticarAsync(IRequestBody<AutenticacionModelInput> autenticacionModel);

        Task<IResponse<TokenModelOutput>> RefreshAccessTokenAsync(IRequestBody<string> tokenModel);

        Task<IResponse<IReadOnlyDictionary<string, object>>> VerificarTokenAsync(IRequestBody<string> tokenModel);

        Task<IResponse<bool>> CerrarSesionAsync(IRequestBody<string> tokenModel);
    }
}
