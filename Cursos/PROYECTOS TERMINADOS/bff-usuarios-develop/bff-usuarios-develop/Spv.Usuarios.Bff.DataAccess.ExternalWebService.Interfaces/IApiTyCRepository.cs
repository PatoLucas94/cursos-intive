using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces
{
    public interface IApiTyCRepository
    {
        ValueTask<ApiTyCVigenteModelOutput> ObtenerVigenteAsync();

        ValueTask<ApiTyCAceptacionModelOutput> AceptarAsync((int IdPersona, string IdTerminosCondiciones) info);

        ValueTask<ApiTyCAceptadosModelOutput> ObtenerAceptadosByPersonIdAsync(string personId);

        ValueTask<ApiTyCAceptadosModelOutput> ObtenerAceptadosAsync((string NumeroDocumento, string Usuario) info);
    }
}
