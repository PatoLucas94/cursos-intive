using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.UsuarioController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class PerfilIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        public WireMockHelper WireMockHelper { get; set; }

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest GetPerfil(Uri uriBase, string username)
        {
            var uri = new Uri(uriBase, ApiUris.Perfil(username));

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        public PerfilIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task PerfilOkAsync()
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
                .WithTitle("PerfilOkAsync")
                .RespondWith(WireMockHelper.Json(apiUsuarioPerfil));

            var request = GetPerfil(_uriBase, "usuarioTestExitoso");

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<PerfilModelResponse>();

            result.Should().NotBeNull();
            result.Id.Should().Be("PersonId1");

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task PerfilErrorAsync()
        {
            // Arrange

            var path = $"{ApiUsuariosUris.Perfil("usuarioTestError")}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("PerfilErrorAsync")
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            var request = GetPerfil(_uriBase, "usuarioTestError");

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeFalse();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
