using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Output;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.BiometriaController.CommonBiometria.Input;
using Spv.Usuarios.Bff.ViewModels.BiometriaController.CommonBiometria.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.BiometriaController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class AutenticacionIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        private WireMockHelper WireMockHelper { get; }

        private static ServiceRequest Autenticacion(Uri uriBase, BiometriaAutenticacionModelRequest model) =>
            ServiceRequest.Post(new Uri(uriBase, ApiUris.BiometriaAutenticacion).AbsoluteUri, model);

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        public AutenticacionIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task AceptadosOkAsync()
        {
            // Arrange
            var autenticacion = ApiBiometriaUris.Autenticacion;

            var response = new BiometriaAutenticacionModelOutput
            {
                IdentificacionDigital = new IdentificacionDigitalOutput(),
                ValidacionManual = true,
                StatusLivenessPasivo = "pasivo",
                StatusObtenidoPlantillaFacial = "facial",
                StatusObtenidoMejorImagenFacial = "mejorimagen",
                StatusObtenidoPlantillaFacialExtendida = "facialExtendida"
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(autenticacion))
                .RespondWith(WireMockHelper.Json(response));

            // Act
            var request = Autenticacion(
                _uriBase,
                new BiometriaAutenticacionModelRequest
                {
                    IdPersona = 12345,
                    DatosBiometricos = new DatosBiometricosModelRequest()
                    {
                        MejorImagenFacial = "mejorimagen",
                        PlantillaFacialExtendida = "facialExtendida"
                    }
                }
            );

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<BiometriaAutenticacionModelResponse>();

            result.Should().NotBeNull();
            result.ValidacionManual.Should().Be(response.ValidacionManual);
            result.StatusObtenidoPlantillaFacialExtendida.Should().Be(response.StatusObtenidoPlantillaFacialExtendida);
            result.StatusLivenessPasivo.Should().Be(response.StatusLivenessPasivo);
            result.StatusObtenidoMejorImagenFacial.Should().Be(response.StatusObtenidoMejorImagenFacial);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
