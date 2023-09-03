using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers
{
    public abstract class ControllerIntegrationTest : ControllerTest
    {
        private readonly ServerFixture _server;

        protected ControllerIntegrationTest(ServerFixture server)
        {
            _server = server;
        }

        protected async Task<HttpResponseMessage> SendAsync(
            ServiceRequest request,
            string canal = "OBI",
            string usuario = "user",
            string aplicacion = "seguros",
            string requestId = "dae34444a44d4e4f",
            string gateway = "AUTOGESTION",
            IReadOnlyDictionary<string, string> headers = null
        )
        {
            var client = _server.HttpServer.HttpClient;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HeaderNames.UserHeaderName, usuario);
            client.DefaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, canal);
            client.DefaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, aplicacion);
            client.DefaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, requestId);
            client.DefaultRequestHeaders.Add(HeaderNames.GateWay, gateway);

            if (headers != null)
            {
                foreach (var (key, value) in headers)
                    client.DefaultRequestHeaders.Add(key, value);
            }

            return await request.SendToAsync(client);
        }

        [Theory]
        [InlineData("", "pepe", "seguros", HeaderNames.ChannelHeaderName)]
        [InlineData("    ", "pepe", "seguros", HeaderNames.ChannelHeaderName)]
        [InlineData("obi", "", "seguros", HeaderNames.UserHeaderName)]
        [InlineData("obi", "    ", "seguros", HeaderNames.UserHeaderName)]
        [InlineData("obi", "pepe", "", HeaderNames.ApplicationHeaderName)]
        [InlineData("obi", "pepe", "    ", HeaderNames.ApplicationHeaderName)]
        protected async Task SetRequiredHeaders(string canal, string usuario, string aplicacion, string headerName)
        {
            // Arrange
            foreach (var request in AllRequests)
            {
                // Act
                var response = await SendAsync(request, canal, usuario, aplicacion);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                    $"Debió fallar con 400 para {request} con el header {headerName} sin especificar");

                var content = await response.Content.ReadAsAsync<ErrorDetailModel>();
                content.Should().NotBeNull();

                content.Errors.Find(x =>
                    x.Detail.Contains(headerName)).Should().NotBeNull($"{request} debe requerir {headerName}"
                );
            }
        }

        protected static string GetUrlEncode<T>(T model)
        {
            var properties = model.GetType()
                .GetProperties()
                .Where(p => p.GetValue(model, null) != null)
                .Select(p => p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(model, null)!.ToString()));

            var queryString = string.Join("&", properties.ToArray());

            return queryString;
        }
    }
}
