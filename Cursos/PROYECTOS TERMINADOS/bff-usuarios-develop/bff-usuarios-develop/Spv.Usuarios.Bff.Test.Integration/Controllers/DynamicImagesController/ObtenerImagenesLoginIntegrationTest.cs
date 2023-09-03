using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.DynamicImagesService.Output;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.DynamicImagesController.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.DynamicImagesController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ObtenerImagenesLoginIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        public WireMockHelper WireMockHelper { get; set; }
        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest GetObtenerImagenesLogin(Uri uriBase)
        {
            var uri = new Uri(uriBase, ApiUris.ObtenerImagesLogin);

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        public ObtenerImagenesLoginIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task ObtenerImagenesLoginAsync()
        {
            var listaimagenes = new List<ImagenLoginModelOutput>();
            var imagen1 = new ImagenLoginModelOutput()
            {
                Imagen = new byte[2]
            };
            var imagen2 = new ImagenLoginModelOutput()
            {
                Imagen = new byte[2]
            };
            listaimagenes.Add(imagen1);
            listaimagenes.Add(imagen2);

            var path = $"{ApiUsuariosUris.ObtenerImagenesLogin()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(listaimagenes));

            var request = GetObtenerImagenesLogin(_uriBase);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<List<ObtenerImagenesLoginModelResponse>>();
            
            result.Should().NotBeNull();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
