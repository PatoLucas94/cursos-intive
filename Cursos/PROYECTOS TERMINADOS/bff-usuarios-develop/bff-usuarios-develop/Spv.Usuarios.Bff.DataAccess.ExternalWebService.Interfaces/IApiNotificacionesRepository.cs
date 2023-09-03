using System.Net.Http;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces
{
    public interface IApiNotificacionesRepository
    {
        Task<HttpResponseMessage> CrearYEnviarTokenAsync(ApiNotificacionesCrearYEnviarTokenModelInput body);
        Task<HttpResponseMessage> ValidarTokenAsync(ApiNotificacionesValidarTokenModelInput body);
        Task<HttpResponseMessage> EnviarEmailAsync(ApiNotificacionesEnviarEmailModelInput body);
    }
}