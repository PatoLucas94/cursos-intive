using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.ClaveController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ValidarClaveSmsIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        public WireMockHelper WireMockHelper { get; set; }

        public Dictionary<string, string> ErrorHeaders = new Dictionary<string, string>
        {
            { HeaderNames.RequestIdHeaderName , "ErrorUser" }
        };

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest PostValidarClaveSms(Uri uriBase, ValidacionClaveSmsModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.ValidacionClaveSms);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public ValidarClaveSmsIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task ValidarClaveSmsAsyncOk()
        {
            // Arrange
            var path = $"{ApiNotificacionesUris.ValidarToken()}";

            var identificador = Guid.NewGuid().ToString();

            var validarTokenOkExpectedResult = new ApiNotificacionesValidarTokenModelOutput
            {
                estado = ApiNotificacionesHelper.ValidacionTokenSmsEstadoUtilizado,
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("ValidarClaveSmsAsyncOk")
                .RespondWith(WireMockHelper.RespondWithAccepted(validarTokenOkExpectedResult));

            var request = PostValidarClaveSms(_uriBase, new ValidacionClaveSmsModelRequest { IdPersona = "1", Identificador = identificador, ClaveSms = "123456" });

            // Act
            var sut = await SendAsync(request, requestId: "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status202Accepted);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarClaveSmsAsyncError()
        {
            // Arrange
            var path = $"{ApiNotificacionesUris.ValidarToken()}";

            var validarTokenInvalidoExpectedResult = new ApiNotificacionesValidarTokenModelOutput
            {
                estado = ApiNotificacionesHelper.ValidacionTokenSmsEstadoInvalido,
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, ErrorHeaders))
                .WithTitle("ValidarClaveSmsAsyncError")
                .RespondWith(WireMockHelper.RespondWithAccepted(validarTokenInvalidoExpectedResult));

            var request = PostValidarClaveSms(_uriBase, new ValidacionClaveSmsModelRequest { IdPersona = "1", Identificador = Guid.NewGuid().ToString(), ClaveSms = "123456" });

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
