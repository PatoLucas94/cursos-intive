using System.Threading.Tasks;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.LogEvents;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Exceptions;
using Spv.Usuarios.Service.Interface;

namespace Spv.Usuarios.Service
{
    public class ConfiguracionesV2Service : IConfiguracionesV2Service
    {
        private int? _diasParaForzarCambioDeClaveV2;
        private int? _cantidadDeIntentosDeLoginV2;
        private int? _cantidadDeIntentosDeClaveDeCanales;
        private int? _cantidadDeHistorialDeCambiosDeClaveV2;
        private int? _cantidadDeHistorialDeCambiosDeNombreUsuario;
        private readonly IConfiguracionV2Repository _configuracionV2Repository;

        public ConfiguracionesV2Service(IConfiguracionV2Repository configuracionRepository)
        {
            _configuracionV2Repository = configuracionRepository;
        }

        public async Task<int> ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync()
        {
            return _diasParaForzarCambioDeClaveV2 ??= await CantidadDiasParaForzarCambioDeClaveAsync(_configuracionV2Repository);
        }

        public async Task<int> ObtenerConfiguracionCantidadDeIntentosDeLoginAsync()
        {
            return _cantidadDeIntentosDeLoginV2 ??= await CantidadDeIntentosDeLoginAsync(_configuracionV2Repository);
        }

        public async Task<int> ObtenerConfiguracionCantidadDeHistorialDeCambiosDeClaveAsync()
        {
            return _cantidadDeHistorialDeCambiosDeClaveV2 ??= await CantidadDeHistorialDeCambiosDeClaveAsync(_configuracionV2Repository);
        }

        public async Task<int?> ObtenerConfiguracionCantidadDeIntentosDeClaveDeCanalesAsync()
        {
            return _cantidadDeIntentosDeClaveDeCanales ??= await CantidadDeIntentosDeClaveDeCanalesAsync(_configuracionV2Repository);
        }

        public async Task<int> ObtenerConfiguracionCantidadDeHistorialDeCambiosDeNombreUsuarioAsync()
        {
            return _cantidadDeHistorialDeCambiosDeNombreUsuario ??= await CantidadDeHistorialDeCambiosDeNombreUsuarioAsync(_configuracionV2Repository);
        }

        private static async Task<int> CantidadDeIntentosDeLoginAsync(IConfiguracionV2Repository configuracionV2Repository)
        {
            var configuration = await configuracionV2Repository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeUsers,
                AppConstants.CantidadDeIntentosDeLoginKey);

            if (!int.TryParse(configuration?.Value, out var cantidadDeIntentosDeLogin) && cantidadDeIntentosDeLogin <= 0)
            {
                throw new BusinessException(UsuarioServiceEvents.ExceptionAlObtenerConfiguracionCantidadDeIntentosDeLogin);
            }

            return cantidadDeIntentosDeLogin;
        }

        private static async Task<int> CantidadDeIntentosDeClaveDeCanalesAsync(IConfiguracionV2Repository configuracionV2Repository)
        {
            var configuration = await configuracionV2Repository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeChannelsKey,
                AppConstants.CantidadDeIntentosDeClaveDeCanalesKey);

            if (!int.TryParse(configuration?.Value, out var cantidadDeIntentosDeClaveDeCanales) && cantidadDeIntentosDeClaveDeCanales <= 0)
            {
                throw new BusinessException(ClaveCanalesServiceEvents.ExceptionAlObtenerConfiguracionCantidadDeIntentosDeClaveDeCanales);
            }

            return cantidadDeIntentosDeClaveDeCanales;
        }

        private static async Task<int> CantidadDeHistorialDeCambiosDeClaveAsync(IConfiguracionV2Repository configuracionV2Repository)
        {
            var configuration = await configuracionV2Repository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeUsers,
                AppConstants.CantidadDeHistorialDeCambiosDeClave);

            if (!int.TryParse(configuration?.Value, out var cantidadDeHistorialDeCambiosDeClave) && cantidadDeHistorialDeCambiosDeClave <= 0)
            {
                throw new BusinessException(UsuarioServiceEvents.ExceptionAlObtenerConfiguracionDeHistorialDeCambiosDeClave);
            }

            return cantidadDeHistorialDeCambiosDeClave;
        }

        private static async Task<int> CantidadDeHistorialDeCambiosDeNombreUsuarioAsync(IConfiguracionV2Repository configuracionV2Repository)
        {
            var configuration = await configuracionV2Repository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeUsers,
                AppConstants.CantidadDeHistorialDeCambiosDeNombreUsuario);

            if (!int.TryParse(configuration?.Value, out var cantidadDeHistorialDeCambiosDeNombreUsuario) &&
                cantidadDeHistorialDeCambiosDeNombreUsuario <= 0)
            {
                throw new BusinessException(UsuarioServiceEvents.ExceptionAlObtenerConfiguracionDeHistorialDeCambiosDeNombreUsuario);
            }

            return cantidadDeHistorialDeCambiosDeNombreUsuario;
        }

        private static async Task<int> CantidadDiasParaForzarCambioDeClaveAsync(IConfiguracionV2Repository configuracionV2Repository)
        {
            var configuration = await configuracionV2Repository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeUsers,
                AppConstants.DiasParaForzarCambioDeClaveKey);

            if (!int.TryParse(configuration?.Value, out var cantidadDiasParaForzarCambioDeClave) &&
                cantidadDiasParaForzarCambioDeClave <= 0)
            {
                throw new BusinessException(UsuarioServiceEvents.ExceptionAlObtenerConfiguracionDeCantidadDiasParaForzarCambioDeClave);
            }

            return cantidadDiasParaForzarCambioDeClave;
        }
    }
}
