using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Client.ScoreOperaciones.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface IScoreOperacionesService
    {
        Task<IResponse<ApiScoreOperacionesInicioSesionModelOutput>> InicioSesionAsync(string numeroDocumento, string nombreUsuario, string deviceId);
    }
}
