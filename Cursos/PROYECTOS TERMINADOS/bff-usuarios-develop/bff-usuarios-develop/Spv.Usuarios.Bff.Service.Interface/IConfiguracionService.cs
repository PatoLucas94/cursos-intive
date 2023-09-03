using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.ConfiguracionService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.ConfiguracionService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface IConfiguracionService
    {
        Task<IResponse<LoginHabilitadoModelOutput>> LoginHabilitadoAsync(IRequestBody<LoginHabilitadoModelInput> LoginModel);
    }
}
