using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v1._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class RegistracionIntegrationTest : ControllerIntegrationTest
    {
        private readonly ServerFixture _server;
        private readonly Uri _uriBase;

        private static ServiceRequest PostRegistracion(Uri uriBase, RegistracionModelRequest registracionModelRequest)
        {
            var uri = new Uri(uriBase, ApiUris.Registracion);

            return ServiceRequest.Post(uri.AbsoluteUri, registracionModelRequest);
        }

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostRegistracion(
                _uriBase,
                new RegistracionModelRequest
                {
                    NroCliente = "1",
                    Usuario = "usuario",
                    Clave = "clave123",
                    IdEstadoUsuario = 3,
                    Nombre = "nombre",
                    Apellido = "apellido",
                    IdPais = "080",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    ExtractoDeRecibo = true,
                    Cuil = "20111111119",
                    ControlFull = true,
                    IdPersona = "1"
                })
        };

        public RegistracionIntegrationTest(ServerFixture server) : base(server)
        {
            _server = server;
            _uriBase = _server.HttpServer.TestServer.BaseAddress;
        }

        [Fact]
        public async Task Registracion()
        {
            // Arrange
            var registracionModelRequest = new RegistracionModelRequest
            {
                NroCliente = "1",
                Usuario = "usuario11",
                Clave = "Clave123",
                IdEstadoUsuario = 3,
                Nombre = "nombre",
                Apellido = "apellido",
                IdPais = "080",
                IdTipoDocumento = 4,
                NroDocumento = "87654321",
                ExtractoDeRecibo = true,
                Cuil = "20111111119",
                ControlFull = true,
                IdPersona = "1"
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var response = new ObjectResult(await sut.Content.ReadAsAsync<object>());

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Conflicto_Usuario_ya_existe()
        {
            // Arrange
            var registracionModelRequest = new RegistracionModelRequest
            {
                NroCliente = "1",
                Usuario = "UsuarioTest1",
                Clave = "Clave123",
                IdEstadoUsuario = 3,
                Nombre = "nombre",
                Apellido = "apellido",
                IdPais = "080",
                IdTipoDocumento = 4,
                NroDocumento = "11222333",
                ExtractoDeRecibo = true,
                Cuil = "20112223339",
                ControlFull = true,
                IdPersona = "1"
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var response = new ObjectResult(await sut.Content.ReadAsAsync<object>());

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Conflict);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Registracion_SinHeaders_BodyCorrecto_BadRequest()
        {
            // Arrange
            _server.HttpServer.HttpClient.DefaultRequestHeaders.Clear();

            var uri = new Uri(_uriBase, ApiUris.Registracion);

            var registracionModelRequest = new RegistracionModelRequest
            {
                NroCliente = "1",
                Usuario = "usuario",
                Clave = "clave123",
                IdEstadoUsuario = 3,
                Nombre = "nombre",
                Apellido = "apellido",
                IdPais = "080",
                IdTipoDocumento = 4,
                NroDocumento = "12345678",
                ExtractoDeRecibo = true,
                Cuil = "20111111119",
                ControlFull = true,
                IdPersona = "1"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(registracionModelRequest),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Registracion_HeadersIncorrectos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add("X-User", "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.Registracion);

            var registracionModelRequest = new RegistracionModelRequest
            {
                NroCliente = "1",
                Usuario = "usuario",
                Clave = "clave123",
                IdEstadoUsuario = 3,
                Nombre = "nombre",
                Apellido = "apellido",
                IdPais = "080",
                IdTipoDocumento = 4,
                NroDocumento = "12345678",
                ExtractoDeRecibo = true,
                Cuil = "20111111119",
                ControlFull = true,
                IdPersona = "1"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(registracionModelRequest),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Registracion_HeadersIncompletos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");

            var uri = new Uri(_uriBase, ApiUris.Registracion);

            var registracionModelRequest = new RegistracionModelRequest
            {
                NroCliente = "1",
                Usuario = "usuario",
                Clave = "clave123",
                IdEstadoUsuario = 3,
                Nombre = "nombre",
                Apellido = "apellido",
                IdPais = "080",
                IdTipoDocumento = 4,
                NroDocumento = "12345678",
                ExtractoDeRecibo = true,
                Cuil = "20111111119",
                ControlFull = true,
                IdPersona = "1"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(registracionModelRequest),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Registracion_BodyIncorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.Registracion);

            var content = new StringContent(
                JsonConvert.SerializeObject(new { username = "xxxxx", password = "yyyyy", documentNumberX = "11111" }),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }
    }
}