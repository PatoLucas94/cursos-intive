using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class MigracionIntegrationTest : ControllerIntegrationTest
    {
        private readonly ServerFixture _server;
        private readonly Uri _uriBase;

        private static ServiceRequest PostMigracion(Uri uriBase, MigracionModelRequest migracionModelRequest)
        {
            var uri = new Uri(uriBase, ApiUris.MigracionV2);

            return ServiceRequest.Post(uri.AbsoluteUri, migracionModelRequest);
        }

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostMigracion(
                _uriBase, 
                new MigracionModelRequest
                {
                    IdPersona = 12345678, 
                    Usuario = "usuarioTest1", 
                    Clave = "1432"
                })
        };

        public MigracionIntegrationTest(ServerFixture server) : base(server)
        {
            _server = server;
            _uriBase = _server.HttpServer.TestServer.BaseAddress;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Migracion(
            long idPersona, 
            string usuario, 
            string clave,
            HttpStatusCode httpStatusCode)
        {
            // Arrange
            var migracionModelRequest = new MigracionModelRequest
            {
                IdPersona = idPersona,
                Usuario = usuario,
                Clave = clave
            };

            var request = PostMigracion(_uriBase, migracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);
        }

        [Fact]
        public async Task Migracion_BadRequest()
        {
            // Arrange
            var migracionModelRequest = new MigracionModelRequest
            {
                IdPersona = 111111,
                Usuario = "usuario1",
                Clave = "1234"
            };

            var request = PostMigracion(_uriBase, migracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var response = await sut.Content.ReadAsAsync<MigracionModelResponse>();

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Migracion_SinHeaders_BodyCorrecto_BadRequest()
        {
            // Arrange
            _server.HttpServer.HttpClient.DefaultRequestHeaders.Clear();

            var uri = new Uri(_uriBase, ApiUris.MigracionV2);

            var migracionModelRequest = new MigracionModelRequest
            {
                IdPersona = 123456,
                Usuario = "usuario1",
                Clave = "1234"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(migracionModelRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Migracion_HeadersIncorrectos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add("X-User", "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.MigracionV2);

            var migracionModelRequest = new MigracionModelRequest
            {
                IdPersona = 123456,
                Usuario = "usuario1",
                Clave = "1234"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(migracionModelRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Migracion_HeadersIncompletos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");

            var uri = new Uri(_uriBase, ApiUris.MigracionV2);

            var migracionModelRequest = new MigracionModelRequest
            {
                IdPersona = 123456,
                Usuario = "usuario1",
                Clave = "1234"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(migracionModelRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Migracion_BodyIncorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.MigracionV2);

            var content = new StringContent(
                JsonConvert.SerializeObject(new { username = "xxxxx", password = "yyyyy", documentNumer = "11111" }),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { "12121212", "usuarioTest12", "1432", HttpStatusCode.Created },
                new object[] { "10002", "usuarioTest2", "2341", HttpStatusCode.Conflict }, // Usuario ya registrado
                new object[] { "31423142", "usuarioTest3", "1432", HttpStatusCode.NotFound }, // Usuario Inexistente
            };
    }
}
