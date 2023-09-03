using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.ClaveController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ObtenerEstadoIntegrationTest : ControllerIntegrationTest
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

        private static ServiceRequest PostValidarClaveCanales(Uri uriBase, ObtenerEstadoModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.ObtenerEstadoClaveCanales);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public ObtenerEstadoIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task ObtenerEstadoAsyncOk()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.ObtenerEstadoClaveCanales()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .WithTitle("ObtenerEstadoOk")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            var request = PostValidarClaveCanales(_uriBase, new ObtenerEstadoModelRequest
            {
                IdTipoDocumento = 4,
                NumeroDocumento = "12345678"
            });

            // Act
            var sut = await SendAsync(request, requestId: "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status202Accepted);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
