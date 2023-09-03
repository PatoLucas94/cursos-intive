using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface IBiometriaService
    {
        Task<IResponse<BiometriaAutenticacionModelOutput>> AutenticarAsync(
            IRequestBody<BiometriaAutenticacionModelInput> autenticacionModel
        );
    }
}
