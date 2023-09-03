using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Spv.Usuarios.Api.ViewModels.DynamicImagesController.Output;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;
using static Spv.Usuarios.Common.Testing.Attributes.PriorityAttribute;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.DynamicImagesController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ObtenerImagesLoginIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        private WireMockHelper WireMockHelper { get; }

        private static ServiceRequest GetObtenerImagesLogin(Uri uriBase)
        {
            var uri = new Uri(uriBase, $"{ApiUris.ObtenerImagesLogin}");
            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            GetObtenerImagesLogin(_uriBase)
        };

        public ObtenerImagesLoginIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        readonly List<ImageLoginModelResponse> _listImageLoginModelResponse = new List<ImageLoginModelResponse>
        {
            new ImageLoginModelResponse
            {
                Id = 1,
                Nombre = "imagen1",
                Orden = 1
            },
            new ImageLoginModelResponse
            {
                Id = 2,
                Nombre = "imagen2",
                Orden = 2
            }
        };

        [Fact, TestPriority(0)]
        public async Task ObtenerImagesLogin()
        {
            // Arrange
            var path = $"{ApiUris.ObtenerImagesLogin}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(_listImageLoginModelResponse));

            var request = GetObtenerImagesLogin(_uriBase);

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
