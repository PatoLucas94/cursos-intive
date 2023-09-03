using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.TyCController.CommonTyC.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.TyCController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class AceptadosPersonIdIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        private WireMockHelper WireMockHelper { get; }

        private static ServiceRequest Aceptados(Uri uriBase, string personId) => ServiceRequest.Get(
            new Uri(uriBase, ApiUris.TyCAceptadosByPersonId(personId)).AbsoluteUri
        );

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        public AceptadosPersonIdIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task AceptadosOkAsync()
        {
            // Arrange
            var personId = "123456";
            var canal = "OBI";
            var concepto = "ALTAUSUARIODIGITAL";
            var pathTyCAceptados = ApiTyCUris.Aceptados(canal, concepto, personId);
            var pathTyCHabilitados = ApiUsuariosUris.TerminosYCondicionesHabilitado();

            VigenteIntegrationTest.ObtenerVigenteMock(WireMockHelper, canal, concepto);

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathTyCAceptados))
                .RespondWith(
                    WireMockHelper.Json(
                        new ApiTyCAceptadosModelOutput
                        {
                            id_terminos_condiciones = "9f825d1c-5d60-45db-8b72-5daecf800a6b",
                            vigencia_desde = DateTime.Parse("2021-09-15T00:00:00"),
                            fecha_aceptacion = DateTime.Parse("2021-09-15T00:00:00"),
                            status_code = HttpStatusCode.OK
                        }
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathTyCHabilitados))
                .RespondWith(
                    WireMockHelper.Json(
                        new ApiUsuariosTycHabilitadoModelOutput
                        {
                            habilitado = true
                        }
                    )
                );

            // Act
            var request = Aceptados(_uriBase, personId);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<AceptadosModelResponse>();

            result.Should().NotBeNull();
            result.Id.Should().Be("9f825d1c-5d60-45db-8b72-5daecf800a6b");
            result.Contenido.Should().NotBeNullOrWhiteSpace();

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
