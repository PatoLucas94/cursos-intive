using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.Services
{
    public interface IResponse
    {
        int StatusCode { get; }
        public bool IsOk => StatusCode >= 200 && StatusCode < 300;
        IReadOnlyDictionary<string, string> Headers { get; }
    }

    public static class Responses
    {
        public static IResponse<T> Ok<T>(T payload)
        {
            return Ok(payload, new Dictionary<string, string>(0));
        }

        public static IResponse<T> Ok<T>(T payload, IReadOnlyDictionary<string, string> headers)
        {
            return Ok(StatusCodes.Status200OK, headers, payload);
        }

        public static IResponse<T> Ok<T>(int statusCode, IReadOnlyDictionary<string, string> headers, T payload)
        {
            return new OkResponse<T>(statusCode, headers, payload);
        }

        public static IResponse<T> Created<T>(T payload)
        {
            return Ok(StatusCodes.Status201Created, new Dictionary<string, string>(0), payload);
        }

        public static IResponse<T> Accepted<T>(T payload)
        {
            return Ok(StatusCodes.Status202Accepted, new Dictionary<string, string>(0), payload);
        }

        public static IResponse<T> NoContent<T>(T payload) =>
            Ok(StatusCodes.Status202Accepted, new Dictionary<string, string>(0), payload);

        public static IResponse<T> BadRequest<T>(string message)
        {
            return ClientError<T>(StatusCodes.Status400BadRequest, new Dictionary<string, string>(0), message,
                ErrorTypeConstants.Business);
        }

        public static IResponse<T> Unauthorized<T>(string internalCode, string message)
        {
            return ClientError<T>(StatusCodes.Status401Unauthorized, new Dictionary<string, string>(0), message,
                ErrorTypeConstants.Business, internalCode);
        }

        public static IResponse<T> NotFound<T>(string message)
        {
            return ClientError<T>(StatusCodes.Status404NotFound, new Dictionary<string, string>(0), message,
                ErrorTypeConstants.Business);
        }

        public static IResponse<T> Conflict<T>(string message)
        {
            return ClientError<T>(StatusCodes.Status409Conflict, new Dictionary<string, string>(0), message);
        }

        public static IResponse<T> Conflict<T>(string internalCode, string message)
        {
            return ClientError<T>(StatusCodes.Status409Conflict, new Dictionary<string, string>(0), message,
                ErrorTypeConstants.Business, internalCode);
        }

        public static IResponse<T> ClientError<T>(int statusCode, IReadOnlyDictionary<string, string> headers,
            string message, string errorType = null, string internalCode = "")
        {
            return new ClientErrorResponse<T>(statusCode, headers, message, errorType, internalCode);
        }

        public static IResponse<IReadOnlyCollection<T>> Page<T>(IReadOnlyCollection<T> payload,
            int totalNumberOfRecords)
        {
            return new PaginationResponse<T>(new Dictionary<string, string>(0), payload, totalNumberOfRecords);
        }

        public static IResponse<IReadOnlyCollection<T>> PageNotFound<T>(string message)
        {
            return new ClientErrorResponse<IReadOnlyCollection<T>>(StatusCodes.Status404NotFound,
                new Dictionary<string, string>(0), message);
        }

        public static IResponse<T> InternalServerError<T>(Exception e)
        {
            return ServerError<T>(StatusCodes.Status500InternalServerError, new Dictionary<string, string>(0), e);
        }

        public static IResponse<T> ServerError<T>(int statusCode, IReadOnlyDictionary<string, string> headers,
            Exception exception)
        {
            return new ServerErrorResponse<T>(statusCode, headers, exception);
        }

        internal static void AppendAll(this IDictionary<string, string> destination,
            IReadOnlyDictionary<string, string> source)
        {
            foreach (var p in source)
            {
                destination.Add(p.Key, p.Value);
            }
        }
    }

    internal abstract class Response : IResponse
    {
        public int StatusCode { get; }
        public IReadOnlyDictionary<string, string> Headers { get; }

        protected Response(int statusCode, IReadOnlyDictionary<string, string> headers)
        {
            StatusCode = statusCode;
            Headers = Arg.NonNull(headers, nameof(headers));
        }

        public override string ToString()
        {
            var headers = string.Join(", ", Headers.Select(h => $"{h.Key}:'{h.Value}'"));
            var s = $"{GetTypeName(GetType())}, StatusCode: {StatusCode}";
            return headers.Length > 0 ? $"{s} {headers}" : s;
        }

        private static string GetTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                return type.Name.Substring(
                           0,
                           type.Name.IndexOf('`', StringComparison.InvariantCulture)) + "<" +
                       string.Join(", ", type.GenericTypeArguments.Select(t => t.Name)) + ">";
            }

            return type.Name;
        }
    }

    public interface IResponse<out T> : IResponse
    {
        IResponse<TR> Map<TR>(Func<T, TR> mapper);
        IResponse<TR> Bind<TR>(Func<T, IResponse<TR>> binder);
        Task<IResponse<TR>> BindAsync<TR>(Func<T, Task<IResponse<TR>>> binder);

        TR Match<TR>(Func<IOkResponse<T>, TR> okCase, Func<IClientErrorResponse<T>, TR> clientErrorCase,
            Func<IServerErrorResponse<T>, TR> serverErrorCase);

        IResponse<T> Accept(Action<IOkResponse<T>> onOk, Action<IClientErrorResponse<T>> onClientError,
            Action<IServerErrorResponse<T>> onServerError);

        IResponse<T> OnOk(Action<T> onOk);
    }

    internal abstract class Response<T> : Response, IResponse<T>
    {
        protected Response(int statusCode, IReadOnlyDictionary<string, string> headers) : base(statusCode, headers)
        {
        }

        public IResponse<TR> Map<TR>(Func<T, TR> mapper) => MapCore(Arg.NonNull(mapper, nameof(mapper)));

        protected abstract IResponse<TR> MapCore<TR>(Func<T, TR> mapper);

        public IResponse<TR> Bind<TR>(Func<T, IResponse<TR>> binder) => BindCore(Arg.NonNull(binder, nameof(binder)));

        protected abstract IResponse<TR> BindCore<TR>(Func<T, IResponse<TR>> binder);

        public async Task<IResponse<TR>> BindAsync<TR>(Func<T, Task<IResponse<TR>>> binder) =>
            await BindAsyncCore(Arg.NonNull(binder, nameof(binder)));

        protected abstract Task<IResponse<TR>> BindAsyncCore<TR>(Func<T, Task<IResponse<TR>>> binder);

        public TR Match<TR>(Func<IOkResponse<T>, TR> okCase, Func<IClientErrorResponse<T>, TR> clientErrorCase,
            Func<IServerErrorResponse<T>, TR> serverErrorCase) =>
            MatchCore(Arg.NonNull(okCase, nameof(okCase)), Arg.NonNull(clientErrorCase, nameof(clientErrorCase)),
                Arg.NonNull(serverErrorCase, nameof(serverErrorCase)));

        protected abstract TR MatchCore<TR>(Func<IOkResponse<T>, TR> okCase,
            Func<IClientErrorResponse<T>, TR> clientErrorCase, Func<IServerErrorResponse<T>, TR> serverErrorCase);

        public IResponse<T> Accept(Action<IOkResponse<T>> onOk, Action<IClientErrorResponse<T>> onClientError,
            Action<IServerErrorResponse<T>> onServerError)
        {
            DoCore(Arg.NonNull(onOk, nameof(onOk)), Arg.NonNull(onClientError, nameof(onClientError)),
                Arg.NonNull(onServerError, nameof(onServerError)));
            return this;
        }

        public IResponse<T> OnOk(Action<T> onOk)
        {
            OnOkCore(Arg.NonNull(onOk, nameof(onOk)));
            return this;
        }

        protected abstract void DoCore(Action<IOkResponse<T>> onOk, Action<IClientErrorResponse<T>> onClientError,
            Action<IServerErrorResponse<T>> onServerError);

        protected abstract void OnOkCore(Action<T> onOk);
    }

    public interface IOkResponse<out T> : IResponse<T>
    {
        public T Payload { get; }
    }

    internal class OkResponse<T> : Response<T>, IOkResponse<T>
    {
        public OkResponse(int statusCode, IReadOnlyDictionary<string, string> headers, T payload) : base(
            Arg.InRange(statusCode, 200, 299, nameof(statusCode)), headers) =>
            Payload = payload;

        protected sealed override IResponse<TR> MapCore<TR>(Func<T, TR> mapper) =>
            new OkResponse<TR>(StatusCode, Headers, mapper.Invoke(Payload));

        protected sealed override IResponse<TR> BindCore<TR>(Func<T, IResponse<TR>> binder) => Match(binder(Payload));

        protected sealed override async Task<IResponse<TR>> BindAsyncCore<TR>(Func<T, Task<IResponse<TR>>> binder) =>
            Match(await binder(Payload));

        protected sealed override TR MatchCore<TR>(Func<IOkResponse<T>, TR> okCase,
            Func<IClientErrorResponse<T>, TR> clientErrorCase,
            Func<IServerErrorResponse<T>, TR> serverErrorCase) =>
            okCase(this);

        protected sealed override void DoCore(Action<IOkResponse<T>> onOk,
            Action<IClientErrorResponse<T>> onClientError, Action<IServerErrorResponse<T>> onServerError) =>
            onOk(this);

        protected sealed override void OnOkCore(Action<T> onOk) => onOk(Payload);

        public T Payload { get; }

        public override string ToString()
        {
            return $"{base.ToString()}, Payload:{Payload}";
        }

        private IResponse<TR> Match<TR>(IResponse<TR> innerResponse)
        {
            return Arg.NonNull(innerResponse, nameof(innerResponse))
                .Match(
                    ok => Responses.Ok(ok.StatusCode, ConcatHeaders(ok.Headers), ok.Payload),
                    ce => Responses.ClientError<TR>(ce.StatusCode, ConcatHeaders(ce.Headers), ce.Message),
                    se => Responses.ServerError<TR>(se.StatusCode, ConcatHeaders(se.Headers), se.Exception));
        }

        private IReadOnlyDictionary<string, string> ConcatHeaders(IReadOnlyDictionary<string, string> headers)
        {
            var newHeaders = new Dictionary<string, string>(Headers.Count + headers.Count);
            newHeaders.AppendAll(Headers);
            newHeaders.AppendAll(headers);
            return newHeaders;
        }
    }

    internal abstract class ErrorResponse<T> : Response<T>
    {
        protected ErrorResponse(int statusCode, IReadOnlyDictionary<string, string> headers) : base(statusCode, headers)
        {
        }

        protected sealed override IResponse<TR> MapCore<TR>(Func<T, TR> mapper) => OfType<TR>();

        protected sealed override IResponse<TR> BindCore<TR>(Func<T, IResponse<TR>> binder) => OfType<TR>();

        protected override Task<IResponse<TR>> BindAsyncCore<TR>(Func<T, Task<IResponse<TR>>> binder) =>
            Task.FromResult(OfType<TR>());

        protected abstract IResponse<TR> OfType<TR>();

        protected sealed override void OnOkCore(Action<T> onOk)
        {
        }
    }

    public interface IClientErrorResponse<out T> : IResponse<T>
    {
        string Message { get; }

        string ErrorType { get; }

        string InternalCode { get; }
    }

    internal sealed class ClientErrorResponse<T> : ErrorResponse<T>, IClientErrorResponse<T>
    {
        public ClientErrorResponse(int statusCode, IReadOnlyDictionary<string, string> headers, string message,
            string errorType = null, string internalCode = "")
            : base(Arg.InRange(statusCode, 400, 499, nameof(statusCode)),
                headers)
        {
            Message = message;
            ErrorType = errorType;
            InternalCode = internalCode;
        }

        protected override TR MatchCore<TR>(Func<IOkResponse<T>, TR> okCase,
            Func<IClientErrorResponse<T>, TR> clientErrorCase, Func<IServerErrorResponse<T>, TR> serverErrorCase) =>
            clientErrorCase(this);

        protected override void DoCore(Action<IOkResponse<T>> onOk, Action<IClientErrorResponse<T>> onClientError,
            Action<IServerErrorResponse<T>> onServerError) =>
            onClientError(this);

        protected override IResponse<TR> OfType<TR>() =>
            new ClientErrorResponse<TR>(StatusCode, Headers, Message, ErrorType, InternalCode);

        public string Message { get; }

        public string ErrorType { get; }

        public string InternalCode { get; }

        public override string ToString()
        {
            return $"{base.ToString()}, Message:{Message}";
        }
    }

    public interface IServerErrorResponse<out T> : IResponse<T>
    {
        Exception Exception { get; }
    }

    internal sealed class ServerErrorResponse<T> : ErrorResponse<T>, IServerErrorResponse<T>
    {
        public Exception Exception { get; }

        public ServerErrorResponse(int statusCode, IReadOnlyDictionary<string, string> headers, Exception e) :
            base(Arg.InRange(statusCode, 500, 599, nameof(statusCode)), headers) => Exception = e;

        protected override IResponse<TR> OfType<TR>() => new ServerErrorResponse<TR>(StatusCode, Headers, Exception);

        protected override TR MatchCore<TR>(Func<IOkResponse<T>, TR> okCase,
            Func<IClientErrorResponse<T>, TR> clientErrorCase, Func<IServerErrorResponse<T>, TR> serverErrorCase) =>
            serverErrorCase(this);

        protected override void DoCore(Action<IOkResponse<T>> onOk, Action<IClientErrorResponse<T>> onClientError,
            Action<IServerErrorResponse<T>> onServerError) =>
            onServerError(this);

        public override string ToString()
        {
            return $"{base.ToString()}, Exception:{Exception}";
        }
    }

    internal sealed class PaginationResponse<T> : OkResponse<IReadOnlyCollection<T>>
    {
        public PaginationResponse(IReadOnlyDictionary<string, string> headers, IReadOnlyCollection<T> payload,
            int totalNumberOfRecords) : base(
            DetermineStatusCode(Arg.NonNull(payload, nameof(payload)), totalNumberOfRecords),
            AppendHeader(headers, totalNumberOfRecords), payload)
        {
        }

        private static int DetermineStatusCode(IReadOnlyCollection<T> page, int totalNumberOfRecords)
        {
            if (page.Count == 0)
            {
                return StatusCodes.Status204NoContent;
            }

            return page.Count >= totalNumberOfRecords ? 200 : 206;
        }

        private static IReadOnlyDictionary<string, string> AppendHeader(IReadOnlyDictionary<string, string> headers,
            int totalNumberOfRecords)
        {
            var newHeaders = new Dictionary<string, string>(headers.Count + 1);
            newHeaders.AppendAll(headers);
            newHeaders.Add("X-Total-Count", totalNumberOfRecords.ToString(CultureInfo.InvariantCulture));
            return newHeaders;
        }
    }
}
