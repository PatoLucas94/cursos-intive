using System;
using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.Services
{
    public interface IRequest
    {
        public string XRequestId { get; }

        public string DeviceId { get; }
    }

    public class Request : IRequest
    {
        public string XRequestId { get; }

        public string DeviceId { get; }

        public Request(string xRequestId)
        {
            XRequestId = xRequestId;
        }

        public Request(string xRequestId, string deviceId)
        {
            XRequestId = xRequestId;
            DeviceId = deviceId;
        }
    }

    public interface IRequestBody<out T> : IRequest
    {
        T Body { get; }
        IRequestBody<TR> Map<TR>(Func<T, TR> mapper);
        IRequestBody<TR> MakeRelated<TR>(TR body);
    }

    [ExcludeFromCodeCoverage]
    public class RequestBody<T> : Request, IRequestBody<T>
    {
        public T Body { get; }

        public RequestBody(string xRequestId, T body) : base(xRequestId)
        {
            Body = body;
        }

        public RequestBody(string xRequestId, T body, string DeviceId) : base(xRequestId, DeviceId)
        {
            Body = body;
        }

        public IRequestBody<TR> Map<TR>(Func<T, TR> mapper)
        {
            Arg.NonNull(mapper, nameof(mapper));
            return MakeRelated(mapper(Body));
        }

        public IRequestBody<TR> MakeRelated<TR>(TR body)
        {
            return new RequestBody<TR>(XRequestId, body);
        }
    }
}
