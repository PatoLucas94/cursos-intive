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
    public class PerfilV2IntegrationTest : ControllerIntegrationTest
    {
        private const long IdPersonaCorrecto = 1447;
        private const long IdPersonaIncorrecto = 1111;

        private readonly Uri _uriBase;
        public WireMockHelper WireMockHelper { get; set; }
        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest GetPerfil(Uri uriBase, long idPersona)
        {
            var uri = new Uri(uriBase, ApiUris.PerfilV2(idPersona));

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        public PerfilV2IntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task PerfilOkAsync()
        {
            // Arrange
            var apiUsuarioPerfil = new ApiUsuariosPerfilModelOutputV2
            {
                ultimo_login = DateTime.MinValue,
                id_persona = IdPersonaCorrecto,
                nro_documento = "11223344",
                tipo_documento = 1,
                email = "test@test.com",
                nombre = "Usuario1",
                apellido = "Test1",
                genero = "masculino",
                pais = 80,
                fecha_ultimo_cambio_clave = DateTime.MinValue,
                fecha_vencimiento_clave = DateTime.MinValue
            };

            var path = $"{ApiUsuariosUris.PerfilPathV2(IdPersonaCorrecto)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("PerfilOkAsync")
                .RespondWith(WireMockHelper.Json(apiUsuarioPerfil));

            var request = GetPerfil(_uriBase, IdPersonaCorrecto);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<PerfilModelResponseV2>();

            result.Should().NotBeNull();
            result.Email.Should().Be("test@test.com");

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task PerfilErrorAsync()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.PerfilPathV2(IdPersonaCorrecto)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("PerfilErrorAsync")
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            var request = GetPerfil(_uriBase, IdPersonaIncorrecto);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeFalse();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
