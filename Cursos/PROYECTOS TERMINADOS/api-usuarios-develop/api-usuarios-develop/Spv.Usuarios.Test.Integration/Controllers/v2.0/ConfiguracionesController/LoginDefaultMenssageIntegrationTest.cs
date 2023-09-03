using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Spv.Usuarios.Api.ViewModels.ConfiguracionesController.Output;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;
using static Spv.Usuarios.Common.Testing.Attributes.PriorityAttribute;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.ConfiguracionesController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class LoginDefaultMenssageIntegrationTest : ControllerIntegrationTest
    {
        private readonly ServerFixture _server;
        private readonly Uri _uriBase;

        private WireMockHelper WireMockHelper { get; }

        private static ServiceRequest GetMensajeDefaultLoginDeshabilitado(Uri uriBase, object request)
        {
            var queryString = GetUrlEncode(request);
            var uri = new Uri(uriBase, $"{ApiUris.MensajeDefaultLoginDeshabilitado}?{queryString}");
            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            GetMensajeDefaultLoginDeshabilitado(
                _uriBase,
                new LoginMessageModelResponse()
            )
        };

        public LoginDefaultMenssageIntegrationTest(ServerFixture server) : base(server)
        {
            _server = server;
            _uriBase = _server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = _server.WireMock;
        }

        readonly LoginMessageModelResponse _loginMessageModelResponse = new LoginMessageModelResponse
        {
            Mensaje = "mensaje1",
        };

        [Fact, TestPriority(0)]
        public async Task ObtenerMensajeDefaulLoginDeshabilitado()
        {
            // Arrange
            var path = $"{ApiUris.MensajeDefaultLoginDeshabilitado}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(_loginMessageModelResponse));

            var request = GetMensajeDefaultLoginDeshabilitado(_uriBase, new LoginMessageModelResponse());

            // Act
            var sut = await SendAsync(request);

            var response = new ObjectResult(await sut.Content.ReadAsAsync<object>());

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
