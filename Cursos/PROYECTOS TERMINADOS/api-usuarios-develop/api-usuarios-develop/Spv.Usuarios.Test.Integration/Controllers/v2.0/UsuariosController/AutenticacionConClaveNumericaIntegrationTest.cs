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
using Spv.Usuarios.Common.Errors;
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
    public class AutenticacionConClaveNumericaIntegrationTest : ControllerIntegrationTest
    {
        private readonly ServerFixture _server;
        private readonly Uri _uriBase;

        private readonly AuditoriaLogV2Repository _auditoriaV2Repository;
        private readonly UsuarioV2Repository _usuarioV2Repository;
        private WireMockHelper WireMockHelper { get; }

        private static ServiceRequest PostAutenticacion(Uri uriBase, AutenticacionClaveNumericaModelRequest autenticacionClaveNumericaModelRequestV2)
        {
            var uri = new Uri(uriBase, ApiUris.AutenticacionConClaveNumericaV2);

            return ServiceRequest.Post(uri.AbsoluteUri, autenticacionClaveNumericaModelRequestV2);
        }

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostAutenticacion(
                _uriBase, 
                new AutenticacionClaveNumericaModelRequest
                {
                    IdPais = 80, 
                    IdTipoDocumento = 4, 
                    Clave = "clave",
                    NroDocumento = "nroDocumento"
                })
        };

        public AutenticacionConClaveNumericaIntegrationTest(ServerFixture server) : base(server)
        {
            _server = server;
            _uriBase = server.HttpServer.TestServer.BaseAddress;

            var dbV2 = server.HttpServer.TestServer.Services.GetRequiredService<GenericDbContextV2>();

            _auditoriaV2Repository = new AuditoriaLogV2Repository(dbV2);
            _usuarioV2Repository = new UsuarioV2Repository(dbV2);

            WireMockHelper = server.WireMock;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task AutenticacionConClaveNumericaUsuarioDeNuevoModelo(
            int paisId,
            string clave,
            string nroDocumento,
            int tipoDocumentoId,
            HttpStatusCode httpStatusCode,
            string mensaje = null,
            int? expectedPersonId = null,
            string estadoPassword = null,
            DateTime? passwordExpirationDate = null)
        {
            // Arrange
            var autenticacionModelRequestV2 = new AutenticacionClaveNumericaModelRequest
            {
                IdPais = paisId,
                Clave = clave,
                NroDocumento = nroDocumento,
                IdTipoDocumento = tipoDocumentoId
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

            if (expectedPersonId != null && httpStatusCode == HttpStatusCode.Accepted)
            {
                var response = await sut.Content.ReadAsAsync<AutenticacionClaveNumericaModelResponse>();

                response.PersonId.Should().Be(expectedPersonId);
                response.EstadoPassword.Should().Be(estadoPassword);
                response.FechaExpiracionPassword.Should().Be(passwordExpirationDate);
            }
        }

        [Theory]
        [MemberData(nameof(DatosWsNsbt))]
        public async Task AutenticacionConClaveNumericaUsuarioDeWsNsbt(
            string clave,
            HttpStatusCode httpStatusCode,
            string file,
            string mensaje = null)
        {
            // Arrange
            var autenticacionModelRequestV2 = new AutenticacionClaveNumericaModelRequest
            {
                IdPais = 80,
                Clave = clave,
                NroDocumento = "12345678",
                IdTipoDocumento = 1
            };

            WireMockHelper.ServiceMock.Reset();

            var request = PostAutenticacion(_uriBase, autenticacionModelRequestV2);

            var response = $"../../../ExternalServices/XmlResponses/NSBTWS/{file}.xml";

            var path = $"{NsbtWsUris.Execute()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.WebService(path))
                .WithTitle(file)
                .RespondWith(WireMockHelper.Xml(response));

            var personaPath = $"{ApiPersonasUris.Persona("12345678", 1, 80)}";

            var expectedPerson = new PersonaModelResponse
            {
                id = 100,
                tipo_persona = "Test tipo_persona",
                links = new Links
                {
                    empty = false
                }
            };

            WireMockHelper.ServiceMock
            .Given(WireMockHelper.Get(personaPath))
            .WithTitle("PersonaOk_WS_NSBT")
            .RespondWith(WireMockHelper.Json(expectedPerson));


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
                var result = await sut.Content.ReadAsAsync<AutenticacionClaveNumericaModelResponse>();

                result.PersonId.Should().Be(100);
                result.EstadoPassword.Should().Be(mensaje);
            }

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task AutenticacionConClaveNumerica_Usuario_Incorrecto()
        {
            // Arrange
            var autenticacionModelRequest = new AutenticacionClaveNumericaModelRequest
            {
                Clave = "Info1212",
                NroDocumento = "12345678",
                IdPais = 80,
                IdTipoDocumento = 1
            };

            var request = PostAutenticacion(_uriBase, autenticacionModelRequest);

            const string response = "../../../ExternalServices/XmlResponses/NSBTWS/PIN_ERROR.xml";

            var path = $"{NsbtWsUris.Execute()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.WebService(path))
                .WithTitle("NSBT_ERROR")
                .RespondWith(WireMockHelper.Xml(response));

            // Act
            var sut = await SendAsync(request);

            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var errorDetailModel = await sut.Content.ReadAsAsync<ErrorDetailModel>();

            var error = errorDetailModel.Errors.First();

            error.Detail.Should().Be(ErrorCode.UsuarioIncorrecto.ErrorDescription);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task AutenticacionConClaveNumerica_Correcta_Con_Auditoria()
        {
            // Arrange
            var autenticacionModelRequest = new AutenticacionClaveNumericaModelRequest
            {
                Clave = "Info1212",
                NroDocumento = "33701133",
                IdPais = 80,
                IdTipoDocumento = 1
            };

            var request = PostAutenticacion(_uriBase, autenticacionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var usuarioBd = _usuarioV2Repository.ObtenerUsuarioByDocumentNumber(
                autenticacionModelRequest.NroDocumento).FirstOrDefault(x =>
                x.Username.Equals("uZFY6DG+JqTZWHQzaUozviEia1wE2vUEYlICQVujlUI"));

            var auditorias = _auditoriaV2Repository.Filter(a => a.UserId == usuarioBd.GetUserId()).ToList();

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Accepted);

            auditorias.Should().NotBeEmpty();
            auditorias.Count.Should().BeGreaterThan(0);

            usuarioBd.Should().NotBeNull();

            var auditoria = auditorias.FirstOrDefault();

            if (auditoria != null)
            {
                auditoria.EventTypeId.Should().Be(7);
                auditoria.EventResultId.Should().Be((int)EventResults.Ok);
                auditoria.UserId.Should().Be(usuarioBd?.GetUserId());
            }
        }

        [Fact]
        public async Task AutenticacionConClaveNumerica_Incorrecta_Con_Auditoria_Usuario_Bloqueado()
        {
            // Arrange
            var autenticacionModelRequestV2 = new AutenticacionClaveNumericaModelRequest
            {
                Clave = "Info1212",
                NroDocumento = "48129347",
                IdPais = 80,
                IdTipoDocumento = 1
            };

            var request = PostAutenticacion(_uriBase, autenticacionModelRequestV2);

            // Act
            var sut = await SendAsync(request);
            var usuarioBd = _usuarioV2Repository.ObtenerUsuarioByDocumentNumber(
                autenticacionModelRequestV2.NroDocumento).FirstOrDefault(x =>
                x.Username.Equals("v+PMWDoWC0ce5v3Q7i0wQHtD2wGgqQg8vkZ0kzCtlAg"));

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
                auditoria.EventTypeId.Should().Be(7);
                auditoria.EventResultId.Should().Be((int)EventResults.Error);
                auditoria.UserId.Should().Be(usuarioBd?.GetUserId());
            }
        }

        [Fact]
        public async Task AutenticacionConClaveNumerica_SinHeaders_BodyCorrecto_BadRequest()
        {
            // Arrange
            _server.HttpServer.HttpClient.DefaultRequestHeaders.Clear();

            var uri = new Uri(_uriBase, ApiUris.AutenticacionConClaveNumericaV2);

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
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task AutenticacionConClaveNumerica_HeadersIncorrectos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add("X-User", "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.AutenticacionConClaveNumericaV2);

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
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task AutenticacionConClaveNumerica_HeadersIncompletos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");

            var uri = new Uri(_uriBase, ApiUris.AutenticacionConClaveNumericaV2);

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
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task AutenticacionConClaveNumerica_BodyIncorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.AutenticacionConClaveNumericaV2);

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
            // int paisId, string clave, string nroDocumento, int tipoDocumentoId, HttpStatusCode httpStatusCode, string mensaje = null, int? expectedPersonId = null, string estadoPassword = null
            // Accepted
            new object[]
            {
                80, 
                "Info1212", 
                "12345678", 
                4,
                HttpStatusCode.Accepted, 
                null, 
                14155917, 
                ErrorConstants.CodigoPasswordNoExpirado, 
                DateTime.Today.AddDays(180)
            }
            ,
            // Datos Correctos
            new object[]
            {
                80,
                "Info1212",
                "41234567",
                1,
                HttpStatusCode.Accepted,
                null,
                4,
                ErrorConstants.CodigoPasswordExpirado,
                DateTime.Today.AddDays(-1)
            }, // Clave Vencida

            // Bad Request
            new object[] { null, "", "", 0, HttpStatusCode.BadRequest },
            new object[] { null, "", "11222333", 0, HttpStatusCode.BadRequest },
            new object[] { null, "123","", 0, HttpStatusCode.BadRequest },
            new object[] { null, "123","11222333", 0, HttpStatusCode.BadRequest },
            new object[] { 0, "", "", 0, HttpStatusCode.BadRequest },
            new object[] { 0, "", "11222333", 0, HttpStatusCode.BadRequest },
            new object[] { 0, "123","", 0, HttpStatusCode.BadRequest },
            new object[] { null, null, null, null, HttpStatusCode.BadRequest },
            new object[] { null, null, "11222333", 0, HttpStatusCode.BadRequest },
            new object[] { null, "123", null, null, HttpStatusCode.BadRequest },
            new object[] { null, "123","11222333", 0, HttpStatusCode.BadRequest },
            new object[] { 0, null, null, null, HttpStatusCode.BadRequest },
            new object[] { 0, null, "11222333", 0, HttpStatusCode.BadRequest },
            new object[] { 0, "123", null, null, HttpStatusCode.BadRequest },

            // string countryId, string clave, string nroDocumento, int documentTypeId, HttpStatusCode httpStatusCode, string mensaje = null
            // Unauthorized
            new object[]
            {
                80,
                "4286",
                "48129349",
                1,
                HttpStatusCode.Unauthorized,
                ErrorCode.UsuarioSuspendido.ErrorDescription
            }, // Usuario Suspendido
            new object[]
            {
                80,
                "Info1212",
                "51234567",
                1,
                HttpStatusCode.Unauthorized,
                ErrorCode.UsuarioBloqueado.ErrorDescription
            }, // Usuario Bloqueado
            new object[]
            {
                80,
                "Info1212",
                "21234567",
                4,
                HttpStatusCode.Unauthorized,
                ErrorCode.UsuarioInactivo.ErrorDescription
            }, // Usuario Inactivo --
            new object[]
            {
                80,
                "Info1213",
                "31234567",
                1,
                HttpStatusCode.Unauthorized,
                ErrorCode.UsuarioIncorrecto.ErrorDescription
            }
        };

        public static IEnumerable<object[]> DatosWsNsbt =>
        new List<object[]>
        {
            // Accepted
            new object[]
            {
                "Info1212", 
                HttpStatusCode.Accepted, 
                "PIN_OK", 
                ErrorConstants.CodigoPasswordNoExpirado
            }, // Datos Correctos
            new object[]
            {
                "Info1212", 
                HttpStatusCode.Accepted, 
                "PIN_OK_VENCIDO", 
                ErrorConstants.CodigoPasswordExpiradoBta
            }, // Datos Correctos con pin vencido

            // Unauthorized
            new object[]
            {
                "Info1213", 
                HttpStatusCode.Unauthorized, 
                "PIN_OK", 
                ErrorCode.UsuarioIncorrecto.ErrorDescription
            }, // clave incorrecta
            new object[]
            {
                "Info1212", 
                HttpStatusCode.Unauthorized, 
                "PIN_ERROR", 
                ErrorCode.UsuarioIncorrecto.ErrorDescription
            }, // Datos incorrectos al consultar a WS
            new object[]
            {
                "Info1212", 
                HttpStatusCode.Unauthorized,
                "PIN_OK_BLOQUEADO",
                ErrorCode.UsuarioBloqueado.ErrorDescription
            }, // Datos Correctos con usuario bloqueado por intentos
        };
    }
}
