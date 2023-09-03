using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.CatalogoClient.Output;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.CatalogoController.CommonCatalogo.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.CatalogoController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class PaisesIntegrationTest : ControllerIntegrationTest
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

        private static ServiceRequest GetPaises(Uri uriBase)
        {
            var uri = new Uri(uriBase, ApiUris.Paises());

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        public PaisesIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task PaisesAsyncOk()
        {
            // Arrange
            var pathPaises = ApiCatalogoUris.Paises();

            var paises = new List<ApiCatalogoPaisesModelOutput>
            {
                new ApiCatalogoPaisesModelOutput { codigo = 1, descripcion = "Pais 1" },
                new ApiCatalogoPaisesModelOutput { codigo = 2, descripcion = "Pais 2" },
                new ApiCatalogoPaisesModelOutput { codigo = 3, descripcion = "Pais 3" }
            };

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.GetWithHeaders(pathPaises, SuccessHeaders))
                   .WithTitle("PaisesAsyncOk")
                   .RespondWith(WireMockHelper.Json(paises));

            var request = GetPaises(_uriBase);

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = await sut.Content.ReadAsAsync<List<PaisesModelResponse>>();

            response.Count.Should().Be(3);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task PaisesAsyncError()
        {
            // Arrange
            var pathPaises = ApiCatalogoUris.Paises();

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.GetWithHeaders(pathPaises, ErrorHeaders))
                   .WithTitle("PaisesAsyncError")
                   .RespondWith(WireMockHelper.RespondWithBadRequest());

            var request = GetPaises(_uriBase);

            // Act
            var sut = await SendAsync(request, requestId: "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
