using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.SSORepository.Output;

namespace Spv.Usuarios.DataAccess.ExternalWebService.Interfaces
{
    public interface ISsoRepository
    {
        Task<(TokenModelOutput Response, ErrorModel Error)> GetTokenAsync(
            string documentNumber,
            string userName,
            string password,
            string canal
        );

        Task<(TokenModelOutput Response, ErrorModel Error)> RefreshAccessTokenAsync(string refreshToken);

        Task<(IReadOnlyDictionary<string, object> Response, ErrorModel Error)> GetIntrospectAsync(string accessToken);

        Task<(bool Response, ErrorModel Error)> GetLogoutAsync(string refreshToken);
    }
}
