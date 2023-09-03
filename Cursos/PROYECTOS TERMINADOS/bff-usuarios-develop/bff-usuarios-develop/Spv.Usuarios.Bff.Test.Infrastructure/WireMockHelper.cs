using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using WireMock.Logging;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace Spv.Usuarios.Bff.Test.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public sealed class WireMockHelper : IDisposable
    {
        public WireMockServer ServiceMock { get; }
        private readonly IWireMockLogger _logger = new WireMockConsoleLogger();

        public WireMockHelper(string urlHost, string port)
        {
            ServiceMock = WireMockServer.Start(new WireMockServerSettings()
            {
                Urls = new[] { $"{urlHost}:{port}" },
                StartAdminInterface = true,
                Logger = _logger
            });
        }

        public void Stop()
        {
            ServiceMock.Stop();
        }

        public void ResetMapping()
        {
            ServiceMock.ResetMappings();
        }

        public void Dispose()
        {
            ServiceMock.Stop();
            ServiceMock.Dispose();
        }

        public static IRequestBuilder Get(string uri)
        {
            return WithParameters(uri).UsingGet();
        }

        public static IRequestBuilder GetWithHeaders(string uri, Dictionary<string, string> headers)
        {
            var request = WithParameters(uri);

            return WithHeaders(request, headers).UsingGet();
        }

        public static IRequestBuilder Post(string uri)
        {
            return WithParameters(uri).UsingPost();
        }

        public static IRequestBuilder PostWithHeaders(string uri, Dictionary<string, string> headers)
        {
            var request = WithParameters(uri);

            return WithHeaders(request, headers).UsingPost();
        }

        public IRequestBuilder Put(string uri)
        {
            return Request.Create().WithPath(uri).UsingPut();
        }

        public static IRequestBuilder Patch(string uri)
        {
            return Request.Create().WithPath(uri).UsingPatch();
        }

        public IRequestBuilder PatchWithHeaders(string uri, Dictionary<string, string> headers)
        {
            var request = WithParameters(uri);

            return WithHeaders(request, headers).UsingPatch();
        }

        private static IRequestBuilder WithParameters(string uri)
        {
            var qmIndex = uri.IndexOf('?');
            var path = uri;
            var query = string.Empty;

            if (qmIndex > 0)
            {
                path = uri[..qmIndex];
                query = uri[(qmIndex + 1)..];
            }

            var req = Request.Create().WithPath(path);
            var parameters = HttpUtility.ParseQueryString(query);

            return parameters.AllKeys.Aggregate(req,
                (current, key) => current.WithParam(key, MatchBehaviour.AcceptOnMatch, parameters[key]));
        }

        private static IRequestBuilder WithHeaders(IRequestBuilder req, Dictionary<string, string> headers)
        {
            foreach (var entry in headers)
                req.WithHeader(entry.Key, entry.Value);

            return req;
        }

        public static IResponseBuilder Json(object o)
        {
            return Response.Create().WithBodyAsJson(o, Encoding.UTF8, false)
                .WithHeader("Content-Type", "application/json");
        }

        public static IResponseBuilder RespondWithStatusCode(HttpStatusCode statusCode, object o = null)
        {
            if (o != null)
            {
                return Response.Create().WithStatusCode(statusCode).WithBodyAsJson(o, Encoding.UTF8, false)
                    .WithHeader("Content-Type", "application/json");
            }

            return Response.Create().WithStatusCode(statusCode);
        }

        public static IResponseBuilder RespondWithBadRequest()
        {
            return Response.Create().WithStatusCode(StatusCodes.Status400BadRequest);
        }

        public static IResponseBuilder RespondWithBadRequest(object o)
        {
            return Response.Create().WithBodyAsJson(o, Encoding.UTF8, false)
                .WithStatusCode(StatusCodes.Status400BadRequest);
        }

        public static IResponseBuilder RespondWithAccepted()
        {
            return Response.Create().WithStatusCode(StatusCodes.Status202Accepted);
        }

        public static IResponseBuilder RespondWithAccepted(object o)
        {
            return Response.Create().WithBodyAsJson(o, Encoding.UTF8, false)
                .WithStatusCode(StatusCodes.Status202Accepted);
        }

        public static IResponseBuilder RespondWithCreated()
        {
            return Response.Create().WithStatusCode(StatusCodes.Status201Created);
        }

        public static IResponseBuilder RespondWithConflict()
        {
            return Response.Create().WithStatusCode(StatusCodes.Status409Conflict);
        }

        public static IResponseBuilder RespondWithConflict(object o)
        {
            return Response.Create().WithBodyAsJson(o, Encoding.UTF8, false)
                .WithStatusCode(StatusCodes.Status409Conflict);
        }

        public static IResponseBuilder RespondWithUnauthorized()
        {
            return Response.Create().WithStatusCode(StatusCodes.Status401Unauthorized);
        }

        public static IResponseBuilder RespondWithUnauthorized(object o)
        {
            return Response.Create().WithBodyAsJson(o, Encoding.UTF8, false)
                .WithStatusCode(StatusCodes.Status401Unauthorized);
        }

        public static IResponseBuilder RespondWithNotFound()
        {
            return Response.Create().WithStatusCode(StatusCodes.Status404NotFound);
        }

        public static IResponseBuilder RespondWithNotFound(object o)
        {
            return Response.Create().WithBodyAsJson(o, Encoding.UTF8, false)
                .WithStatusCode(StatusCodes.Status404NotFound);
        }
    }
}
