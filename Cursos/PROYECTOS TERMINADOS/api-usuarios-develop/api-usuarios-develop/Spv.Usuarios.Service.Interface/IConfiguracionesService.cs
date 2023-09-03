using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.ConfiguracionesService.Input;
using Spv.Usuarios.Common.Dtos.ConfiguracionesService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Service.Interface
{
    public interface IConfiguracionesService
    {
        Task<int> ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync();
        
        Task<int> ObtenerConfiguracionCantidadDeIntentosDeLoginAsync();

        Task<int> ObtenerConfiguracionCantidadDeHistorialDeCambiosDeClaveAsync();

        Task<bool> ObtenerConfiguracionRegistracionNuevoModeloEstaHabilitadaAsync();

        Task<IResponse<TerminosYCondicionesHabilitadoModelOutput>> ObtenerConfiguracionTerminosYCondicionesHabilitadoAsync(
            IRequestBody<TerminosYCondicionesHabilitadoModelInput> TerminosYCondicionesInput);

        Task<IResponse<LoginHabilitadoModelOutput>> ObtenerConfiguracionLoginHabilitadoAsync(
            IRequestBody<LoginHabilitadoModelInput> LoginInput);

        Task<IResponse<LoginMessageModelOutput>> ObtenerConfiguracionLoginDefaultMenssageAsync(
            IRequestBody<LoginMessageModelInput> LoginInput);

        Task<IResponse<LoginMessageModelOutput>> ObtenerConfiguracionLoginMenssageAsync(
            IRequestBody<LoginMessageModelInput> LoginInput);

    }
}
