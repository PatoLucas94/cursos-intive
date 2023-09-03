using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.UsuarioController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class CambioDeCredencialesIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        private WireMockHelper WireMockHelper { get; set; }

        private static readonly string nroDocumentoPersonaCorrecta = "23734745";
        private static readonly string nroDocumentoPersonaCorrecta2 = "12345678";
        private static readonly int idPersonaCorrecta = 14155917;
        private static readonly int idPersonaCorrecta2 = 11112222;

        private static IEnumerable<ApiUsuariosPerfilModelOutputV2> ApiUsuariosPerfilOutput =>
        new List<ApiUsuariosPerfilModelOutputV2>
        {
            new ApiUsuariosPerfilModelOutputV2
            {
                nro_documento = nroDocumentoPersonaCorrecta,
                tipo_documento = 1,
                email = "mail@mail.com",
                id_persona = idPersonaCorrecta
            },
            new ApiUsuariosPerfilModelOutputV2
            {
                nro_documento = nroDocumentoPersonaCorrecta2,
                tipo_documento = 1,
                email = "mail@mail.com",
                id_persona = idPersonaCorrecta2
            }
        };

        public CambioDeCredencialesIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest CambiarCredenciales(Uri uriBase, CambioDeCredencialesModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.CambioDeCredenciales);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public Dictionary<string, string> SuccessHeaders = new Dictionary<string, string>
        {
            { HeaderNames.RequestIdHeaderName , "SuccessUser" }
        };

        public Dictionary<string, string> ErrorHeaders = new Dictionary<string, string>
        {
            { HeaderNames.RequestIdHeaderName , "ErrorUser" }
        };

        [Fact]
        public async Task ModificarCredencialesOkAsync()
        {
            // Arrange
            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.CambioDeCredenciales()}"))
                .RespondWith(WireMockHelper.RespondWithStatusCode(HttpStatusCode.OK));

            var path = $"{ApiUsuariosUris.InhabilitacionClaveCanales()}";
            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(idPersonaCorrecta)}";
            var pathNotificaciones = $"{ApiNotificacionesUris.EnviarEmail()}";
            var pathValidacionClaveCanales = ApiUsuariosUris.ValidacionClaveCanales();

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionClaveCanales))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .WithTitle("ModificarCredencialesOkAsync-InhabilitacionClaveCanales")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.Get(pathPerfilV2))
                   .WithTitle("ModificarCredencialesOkAsync")
                   .RespondWith(WireMockHelper.Json(ApiUsuariosPerfilOutput.First(x => x.id_persona == idPersonaCorrecta)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(pathNotificaciones, SuccessHeaders))
                .WithTitle("ModificarCredencialesOkAsync-EnviarEmail")
                .RespondWith(WireMockHelper.RespondWithAccepted(
                    new ApiNotificacionesEnviarEmailModelOutput
                    {
                        id = 1,
                        estado = "PENDIENTE_ENVIO"
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(ApiNotificacionesUris.EnviarEmail(), SuccessHeaders))
                .WithTitle("ModificarCredencialesOkAsync-EnviarEmail")
                .RespondWith(WireMockHelper.RespondWithAccepted(
                    new ApiNotificacionesEnviarEmailModelOutput
                    {
                        id = 1,
                        estado = "PENDIENTE_ENVIO"
                    }));

            var request = CambiarCredenciales(
                _uriBase,
                new CambioDeCredencialesModelRequest
                {
                    IdPersona = idPersonaCorrecta.ToString(),
                    NuevaClave = "1423",
                    NuevoUsuario = "UserTest01",
                    ClaveCanales = "12345678"
                });

            // Act
            var sut = await SendAsync(request, requestId: "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status200OK);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ModificarCredencialesAsyncAsync_Conflict_CredencialesYaUtilizadasAsync()
        {
            // Arrange
            var response = new ApiUsuariosErrorResponse
            {
                Codigo = 409,
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("CREDYU", "", "", "Error de prueba Conflict", "")
                }
            };

            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(idPersonaCorrecta)}";
            var pathValidacionClaveCanales = ApiUsuariosUris.ValidacionClaveCanales();

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionClaveCanales))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.Get(pathPerfilV2))
                   .WithTitle("ModificarCredencialesOkAsync_Conflict_CredencialesYaUtilizadasAsync")
                   .RespondWith(WireMockHelper.Json(ApiUsuariosPerfilOutput.First(x => x.id_persona == idPersonaCorrecta)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.CambioDeCredenciales()}"))
                .WithTitle("ModificarCredencialesAsyncAsync_Conflict_CredencialesYaUtilizadasAsync")
                .RespondWith(WireMockHelper.RespondWithConflict(response));

            var request = CambiarCredenciales(
                _uriBase,
                new CambioDeCredencialesModelRequest
                {
                    IdPersona = idPersonaCorrecta.ToString(),
                    NuevaClave = "1423",
                    NuevoUsuario = "UserTest01",
                    ClaveCanales = "12345678"
                });

            // Act
            var sut = await SendAsync(request, requestId: "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status409Conflict);

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();

            result.Errores.First().Detalle.Should().Be(response.Errores.First().Detalle);
            result.Errores.First().Codigo.Should().Be(response.Errores.First().Codigo);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ModificarCredencialesAsyncAsync_Conflict_UsuarioYaUtilizadoAsync()
        {
            // Arrange
            var response = new ApiUsuariosErrorResponse
            {
                Codigo = 409,
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("UYU", "", "", "Error de prueba Conflict", "")
                }
            };

            var pathValidacionClaveCanales = ApiUsuariosUris.ValidacionClaveCanales();
            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(idPersonaCorrecta)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionClaveCanales))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.Get(pathPerfilV2))
                   .WithTitle("ModificarCredencialesOkAsync_Conflict_UsuarioYaUtilizadoAsync")
                   .RespondWith(WireMockHelper.Json(ApiUsuariosPerfilOutput.First(x => x.id_persona == idPersonaCorrecta)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.CambioDeCredenciales()}"))
                .WithTitle("ModificarCredencialesAsyncAsync_Conflict_UsuarioYaUtilizadoAsync")
                .RespondWith(WireMockHelper.RespondWithConflict(response));

            var request = CambiarCredenciales(
                _uriBase,
                new CambioDeCredencialesModelRequest
                {
                    IdPersona = idPersonaCorrecta.ToString(),
                    NuevaClave = "1423",
                    NuevoUsuario = "UserTest01",
                    ClaveCanales = "12345678"
                });

            // Act
            var sut = await SendAsync(request, requestId: "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status409Conflict);

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();

            result.Errores.First().Detalle.Should().Be(response.Errores.First().Detalle);
            result.Errores.First().Codigo.Should().Be(response.Errores.First().Codigo);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ModificarCredencialesAsyncAsync_Conflict_ClaveYaUtilizadaAsync()
        {
            // Arrange
            var response = new ApiUsuariosErrorResponse
            {
                Codigo = 409,
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("CYU", "", "", "Error de prueba Conflict", "")
                }
            };

            var pathValidacionClaveCanales = ApiUsuariosUris.ValidacionClaveCanales();
            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(idPersonaCorrecta)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionClaveCanales))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.Get(pathPerfilV2))
                   .WithTitle("ModificarCredencialesOkAsync_Conflict_ClaveYaUtilizadaAsync")
                   .RespondWith(WireMockHelper.Json(ApiUsuariosPerfilOutput.First(x => x.id_persona == idPersonaCorrecta)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.CambioDeCredenciales()}"))
                .WithTitle("ModificarCredencialesAsyncAsync_Conflict_ClaveYaUtilizadaAsync")
                .RespondWith(WireMockHelper.RespondWithConflict(response));

            var request = CambiarCredenciales(
                _uriBase,
                new CambioDeCredencialesModelRequest
                {
                    IdPersona = idPersonaCorrecta.ToString(),
                    NuevaClave = "1423",
                    NuevoUsuario = "UserTest01",
                    ClaveCanales = "12345678"
                });

            // Act
            var sut = await SendAsync(request, requestId: "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status409Conflict);

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();

            result.Errores.First().Detalle.Should().Be(response.Errores.First().Detalle);
            result.Errores.First().Codigo.Should().Be(response.Errores.First().Codigo);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ModificarCredencialesAsyncAsyncOk_Inhabilitacion_Clave_ErrorAsync()
        {
            // Arrange
            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.CambioDeCredenciales()}"))
                .WithTitle("ModificarCredencialesAsyncAsyncOk_Inhabilitacion_Clave_ErrorAsync")
                .RespondWith(WireMockHelper.RespondWithStatusCode(HttpStatusCode.OK));

            var path = $"{ApiUsuariosUris.InhabilitacionClaveCanales()}";
            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(idPersonaCorrecta)}";
            var pathValidacionClaveCanales = ApiUsuariosUris.ValidacionClaveCanales();

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionClaveCanales))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, ErrorHeaders))
                .WithTitle("ModificarCredencialesAsyncAsyncOk_Inhabilitacion_Clave_ErrorAsync-InhabilitacionClaveCanales")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.Get(pathPerfilV2))
                   .WithTitle("ModificarCredencialesOkAsync_Inhabilitacion_Clave_ErrorAsync")
                   .RespondWith(WireMockHelper.Json(ApiUsuariosPerfilOutput.First(x => x.id_persona == idPersonaCorrecta)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(ApiNotificacionesUris.EnviarEmail(), SuccessHeaders))
                .WithTitle("ModificarCredencialesAsyncAsyncOk_Inhabilitacion_Clave_ErrorAsync-EnviarEmail")
                .RespondWith(WireMockHelper.RespondWithAccepted(
                    new ApiNotificacionesEnviarEmailModelOutput
                    {
                        id = 1,
                        estado = "PENDIENTE_ENVIO"
                    }));

            var request = CambiarCredenciales(
                _uriBase,
                new CambioDeCredencialesModelRequest
                {
                    IdPersona = idPersonaCorrecta.ToString(),
                    NuevaClave = "1423",
                    NuevoUsuario = "UserTest01",
                    ClaveCanales = "12345678"
                });

            // Act
            var sut = await SendAsync(request, requestId: "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status200OK);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ModificarCredencialesAsyncOk_EnvioMail_ErrorAsync()
        {
            // Arrange
            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.CambioDeCredenciales()}"))
                .WithTitle("ModificarCredencialesAsyncOk_EnvioMail_ErrorAsync")
                .RespondWith(WireMockHelper.RespondWithStatusCode(HttpStatusCode.OK));

            var path = $"{ApiUsuariosUris.InhabilitacionClaveCanales()}";
            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(idPersonaCorrecta)}";
            var pathNotificaciones = $"{ApiNotificacionesUris.EnviarEmail()}";
            var pathValidacionClaveCanales = ApiUsuariosUris.ValidacionClaveCanales();

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionClaveCanales))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .WithTitle("ModificarCredencialesAsyncOk_EnvioMail_ErrorAsync-InhabilitacionClaveCanales")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.Get(pathPerfilV2))
                   .WithTitle("ModificarCredencialesOkAsync_EnvioMail_ErrorAsync")
                   .RespondWith(WireMockHelper.Json(ApiUsuariosPerfilOutput.First(x => x.id_persona == idPersonaCorrecta)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(pathNotificaciones, ErrorHeaders))
                .WithTitle("ModificarCredencialesAsyncOk_EnvioMail_ErrorAsync-Notificaciones")
                .RespondWith(WireMockHelper.RespondWithNotFound());

            var request = CambiarCredenciales(
                _uriBase,
                new CambioDeCredencialesModelRequest
                {
                    IdPersona = idPersonaCorrecta.ToString(),
                    NuevaClave = "1423",
                    NuevoUsuario = "UserTest01",
                    ClaveCanales = "12345678"
                });

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status200OK);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ModificarCredencialesAsync_Clave_Canales_No_Propia()
        {
            // Arrange
            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(idPersonaCorrecta)}";
            var pathValidacionClaveCanales = ApiUsuariosUris.ValidacionClaveCanales();

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionClaveCanales))
                .RespondWith(WireMockHelper.RespondWithUnauthorized());

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.Get(pathPerfilV2))
                   .WithTitle("ModificarCredencialesOkAsync-Clave_Canales_No_Propia")
                   .RespondWith(WireMockHelper.Json(ApiUsuariosPerfilOutput.First(x => x.id_persona == idPersonaCorrecta)));

            var request = CambiarCredenciales(
                _uriBase,
                new CambioDeCredencialesModelRequest
                {
                    IdPersona = idPersonaCorrecta.ToString(),
                    NuevaClave = "1423",
                    NuevoUsuario = "UserTest01",
                    ClaveCanales = "12345678",
                    IsClaveCanales = true
                });

            // Act
            var sut = await SendAsync(request, requestId: "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
