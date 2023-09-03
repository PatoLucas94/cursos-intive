using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.TyCService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface ITyCService
    {
        Task<IResponse<VigenteModelOutput>> ObtenerVigenteAsync(IRequestBody<string> vigenteModel);

        Task<VigenteModelOutput> ObtenerVigenteAsync();

        Task<IResponse<AceptadosModelOutput>> ObtenerAceptadosByPersonIdAsync(IRequestBody<string> personId);

        Task<IResponse<AceptadosModelOutput>> ObtenerAceptadosAsync(
            IRequestBody<(string NumeroDocumento, string Usuario)> credenciales
        );

        Task<ApiTyCAceptacionModelOutput> AceptarUsuarioEncriptadoAsync(
            (string NumeroDocumento, string UsuarioEncriptado) credenciales
        );
    }
}
