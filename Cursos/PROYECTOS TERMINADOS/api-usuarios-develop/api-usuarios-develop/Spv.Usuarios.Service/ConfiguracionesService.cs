using System.Threading.Tasks;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.ConfiguracionesService.Input;
using Spv.Usuarios.Common.Dtos.ConfiguracionesService.Output;
using Spv.Usuarios.Common.LogEvents;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Domain.Exceptions;
using Spv.Usuarios.Domain.Services;
using Spv.Usuarios.Service.Interface;

namespace Spv.Usuarios.Service
{
    public class ConfiguracionesService : IConfiguracionesService
    {
        private int? _diasParaForzarCambioDeClave;
        private int? _cantidadDeIntentosDeLogin;
        private int? _cantidadDeHistorialDeCambiosDeClave;
        private readonly IConfiguracionRepository _configuracionRepository;

        public ConfiguracionesService(IConfiguracionRepository configuracionRepository)
        {
            _configuracionRepository = configuracionRepository;
        }

        public async Task<int> ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync()
        {
            return _diasParaForzarCambioDeClave ??= await CantidadDiasParaForzarCambioDeClaveAsync(_configuracionRepository);
        }

        public async Task<int> ObtenerConfiguracionCantidadDeIntentosDeLoginAsync()
        {
            return _cantidadDeIntentosDeLogin ??= await CantidadDeIntentosDeLoginAsync(_configuracionRepository);
        }

        public async Task<int> ObtenerConfiguracionCantidadDeHistorialDeCambiosDeClaveAsync()
        {
            return _cantidadDeHistorialDeCambiosDeClave ??= await CantidadDeHistorialDeCambiosDeClaveAsync(_configuracionRepository);
        }

        public async Task<bool> ObtenerConfiguracionRegistracionNuevoModeloEstaHabilitadaAsync()
        {
            return await RegistracionNuevoModeloHabilitadaAsync(_configuracionRepository);
        }
        private static async Task<bool> RegistracionNuevoModeloHabilitadaAsync(IConfiguracionRepository configuracionRepository)
        {
            var configuration = await configuracionRepository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeDigitalCredentials,
                AppConstants.RegistracionNuevoModeloHabilitado);

            if (!bool.TryParse(configuration?.Value, out var habilitado))
            {
                throw new BusinessException(UsuarioServiceEvents.ExceptionAlObtenerConfiguracionRegistracionNuevoModeloHabilitado);
            }

            return habilitado;
        }

        public async Task<IResponse<TerminosYCondicionesHabilitadoModelOutput>> ObtenerConfiguracionTerminosYCondicionesHabilitadoAsync(
             IRequestBody<TerminosYCondicionesHabilitadoModelInput> TerminosYCondicionesInput)
        {
            var TYCHabilitado = new TerminosYCondicionesHabilitadoModelOutput();
            TYCHabilitado.Habilitado =  await TerminosYCondicionesHabilitadoAsync(_configuracionRepository);
            return Responses.Ok(TYCHabilitado);
        }

        private static async Task<bool> TerminosYCondicionesHabilitadoAsync(IConfiguracionRepository configuracionRepository)
        {
            var configuration = await configuracionRepository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeDigitalCredentials,
                AppConstants.TerminosYCondicionesHabilitado);

            if (!bool.TryParse(configuration?.Value, out var habilitado))
            {
                return false;
            }

            return habilitado;
        }

        public async Task<IResponse<LoginHabilitadoModelOutput>> ObtenerConfiguracionLoginHabilitadoAsync(
            IRequestBody<LoginHabilitadoModelInput> LoginInput)
        {
            var LoginHabilitado = new LoginHabilitadoModelOutput();
            LoginHabilitado.Habilitado = await ConfiguracionesService.LoginHabilitado(_configuracionRepository);
            return Responses.Ok(LoginHabilitado); 
        }  

        private static async Task<string> LoginHabilitado(IConfiguracionRepository configuracionRepository)
        {
            var configuration = await configuracionRepository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeDigitalCredentials,
                AppConstants.LogInDisabled);

            if (configuration == null || string.IsNullOrEmpty(configuration.Value))
            {
                return string.Empty;
            }

            return configuration.Value;
        }

        public async Task<IResponse<LoginMessageModelOutput>> ObtenerConfiguracionLoginDefaultMenssageAsync(
            IRequestBody<LoginMessageModelInput> LoginInput)
        {
            var LoginMessage = new LoginMessageModelOutput();  
            LoginMessage.Mensaje = await LoginDefaultMenssage(_configuracionRepository);
            return Responses.Ok(LoginMessage);
        }

        private static async Task<string> LoginDefaultMenssage(IConfiguracionRepository configuracionRepository)
        {
            var configuration = await configuracionRepository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeDigitalCredentials,
                AppConstants.LogInDefaultDisabledMessage);

            if(configuration == null)
            {
                return string.Empty;
            }
            return configuration.Value;
        }

        public async Task<IResponse<LoginMessageModelOutput>> ObtenerConfiguracionLoginMenssageAsync(
            IRequestBody<LoginMessageModelInput> LoginInput)
        {
            var LoginMessage = new LoginMessageModelOutput();
            LoginMessage.Mensaje = await LoginMenssage(_configuracionRepository);
            return Responses.Ok(LoginMessage);
        }

        private static async Task<string> LoginMenssage(IConfiguracionRepository configuracionRepository)
        {
            var configuration = await configuracionRepository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeDigitalCredentials,
                AppConstants.LogInDisabledMessage);

            if (configuration == null)
            {
                return string.Empty;
            }
            return configuration.Value;
        }

        private static async Task<int> CantidadDeIntentosDeLoginAsync(IConfiguracionRepository configuracionRepository)
        {
            var configuration = await configuracionRepository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeUsers,
                AppConstants.CantidadDeIntentosDeLoginKey);

            if (!int.TryParse(configuration?.Value, out var cantidadDeIntentosDeLogin) && cantidadDeIntentosDeLogin <= 0)
            {
                throw new BusinessException(UsuarioServiceEvents.ExceptionAlObtenerConfiguracionCantidadDeIntentosDeLogin);
            }

            return cantidadDeIntentosDeLogin;
        }

        private static async Task<int> CantidadDeHistorialDeCambiosDeClaveAsync(IConfiguracionRepository configuracionRepository)
        {
            var configuration = await configuracionRepository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeUsers,
                AppConstants.CantidadDeHistorialDeCambiosDeClave);

            if (!int.TryParse(configuration?.Value, out var cantidadDeHistorialDeCambiosDeClave) && cantidadDeHistorialDeCambiosDeClave <= 0)
            {
                throw new BusinessException(UsuarioServiceEvents.ExceptionAlObtenerConfiguracionDeHistorialDeCambiosDeClave);
            }

            return cantidadDeHistorialDeCambiosDeClave;
        }

        private static async Task<int> CantidadDiasParaForzarCambioDeClaveAsync(IConfiguracionRepository configuracionRepository)
        {
            var configuration = await configuracionRepository.ObtenerConfiguracion(
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
