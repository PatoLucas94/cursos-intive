using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.TyCService.Output;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.TyCController.CommonTyC.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.TyCController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class VigenteIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private WireMockHelper WireMockHelper { get; }

        public VigenteIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        private static ServiceRequest Vigente(Uri uriBase, string concepto) =>
            ServiceRequest.Get(new Uri(uriBase, ApiUris.TyCVigente(concepto)).AbsoluteUri);

        [Fact]
        public async Task ObtenerVigenteOkAsync()
        {
            // Arrange
            const string canal = "OBI";
            const string concepto = "ALTAUSUARIODIGITAL";

            ObtenerVigenteMock(WireMockHelper, canal, concepto);

            // Act
            var request = Vigente(_uriBase, concepto);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<VigenteModelResponse>();

            result.Should().NotBeNull();
            result.Id.Should().Be("9f825d1c-5d60-45db-8b72-5daecf800a6b");
            result.VigenciaDesde.Should().Be(DateTime.Parse("2021-09-15T00:00:00"));
            result.Contenido.Should().NotBeNullOrWhiteSpace();
            result.Canales.Count.Should().Be(1);
            result.Conceptos.Count.Should().Be(1);

            var canal1 = result.Canales.First();
            canal1.Codigo.Should().Be("4");
            canal1.Descripcion.Should().Be("OBI");

            var concepto1 = result.Conceptos.First();
            concepto1.Codigo.Should().Be("5");
            concepto1.Descripcion.Should().Be("ALTAUSUARIODIGITAL");

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        internal static void ObtenerVigenteMock(WireMockHelper WireMockHelper, string canal, string concepto)
        {
            var pathTyCVigente = $"{ApiTyCUris.Vigente(canal, concepto)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathTyCVigente))
                .RespondWith(
                    WireMockHelper.Json(
                        new VigenteModelOutput
                        {
                            id = "9f825d1c-5d60-45db-8b72-5daecf800a6b",
                            vigencia_desde = DateTime.Parse("2021-09-15T00:00:00"),
                            contenido = "<!DOCTYPE HTML PUBLIC \\\"-//W3C//DTD HTML 4.0 Transitional//EN\\\"><HTML> " +
                                        "<HEAD> <title>Banco Supervielle</title> </HEAD> " +
                                        "<BODY> Algún contenido HTML </BODY></HTML>",
                            canales = new List<CanalModelOutput>
                            {
                                new CanalModelOutput
                                {
                                    codigo = "4",
                                    descripcion = canal
                                }
                            },
                            conceptos = new List<ConceptoModelOutput>
                            {
                                new ConceptoModelOutput
                                {
                                    codigo = "5",
                                    descripcion = concepto
                                }
                            }
                        }
                    )
                );
        }
    }
}
