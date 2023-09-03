using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.SsoController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class AutenticacionIntegrationTest : ControllerIntegrationTest
    {
        private readonly ServerFixture _server;
        private readonly Uri _uriBase;

        private WireMockHelper WireMockHelper { get; }

        private static ServiceRequest PostAutenticacion(
            Uri uriBase,
            AutenticacionModelRequestV2 autenticacionModelRequestV2
        )
        {
            var uri = new Uri(uriBase, ApiUris.AutenticacionKeycloakV2);

            return ServiceRequest.Post(uri.AbsoluteUri, autenticacionModelRequestV2);
        }

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostAutenticacion(
                _uriBase,
                new AutenticacionModelRequestV2
                {
                    Usuario = "usuario",
                    Clave = "clave",
                    NumeroDocumento = "nroDocumento"
                }
            )
        };

        public AutenticacionIntegrationTest(ServerFixture server) : base(server)
        {
            _server = server;
            _uriBase = _server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = _server.WireMock;
        }

        [Fact]
        public async Task Autenticacion_SinHeaders_BodyCorrecto_BadRequest()
        {
            // Arrange
            _server.HttpServer.HttpClient.DefaultRequestHeaders.Clear();

            var uri = new Uri(_uriBase, ApiUris.AutenticacionKeycloakV2);

            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = "usuario",
                Clave = "clave",
                NumeroDocumento = "11222333"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(autenticacionModelRequestV2),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content);
            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Autenticacion_HeadersIncorrectos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;

            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add("X-User", "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.AutenticacionKeycloakV2);

            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = "usuario",
                Clave = "clave",
                NumeroDocumento = "11222333"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(autenticacionModelRequestV2),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content);
            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Autenticacion_HeadersIncompletos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;

            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");

            var uri = new Uri(_uriBase, ApiUris.AutenticacionKeycloakV2);

            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = "usuario",
                Clave = "clave",
                NumeroDocumento = "11222333"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(autenticacionModelRequestV2),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content);
            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Autenticacion_BodyIncorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;

            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.AutenticacionKeycloakV2);

            var content = new StringContent(
                JsonConvert.SerializeObject(new { username = "xxxxx", password = "yyyyy", documentNumer = "11111" }),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content);
            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }
    }
}
