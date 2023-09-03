using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.PersonaService.Output;
using Spv.Usuarios.Common.Testing.Attributes;
using Spv.Usuarios.DataAccess.EntityFramework;
using Spv.Usuarios.DataAccess.EntityFramework.V2;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Test.Infrastructure;
using Spv.Usuarios.Test.Integration.ExternalServices;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    [TestCaseOrderer("Spv.Usuarios.Common.Testing.PriorityOrderer", "Spv.Usuarios.Common")]
    public class AutenticacionIntegrationTest : ControllerIntegrationTest
    {
        private readonly ServerFixture _server;
        private readonly Uri _uriBase;

        private readonly AuditoriaLogV2Repository _auditoriaV2Repository;
        private readonly UsuarioV2Repository _usuarioV2Repository;
        private WireMockHelper WireMockHelper { get; set; }

        private static ServiceRequest PostAutenticacion(
            Uri uriBase,
            AutenticacionModelRequestV2 autenticacionModelRequestV2)
        {
            var uri = new Uri(uriBase, ApiUris.AutenticacionV2);

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
                })
        };

        public AutenticacionIntegrationTest(ServerFixture server) : base(server)
        {
            _server = server;
            _uriBase = _server.HttpServer.TestServer.BaseAddress;

            var dbV2 = _server.HttpServer.TestServer.Services.GetRequiredService<GenericDbContextV2>();

            _auditoriaV2Repository = new AuditoriaLogV2Repository(dbV2);
            _usuarioV2Repository = new UsuarioV2Repository(dbV2);

            WireMockHelper = _server.WireMock;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Autenticacion(
            string usuario,
            string clave,
            string nroDocumento,
            int? idPersona,
            HttpStatusCode httpStatusCode,
            string mensaje = null)
        {
            // Arrange
            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = usuario,
                Clave = clave,
                NumeroDocumento = nroDocumento
            };

            var request = PostAutenticacion(_uriBase, autenticacionModelRequestV2);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            if (!string.IsNullOrWhiteSpace(mensaje) && httpStatusCode == HttpStatusCode.Unauthorized)
            {
                var errorDetailModel = await sut.Content.ReadAsAsync<ErrorDetailModel>();

                var error = errorDetailModel.Errors.First();

                error.Detail.Should().Be(mensaje);
            }

            if (httpStatusCode == HttpStatusCode.Accepted)
            {
                var response = await sut.Content.ReadAsAsync<AutenticacionModelResponseV2>();
                response.PersonId.Should().Be(idPersona);
            }
        }

        [Fact, PriorityAttribute.TestPriority(0)]
        public async Task Autenticacion_Correcta_Sin_PersonId_InternalServerError()
        {
            // Arrange
            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = "User Test 10",
                Clave = "Info1212",
                NumeroDocumento = "24789456"
            };

            var request = PostAutenticacion(_uriBase, autenticacionModelRequestV2);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Autenticacion_Correcta_Sin_PersonId_OK()
        {
            // Arrange
            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = "User Test 10",
                Clave = "Info1212",
                NumeroDocumento = "24789456"
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

            var path = $"{ApiPersonasUris.Persona("24789456", 4, 80)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(expectedPerson));

            var request = PostAutenticacion(_uriBase, autenticacionModelRequestV2);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Accepted);

            var response = await sut.Content.ReadAsAsync<AutenticacionModelResponseV2>();
            response.PersonId.Should().Be(100);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task Autenticacion_Correcta_Sin_PersonId_NotOK()
        {
            // Arrange
            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = "User Test 10",
                Clave = "Info1212",
                NumeroDocumento = "24789456"
            };

            const string expectedNotFound =
                @"{
                    'status': 'NOT_FOUND',
                    'timestamp': '22-02-2021 05:55:10',
                    'message': 'No existe persona que coincida con los datos ingresados',
                    'sub_errors': []
                }";

            var path = $"{ApiPersonasUris.Persona("15975382", 4, 80)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(expectedNotFound));

            var request = PostAutenticacion(_uriBase, autenticacionModelRequestV2);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task Autenticacion_Correcta_Con_Auditoria()
        {
            // Arrange
            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = "User Test 1",
                Clave = "Info1212",
                NumeroDocumento = "12345678"
            };

            var request = PostAutenticacion(_uriBase, autenticacionModelRequestV2);

            // Act
            var sut = await SendAsync(request);

            var usuarioBd = _usuarioV2Repository.ObtenerUsuarioByDocumentNumber(
                autenticacionModelRequestV2.NumeroDocumento).FirstOrDefault(x =>
                x.Username.Equals("WWu/LIQRXGNQdYK/KKdUNaoAkT/oedNBtbNo980lfTI"));

            var auditorias = _auditoriaV2Repository.Filter(a => 
                a.UserId == usuarioBd.GetUserId()).ToList();

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Accepted);

            var response = await sut.Content.ReadAsAsync<AutenticacionModelResponseV2>();
            response.PersonId.Should().Be(14155917);

            auditorias.Should().NotBeEmpty();
            auditorias.Count.Should().BeGreaterThan(0);

            usuarioBd.Should().NotBeNull();

            var auditoria = auditorias.FirstOrDefault();

            if (auditoria != null)
            {
                auditoria.EventTypeId.Should().Be(1);
                auditoria.EventResultId.Should().Be((int)EventResults.Ok);
                auditoria.UserId.Should().Be(usuarioBd?.GetUserId());
            }
        }

        [Fact]
        public async Task Autenticacion_Incorrecta_Con_Auditoria_Usuario_Bloqueado()
        {
            // Arrange
            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = "User Test 7",
                Clave = "Info1212",
                NumeroDocumento = "71234567"
            };

            var request = PostAutenticacion(_uriBase, autenticacionModelRequestV2);

            // Act
            var sut = await SendAsync(request);
            var usuarioBd = _usuarioV2Repository.ObtenerUsuarioByDocumentNumber(
                autenticacionModelRequestV2.NumeroDocumento).FirstOrDefault(x =>
                x.Username.Equals("2p2yUkuajp89tg1b5D5zygP4AjWtgmVG5isvJlLHwUM"));

            var auditorias = _auditoriaV2Repository.Filter(a => 
                a.UserId == usuarioBd.GetUserId()).ToList();

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            auditorias.Should().NotBeEmpty();
            auditorias.Count.Should().BeGreaterThan(0);

            usuarioBd.Should().NotBeNull();

            var auditoria = auditorias.FirstOrDefault();

            if (auditoria != null)
            {
                auditoria.EventTypeId.Should().Be(1);
                auditoria.EventResultId.Should().Be((int)EventResults.Error);
                auditoria.UserId.Should().Be(usuarioBd?.GetUserId());
            }
        }

        [Fact]
        public async Task Autenticacion_SinHeaders_BodyCorrecto_BadRequest()
        {
            // Arrange
            _server.HttpServer.HttpClient.DefaultRequestHeaders.Clear();

            var uri = new Uri(_uriBase, ApiUris.AutenticacionV2);

            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = "usuario",
                Clave = "clave",
                NumeroDocumento = "11222333"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(autenticacionModelRequestV2),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content)
                .ConfigureAwait(false);

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

            var uri = new Uri(_uriBase, ApiUris.AutenticacionV2);

            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = "usuario",
                Clave = "clave",
                NumeroDocumento = "11222333"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(autenticacionModelRequestV2),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content)
                .ConfigureAwait(false);

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

            var uri = new Uri(_uriBase, ApiUris.AutenticacionV2);

            var autenticacionModelRequestV2 = new AutenticacionModelRequestV2
            {
                Usuario = "usuario",
                Clave = "clave",
                NumeroDocumento = "11222333"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(autenticacionModelRequestV2),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content)
                .ConfigureAwait(false);

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

            var uri = new Uri(_uriBase, ApiUris.AutenticacionV2);

            var content = new StringContent(
                JsonConvert.SerializeObject(new { username = "xxxxx", password = "yyyyy", documentNumer = "11111" }),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content)
                .ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        public static IEnumerable<object[]> Datos =>
        new List<object[]>
        {
            // Usuario - Clave - NroDocumento - IdPersona - HttpStatusCode
            // Accepted
            new object[] { "User Test 1", "Info1212", "12345678", 14155917, HttpStatusCode.Accepted }, // Datos Correctos
            new object[] { "User Test 4", "Info1212", "41234567", 4, HttpStatusCode.Accepted }, // Clave Vencida

            // Bad Request
            new object[] { "", "", "", null, HttpStatusCode.BadRequest },
            new object[] { "", "", "11222333", null, HttpStatusCode.BadRequest },
            new object[] { "", "123","", null, HttpStatusCode.BadRequest },
            new object[] { "", "123","11222333", null, HttpStatusCode.BadRequest },
            new object[] { "juan", "", "", null, HttpStatusCode.BadRequest },
            new object[] { "juan", "", "11222333", null, HttpStatusCode.BadRequest },
            new object[] { "juan", "123","", null, HttpStatusCode.BadRequest },
            new object[] { null, null, null, null, HttpStatusCode.BadRequest },
            new object[] { null, null, "11222333", null, HttpStatusCode.BadRequest },
            new object[] { null, "123", null, null, HttpStatusCode.BadRequest },
            new object[] { null, "123","11222333", null, HttpStatusCode.BadRequest },
            new object[] { "juan", null, null, null, HttpStatusCode.BadRequest },
            new object[] { "juan", null, "11222333", null, HttpStatusCode.BadRequest },
            new object[] { "juan", "123", null, null, HttpStatusCode.BadRequest },

            // Unauthorized
            new object[] { "UsuarioTestX", "Info1212", "12345678", null, HttpStatusCode.Unauthorized }, // Usuario Incorrecto
            new object[] { "User Test 1", "Info1213", "12345678", null, HttpStatusCode.Unauthorized }, // Clave Incorrecta
            new object[] { "User Test 1", "Info1212", "22222222", null, HttpStatusCode.Unauthorized }, // NroDoc. Incorrecto
            new object[] { "User Test Migrated", "Info1212", "22222222", 12345678, HttpStatusCode.Unauthorized }, // Usuario existente en v2 y v1 de bd pero con Nro. doc incorrecto
            new object[] { "User Test 5", "Info1212", "51234567", null, HttpStatusCode.Unauthorized }, // Usuario Bloqueado
            new object[] { "User Test 2", "Info1212", "21234567", null, HttpStatusCode.Unauthorized }, // Usuario Inactivo
            new object[] { "User Test 3", "Info1213", "31234567", null, HttpStatusCode.Unauthorized, MessageConstants.UsuarioIncorrecto }
        };
    }
}
