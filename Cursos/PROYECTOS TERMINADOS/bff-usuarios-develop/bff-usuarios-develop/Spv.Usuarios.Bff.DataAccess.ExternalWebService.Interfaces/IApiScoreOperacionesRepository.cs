using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Client.ScoreOperaciones.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.ScoreOperaciones.Output;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces
{
    public interface IApiScoreOperacionesRepository
    {
        Task<ApiScoreOperacionesModelOutput> UpdateCredentialsAsync(ApiScoreOperacionesModelInput body);

        Task<ApiScoreOperacionesRegistracionModelOutput> RegistracionAsync(ApiScoreOperacionesRegistracionModelInput body);

        Task<ApiScoreOperacionesInicioSesionModelOutput> InicioSesionAsync(ApiScoreOperacionesInicioSesionModelInput body);
    }
}
