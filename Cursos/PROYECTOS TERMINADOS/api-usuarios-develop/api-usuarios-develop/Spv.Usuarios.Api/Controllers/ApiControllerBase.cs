using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Polly.Timeout;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.Controllers
{
    /// <response code="400">BadRequest. El servidor no puede aceptar la solicitud, el cliente debe revisar los argumentos y valores de headers.</response>
    /// <response code="500">Internal Server Error. El servidor no puede procesar la solicitud debido a un problema interno.</response>
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetailModel))]
    [Produces("application/json")]
    public class ApiControllerBase<T> : ControllerBase
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Service
        /// </summary>
        public T Service { get; }

        /// <summary>
        /// Logger
        /// </summary>
        public ILogger<T> Logger { get; }

        /// <summary>
        /// ApiControllerBase
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        protected ApiControllerBase(T service, ILogger<T> logger)
        {
            Service = service;
            Logger = logger;
        }

        /// <summary>
        /// ApiControllerBase
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        protected ApiControllerBase(T service, ILogger<T> logger, IMapper mapper)
        {
            _mapper = mapper;
            Service = service;
            Logger = logger;
        }

        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="serviceOperation"></param>
        /// <param name="request"></param>
        /// <param name="mapper"></param>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TO"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <returns></returns>
        protected async Task<IActionResult> ExecuteAsync<TI, TO, TR>(
            Func<IRequestBody<TI>, Task<IResponse<TO>>> serviceOperation,
            IRequestBody<TI> request,
            Func<Task<IResponse<TO>>, Task<IResponse<TR>>> mapper
        )
        {
            (serviceOperation, request, mapper) = CheckArguments(serviceOperation, request, mapper);

            try
            {
                var stopwatch = Stopwatch.StartNew();
                var response = await mapper(serviceOperation(request));
                stopwatch.Stop();

                return ProcessActionResult<TR>(request, stopwatch, response);
            }
            catch (Exception e)
            {
                return ProcessException(request, e);
            }
        }

        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="serviceOperation"></param>
        /// <param name="request"></param>
        /// <param name="mapper"></param>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TO"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <returns></returns>
        protected async Task<IActionResult> ExecuteAsync<TI, TO, TR>(
            Func<IRequestBody<TI>, Task<IResponse<TO>>> serviceOperation,
            IRequestBody<TI> request,
            Func<Task<IResponse<TO>>, IMapper, Task<IResponse<TR>>> mapper
        )
        {
            (serviceOperation, request, mapper) = CheckArguments(serviceOperation, request, mapper);
            try
            {
                var stopwatch = Stopwatch.StartNew();
                var response = await mapper(serviceOperation(request), _mapper);
                stopwatch.Stop();

                return ProcessActionResult<TR>(request, stopwatch, response);
            }
            catch (Exception e)
            {
                return ProcessException(request, e);
            }
        }

        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="serviceOperation"></param>
        /// <param name="request"></param>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TO"></typeparam>
        /// <returns></returns>
        protected async Task<IActionResult> ExecuteAsync<TI, TO>(
            Func<IRequestBody<TI>, Task<IResponse<TO>>> serviceOperation,
            IRequestBody<TI> request
        )
        {
            (serviceOperation, request) = CheckArguments(serviceOperation, request);

            try
            {
                var stopwatch = Stopwatch.StartNew();
                var response = await serviceOperation(request);
                stopwatch.Stop();

                return ProcessActionResult(request, stopwatch, response);
            }
            catch (Exception e)
            {
                return ProcessException(request, e);
            }
        }

        private IActionResult ProcessException(IRequest request, Exception e)
        {
            switch (e)
            {
                case OutOfMemoryException _:
                case StackOverflowException _:
                    throw e;
                case TimeoutRejectedException _:
                case TimeoutException _:
                    return BuildClientError(request.XRequestId, StatusCodes.Status408RequestTimeout, e.Message);
                default:
                    return BuildServerError(request.XRequestId, StatusCodes.Status500InternalServerError, e);
            }
        }

        private IActionResult ProcessActionResult<TR>(IRequest request, Stopwatch stopwatch, IResponse<TR> response)
        {
            if (Logger.IsEnabled(LogLevel.Information))
            {
                var httpRequest = ControllerContext.HttpContext.Request;
                var operation = $"{httpRequest.Method} {httpRequest.Path.Value}{httpRequest.QueryString.Value}";

                Logger.LogInformation(
                    "RequestId:{RequestXRequestId}, Controller:{ActionDescriptorControllerName}, Operation:{Operation}, Execution time:{StopwatchElapsed}",
                    request.XRequestId,
                    ControllerContext.ActionDescriptor.ControllerName,
                    operation,
                    stopwatch.Elapsed
                );
            }

            foreach (var (key, value) in response.Headers)
                HttpContext.Response.Headers.Add(key, new StringValues(value));

            return response.Match(
                ok => StatusCode(ok.StatusCode, ok.Payload),
                clientError => BuildClientError(
                    request.XRequestId,
                    clientError.StatusCode,
                    clientError.Message,
                    clientError.ErrorType,
                    clientError.InternalCode
                ),
                serverError => BuildServerError(request.XRequestId, serverError.StatusCode, serverError.Exception)
            );
        }

        private static (TS, TR, TM) CheckArguments<TS, TR, TM>(TS serviceOperation, TR request, TM mapper)
        {
            CheckArguments(serviceOperation, request);

            if (mapper is null)
                throw new ArgumentNullException(nameof(mapper));

            return (serviceOperation, request, mapper);
        }

        private static (TS, TR) CheckArguments<TS, TR>(TS serviceOperation, TR request)
        {
            if (serviceOperation is null)
                throw new ArgumentNullException(nameof(serviceOperation));

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return (serviceOperation, request);
        }

        private static string JoinHeaderValues(StringValues values)
        {
            return string.Join(", ", values.AsEnumerable());
        }

        private static string GetHeadersValues(IHeaderDictionary headers, Func<string, bool> filter)
        {
            var headerValues = from header in headers
                where filter(header.Key)
                select $"{header.Key}:{JoinHeaderValues(header.Value)}";
            return string.Join(", ", headerValues);
        }

        private ObjectResult BuildServerError(string requestId, int statusCode, Exception e)
        {
            var origen = ControllerContext.ActionDescriptor.ControllerName;
            var request = ControllerContext.HttpContext.Request;
            var operation = $"{request.Method} {request.Path.Value}{request.QueryString.Value}";
            var headerValues = GetHeadersValues(request.Headers, HeaderNames.IsOneOf);
            var message = $"Error inesperado en la ejecución de '{operation}' headers:{headerValues}";
            Logger.LogError(e, message);

            var error = new ApiError(
                statusCode.ToString(),
                message,
                $"{origen}Controller",
                e.Message,
                requestId
            );

            var httpError = new ErrorDetailModel(
                ((HttpStatusCode)statusCode).ToString(),
                statusCode,
                ErrorTypeConstants.Technical,
                $"Error inesperado en la ejecución de {operation}",
                new List<ApiError> { error }
            );

            return StatusCode(statusCode, httpError);
        }

        private ObjectResult BuildClientError(
            string requestId,
            int statusCode,
            object data,
            string errorType = ErrorTypeConstants.Technical,
            string internalCode = ""
        )
        {
            var origen = ControllerContext.ActionDescriptor.ControllerName;
            var request = ControllerContext.HttpContext.Request;
            var operation = $"{request.Method} {request.Path.Value}{request.QueryString.Value}";
            var headerValues = GetHeadersValues(request.Headers, HeaderNames.IsOneOf);
            var message = $"Error en la ejecución de '{operation}' headers:{headerValues}";

            Logger.LogError("{Message} {Data}", message, data);

            var error = new ApiError(
                !string.IsNullOrWhiteSpace(internalCode) ? internalCode : statusCode.ToString(),
                message,
                $"{origen}Controller",
                $"{data}",
                requestId
            );

            var httpError = new ErrorDetailModel(
                ((HttpStatusCode)statusCode).ToString(),
                statusCode,
                errorType,
                $"Error en la ejecución de {operation}",
                new List<ApiError> { error }
            );

            return StatusCode(statusCode, httpError);
        }
    }
}