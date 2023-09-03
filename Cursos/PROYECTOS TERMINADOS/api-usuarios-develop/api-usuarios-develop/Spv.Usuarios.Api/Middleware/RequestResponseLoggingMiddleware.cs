using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Newtonsoft.Json;
using Spv.Usuarios.Api.ViewModels.ClaveCanalesController.CommonClaveCanales.Input;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Common.Constants;

namespace Spv.Usuarios.Api.Middleware
{
    /// <summary>
    /// RequestResponseLoggingMiddleware
    /// </summary>
    public class RequestResponseLoggingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        /// <summary>
        /// RequestResponseLoggingMiddleware
        /// </summary>
        /// <param name="loggerFactory"></param>
        public RequestResponseLoggingMiddleware(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        /// <summary>
        /// InvokeAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await LogRequest(context);
            await LogResponse(context, next);
        }

        private async Task LogRequest(HttpContext context)
        {
            var request = context.Request;
            var requestTime = new DateTimeOffset();
            context.Request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            var body = ReadStreamInChunks(requestStream);

            if (!string.IsNullOrEmpty(body))
            {
                switch (request.Path.Value)
                {
                    case string autenticacion when request.Path.Value.Contains(ApiUris.Autenticacion):
                        var autenticacionModelRequest = JsonConvert.DeserializeObject<AutenticacionModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                        request.Host, request.Path, request.QueryString, autenticacionModelRequest.ToString(), requestTime);
                        break;
                    case string registracion when request.Path.Value.Contains(ApiUris.Registracion):
                        var registracionModelRequest = JsonConvert.DeserializeObject<RegistracionModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                            request.Host, request.Path, request.QueryString, registracionModelRequest.ToString(), requestTime);
                        break;
                    case string autenticacionV2 when request.Path.Value.Contains(ApiUris.AutenticacionV2):
                        var autenticacionModelRequestV2 = JsonConvert.DeserializeObject<AutenticacionModelRequestV2>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                            request.Host, request.Path, request.QueryString, autenticacionModelRequestV2.ToString(), requestTime);
                        break;
                    case string autenticacionDeClave when request.Path.Value.Contains(ApiUris.AutenticacionConClaveNumericaV2):
                        var autenticacionClaveModelRequest = JsonConvert.DeserializeObject<AutenticacionClaveNumericaModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                            request.Host, request.Path, request.QueryString, autenticacionClaveModelRequest.ToString(), requestTime);
                        break;
                    case string cambioDeClave when request.Path.Value.Contains(ApiUris.CambiarClaveUsuarioV2):
                        var cambioDeClaveModelRequest = JsonConvert.DeserializeObject<CambioDeClaveModelRequestV2>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                            request.Host, request
                            .Path, request.QueryString, cambioDeClaveModelRequest.ToString(), requestTime);
                        break;
                    case string cambioDeCredenciales when request.Path.Value.Contains(ApiUris.CambiarCredencialesUsuarioV2):
                        var cambioDeCredencialesModelRequest = JsonConvert.DeserializeObject<CambioDeCredencialesModelRequestV2>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                            request.Host, request.Path, request.QueryString, cambioDeCredencialesModelRequest.ToString(), requestTime);
                        break;
                    case string registracion when request.Path.Value.Contains(ApiUris.RegistracionV2):
                        var registracionModelRequestV2 = JsonConvert.DeserializeObject<RegistracionModelRequestV2>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                            request.Host, request.Path, request.QueryString, registracionModelRequestV2.ToString(), requestTime);
                        break;
                    case string migracion when request.Path.Value.Contains(ApiUris.MigracionV2):
                        var migracionModelRequest = JsonConvert.DeserializeObject<MigracionModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                            request.Host, request.Path, request.QueryString, migracionModelRequest.ToString(), requestTime);
                        break;
                    case string autenticacion when request.Path.Value.Contains(ApiUris.ClaveCanalesInhabilitacionV2):
                        var inhabilitacionModelRequest = JsonConvert.DeserializeObject<InhabilitacionModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                        request.Host, request.Path, request.QueryString, inhabilitacionModelRequest.ToString(), requestTime);
                        break;
                    case string autenticacion when request.Path.Value.Contains(ApiUris.ClaveCanalesValidacionV2):
                        var validacionModelRequest = JsonConvert.DeserializeObject<ValidacionModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                        request.Host, request.Path, request.QueryString, validacionModelRequest.ToString(), requestTime);
                        break;
                    case string autenticacion when request.Path.Value.Contains(ApiUris.AutenticacionKeycloakV2):
                        var autenticacionTokenModelRequest = JsonConvert.DeserializeObject<AutenticacionModelRequestV2>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                        request.Host, request.Path, request.QueryString, autenticacionTokenModelRequest.ToString(), requestTime);
                        break;
                    case string actualizarToken when request.Path.Value.Contains(ApiUris.ActualizarTokenKeycloakV2):
                        var actualizarTokenModelRequest = JsonConvert.DeserializeObject<string>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                        request.Host, request.Path, request.QueryString, actualizarTokenModelRequest.ToString(), requestTime);
                        break;
                    case string verificarToken when request.Path.Value.Contains(ApiUris.VerificarTokenKeycloakV2):
                        var verificarTokenModelRequest = JsonConvert.DeserializeObject<string>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                        request.Host, request.Path, request.QueryString, verificarTokenModelRequest.ToString(), requestTime);
                        break;
                    case string cerrarSesion when request.Path.Value.Contains(ApiUris.CerrarSesionKeycloakV2):
                        var crrarSesionModelRequest = JsonConvert.DeserializeObject<string>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                        request.Host, request.Path, request.QueryString, crrarSesionModelRequest.ToString(), requestTime);
                        break;
                    default:
                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}", request.Scheme,
                    request.Host, request.Path, request.QueryString, body, requestTime);
                        break;

                }
            }

                context.Request.Body.Position = 0;
        }

        /// <summary>
        /// LogResponse
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task LogResponse(HttpContext context, RequestDelegate next)
        {
            var responseTime = new DateTimeOffset();
            var request = context.Request;
            var response = context.Response;

            var originalBodyStream = context.Response.Body;

            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogDebug("Response: {Schema} {@Host} {@Path} {@QueryString} {StatusCode} {@Body} {@ResponseTime}",
                request.Scheme, request.Host, request.Path, request.QueryString, response.StatusCode,
                body, responseTime);

            await responseBody.CopyToAsync(originalBodyStream);
        }

        /// <summary>
        /// ReadStreamInChunks
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }
}