using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Bff.Common.Dtos.Client.SofttokenClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.SoftToken.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class SoftTokenRepositoryIntegrationTest
    {
        private readonly IApiSoftTokenRepository _softTokenRepository;

        public WireMockHelper WireMockHelper { get; set; }

        public SoftTokenRepositoryIntegrationTest(ServerFixture server)
        {
            var softTokenRepository = server.HttpServer.TestServer.Services.GetRequiredService<IApiSoftTokenRepository>();

            _softTokenRepository = softTokenRepository;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task SoftTokenValidoOkAsync()
        {
            // Arrange
            var softToken = new SoftTokenModelOutput
            {
                Detalle = string.Empty,
                Estado = "OK",
                Bloqueado = false,
                Identificador = "sts_11062858",
            };

            var path = $"{ApiSoftTokenUris.Valido("sts_11062858")}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("SoftTokenValidoAsync")
                .RespondWith(WireMockHelper.Json(softToken));

            // Act
            var result = await _softTokenRepository.SoftTokenValidoAsync(
                new ApiSoftTokenValidoModelInput
                {
                    Identificador = "sts_11062858",
                    Token = "123456"
                });

            // Assert
            result.Should().NotBeNull();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task SoftTokenValidoInvalidoAsync()
        {
            // Arrange
            var softToken = new SoftTokenModelOutput
            {
                Detalle = string.Empty,
                Estado = "INVALIDO",
                Bloqueado = false,
                Identificador = "sts_11062858",
            };

            var path = $"{ApiSoftTokenUris.Valido("sts_11062858")}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("SoftTokenValidoAsync")
                .RespondWith(WireMockHelper.Json(softToken));

            // Act
            var result = await _softTokenRepository.SoftTokenValidoAsync(
                new ApiSoftTokenValidoModelInput
                {
                    Identificador = "sts_11062858",
                    Token = "123456"
                });

            // Assert
            result.Should().NotBeNull();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task SoftTokenHabilitadoOkAsync()
        {
            // Arrange
            var softToken = new SoftTokenModelOutput
            {
                Detalle = string.Empty,
                Estado = "OK",
                Bloqueado = false,
                Identificador = "sts_11062858",
            };

            var path = $"{ApiSoftTokenUris.Habilitado("sts_11062858")}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("TokenHabilitadoAsync")
                .RespondWith(WireMockHelper.Json(softToken));

            // Act
            var result = await _softTokenRepository.TokenHabilitadoAsync(
                new ApiSoftTokenModelInput
                {
                    Identificador = "sts_11062858"
                });

            // Assert
            result.Should().NotBeNull();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task SoftTokenNOHabilitadoOkAsync()
        {
            // Arrange
            var softToken = new SoftTokenModelOutput
            {
                Detalle = "No existe un token registrado para el identificador \"sts_111111111\"",
                Estado = "Not Found",
                Bloqueado = false,
                Identificador = "sts_11062858",
            };

            var path = $"{ApiSoftTokenUris.Habilitado("sts_11063858")}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("TokenHabilitadoAsync")
                .RespondWith(WireMockHelper.Json(softToken));

            // Act
            var result = await _softTokenRepository.TokenHabilitadoAsync(
                new ApiSoftTokenModelInput
                {
                    Identificador = "sts_11062858"
                });

            // Assert
            result.Should().BeNull();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
