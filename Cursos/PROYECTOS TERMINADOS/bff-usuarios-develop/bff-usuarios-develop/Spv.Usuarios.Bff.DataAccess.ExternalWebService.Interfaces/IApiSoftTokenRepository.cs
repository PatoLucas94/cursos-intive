using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Client.SofttokenClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.SofttokenClient.Output;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces
{
    public interface IApiSoftTokenRepository
    {
        Task<ApiSoftTokenModelOutput> TokenHabilitadoAsync(ApiSoftTokenModelInput body);

        Task<ApiSoftTokenModelOutput> SoftTokenValidoAsync(ApiSoftTokenValidoModelInput body);
    }
}
