using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Client.ClaveCliente.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Utils;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService
{
    public class ApiUsuariosRepositoryV2 : IApiUsuariosRepositoryV2
    {
        private readonly IApiUsuariosHelper _apiUsuariosHelper;
        private readonly IRhSsoHelper _rhSsoHelper;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguracionHelper _configuracionHelper;

        public ApiUsuariosRepositoryV2(
            IApiUsuariosHelper apiUsuariosHelper,
            IMemoryCache memoryCache,
            IRhSsoHelper rhSsoHelper,
            IConfiguracionHelper configuracionHelper
        )
        {
            _apiUsuariosHelper = apiUsuariosHelper;
            _memoryCache = memoryCache;
            _rhSsoHelper = rhSsoHelper;
            _configuracionHelper = configuracionHelper;
        }

        public async Task<HttpResponseMessage> ValidarClaveCanalesAsync(
            ApiUsuariosValidacionClaveCanalesModelInput body
        )
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.ValidarClaveCanalesPath());

            return await _apiUsuariosHelper.PostRequestApiUsuariosAsync(
                path,
                body,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );
        }

        public async Task<HttpResponseMessage> InhabilitarClaveCanalesAsync(
            ApiUsuariosInhabilitacionClaveCanalesModelInput body
        )
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.InhabilitarClaveCanalesPath());

            return await _apiUsuariosHelper.PostRequestApiUsuariosAsync(
                path,
                body,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );
        }

        public async Task<HttpResponseMessage> RegistrarUsuarioV2Async(ApiUsuariosRegistracionV2ModelInput body)
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.RegistrarUsuarioPath());

            return await _apiUsuariosHelper.PostRequestApiUsuariosAsync(
                path,
                body,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );
        }

        public async Task<HttpResponseMessage> ValidarExistenciaAsync(ApiUsuariosValidacionExistenciaModelInput body)
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.ValidarExistenciaPath());

            return await _apiUsuariosHelper.PostRequestApiUsuariosAsync(
                path,
                body,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );
        }

        public async Task<HttpResponseMessage> CambiarCredencialesAsync(ApiUsuariosCambioDeCredencialesModelInput body)
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.CambiarCredencialesPath());

            return await _apiUsuariosHelper.PostRequestApiUsuariosAsync(
                path,
                body,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion(),
                _apiUsuariosHelper.ApiUsuariosXGateway()
            );
        }

        public async Task<HttpResponseMessage> MigrarUsuarioAsync(ApiUsuariosMigracionModelInput body)
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.MigrarUsuarioPath());

            return await _apiUsuariosHelper.PostRequestApiUsuariosAsync(
                path,
                body,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );
        }

        private string ObtenerPathConBase(string path)
        {
            return string.Concat(_apiUsuariosHelper.BasePath(), path);
        }

        public async Task<HttpResponseMessage> ObtenerPerfilAsync(long IdPersona)
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.PerfilPathV2());

            path = path.Replace("{0}", IdPersona.ToString());

            return await _apiUsuariosHelper.GetRequestApiUsuarioAsync(
                path,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );
        }

        public async ValueTask<ApiUsuariosObtenerUsuariosModelOutputV2> ObtenerUsuarioAsync(
            (string NumeroDocumento, string Usuario) info
        )
        {
            var cacheKey = Cache.Usuario.ObtenerUsuario(info.NumeroDocumento, info.Usuario);

            _memoryCache.TryGetValue(cacheKey, out ApiUsuariosObtenerUsuariosModelOutputV2 obtenerUsuario);

            if (obtenerUsuario != null)
                return obtenerUsuario;

            var path = ObtenerPathConBase(_apiUsuariosHelper.ObtenerUsuarioPath())
                .Replace("{0}", info.NumeroDocumento)
                .Replace("{1}", info.Usuario);

            var response = await _apiUsuariosHelper.GetRequestApiUsuarioAsync(
                path,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ApiUsuariosObtenerUsuariosModelOutputV2>(json);

            if (model is null)
                return null;

            _memoryCache.Set(cacheKey, model, _apiUsuariosHelper.ExpirationCache());

            return model;
        }

        public async Task<HttpResponseMessage> ActualizarPersonIdAsync(ApiUsuariosActualizarPersonIdModelInput body)
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.ActualizarPersonIdPath());

            return await _apiUsuariosHelper.PatchRequestApiUsuariosAsync(
                path,
                body,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );
        }

        public async Task<HttpResponseMessage> CambiarClaveAsync(ApiUsuariosCambioDeClaveModelInput body)
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.CambiarClavePath());

            return await _apiUsuariosHelper.PostRequestApiUsuariosAsync(
                path,
                body,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion(),
                _apiUsuariosHelper.ApiUsuariosXGateway()
            );
        }

        public async Task<HttpResponseMessage> AutenticacionAsync(ApiUsuariosAutenticacionV2ModelInput body) =>
            await _apiUsuariosHelper.PostRequestApiUsuariosAsync(
                ObtenerPathConBase(_apiUsuariosHelper.AutenticacionPath()),
                body,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _rhSsoHelper.XAplicacion()
            );

        public async Task<ApiUsuariosTycHabilitadoModelOutput> ObtenerTYCHabilitadoAsync()
        {
            var path = ObtenerPathConBase(_configuracionHelper.TYCHabilitadoPath());

            var response = await _configuracionHelper.GetRequestApiUsuarioAsync(
                path,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );

            var json = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ApiUsuariosTycHabilitadoModelOutput>(json);

            return model;
        }

        public async Task<ApiUsuariosLoginHabilitadoModelOutput> ObtenerLoginHabilitadoAsync()
        {
            var path = ObtenerPathConBase(_configuracionHelper.LoginHabilitadoPath());

            var response = await _configuracionHelper.GetRequestApiUsuarioAsync(
                path,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );

            var json = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ApiUsuariosLoginHabilitadoModelOutput>(json);

            return model;
        }

        public async Task<ApiUsuariosLoginMessageModelOutput> ObtenerMensajeDefaultLoginDeshabilitadoAsync()
        {
            var path = ObtenerPathConBase(_configuracionHelper.MensajeDefaultLoginDeshabilitadoPath());

            var response = await _configuracionHelper.GetRequestApiUsuarioAsync(
                path,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );

            var json = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ApiUsuariosLoginMessageModelOutput>(json);

            return model;
        }

        public async Task<ApiUsuariosLoginMessageModelOutput> ObtenerMensajeLoginDeshabilitadoAsync()
        {
            var path = ObtenerPathConBase(_configuracionHelper.MensajeLoginDeshabilitadoPath());

            var response = await _configuracionHelper.GetRequestApiUsuarioAsync(
                path,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );

            var json = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ApiUsuariosLoginMessageModelOutput>(json);

            return model;
        }

        public async Task<HttpResponseMessage> ObtenerImagenesLoginAsync()
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.ImagenesLoginPath());

            return await _configuracionHelper.GetRequestApiUsuarioAsync(
                path,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion()
            );
        }

        public async Task<HttpResponseMessage> ObtenerEstadoClaveCanalesAsync(
            ApiUsuariosObtenerEstadoClaveCanalesModelInput body
        )
        {
            var path = ObtenerPathConBase(_apiUsuariosHelper.ObtenerEstadoClaveCanalesPath());

            return await _apiUsuariosHelper.PostRequestApiUsuariosAsync(
                path,
                body,
                _apiUsuariosHelper.ApiUsuariosXCanal(),
                _apiUsuariosHelper.ApiUsuariosXUsuario(),
                _apiUsuariosHelper.ApiUsuariosXAplicacion(),
                _apiUsuariosHelper.ApiUsuariosXGateway()
            );
        }
    }
}
