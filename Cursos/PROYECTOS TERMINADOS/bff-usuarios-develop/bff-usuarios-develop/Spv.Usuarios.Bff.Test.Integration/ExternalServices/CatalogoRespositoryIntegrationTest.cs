using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Bff.Common.Dtos.Client.CatalogoClient.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    [TestCaseOrderer("Spv.Usuarios.Bff.Common.Testing.PriorityOrderer", "Spv.Usuarios.Bff.Common")]
    public class CatalogoRepositoryIntegrationTest
    {
        private readonly IApiCatalogoRepository _catalogoRepository;
        public WireMockHelper WireMockHelper { get; set; }

        public CatalogoRepositoryIntegrationTest(ServerFixture server)
        {
            var catalogoRepository = server.HttpServer.TestServer.Services.GetRequiredService<IApiCatalogoRepository>();

            _catalogoRepository = catalogoRepository;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task GetPaisesOkAsync()
        {
            // Arrange
            var path = $"{ApiCatalogoUris.Paises()}";

            var paises = new List<ApiCatalogoPaisesModelOutput>
            {
                new ApiCatalogoPaisesModelOutput { codigo = 1, descripcion = "Pais 1" },
                new ApiCatalogoPaisesModelOutput { codigo = 2, descripcion = "Pais 2" },
                new ApiCatalogoPaisesModelOutput { codigo = 3, descripcion = "Pais 3" }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("GetPaisesOkAsync")
                .RespondWith(WireMockHelper.Json(paises));

            // Act
            var result = await _catalogoRepository.ObtenerPaisesAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(3);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task GetPaisesErrorAsync()
        {
            // Arrange
            var path = $"{ApiCatalogoUris.Paises()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("GetPaisesErrorAsync")
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            // Act
            var result = await _catalogoRepository.ObtenerPaisesAsync();

            // Assert
            result.Should().BeNull();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task GetTiposDocumentoOkAsync()
        {
            // Arrange
            var path = $"{ApiCatalogoUris.TiposDocumento()}";

            var tiposDoc = new List<ApiCatalogoTiposDocumentoModelOutput>
            {
                new ApiCatalogoTiposDocumentoModelOutput { codigo = 1, descripcion = "Tipo doc 1" },
                new ApiCatalogoTiposDocumentoModelOutput { codigo = 2, descripcion = "Tipo doc 2" },
                new ApiCatalogoTiposDocumentoModelOutput { codigo = 3, descripcion = "Tipo doc 3" }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("GetTiposDocumentoOkAsync")
                .RespondWith(WireMockHelper.Json(tiposDoc));

            // Act
            var result = await _catalogoRepository.ObtenerTiposDocumentoAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(3);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task GetTiposDocumentoErrorAsync()
        {
            // Arrange
            var path = $"{ApiCatalogoUris.TiposDocumento()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("GetTiposDocumentoErrorAsync")
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            // Act
            var result = await _catalogoRepository.ObtenerTiposDocumentoAsync();

            // Assert
            result.Should().BeNull();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
