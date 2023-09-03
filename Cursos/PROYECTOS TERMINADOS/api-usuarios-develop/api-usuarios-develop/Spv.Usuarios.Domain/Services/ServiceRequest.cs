using System;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.Services
{
    public interface IRequest
    {
        public string XRequestId { get; }
        public string XCanal { get; }
        public string XUsuario { get; }
        public string XAplicacion { get; }
        public string XGateway { get; }
    }

    public class Request : IRequest
    {
        public string XRequestId { get; }
        public string XCanal { get; }
        public string XUsuario { get; }
        public string XAplicacion { get; }
        public string XGateway { get; }

        public Request(string xRequestId, string xCanal, string xUsuario, string xAplicacion)
        {
            XRequestId = xRequestId;
            XCanal = xCanal;
            XUsuario = xUsuario;
            XAplicacion = xAplicacion;
        }

        protected Request(string xRequestId, string xCanal, string xUsuario, string xAplicacion, string xGateway) :
            this(xRequestId, xCanal, xUsuario, xAplicacion)
        {
            XGateway = xGateway;
        }
    }

    public interface IRequestBody<out T> : IRequest
    {
        T Body { get; }
        IRequestBody<TR> Map<TR>(Func<T, TR> mapper);
        IRequestBody<TR> MakeRelated<TR>(TR body);
    }

    public class RequestBody<T> : Request, IRequestBody<T>
    {
        public T Body { get; }

        public RequestBody(string xRequestId, string xCanal, string xUsuario, string xAplicacion, T body) :
            base(xRequestId, xCanal, xUsuario, xAplicacion)
        {
            Body = body;
        }

        public RequestBody(
            string xRequestId,
            string xCanal,
            string xUsuario,
            string xAplicacion,
            string xGateway,
            T body
        ) : base(xRequestId, xCanal, xUsuario, xAplicacion, xGateway)
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
            return new RequestBody<TR>(XRequestId, XCanal, XUsuario, XAplicacion, body);
        }
    }
}
