using System.Threading.Tasks;

namespace Spv.Usuarios.Service.Interface
{
    public interface IConfiguracionesV2Service
    {
        Task<int> ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync();

        Task<int> ObtenerConfiguracionCantidadDeIntentosDeLoginAsync();

        Task<int?> ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesAsync();

        Task<int> ObtenerConfiguracionCantidadDeHistorialDeCambiosDeClaveAsync();

        Task<int> ObtenerConfiguracionCantidadDeHistorialDeCambiosDeNombreUsuarioAsync();
    }
}
