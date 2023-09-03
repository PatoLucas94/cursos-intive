using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.TyCService.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class TyCRepositoryIntegrationTest
    {
        private readonly IApiTyCRepository _tycRepository;
        public WireMockHelper WireMockHelper { get; set; }

        public TyCRepositoryIntegrationTest(ServerFixture server)
        {
            var tycRepository = server.HttpServer.TestServer.Services.GetRequiredService<IApiTyCRepository>();

            _tycRepository = tycRepository;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task ObtenerVigenteOkAsync()
        {
            // Arrange
            var tycVigente = new VigenteModelOutput
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
                        descripcion = "OBI"
                    }
                },
                conceptos = new List<ConceptoModelOutput>
                {
                    new ConceptoModelOutput
                    {
                        codigo = "5",
                        descripcion = "ALTAUSUARIODIGITAL"
                    }
                }
            };

            var path = $"{ApiTyCUris.Vigente("OBI", "ALTAUSUARIODIGITAL")}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .WithTitle("ObtenerVigenteOkAsync")
                .RespondWith(WireMockHelper.Json(tycVigente));

            // Act
            var result = await _tycRepository.ObtenerVigenteAsync();

            // Assert
            result.Should().NotBeNull();
            result.id.Should().Be("9f825d1c-5d60-45db-8b72-5daecf800a6b");
            result.vigencia_desde.Should().Be(DateTime.Parse("2021-09-15T00:00:00"));
            result.contenido.Should().NotBeNullOrWhiteSpace();
            result.canales.Count.Should().Be(1);
            result.conceptos.Count.Should().Be(1);

            var canal1 = result.canales.First();
            canal1.codigo.Should().Be("4");
            canal1.descripcion.Should().Be("OBI");

            var concepto1 = result.conceptos.First();
            concepto1.codigo.Should().Be("5");
            concepto1.descripcion.Should().Be("ALTAUSUARIODIGITAL");

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
        
        [Fact]
        public async Task AceptarVigenteOkAsync()
        {
            // Arrange
            const string idTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b";
            var dateTimeNow = DateTime.Now;

            var tycAceptacionResponse = new ApiTyCAceptacionModelOutput
            {
                id_terminos_condiciones = idTyC,
                fecha_aceptacion = dateTimeNow,
                status_code = HttpStatusCode.OK
            };

            var path = $"{ApiTyCUris.Aceptados()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("AceptarVigenteOkAsync")
                .RespondWith(WireMockHelper.Json(tycAceptacionResponse));

            // Act
            var result = await _tycRepository.AceptarAsync((1, "ALTAUSUARIODIGITAL"));

            // Assert
            result.Should().NotBeNull();
            result.id_terminos_condiciones.Should().Be(idTyC);
            result.fecha_aceptacion.Should().Be(dateTimeNow);
            result.status_code.Should().Be(HttpStatusCode.OK);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
