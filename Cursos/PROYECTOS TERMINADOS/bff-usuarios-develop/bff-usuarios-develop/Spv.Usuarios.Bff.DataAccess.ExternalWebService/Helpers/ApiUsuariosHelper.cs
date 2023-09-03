using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    /// <summary>
    /// ApiUsuariosHelper: servicio para obtener configuraciones necesarias para interactuar con api-usuarios
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiUsuariosHelper : BaseHelper, IApiUsuariosHelper
    {
        private readonly ApiUsuariosConfigurationsOptions _apiUsuariosConfigurations;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiUsuariosConfigurationOptions"></param>
        /// <param name="apiServicesConfigurations"></param>
        public ApiUsuariosHelper(
            IOptions<ApiUsuariosConfigurationsOptions> apiUsuariosConfigurationOptions,
            IOptions<ApiServicesHeadersConfigurationOptions> apiServicesConfigurations,
            IHttpClientFactory httpClientFactory
        ) : base(apiServicesConfigurations)
        {
            _apiUsuariosConfigurations = apiUsuariosConfigurationOptions.Value;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Canal utilizado para consumir api-usuarios
        /// </summary>
        /// <returns></returns>
        public string ApiUsuariosXCanal()
        {
            return XCanal(ExternalServicesNames.ApiUsuarios);
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Usuario utilizado para consumir api-usuarios
        /// </summary>
        /// <returns></returns>
        public string ApiUsuariosXUsuario()
        {
            return XUsuario(ExternalServicesNames.ApiUsuarios);
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Aplicacion utilizado para consumir api-usuarios
        /// </summary>
        /// <returns></returns>
        public string ApiUsuariosXAplicacion()
        {
            return XAplicacion(ExternalServicesNames.ApiUsuarios);
        }

        /// <summary>
        /// Obtener valor correspondiente al header X-Aplicacion utilizado para consumir api-usuarios
        /// </summary>
        /// <returns></returns>
        public string ApiUsuariosXGateway()
        {
            return XGateway(ExternalServicesNames.ApiUsuarios);
        }

        /// <summary>
        /// Base Path del endpoint api-usuarios
        /// </summary>
        /// <returns></returns>
        public string BasePath()
        {
            return _apiUsuariosConfigurations.BasePath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.BasePath),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Path del endpoint para obtener el identificador de la persona
        /// </summary>
        /// <returns></returns>
        public string PerfilPath()
        {
            return _apiUsuariosConfigurations.PerfilPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.PerfilPath),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Path del endpoint para obtener el perfil
        /// </summary>
        /// <returns></returns>
        public string PerfilPathV2()
        {
            return _apiUsuariosConfigurations.PerfilPathV2 ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.PerfilPathV2),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Obtener path de endpoint que valida clave de canales
        /// </summary>
        /// <returns></returns>
        public string ValidarClaveCanalesPath()
        {
            return _apiUsuariosConfigurations.ValidarClaveCanalesPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.ValidarClaveCanalesPath),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Obtener path de endpoint que inhabilita clave de canales
        /// </summary>
        /// <returns></returns>
        public string InhabilitarClaveCanalesPath()
        {
            return _apiUsuariosConfigurations.InhabilitarClaveCanalesPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.InhabilitarClaveCanalesPath),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Obtener path de endpoint que registra un usuario en el nuevo modelo
        /// </summary>
        /// <returns></returns>
        public string RegistrarUsuarioPath()
        {
            return _apiUsuariosConfigurations.RegistrarUsuarioPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.RegistrarUsuarioPath),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Obtener path de endpoint que verifica si un usuario posee credenciales en el sistema
        /// </summary>
        /// <returns></returns>
        public string ValidarExistenciaPath()
        {
            return _apiUsuariosConfigurations.ValidarExistenciaPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.ValidarExistenciaPath),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Obtener path de endpoint que verifica si un usuario (username) existe en el sistema HBI Legacy
        /// </summary>
        /// <returns></returns>
        public string ValidarExistenciaHbiPath()
        {
            return _apiUsuariosConfigurations.ValidarExistenciaHbiPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.ValidarExistenciaHbiPath),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Obtener path de endpoint que realiza el cambio de credenciales un usuario
        /// </summary>
        /// <returns></returns>
        public string CambiarCredencialesPath()
        {
            return _apiUsuariosConfigurations.CambiarCredencialesPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.CambiarCredencialesPath),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Obtener path de endpoint que realiza el cambio de clave un usuario
        /// </summary>
        /// <returns></returns>
        public string CambiarClavePath()
        {
            return _apiUsuariosConfigurations.CambiarClavePath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.CambiarClavePath),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Obtener path de endpoint que migra un usuario al nuevo modelo
        /// </summary>
        /// <returns></returns>
        public string MigrarUsuarioPath()
        {
            return _apiUsuariosConfigurations.MigrarUsuarioPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.MigrarUsuarioPath),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Obtener path de endpoint que Actualiza el PersonId
        /// </summary>
        /// <returns></returns>
        public string ActualizarPersonIdPath()
        {
            return _apiUsuariosConfigurations.ActualizarPersonIdPath ?? ThrowNewExceptionPorFaltaDeKey(
                nameof(_apiUsuariosConfigurations.ActualizarPersonIdPath),
                ExternalServicesNames.ApiUsuarios
            );
        }

        /// <summary>
        /// Obtener path de endpoint para obtener perfil de usuario 
        /// </summary>
        /// <returns></returns>
        public string ObtenerUsuarioPath() => _apiUsuariosConfigurations.ObtenerUsuarioPath ??
                                              ThrowNewExceptionPorFaltaDeKey(
                                                  nameof(_apiUsuariosConfigurations.ObtenerUsuarioPath),
                                                  ExternalServicesNames.ApiUsuarios
                                              );

        /// <summary>
        /// Obtener path de endpoint de autenticación
        /// </summary>
        /// <returns></returns>
        public string AutenticacionPath() => _apiUsuariosConfigurations.AutenticacionPath ??
                                             ThrowNewExceptionPorFaltaDeKey(
                                                 nameof(_apiUsuariosConfigurations.AutenticacionPath),
                                                 ExternalServicesNames.ApiUsuarios
                                             );

        /// <summary>
        /// Obtener path de endpoint de ImagenesLogin
        /// </summary>
        /// <returns></returns>
        public string ImagenesLoginPath() => _apiUsuariosConfigurations.ObtenerImagenesLoginPath ??
                                             ThrowNewExceptionPorFaltaDeKey(
                                                 nameof(_apiUsuariosConfigurations.ObtenerImagenesLoginPath),
                                                 ExternalServicesNames.ApiUsuarios
                                             );

        /// <summary>
        /// Obtener path de endpoint de Estado clave canales
        /// </summary>
        /// <returns></returns>
        public string ObtenerEstadoClaveCanalesPath() => _apiUsuariosConfigurations.ObtenerEstadoClaveCanalesPath ??
                                             ThrowNewExceptionPorFaltaDeKey(
                                                 nameof(_apiUsuariosConfigurations.ObtenerEstadoClaveCanalesPath),
                                                 ExternalServicesNames.ApiUsuarios
                                             );

        public async Task<HttpResponseMessage> PostRequestApiUsuariosAsync(
            string path,
            object body,
            string canal = null,
            string usuario = null,
            string aplicacion = null,
            string gateway = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);

            if (body != null)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body, new JsonSerializerOptions()),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json
                );
            }

            #region Requerido para UTs

            if (!string.IsNullOrWhiteSpace(canal))
            {
                request.Headers.Add(HeaderNames.ChannelHeaderName, canal);
            }

            if (!string.IsNullOrWhiteSpace(usuario))
            {
                request.Headers.Add(HeaderNames.UserHeaderName, usuario);
            }

            if (!string.IsNullOrWhiteSpace(aplicacion))
            {
                request.Headers.Add(HeaderNames.ApplicationHeaderName, aplicacion);
            }

            if (!string.IsNullOrWhiteSpace(gateway))
            {
                request.Headers.Add(HeaderNames.GateWayHeaderName, gateway);
            }

            #endregion

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiUsuarios);

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetRequestApiUsuarioAsync(
            string path,
            string canal,
            string usuario,
            string aplicacion)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            #region Requerido para UTs

            if (!string.IsNullOrWhiteSpace(canal))
            {
                request.Headers.Add(HeaderNames.ChannelHeaderName, canal);
            }

            if (!string.IsNullOrWhiteSpace(usuario))
            {
                request.Headers.Add(HeaderNames.UserHeaderName, usuario);
            }

            if (!string.IsNullOrWhiteSpace(aplicacion))
            {
                request.Headers.Add(HeaderNames.ApplicationHeaderName, aplicacion);
            }

            #endregion

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiUsuarios);

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PatchRequestApiUsuariosAsync(
            string path,
            object body,
            string canal = null,
            string usuario = null,
            string aplicacion = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, path);

            if (body != null)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body, new JsonSerializerOptions()),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json
                );
            }

            #region Requerido para UTs

            if (!string.IsNullOrWhiteSpace(canal))
            {
                request.Headers.Add(HeaderNames.ChannelHeaderName, canal);
            }

            if (!string.IsNullOrWhiteSpace(usuario))
            {
                request.Headers.Add(HeaderNames.UserHeaderName, usuario);
            }

            if (!string.IsNullOrWhiteSpace(aplicacion))
            {
                request.Headers.Add(HeaderNames.ApplicationHeaderName, aplicacion);
            }

            #endregion

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiUsuarios);

            return await client.SendAsync(request);
        }

        public DateTimeOffset ExpirationCache() => DateTimeOffset.Now.AddMinutes(
            _apiUsuariosConfigurations.CacheExpiracionTyCMinutos
        );

        public DateTimeOffset ExpirationCacheImagenesLogin() => DateTimeOffset.Now.AddMinutes(
            _apiUsuariosConfigurations.CacheExpiracionImagenesLoginMinutos
        );
    }
}
