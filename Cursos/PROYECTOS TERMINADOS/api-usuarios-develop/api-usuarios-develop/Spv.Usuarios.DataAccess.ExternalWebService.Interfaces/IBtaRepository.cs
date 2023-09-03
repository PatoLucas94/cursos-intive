using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;

namespace Spv.Usuarios.DataAccess.ExternalWebService.Interfaces
{
    public interface IBtaRepository
    {
        Task<ObtenerPinModelOutput> ObtenerPinAsync(string numeroDocumento, int tipoDocumento, int paisDocumento, string token);

        Task<TokenBtaModelOutput> ObtenerToken();
    }
}
