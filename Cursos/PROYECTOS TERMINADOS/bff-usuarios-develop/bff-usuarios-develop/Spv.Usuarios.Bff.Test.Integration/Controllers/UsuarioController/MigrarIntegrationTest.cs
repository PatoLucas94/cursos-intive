using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.UsuarioController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class MigrarIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        public WireMockHelper WireMockHelper { get; set; }

        public Dictionary<string, string> ErrorHeaders = new Dictionary<string, string>
        {
            { HeaderNames.RequestIdHeaderName , "ErrorUser" }
        };

        public Dictionary<string, string> SuccessHeaders = new Dictionary<string, string>
        {
            { HeaderNames.RequestIdHeaderName , "SuccessUser" }
        };

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest PostMigrar(Uri uriBase, MigracionModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.MigracionV2);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public MigrarIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task MigracionAsyncCreatedAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.Migracion()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .WithTitle("MigracionAsyncCreatedAsync")
                .RespondWith(WireMockHelper.RespondWithCreated());

            var request = PostMigrar(
                _uriBase,
                new MigracionModelRequest
                {

                    IdPersona = "123456",
                    Usuario = "TestUser0",
                    Clave = "4132"
                });

            // Act
            var sut = await SendAsync(request, requestId: "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status201Created);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task MigracionAsyncBadRequestAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.Migracion()}";

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("400", "", "", "Error de prueba BadRequest", "")
                }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, ErrorHeaders))
                .WithTitle("Migracion-BadRequest")
                .RespondWith(WireMockHelper.RespondWithBadRequest(response));

            var request = PostMigrar(
                _uriBase,
                new MigracionModelRequest
                {
                    IdPersona = "123456",
                    Usuario = "TestUser0",
                    Clave = "4132"
                });

            // Act
            var sut = await SendAsync(request, requestId: "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();

            result.Errores.First().Detalle.Should().Be(response.Errores.First().Detalle);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task MigracionAsyncConflict()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.Migracion()}";

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("409", "", "", "Error de prueba Conflict", "")
                }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, ErrorHeaders))
                .WithTitle("Migracion-Conflict")
                .RespondWith(WireMockHelper.RespondWithConflict(response));

            var request = PostMigrar(
                _uriBase,
                new MigracionModelRequest
                {
                    IdPersona = "123456",
                    Usuario = "TestUser0",
                    Clave = "4132"
                });

            // Act
            var sut = await SendAsync(request, requestId: "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status409Conflict);

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();

            result.Errores.First().Detalle.Should().Be(response.Errores.First().Detalle);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
