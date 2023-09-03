using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.PersonaService.Output;

namespace Spv.Usuarios.DataAccess.ExternalWebService.Interfaces
{
    public interface IPersonasRepository
    {
        Task<PersonaModelResponse> ObtenerPersona(string numeroDocumento, int tipoDocumento, int paisDocumento, string canal = null, string usuario = null);
        Task<PersonaInfoModelResponse> ObtenerInfoPersona(long personId, string canal = null, string usuario = null);
        Task<PersonaFisicaInfoModelResponse> ObtenerInfoPersonaFisica(long personId, string canal = null, string usuario = null);
    }
}
