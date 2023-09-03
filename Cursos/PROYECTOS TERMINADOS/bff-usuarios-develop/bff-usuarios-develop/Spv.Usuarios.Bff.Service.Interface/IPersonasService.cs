using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface IPersonasService
    {
        Task<IResponse<PersonaModelOutput>> ObtenerPersonaFisicaAsync(IRequestBody<PersonaModelInput> personaModel);
        Task<IResponse<PersonaModelOutput>> ObtenerPersonaFiltroAsync(IRequestBody<PersonaFiltroInput> personaModel);
    }
}
