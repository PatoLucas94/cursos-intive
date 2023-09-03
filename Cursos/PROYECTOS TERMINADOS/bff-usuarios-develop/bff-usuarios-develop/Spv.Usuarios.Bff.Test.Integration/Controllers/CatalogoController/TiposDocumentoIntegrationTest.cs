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
    public class TiposDocumentoIntegrationTest : ControllerIntegrationTest
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

        private static ServiceRequest GetTiposDocumento(Uri uriBase)
        {
            var uri = new Uri(uriBase, ApiUris.TiposDocumento());

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        public TiposDocumentoIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task TiposDocumentoAsyncOk()
        {
            // Arrange
            var pathTiposDocumento = ApiCatalogoUris.TiposDocumento();

            var tiposDocumento = new List<ApiCatalogoTiposDocumentoModelOutput>
            {
                new ApiCatalogoTiposDocumentoModelOutput { codigo = 1, descripcion = "Tipo Doc 1", tipoPersonaQueAplica = "J" },
                new ApiCatalogoTiposDocumentoModelOutput { codigo = 2, descripcion = "Tipo Doc 2", tipoPersonaQueAplica = "F" },
                new ApiCatalogoTiposDocumentoModelOutput { codigo = 3, descripcion = "Tipo Doc 3", tipoPersonaQueAplica = "J" }
            };

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.GetWithHeaders(pathTiposDocumento, SuccessHeaders))
                   .WithTitle("TiposDocumentoAsyncOk")
                   .RespondWith(WireMockHelper.Json(tiposDocumento));

            var request = GetTiposDocumento(_uriBase);

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = await sut.Content.ReadAsAsync<List<TiposDocumentoModelResponse>>();

            response.Count.Should().Be(1);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task TiposDocumentoAsyncError()
        {
            // Arrange
            var pathTiposDocumento = ApiCatalogoUris.TiposDocumento();

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.GetWithHeaders(pathTiposDocumento, ErrorHeaders))
                   .WithTitle("TiposDocumentoAsyncOk")
                   .RespondWith(WireMockHelper.RespondWithBadRequest());

            var request = GetTiposDocumento(_uriBase);

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
