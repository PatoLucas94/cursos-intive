using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Polly.Timeout;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Domain.Errors;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Exceptions;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <response code="400">BadRequest. El servidor no puede aceptar la solicitud, el cliente debe revisar los argumentos y valores de headers.</response>
    /// <response code="500">Internal Server Error. El servidor no puede procesar la solicitud debido a un problema interno.</response>
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetailModel))]
    [Produces("application/json")]
    [EnableCors("obi-frontend")]
    public class ApiControllerBase<T> : ControllerBase
    {
        /// <summary>
        /// Mapper
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        /// Service
        /// </summary>
        public T Service { get; }

        /// <summary>
        /// Logger
        /// </summary>
        private ILogger<T> Logger { get; }

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
        protected ApiControllerBase(T service, ILogger<T> logger, IMapper mapper) : this(service, logger)
        {
            Mapper = mapper;
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
                var response = await mapper(serviceOperation(request), Mapper);
                stopwatch.Stop();

                return ProcessActionResult<TR>(request, stopwatch, response);
            }
            catch (Exception e)
            {
                return ProcessException(request, e);
            }
        }

        private static (TS, TR, TM) CheckArguments<TS, TR, TM>(TS serviceOperation, TR request, TM mapper)
        {
            if (serviceOperation is null)
            {
                throw new ArgumentNullException(nameof(serviceOperation));
            }

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return (serviceOperation, request, mapper);
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
            var message = MessageConstants.ErrorInterno;
            Logger.LogError(e, message);

            var error = new ApiError(statusCode.ToString(),
                message,
                $"{origen}Controller",
                e.Message,
                requestId);

            var httpError = new ErrorDetailModel(((HttpStatusCode)statusCode).ToString(),
                statusCode,
                ErrorTypeConstants.Technical,
                MessageConstants.MensajeGenerico,
                new List<ApiError> { error });

            return StatusCode(statusCode, httpError);
        }

        private ObjectResult BuildClientError(
            string requestId,
            int statusCode,
            object data,
            string errorType = ErrorTypeConstants.Technical,
            string internalCode = "",
            List<InternalCodeAndDetailErrors> internalCodeAndDetailErrors = null
        )
        {
            var origen = ControllerContext.ActionDescriptor.ControllerName;
            var request = ControllerContext.HttpContext.Request;
            var operation = $"{request.Method} {request.Path.Value}{request.QueryString.Value}";
            var headerValues = GetHeadersValues(request.Headers, HeaderNames.IsOneOf);
            var message = $"Error en la ejecución de '{operation}' headers:{headerValues}";
            var errors = new List<ApiError>();

            Logger.LogError("{Message} {Data}", message, data);

            if (internalCodeAndDetailErrors == null)
            {
                var error = new ApiError(
                    !string.IsNullOrWhiteSpace(internalCode) ? internalCode : statusCode.ToString(),
                    MessageConstants.ErrorCliente,
                    $"{origen}Controller",
                    $"{data}",
                    requestId
                );

                errors.Add(error);
            }
            else
            {
                internalCodeAndDetailErrors.ForEach(err =>
                {
                    // do something with entry.Value or entry.Key
                    var error = new ApiError(
                        !string.IsNullOrWhiteSpace(err.InternalCode) ? err.InternalCode : statusCode.ToString(),
                        MessageConstants.ErrorCliente,
                        $"{origen}Controller",
                        $"{err.Detail}",
                        requestId
                    );

                    errors.Add(error);
                });
            }


            var httpError = new ErrorDetailModel(((HttpStatusCode)statusCode).ToString(),
                statusCode,
                errorType,
                MessageConstants.MensajeGenerico,
                errors);

            return StatusCode(statusCode, httpError);
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
                    clientError.InternalCode,
                    clientError.CodesAndDetailsErrors
                ),
                serverError => BuildServerError(request.XRequestId, serverError.StatusCode, serverError.Exception));
        }
    }
}
