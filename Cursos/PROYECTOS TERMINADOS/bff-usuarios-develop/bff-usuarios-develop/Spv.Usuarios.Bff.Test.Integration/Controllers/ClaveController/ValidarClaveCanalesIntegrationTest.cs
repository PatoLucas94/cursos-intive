using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.ClaveController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ValidarClaveCanalesIntegrationTest : ControllerIntegrationTest
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

        private static ServiceRequest PostValidarClaveCanales(Uri uriBase, ValidacionClaveCanalesModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.ValidacionClaveCanales);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public ValidarClaveCanalesIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task CanalesAsyncOk()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.ValidacionClaveCanales()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .WithTitle("CanalesAsyncOk")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            var request = PostValidarClaveCanales(_uriBase, new ValidacionClaveCanalesModelRequest
            {
                ClaveCanales = "12345678", 
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

        [Fact]
        public async Task CanalesAsyncError()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.ValidacionClaveCanales()}";

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba", "")
                }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, ErrorHeaders))
                .WithTitle("CanalesAsyncError")
                .RespondWith(WireMockHelper.RespondWithUnauthorized(response));

            var request = PostValidarClaveCanales(_uriBase, new ValidacionClaveCanalesModelRequest
            {
                ClaveCanales = "12345678", 
                IdTipoDocumento = 4, 
                NumeroDocumento = "12345678"
            });

            // Act
            var sut = await SendAsync(request, requestId: "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
