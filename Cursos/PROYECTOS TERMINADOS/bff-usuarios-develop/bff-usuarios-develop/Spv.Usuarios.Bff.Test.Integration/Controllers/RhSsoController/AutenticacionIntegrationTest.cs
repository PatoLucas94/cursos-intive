using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.RhSsoService.Output;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.Controllers.TyCController;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.RhSsoController.CommonRhSso.Input;
using Spv.Usuarios.Bff.ViewModels.RhSsoController.CommonRhSso.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.RhSsoController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class AutenticacionIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        private WireMockHelper WireMockHelper { get; }

        private static ServiceRequest Autenticacion(Uri uriBase, AutenticacionModelRequest model) =>
            ServiceRequest.Post(new Uri(uriBase, ApiUris.Autenticacion).AbsoluteUri, model);

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        public AutenticacionIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task AceptadosOkAsync()
        {
            // Arrange
            var usuario = "jhon-doe";
            var clave = "123456";
            var numeroDoc = "124578";
            var canal = "OBI";
            var concepto = "ALTAUSUARIODIGITAL";
            var autenticacion = ApiUsuariosUris.SsoAutenticacion;
            var tokenResponse = new TokenModelOutput
            {
                AccessToken = "bearer_token",
                ExpiresIn = 256,
                RefreshToken = "refresh_bearer_token",
                RefreshExpiresIn = 123
            };
            VigenteIntegrationTest.ObtenerVigenteMock(WireMockHelper, canal, concepto);

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(autenticacion))
                .RespondWith(WireMockHelper.Json(tokenResponse));

            // Act
            var request = Autenticacion(
                _uriBase,
                new AutenticacionModelRequest
                {
                    Usuario = usuario,
                    NumeroDocumento = numeroDoc,
                    Clave = clave
                }
            );

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<TokenModelResponse>();

            result.Should().NotBeNull();
            result.AccessToken.Should().Be(tokenResponse.AccessToken);
            result.RefreshToken.Should().Be(tokenResponse.RefreshToken);
            result.RefreshExpiresIn.Should().Be(tokenResponse.RefreshExpiresIn);
            result.ExpiresIn.Should().Be(tokenResponse.ExpiresIn);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
