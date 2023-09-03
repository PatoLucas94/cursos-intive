using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using System.Net.Http;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.ClaveCliente.Input;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces
{
    public interface IApiUsuariosRepositoryV2
    {
        Task<HttpResponseMessage> ValidarClaveCanalesAsync(ApiUsuariosValidacionClaveCanalesModelInput body);

        Task<HttpResponseMessage> InhabilitarClaveCanalesAsync(ApiUsuariosInhabilitacionClaveCanalesModelInput body);

        Task<HttpResponseMessage> RegistrarUsuarioV2Async(ApiUsuariosRegistracionV2ModelInput body);

        Task<HttpResponseMessage> ValidarExistenciaAsync(ApiUsuariosValidacionExistenciaModelInput body);

        Task<HttpResponseMessage> CambiarCredencialesAsync(ApiUsuariosCambioDeCredencialesModelInput body);

        Task<HttpResponseMessage> MigrarUsuarioAsync(ApiUsuariosMigracionModelInput body);

        Task<HttpResponseMessage> ObtenerPerfilAsync(long IdPersona);

        Task<HttpResponseMessage> ActualizarPersonIdAsync(ApiUsuariosActualizarPersonIdModelInput body);

        Task<HttpResponseMessage> CambiarClaveAsync(ApiUsuariosCambioDeClaveModelInput body);

        Task<HttpResponseMessage> AutenticacionAsync(ApiUsuariosAutenticacionV2ModelInput body);

        ValueTask<ApiUsuariosObtenerUsuariosModelOutputV2> ObtenerUsuarioAsync((string NumeroDocumento, string Usuario) info);

        Task<ApiUsuariosTycHabilitadoModelOutput> ObtenerTYCHabilitadoAsync();

        Task<ApiUsuariosLoginHabilitadoModelOutput> ObtenerLoginHabilitadoAsync();

        Task<ApiUsuariosLoginMessageModelOutput> ObtenerMensajeDefaultLoginDeshabilitadoAsync();

        Task<ApiUsuariosLoginMessageModelOutput> ObtenerMensajeLoginDeshabilitadoAsync();

        Task<HttpResponseMessage> ObtenerImagenesLoginAsync();

        Task<HttpResponseMessage> ObtenerEstadoClaveCanalesAsync(ApiUsuariosObtenerEstadoClaveCanalesModelInput body);
    }
}
