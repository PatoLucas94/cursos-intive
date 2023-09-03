using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Input;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input;

namespace Spv.Usuarios.Bff.Middleware
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
                    case { } when request.Path.Value.Contains(ApiUris.CambioDeClave):
                        var cambioDeClaveModelRequest = JsonConvert.DeserializeObject<CambioDeClaveModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}",
                            request.Scheme,
                            request.Host, request.Path, request.QueryString, cambioDeClaveModelRequest.ToString(),
                            requestTime);
                        break;

                    case { } when request.Path.Value.Contains(ApiUris.CambioDeCredenciales):
                        var cambioDeCredencialesModelRequest =
                            JsonConvert.DeserializeObject<CambioDeCredencialesModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}",
                            request.Scheme,
                            request.Host, request.Path, request.QueryString,
                            cambioDeCredencialesModelRequest.ToString(), requestTime);
                        break;

                    case { } when request.Path.Value.Contains(ApiUris.ValidacionClaveCanalesConIdPersona):
                        var validacionClaveCanalesIdPersonaModelRequest =
                            JsonConvert.DeserializeObject<ValidacionClaveCanalesIdPersonaModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}",
                            request.Scheme,
                            request.Host, request.Path, request.QueryString,
                            validacionClaveCanalesIdPersonaModelRequest.ToString(), requestTime);
                        break;

                    case { } when request.Path.Value.Contains(ApiUris.ValidacionClaveCanales):
                        var validacionClaveCanalesModelRequest =
                            JsonConvert.DeserializeObject<ValidacionClaveCanalesModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}",
                            request.Scheme,
                            request.Host, request.Path, request.QueryString,
                            validacionClaveCanalesModelRequest.ToString(), requestTime);
                        break;

                    case { } when request.Path.Value.Contains(ApiUris.ValidacionClaveSms):
                        var validacionClaveSmsModelRequest =
                            JsonConvert.DeserializeObject<ValidacionClaveSmsModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}",
                            request.Scheme,
                            request.Host, request.Path, request.QueryString, validacionClaveSmsModelRequest.ToString(),
                            requestTime);
                        break;

                    case { } when request.Path.Value.Contains(ApiUris.MigracionV2):
                        var migracionModelRequest = JsonConvert.DeserializeObject<MigracionModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}",
                            request.Scheme,
                            request.Host, request.Path, request.QueryString, migracionModelRequest.ToString(),
                            requestTime);
                        break;

                    case { } when request.Path.Value.Contains(ApiUris.RegistracionV2):
                        var registracionModelRequest = JsonConvert.DeserializeObject<RegistracionModelRequest>(body);

                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}",
                            request.Scheme,
                            request.Host, request.Path, request.QueryString, registracionModelRequest.ToString(),
                            requestTime);
                        break;

                    default:
                        _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}",
                            request.Scheme,
                            request.Host, request.Path, request.QueryString, body, requestTime);
                        break;
                }
            }
            else
            {
                _logger.LogDebug("Request: {Schema} {@Host} {@Path} {@QueryString} {Body} {@RequestTime}",
                    request.Scheme,
                    request.Host, request.Path, request.QueryString, body, requestTime);
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
