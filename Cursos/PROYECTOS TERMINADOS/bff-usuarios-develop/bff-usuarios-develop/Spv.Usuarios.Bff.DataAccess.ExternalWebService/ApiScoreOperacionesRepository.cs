using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.ScoreOperaciones.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.ScoreOperaciones.Output;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService
{
    public class ApiScoreOperacionesRepository : IApiScoreOperacionesRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiScoreOperacionesHelper _apiScoreOperacionesHelper;
        private readonly ILogger<ApiScoreOperacionesRepository> _logger;

        public ApiScoreOperacionesRepository(
            IHttpClientFactory httpClientFactory,
            IApiScoreOperacionesHelper apiScoreOperacionesHelper,
            ILogger<ApiScoreOperacionesRepository> logger
        )
        {
            _httpClientFactory = httpClientFactory;
            _apiScoreOperacionesHelper = apiScoreOperacionesHelper;
            _logger = logger;
        }

        private string ObtenerPathConBase(string path)
        {
            return string.Concat(_apiScoreOperacionesHelper.BasePath(), path);
        }

        public async Task<ApiScoreOperacionesModelOutput> UpdateCredentialsAsync(ApiScoreOperacionesModelInput body)
        {
            try
            {
                var path = ObtenerPathConBase(string.Format(_apiScoreOperacionesHelper.UpdateCredentials()));

                var bodyPost = new UpdateCredentialsModel(
                    body.IdPersona,
                    body.CBU,
                    body.IdDispositivo,
                    body.AccionDelEvento,
                    body.Motivo,
                    body.TipoDeAccion,
                    body.NumeroReferencia,
                    body.ActualizarEntidad
                );

                var response = await PostApiScoreOperacionesAsync(
                    path,
                    bodyPost,
                    _apiScoreOperacionesHelper.ApiScoreOperacionesxCanal(),
                    _apiScoreOperacionesHelper.ApiScoreOperacionesxUsuario()
                );

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<ApiScoreOperacionesModelOutput>(
                    responseStream
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ScoreOperacionesEvents.ExceptionCallingUpdateCredentials, ex.Message, ex);
                return new ApiScoreOperacionesModelOutput();
            }
        }

        public async Task<ApiScoreOperacionesRegistracionModelOutput> RegistracionAsync(
            ApiScoreOperacionesRegistracionModelInput body
        )
        {
            try
            {
                var path = ObtenerPathConBase(string.Format(_apiScoreOperacionesHelper.RegistracionScore()));

                var bodyPost = new RegistracionModel(
                    body.IdPersona,
                    body.CBU,
                    body.IdDispositivo,
                    body.Motivo,
                    body.TipoRegistracion
                );

                var response = await PostApiScoreOperacionesAsync(
                    path,
                    bodyPost,
                    _apiScoreOperacionesHelper.ApiScoreOperacionesxCanal(),
                    _apiScoreOperacionesHelper.ApiScoreOperacionesxUsuario()
                );

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer
                    .DeserializeAsync<ApiScoreOperacionesRegistracionModelOutput>(responseStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ScoreOperacionesEvents.ExceptionCallingRegistracionAsync, ex.Message, ex);
                return new ApiScoreOperacionesRegistracionModelOutput();
            }
        }

        public async Task<ApiScoreOperacionesInicioSesionModelOutput> InicioSesionAsync(ApiScoreOperacionesInicioSesionModelInput body)
        {
            try
            {
                var path = ObtenerPathConBase(string.Format(_apiScoreOperacionesHelper.IniciarSesionScore()));

                var bodyPost = new InicioSesionModel(
                    body.IdPersona,
                    body.CBU,
                    body.IdDispositivo,
                    body.Motivo
                );

                var response = await PostApiScoreOperacionesAsync(
                    path,
                    bodyPost,
                    _apiScoreOperacionesHelper.ApiScoreOperacionesxCanal(),
                    _apiScoreOperacionesHelper.ApiScoreOperacionesxUsuario()
                );

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer
                    .DeserializeAsync<ApiScoreOperacionesInicioSesionModelOutput>(responseStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ScoreOperacionesEvents.ExceptionCallingInicioSesionAsync, ex.Message, ex);
                return new ApiScoreOperacionesInicioSesionModelOutput();
            }
        }

        private async Task<HttpResponseMessage> PostApiScoreOperacionesAsync(
            string path,
            object body,
            string canal,
            string usuario
        )
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

            return await RequestApiScoreOperacionesAsync(request, canal, usuario);
        }

        private async Task<HttpResponseMessage> RequestApiScoreOperacionesAsync(
            HttpRequestMessage request,
            string canal,
            string usuario
        )
        {
            #region Requerido para UTs

            if (!string.IsNullOrWhiteSpace(canal))
            {
                request.Headers.Add(HeaderNames.ChannelHeaderName, canal);
            }

            if (!string.IsNullOrWhiteSpace(usuario))
            {
                request.Headers.Add(HeaderNames.UserHeaderName, usuario);
            }

            #endregion

            var client = _httpClientFactory.CreateClient(ExternalServicesNames.ApiScoreOperaciones);

            return await client.SendAsync(request);
        }

        [ExcludeFromCodeCoverage]
        private class UpdateCredentialsModel
        {
            public string id_persona { get; }
            public string cbu { get; }
            public string id_dispositivo { get; }
            public string accion_del_evento { get; }
            public string motivo { get; }
            public string tipo_de_accion { get; }
            public string numero_referencia { get; }
            public string actualizar_entidad { get; }

            public UpdateCredentialsModel(
                string idPersona,
                string cbu,
                string idDispositivo,
                string accionDelEvento,
                string motivo,
                string tipoDeAccion,
                string numeroReferencia,
                string actualizarEntidad
            )
            {
                id_persona = idPersona;
                this.cbu = cbu;
                id_dispositivo = idDispositivo;
                accion_del_evento = accionDelEvento;
                this.motivo = motivo;
                tipo_de_accion = tipoDeAccion;
                numero_referencia = numeroReferencia;
                actualizar_entidad = actualizarEntidad;
            }
        }

        [ExcludeFromCodeCoverage]
        private class RegistracionModel
        {
            public string id_persona { get; }
            public string cbu { get; }
            public string id_dispositivo { get; }
            public string motivo { get; }
            public string tipo_de_registracion { get; }


            public RegistracionModel(
                string idPersona,
                string cbu,
                string idDispositivo,
                string motivo,
                string tipoDeRegistracion
            )
            {
                id_persona = idPersona;
                this.cbu = cbu;
                id_dispositivo = idDispositivo;
                this.motivo = motivo;
                tipo_de_registracion = tipoDeRegistracion;
            }
        }

        [ExcludeFromCodeCoverage]
        private class InicioSesionModel
        {
            public string id_persona { get; }
            public string cbu { get; }
            public string id_dispositivo { get; }
            public string motivo { get; }

            public InicioSesionModel(
                string idPersona,
                string cbu,
                string idDispositivo,
                string motivo
            )
            {
                id_persona = idPersona;
                this.cbu = cbu;
                id_dispositivo = idDispositivo;
                this.motivo = motivo;
            }
        }
    }
}
