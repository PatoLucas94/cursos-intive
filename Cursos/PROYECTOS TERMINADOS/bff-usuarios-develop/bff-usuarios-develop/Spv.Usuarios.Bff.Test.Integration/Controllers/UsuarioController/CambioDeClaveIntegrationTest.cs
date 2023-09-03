using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.ScoreOperaciones.Output;
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
    public class CambioDeClaveIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        private WireMockHelper WireMockHelper { get; set; }

        private const string NroDocumentoPersonaCorrecta2 = "11122233";
        private const int IdPersonaCorrecta = 11223344;
        private const int IdPersonaCorrecta2 = 112233;

        private static IEnumerable<ApiUsuariosPerfilModelOutputV2> ApiUsuariosPerfilInfoOutput =>
            new List<ApiUsuariosPerfilModelOutputV2>
            {
                new ApiUsuariosPerfilModelOutputV2
                {
                    nro_documento = "12331231",
                    tipo_documento = 1,
                    email = "mail@mail.com",
                    id_persona = IdPersonaCorrecta
                },
                new ApiUsuariosPerfilModelOutputV2
                {
                    nro_documento = NroDocumentoPersonaCorrecta2,
                    tipo_documento = 1,
                    email = "mail@mail.com",
                    id_persona = IdPersonaCorrecta2
                }
            };

        public CambioDeClaveIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest CambiarClave(Uri uriBase, CambioDeClaveModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.CambioDeClave);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public Dictionary<string, string> SuccessHeaders = new Dictionary<string, string>
        {
            { HeaderNames.RequestIdHeaderName, "SuccessUser" }
        };

        public Dictionary<string, string> ErrorHeaders = new Dictionary<string, string>
        {
            { HeaderNames.RequestIdHeaderName, "ErrorUser" }
        };

        [Fact]
        public async Task ModificarClaveOkAsync_Usuario()
        {
            // Arrange
            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.CambioDeClave()}"))
                .RespondWith(WireMockHelper.RespondWithStatusCode(HttpStatusCode.OK));

            var path = $"{ApiUsuariosUris.InhabilitacionClaveCanales()}";
            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(IdPersonaCorrecta)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .WithTitle("ModificarClaveOkAsync-InhabilitacionClaveCanales")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPerfilV2))
                .WithTitle("ModificarClaveOkAsync_Usuario")
                .RespondWith(
                    WireMockHelper.Json(ApiUsuariosPerfilInfoOutput.First(x => x.id_persona == IdPersonaCorrecta)));

            var responseScoreOperaciones = new ApiScoreOperacionesModelOutput();
            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiScoreOperacionesUris.UpdateCredentials()))
                .RespondWith(WireMockHelper.RespondWithAccepted(responseScoreOperaciones));

            var request = CambiarClave(
                _uriBase,
                new CambioDeClaveModelRequest
                {
                    IdPersona = IdPersonaCorrecta.ToString(),
                    NuevaClave = "Info1212",
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
        public async Task ModificarClaveAsync_Conflict_ClaveYaUtilizadaAsync()
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

            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(IdPersonaCorrecta2)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPerfilV2))
                .WithTitle("ModificarClaveOkAsync_Conflict_ClaveYaUtilizadaAsync")
                .RespondWith(
                    WireMockHelper.Json(ApiUsuariosPerfilInfoOutput.First(x => x.id_persona == IdPersonaCorrecta2)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.CambioDeClave()}"))
                .WithTitle("ModificarClaveAsync_Conflict_ClaveYaUtilizadaAsync")
                .RespondWith(WireMockHelper.RespondWithConflict(response));

            var responseScoreOperaciones = new ApiScoreOperacionesModelOutput();
            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiScoreOperacionesUris.UpdateCredentials()))
                .RespondWith(WireMockHelper.RespondWithAccepted(responseScoreOperaciones));


            var request = CambiarClave(
                _uriBase,
                new CambioDeClaveModelRequest
                {
                    IdPersona = IdPersonaCorrecta2.ToString(),
                    NuevaClave = "Info1212",
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
        public async Task ModificarClaveAsyncOk_Inhabilitacion_Clave_ErrorAsync()
        {
            // Arrange
            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.CambioDeClave()}"))
                .WithTitle("ModificarClaveAsyncOk_Inhabilitacion_Clave_ErrorAsync")
                .RespondWith(WireMockHelper.RespondWithStatusCode(HttpStatusCode.OK));

            var path = $"{ApiUsuariosUris.InhabilitacionClaveCanales()}";
            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(IdPersonaCorrecta)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, ErrorHeaders))
                .WithTitle("ModificarClaveAsyncOk_Inhabilitacion_Clave_ErrorAsync-InhabilitacionClaveCanales")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPerfilV2))
                .WithTitle("ModificarClaveOkAsync_Inhabilitacion_Clave_ErrorAsync")
                .RespondWith(
                    WireMockHelper.Json(ApiUsuariosPerfilInfoOutput.First(x => x.id_persona == IdPersonaCorrecta)));

            var responseScoreOperaciones = new ApiScoreOperacionesModelOutput();
            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiScoreOperacionesUris.UpdateCredentials()))
                .RespondWith(WireMockHelper.RespondWithAccepted(responseScoreOperaciones));

            var request = CambiarClave(
                _uriBase,
                new CambioDeClaveModelRequest
                {
                    IdPersona = IdPersonaCorrecta.ToString(),
                    NuevaClave = "Info1212",
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
        public async Task ModificarClaveAsync_Clave_Canales_No_Propia()
        {
            // Arrange
            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(IdPersonaCorrecta)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithUnauthorized());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPerfilV2))
                .WithTitle("ModificarClaveOkAsync-NoPropia")
                .RespondWith(
                    WireMockHelper.Json(ApiUsuariosPerfilInfoOutput.First(x => x.id_persona == IdPersonaCorrecta)));

            var responseScoreOperaciones = new ApiScoreOperacionesModelOutput();
            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiScoreOperacionesUris.UpdateCredentials()))
                .RespondWith(WireMockHelper.RespondWithAccepted(responseScoreOperaciones));

            var request = CambiarClave(
                _uriBase,
                new CambioDeClaveModelRequest
                {
                    IdPersona = IdPersonaCorrecta.ToString(),
                    NuevaClave = "Info1212",
                    ClaveCanales = "12345678",
                    IsClaveCanales = true
                });

            // Act
            var sut = await SendAsync(request, requestId: "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
