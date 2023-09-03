using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.PersonaService.Output;
using Spv.Usuarios.DataAccess.EntityFramework;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Test.Infrastructure;
using Spv.Usuarios.Test.Integration.ExternalServices;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v1._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class AutenticacionIntegrationTest : ControllerIntegrationTest
    {
        private readonly ServerFixture _server;
        private readonly Uri _uriBase;

        private readonly AuditoriaRepository _auditoriaRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private WireMockHelper WireMockHelper { get; }

        private static ServiceRequest PostAutenticacion(Uri uriBase, AutenticacionModelRequest autenticacionModelRequest)
        {
            var uri = new Uri(uriBase, ApiUris.Autenticacion);

            return ServiceRequest.Post(uri.AbsoluteUri, autenticacionModelRequest);
        }

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostAutenticacion(_uriBase, new AutenticacionModelRequest { Usuario = "usuario", Clave = "clave"})
        };

        public AutenticacionIntegrationTest(ServerFixture server) : base(server)
        {
            _server = server;
            _uriBase = _server.HttpServer.TestServer.BaseAddress;

            var db = _server.HttpServer.TestServer.Services.GetRequiredService<GenericDbContext>();

            _auditoriaRepository = new AuditoriaRepository(db);
            _usuarioRepository = new UsuarioRepository(db);

            WireMockHelper = _server.WireMock;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Autenticacion(string usuario, string clave, HttpStatusCode httpStatusCode, string mensaje = null)
        {
            // Arrange
            var autenticacionModelRequest = new AutenticacionModelRequest
            {
                Usuario = usuario,
                Clave = clave
            };

            var expectedPerson = new PersonaModelResponse
            {
                id = 100,
                tipo_persona = "Test tipo_persona",
                links = new Links
                {
                    empty = false
                }
            };

            var path = $"{ApiPersonasUris.Persona("11222333", 0, 0)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(expectedPerson));

            var request = PostAutenticacion(_uriBase, autenticacionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var response = new ObjectResult(await sut.Content.ReadAsAsync<object>());

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            if (!string.IsNullOrWhiteSpace(mensaje) && httpStatusCode == HttpStatusCode.Unauthorized && response.Value is JObject jObject)
            {
                var errorDetailModel = jObject.ToObject<ErrorDetailModel>();

                var error = errorDetailModel.Errors.First();

                error.Detail.Should().Be(mensaje);
            }

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task Autenticacion_Correcta_Con_Auditoria()
        {
            // Arrange
            var autenticacionModelRequest = new AutenticacionModelRequest
            {
                Usuario = "UsuarioTest1",
                Clave = "Info1212"
            };

            var request = PostAutenticacion(_uriBase, autenticacionModelRequest);

            // Act
            var sut = await SendAsync(request);
            
            var usuarioBd = _usuarioRepository.Find(u => u.UserName == autenticacionModelRequest.Usuario);
            
            var auditorias = _auditoriaRepository.Filter(a => a.UserId == usuarioBd.UserId).ToList();

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Accepted);

            auditorias.Should().NotBeEmpty();
            auditorias.Count.Should().BeGreaterThan(0);

            usuarioBd.Should().NotBeNull();

            var auditoria = auditorias.FirstOrDefault();

            if (auditoria != null)
            {
                auditoria.ActionId.Should().Be((int)AuditAction.LogOn);
                auditoria.ActionResultId.Should().Be((int)AuditActionResult.LoggedOn);
                auditoria.UserId.Should().Be(usuarioBd.UserId);
            }
        }

        [Fact]
        public async Task Autenticacion_Incorrecta_Sin_Auditoria_Usuario_Bloqueado()
        {
            // Arrange
            var autenticacionModelRequest = new AutenticacionModelRequest
            {
                Usuario = "UsuarioTest2",
                Clave = "Info1212"
            };

            var request = PostAutenticacion(_uriBase, autenticacionModelRequest);

            // Act
            var sut = await SendAsync(request);
            var usuarioBd = _usuarioRepository.Find(u => u.UserName == autenticacionModelRequest.Usuario);

            var auditorias = _auditoriaRepository.Find(a => a.UserId == usuarioBd.UserId);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            auditorias.Should().BeNull();
            usuarioBd.Should().NotBeNull();
        }

        [Fact]
        public async Task Autenticacion_SinHeaders_BodyCorrecto_BadRequest()
        {
            // Arrange
            _server.HttpServer.HttpClient.DefaultRequestHeaders.Clear();

            var uri = new Uri(_uriBase, ApiUris.Autenticacion);

            var autenticacionModelRequest = new AutenticacionModelRequest
            {
                Usuario = "usuario",
                Clave = "clave"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(autenticacionModelRequest),
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
        public async Task Autenticacion_HeadersIncorrectos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add("X-User", "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.Autenticacion);

            var autenticacionModelRequest = new AutenticacionModelRequest
            {
                Usuario = "usuario",
                Clave = "clave"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(autenticacionModelRequest),
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
        public async Task Autenticacion_HeadersIncompletos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");

            var uri = new Uri(_uriBase, ApiUris.Autenticacion);

            var autenticacionModelRequest = new AutenticacionModelRequest
            {
                Usuario = "usuario",
                Clave = "clave"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(autenticacionModelRequest),
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
        public async Task Autenticacion_BodyIncorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.Autenticacion);

            var content = new StringContent(
                JsonConvert.SerializeObject(new { username = "xxxxx", password = "yyyyy" }),
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
            new object[] { "", "123", HttpStatusCode.BadRequest },
            new object[] { null, "123", HttpStatusCode.BadRequest },
            new object[] { "juan", "", HttpStatusCode.BadRequest },
            new object[] { "juan", null, HttpStatusCode.BadRequest },
            new object[] { null, null, HttpStatusCode.BadRequest },
            new object[] { "", "", HttpStatusCode.BadRequest },
            new object[] { "UsuarioTest1", "", HttpStatusCode.BadRequest },
            new object[] { "UsuarioTest1", "Info1212", HttpStatusCode.Accepted },
            new object[] { "UsuarioTest1", "Info1213", HttpStatusCode.Unauthorized },
            new object[] { "UsuarioTestX", "Info1212", HttpStatusCode.Unauthorized },
            new object[] { "UsuarioTest2", "Info1212", HttpStatusCode.Unauthorized },
            new object[] { "UsuarioTest3", "Info1212", HttpStatusCode.Unauthorized },
            new object[] { "UsuarioTest4", "Info1212", HttpStatusCode.Accepted },
            new object[] { "UsuarioTest8", "Info1213", HttpStatusCode.Unauthorized, MessageConstants.SeHaBloqueadoElUsuario }
        };
    }
}
