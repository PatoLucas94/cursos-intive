using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.SofttokenClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.SoftToken.Output;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.SoftTokenController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class HabilitadoIntegrationTest : ControllerIntegrationTest
    {
        public WireMockHelper WireMockHelper { get; set; }

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private readonly Uri _uriBase;

        public HabilitadoIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        private static ServiceRequest Habilitado(Uri uriBase, string param)
        {
            var uri = new Uri(uriBase, ApiUris.SoftTokenHabilitado()+ param);

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        [Fact]
        public async Task HabilitadoOkAsync()
        {
            // Arrange
            const string identificador = "sts_11062858";
            var pathSoftTokenValido = $"{ApiSoftTokenUris.Habilitado(identificador)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathSoftTokenValido))
                .WithTitle("ValidoOkAsync")
                .RespondWith(WireMockHelper.Json(
                    new SoftTokenModelOutput
                    {
                        Detalle = string.Empty,
                        Estado = "OK",
                        Bloqueado = false,
                        Identificador = identificador,
                    }));


            var param = "?canal=OBI&usuario=OBI&identificador="+ identificador;
            // Act
            var request = Habilitado(_uriBase, param);

            var headers = new Dictionary<string, string>();
            headers.Add("XCanal", "OBI");
            headers.Add("XUsuario", "OBI");


            var sut = await SendAsync(request, "dae34444a44d4e4f", headers);


            sut.IsSuccessStatusCode.Should().BeTrue();

            await using var responseStream = await sut.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<ApiSoftTokenModelOutput>(responseStream);


            result.Should().NotBeNull();

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

    }
}
