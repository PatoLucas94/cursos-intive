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
    public class GenerarClaveSmsIntegrationTest : ControllerIntegrationTest
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

        private static ServiceRequest PostGenerarClaveSms(Uri uriBase, GeneracionClaveSmsModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.GeneracionClaveSms());

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public GenerarClaveSmsIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        public static IEnumerable<object[]> Datos =>
           new List<object[]>
           {
               new object[]
               {
                   ApiNotificacionesHelper.CreacionTokenSmsEstadoPendienteEnvio
               },
               new object[]
               {
                   ApiNotificacionesHelper.CreacionTokenSmsEstadoEnviado
               }
           };

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task GenerarClaveSmsAsyncOk(string tokenSmsEstado)
        {
            // Arrange
            var path = $"{ApiNotificacionesUris.CrearYEnviarToken()}";

            var crearYEnviarTokenOkExpectedResult = new ApiNotificacionesCrearYEnviarTokenModelOutput
            {
                estado = tokenSmsEstado
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("GenerarClaveSmsAsyncOk")
                .RespondWith(WireMockHelper.RespondWithAccepted(crearYEnviarTokenOkExpectedResult));

            var request = PostGenerarClaveSms(_uriBase, new GeneracionClaveSmsModelRequest { IdPersona = "1", Telefono = "1122333444" });

            // Act
            var sut = await SendAsync(request, requestId: "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status202Accepted);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task GenerarClaveSmsAsyncError()
        {
            // Arrange
            var path = $"{ApiNotificacionesUris.CrearYEnviarToken()}";

            var crearYEnviarTokenInvalidoExpectedResult = new ApiNotificacionesCrearYEnviarTokenModelOutput
            {
                estado = ApiNotificacionesHelper.ValidacionTokenSmsEstadoInvalido,
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, ErrorHeaders))
                .WithTitle("GenerarClaveSmsAsyncError")
                .RespondWith(WireMockHelper.RespondWithBadRequest(crearYEnviarTokenInvalidoExpectedResult));

            var request = PostGenerarClaveSms(_uriBase, new GeneracionClaveSmsModelRequest { IdPersona = "1", Telefono = "1122333444" });

            // Act
            var sut = await SendAsync(request, "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
