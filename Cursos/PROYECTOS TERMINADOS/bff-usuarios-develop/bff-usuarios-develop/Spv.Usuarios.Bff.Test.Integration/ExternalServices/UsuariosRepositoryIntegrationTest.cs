using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class UsuariosRepositoryIntegrationTest
    {
        private readonly IApiUsuariosRepository _usuarioRepository;
        public WireMockHelper WireMockHelper { get; set; }

        public UsuariosRepositoryIntegrationTest(ServerFixture server)
        {
            var usuarioRepository = server.HttpServer.TestServer.Services.GetRequiredService<IApiUsuariosRepository>();

            _usuarioRepository = usuarioRepository;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task GetPerfilOkAsync()
        {
            // Arrange
            var apiUsuarioPerfil = new ApiUsuariosPerfilModelOutput
            {
                ultimo_login = DateTime.MinValue,
                id_persona = "PersonId1",
                id_usuario = 1,
                email = "test@test.com",
                nombre = "Usuario1",
                apellido = "Test1"
            };

            var path = $"{ApiUsuariosUris.Perfil("usuarioTestExitoso")}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("GetPerfilOkAsync")
                .RespondWith(WireMockHelper.Json(apiUsuarioPerfil));

            // Act
            var result = await _usuarioRepository.ObtenerPerfilAsync("usuarioTestExitoso");

            // Assert
            result.Should().NotBeNull();
            result.ultimo_login.Should().Be(DateTime.MinValue);
            result.id_persona.Should().Be("PersonId1");
            result.id_usuario.Should().Be(1);
            result.email.Should().Be("test@test.com");
            result.nombre.Should().Be("Usuario1");

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task GetPerfilErrorAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.Perfil("usuarioTestError")}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("GetPerfilErrorAsync")
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            // Act
            var result = await _usuarioRepository.ObtenerPerfilAsync("usuarioTestError");

            // Assert
            result.Should().BeNull();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaOkAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.ValidarExistenciaHbi()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Repo-Validacion-Existencia-Created")
                .RespondWith(WireMockHelper.RespondWithCreated());

            // Act
            var result = await _usuarioRepository.ValidarExistenciaAsync(new ApiUsuariosValidacionExistenciaHbiModelInput());

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
            result.StatusCode.Should().Be(HttpStatusCode.Created);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
