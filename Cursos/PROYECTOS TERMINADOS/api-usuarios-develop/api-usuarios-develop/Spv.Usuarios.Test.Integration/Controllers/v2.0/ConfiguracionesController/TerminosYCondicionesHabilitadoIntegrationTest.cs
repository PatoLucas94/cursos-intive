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
    public class TerminosYCondicionesHabilitadoIntegrationTest : ControllerIntegrationTest
    {
        private readonly ServerFixture _server;
        private readonly Uri _uriBase;

        private WireMockHelper WireMockHelper { get; }

        private static ServiceRequest GetTerminosCondicionesHabilitados(Uri uriBase, object request)
        {
            var queryString = GetUrlEncode(request);
            var uri = new Uri(uriBase, $"{ApiUris.TerminosCondicionesHabilitados}?{queryString}");
            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            GetTerminosCondicionesHabilitados(
                _uriBase,
                new TerminosYCondicionesHabilitadoModelResponse()
            )
        };

        public TerminosYCondicionesHabilitadoIntegrationTest(ServerFixture server) : base(server)
        {
            _server = server;
            _uriBase = _server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = _server.WireMock;
        }

        readonly TerminosYCondicionesHabilitadoModelResponse _terminosYCondicionesHabilitadoModelResponse = new TerminosYCondicionesHabilitadoModelResponse
        {
            Habilitado = true,
        };

        [Fact, TestPriority(0)]
        public async Task ObtenerTerminosCondicionesHabilitados()
        {
            // Arrange
            var path = $"{ApiUris.TerminosCondicionesHabilitados}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(_terminosYCondicionesHabilitadoModelResponse));

            var request = GetTerminosCondicionesHabilitados(_uriBase, new TerminosYCondicionesHabilitadoModelResponse());

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
