using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class UsuarioRepositoryV2IntegrationTest
    {
        private readonly IApiUsuariosRepositoryV2 _usuarioRepositoryV2;
        public WireMockHelper WireMockHelper { get; set; }

        public Dictionary<string, string> SuccessHeaders = new Dictionary<string, string>
        {
            { HeaderNames.UserHeaderName , "SuccessUser" },
            { HeaderNames.ChannelHeaderName , "SuccessChannel" }
        };

        public UsuarioRepositoryV2IntegrationTest(ServerFixture server)
        {
            var usuarioRepositoryV2 = server.HttpServer.TestServer.Services.GetRequiredService<IApiUsuariosRepositoryV2>();

            _usuarioRepositoryV2 = usuarioRepositoryV2;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task ValidarCanalesOkAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.ValidacionClaveCanales()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("ValidarCanalesOkAsync")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            // Act
            var result = await _usuarioRepositoryV2.ValidarClaveCanalesAsync(new ApiUsuariosValidacionClaveCanalesModelInput());

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
            result.StatusCode.Should().Be(HttpStatusCode.Accepted);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarCanalesErrorAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.ValidacionClaveCanales()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("ValidarCanalesErrorAsync")
                .RespondWith(WireMockHelper.RespondWithUnauthorized());

            // Act
            var result = await _usuarioRepositoryV2.ValidarClaveCanalesAsync(new ApiUsuariosValidacionClaveCanalesModelInput());

            // Assert
            result.IsSuccessStatusCode.Should().Be(false);
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task RegistrarV2OkAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.RegistracionV2()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("RegistrarV2OkAsync")
                .RespondWith(WireMockHelper.RespondWithCreated());

            // Act
            var result = await _usuarioRepositoryV2.RegistrarUsuarioV2Async(new ApiUsuariosRegistracionV2ModelInput());

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
            result.StatusCode.Should().Be(HttpStatusCode.Created);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task RegistrarV2ErrorAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.RegistracionV2()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Repo-RegistrarV2-BadRequest")
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            // Act
            var result = await _usuarioRepositoryV2.RegistrarUsuarioV2Async(new ApiUsuariosRegistracionV2ModelInput());

            // Assert
            result.IsSuccessStatusCode.Should().Be(false);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Arrange
            WireMockHelper.ResetMapping();

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Repo-RegistrarV2-Conflict")
                .RespondWith(WireMockHelper.RespondWithConflict());

            // Act
            result = await _usuarioRepositoryV2.RegistrarUsuarioV2Async(new ApiUsuariosRegistracionV2ModelInput());

            // Assert
            result.IsSuccessStatusCode.Should().Be(false);
            result.StatusCode.Should().Be(HttpStatusCode.Conflict);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaOkAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Repo-Validacion-Existencia-Created")
                .RespondWith(WireMockHelper.RespondWithCreated());

            // Act
            var result = await _usuarioRepositoryV2.ValidarExistenciaAsync(new ApiUsuariosValidacionExistenciaModelInput());

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
            result.StatusCode.Should().Be(HttpStatusCode.Created);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaErrorAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Repo-Validacion-Existencia-NotFound")
                .RespondWith(WireMockHelper.RespondWithNotFound());

            // Act
            var result = await _usuarioRepositoryV2.ValidarExistenciaAsync(new ApiUsuariosValidacionExistenciaModelInput());

            // Assert
            result.IsSuccessStatusCode.Should().Be(false);
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task MigrarOkAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.Migracion()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("MigrarOkAsync")
                .RespondWith(WireMockHelper.RespondWithCreated());

            // Act
            var result = await _usuarioRepositoryV2.MigrarUsuarioAsync(new ApiUsuariosMigracionModelInput());

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
            result.StatusCode.Should().Be(HttpStatusCode.Created);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task MigrarAsyncErrorBadRequest()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.Migracion()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Repo-Migrar-BadRequest")
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            // Act
            var result = await _usuarioRepositoryV2.MigrarUsuarioAsync(new ApiUsuariosMigracionModelInput());

            // Assert
            result.IsSuccessStatusCode.Should().Be(false);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task MigrarAsyncErrorConflict()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.Migracion()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Repo-Migrar-Conflict")
                .RespondWith(WireMockHelper.RespondWithConflict());

            // Act
            var result = await _usuarioRepositoryV2.MigrarUsuarioAsync(new ApiUsuariosMigracionModelInput());

            // Assert
            result.IsSuccessStatusCode.Should().Be(false);
            result.StatusCode.Should().Be(HttpStatusCode.Conflict);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
