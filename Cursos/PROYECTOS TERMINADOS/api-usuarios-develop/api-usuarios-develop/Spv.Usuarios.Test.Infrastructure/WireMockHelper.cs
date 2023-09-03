using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web;
using WireMock.Logging;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;
using Request = WireMock.RequestBuilders.Request;

namespace Spv.Usuarios.Test.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public sealed class WireMockHelper : IDisposable
    {
        public WireMockServer ServiceMock { get; set; }
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

        public IRequestBuilder WebService(string uri)
        {
            return Request.Create().WithPath(uri);
        }

        public IRequestBuilder Get(string uri)
        {
            return WithParameters(uri).UsingGet();
        }

        public IRequestBuilder GetWithHeaders(string uri, Dictionary<string, string> headers)
        {
            var request = WithParameters(uri);

            return WithHeaders(request, headers).UsingGet();
        }

        public IRequestBuilder Post(string uri)
        {
            return WithParameters(uri).UsingPost();
        }

        public IRequestBuilder Put(string uri)
        {
            return Request.Create().WithPath(uri).UsingPut();
        }

        private static IRequestBuilder WithParameters(string uri)
        {
            var qmIndex = uri.IndexOf('?');
            var path = uri;
            var query = string.Empty;
            if (qmIndex > 0)
            {
                path = uri.Substring(0, qmIndex);
                query = uri.Substring(qmIndex + 1);
            }

            var req = Request.Create().WithPath(path);

            var parameters = HttpUtility.ParseQueryString(query);
            foreach (var key in parameters.AllKeys)
            {
                req = req.WithParam(key, MatchBehaviour.AcceptOnMatch, parameters[key]);
            }

            return req;
        }

        private static IRequestBuilder WithHeaders(IRequestBuilder req, Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> entry in headers)
            {
                req.WithHeader(entry.Key, entry.Value);
            }

            return req;
        }

        public IResponseBuilder Json(object o)
        {
            return Response.Create().WithBodyAsJson(o, Encoding.UTF8, false)
                .WithHeader("Content-Type", "application/json");
        }

        public IResponseBuilder Xml(string o)
        {
            return Response.Create().WithBodyFromFile(o).WithHeader("Content-Type", "text/xml");
        }

        public IResponseBuilder RespondWithBadRequest()
        {
            return Response.Create().WithStatusCode(StatusCodes.Status400BadRequest);
        }

        public IResponseBuilder RespondWithNotFound()
        {
            return Response.Create().WithStatusCode(StatusCodes.Status404NotFound);
        }
    }
}
