using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.ConfiguracionService.Output;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.ConfiguracionController.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.ConfiguracionController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class LoginHabilitadoIntegracionTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        public WireMockHelper WireMockHelper { get; set; }
        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest GetLoginHabilitado(Uri uriBase)
        {
            var uri = new Uri(uriBase, ApiUris.LoginHabilitado);

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        public LoginHabilitadoIntegracionTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task LoginHabilitadoAsync()
        {
            var loginHabilitado = new LoginHabilitadoModelOutput
            {
                Habilitado = "1"
            };

            var mensaje = new ApiUsuariosLoginMessageModelOutput
            {
                mensaje = "mensajeeee"
            };

            var path = $"{ApiUsuariosUris.LoginHabilitado()}";
            var path1 = $"{ApiUsuariosUris.ObtenerMensajeLoginDeshabilitado()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(loginHabilitado));

            WireMockHelper.ServiceMock
                  .Given(WireMockHelper.Get(path1))
                  .RespondWith(WireMockHelper.Json(mensaje));

            var request = GetLoginHabilitado(_uriBase);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<LoginHabilitadoModelResponse>();

            result.Should().NotBeNull();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
