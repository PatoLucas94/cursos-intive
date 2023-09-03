using System.Net.Http;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces
{
    public interface IApiUsuariosRepository
    {
        Task<ApiUsuariosPerfilModelOutput> ObtenerPerfilAsync(string nombreUsuario);
        Task<HttpResponseMessage> ValidarExistenciaAsync(ApiUsuariosValidacionExistenciaHbiModelInput body);
    }
}
