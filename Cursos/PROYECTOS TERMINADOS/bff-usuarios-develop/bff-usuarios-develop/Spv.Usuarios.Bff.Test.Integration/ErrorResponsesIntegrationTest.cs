using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.Controllers;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration
{
    [ExcludeFromCodeCoverage]
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ErrorResponsesIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        private WireMockHelper WireMockHelper { get; set; }

        public Dictionary<string, string> ErrorHeaders = new Dictionary<string, string>
        {
            { HeaderNames.RequestIdHeaderName, "ErrorUser" }
        };

        private readonly Dictionary<string, string> _successHeaders = new Dictionary<string, string>
        {
            { HeaderNames.RequestIdHeaderName, "SuccessUser" }
        };

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest PostRegistrarV2(Uri uriBase, RegistracionModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.RegistracionV2);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public ErrorResponsesIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task InternalServerErrorAsync()
        {
            // Arrange

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(ApiPersonasUris.PersonasFisicaInfo(123456)))
                .WithTitle("RegistracionV2-PersonasFisicaInfoExpected")
                .RespondWith(WireMockHelper.RespondWithNotFound());

            var request = PostRegistrarV2(
                _uriBase,
                new RegistracionModelRequest
                {
                    Clave = "4132",
                    IdPais = 80,
                    IdPersona = "123456",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    Usuario = "TestUser0",
                    Telefono = "3518546564",
                    SmsValidado = true,
                    ClaveCanales = "12345678",
                    IdTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b"
                });

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var result = await sut.Content.ReadAsAsync<ErrorDetailModel>();

            result.Code.Should().Be(500);
            result.Type.Should().Be(ErrorTypeConstants.Technical);
            result.Detail.Should().Be(MessageConstants.MensajeGenerico);

            result.Errors.First().Title.Should().Be(MessageConstants.ErrorInterno);
            result.Errors.First().Detail.Should().Be(MessageConstants.PersonaFisicaInexistente(123456));

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ConflictAsync()
        {
            var apiPersonasFisicaInfoExpectedResponse = new ApiPersonasFisicaInfoModelOutput
            {
                id = 123456,
                pais_documento = 80,
                tipo_documento = 4,
                numero_documento = "12345678",
                emails = new List<Email>
                {
                    new Email
                    {
                        id = 29140,
                        direccion = "asdfgh@simtlix.com",
                        canal_creacion = "HBI",
                        principal = true,
                        confiable = false
                    }
                }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.ValidarExistencia()}"))
                .WithTitle("RegistracionV2Async_ValidarExistencia")
                .RespondWith(WireMockHelper.Json(new ValidacionExistenciaModelOutput
                {
                    Migrado = true,
                    IdPersona = 123456,
                    Telefono = "",
                    Usuario = "usertest",
                    IdEstadoUsuario = 3
                }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(ApiPersonasUris.PersonasFisicaInfo(123456)))
                .WithTitle("RegistracionV2-PersonasFisicaInfoExpected")
                .RespondWith(WireMockHelper.Json(apiPersonasFisicaInfoExpectedResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            var request = PostRegistrarV2(
                _uriBase,
                new RegistracionModelRequest
                {
                    Clave = "4132",
                    IdPais = 80,
                    IdPersona = "123456",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    Usuario = "TestUser0",
                    Telefono = "3518546564",
                    SmsValidado = true,
                    ClaveCanales = "12345678",
                    IdTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b"
                });

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status409Conflict);

            var result = await sut.Content.ReadAsAsync<ErrorDetailModel>();

            result.Code.Should().Be(409);
            result.Type.Should().Be(ErrorTypeConstants.Business);
            result.Detail.Should().Be(MessageConstants.MensajeGenerico);

            result.Errors.First().Title.Should().Be(MessageConstants.ErrorCliente);
            result.Errors.First().Detail.Should().Be(MessageConstants.DocumentoYaExiste);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Theory]
        [MemberData(nameof(ErrorsData))]
        public async Task ErrorsFromExternalServiceAsync(
            int statusCode,
            string type,
            string detail,
            string subErrorCode,
            string subErrorTitle,
            string subErrorDetail,
            HttpStatusCode responseStatusCode)
        {
            // Arrange
            const string idTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b";
            var dateTimeNow = DateTime.Now;

            var tycAceptacionResponse = new ApiTyCAceptacionModelOutput
            {
                id_terminos_condiciones = idTyC,
                fecha_aceptacion = dateTimeNow,
                status_code = HttpStatusCode.OK
            };

            var apiPersonasVerificacionTelefonoModelOutputExpectedResponse =
                new ApiPersonasVerificacionTelefonoModelOutput
                {
                    canal_creacion = string.Empty,
                    fecha_verificacion = string.Empty,
                    id = 1,
                    id_telefono = 12,
                    usuario_creacion = string.Empty,
                    verificado = true
                };

            var apiUsuariosRegistracionErrorResponse = new ApiUsuariosErrorResponse
            {
                Estado = "InternalServerError",
                Codigo = 500,
                Tipo = "interno",
                Detalle = "Texto que contiene ruta de endpoint consumido",
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError(
                        subErrorCode,
                        "Título que contiene ruta de endpoint consumido",
                        "Origen",
                        DetalleApiUsuariosError,
                        "spvTrackId")
                }
            };

            var path = $"{ApiUsuariosUris.RegistracionV2()}";

            SetupCommonWireMocks();

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiTyCUris.Aceptados()}"))
                .WithTitle("RegistracionV2CreatedAsync-TyCAceptacion")
                .RespondWith(WireMockHelper.Json(tycAceptacionResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.TelefonosCreacionPath(123456)}",
                    _successHeaders))
                .WithTitle("RegistracionV2-CreacionTelefono-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.EmailsCreacionPath(123456)}", _successHeaders))
                .WithTitle("RegistracionV2-CreacionEmail-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionEmailModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.TelefonoDobleFactorPath(123456)}",
                    _successHeaders))
                .WithTitle("RegistracionV2-CreacionTelefonoDobleFactor-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput { id = 1 }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.TelefonoVerificacionPath(1)}"))
                .WithTitle("RegistracionV2-VerificarTelefono-OK")
                .RespondWith(WireMockHelper.Json(apiPersonasVerificacionTelefonoModelOutputExpectedResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(ApiNotificacionesUris.EnviarEmail(), _successHeaders))
                .WithTitle("RegistracionV2-EnviarEmail")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, _successHeaders))
                .WithTitle("RegistracionV2-Created")
                .RespondWith(WireMockHelper.RespondWithStatusCode(responseStatusCode,
                    apiUsuariosRegistracionErrorResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            var request = PostRegistrarV2(
                _uriBase,
                new RegistracionModelRequest
                {
                    Clave = "4132",
                    IdPais = 80,
                    IdPersona = "123456",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    Usuario = "TestUser0",
                    Telefono = "3518546564",
                    SmsValidado = true,
                    ClaveCanales = "12345678",
                    IdTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b"
                });

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(responseStatusCode);

            var result = await sut.Content.ReadAsAsync<ErrorDetailModel>();

            result.Code.Should().Be(statusCode);
            result.Type.Should().Be(type);
            result.Detail.Should().Be(detail);

            result.Errors.First().Code.Should().Be(subErrorCode);
            result.Errors.First().Title.Should().Be(subErrorTitle);
            result.Errors.First().Detail.Should().Be(subErrorDetail);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        private void SetupCommonWireMocks()
        {
            var apiUsuariosErrorResponse = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", MessageConstants.DocumentoYaExiste, "")
                }
            };

            var apiPersonasFisicaInfoExpectedResponse = new ApiPersonasFisicaInfoModelOutput
            {
                id = 123456,
                pais_documento = 80,
                tipo_documento = 4,
                numero_documento = "12345678",
                emails = new List<Email>
                {
                    new Email
                    {
                        id = 29140,
                        direccion = "asdfgh@simtlix.com",
                        canal_creacion = "HBI",
                        principal = true,
                        confiable = false
                    }
                }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.ValidarExistencia()}"))
                .WithTitle("RegistracionV2Async_ValidarExistencia")
                .RespondWith(WireMockHelper.RespondWithNotFound(apiUsuariosErrorResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(ApiPersonasUris.PersonasFisicaInfo(123456)))
                .WithTitle("RegistracionV2-PersonasFisicaInfoExpected")
                .RespondWith(WireMockHelper.Json(apiPersonasFisicaInfoExpectedResponse));
        }

        const string DetalleApiUsuariosError = "Detalle api-usuarios";

        // int statusCode, string type, string detail, string subErrorCode, string subErrorTitle, string subErrorDetail 
        public static IEnumerable<object[]> ErrorsData =>
            new List<object[]>
            {
                new object[]
                {
                    500,
                    ErrorTypeConstants.Technical,
                    MessageConstants.MensajeGenerico,
                    "500",
                    MessageConstants.ErrorInterno,
                    DetalleApiUsuariosError,
                    HttpStatusCode.InternalServerError
                },
                new object[]
                {
                    400,
                    null,
                    MessageConstants.MensajeGenerico,
                    "400",
                    MessageConstants.ErrorCliente,
                    DetalleApiUsuariosError,
                    HttpStatusCode.BadRequest
                },
                new object[]
                {
                    404,
                    ErrorTypeConstants.Business,
                    MessageConstants.MensajeGenerico,
                    "404",
                    MessageConstants.ErrorCliente,
                    DetalleApiUsuariosError,
                    HttpStatusCode.NotFound
                },
                new object[]
                {
                    401,
                    ErrorTypeConstants.Business,
                    MessageConstants.MensajeGenerico,
                    "401",
                    MessageConstants.ErrorCliente,
                    DetalleApiUsuariosError,
                    HttpStatusCode.Unauthorized
                },
                new object[]
                {
                    409,
                    ErrorTypeConstants.Business,
                    MessageConstants.MensajeGenerico,
                    "409",
                    MessageConstants.ErrorCliente,
                    DetalleApiUsuariosError,
                    HttpStatusCode.Conflict
                }
            };
    }
}
