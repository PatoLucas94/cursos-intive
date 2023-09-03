using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Test.Infrastructure;
using Spv.Usuarios.Test.Integration.ExternalServices;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ValidacionExistenciaBtaIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        private WireMockHelper WireMockHelper { get; }

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostValidarExistenciaBtaUsuario(_uriBase,
                new ValidacionExistenciaBtaModelRequest { IdTipoDocumento = 4, NroDocumento = "12345678", IdPais = 80 })
        };

        private static ServiceRequest PostValidarExistenciaBtaUsuario(Uri uriBase, ValidacionExistenciaBtaModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.ValidacionExistenciaBtaV2);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public ValidacionExistenciaBtaIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ValidarExistenciaBtaAsync(
            int idPais,
            int idTipoDocumento,
            string nroDocumento,
            HttpStatusCode httpStatusCode,
            bool claveBta,
            string nsbtResponse)
        {
            // Arrange

            var token = new TokenBtaModelOutput();
            var obtenerpin = new ObtenerPinModelOutput();
            obtenerpin.DatosPIN = new DatosPin()
            {
                pin = nsbtResponse
            };

            var path = $"/bantotal/servlet/com.dlya.bantotal.odwsbt_Authenticate_v1?Execute";
            var pathObtenerpin = $"https://btsdesa:9448/bantotal/servlet/com.dlya.bantotal.odwsbt_SVPinBTA_v1?AdministrarPIN";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.WebService(path))
                .RespondWith(WireMockHelper.Json(token));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.WebService(pathObtenerpin))
                .RespondWith(WireMockHelper.Json(obtenerpin));

            var body = new ValidacionExistenciaBtaModelRequest
            {
                IdPais = idPais,
                NroDocumento = nroDocumento,
                IdTipoDocumento = idTipoDocumento
            };

            var request = PostValidarExistenciaBtaUsuario(_uriBase, body);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaBtaModelResponse>();

            result.Should().NotBeNull();
            result.ClaveBt.Should().Be(claveBta);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        public static IEnumerable<object[]> Datos =>
        new List<object[]>
        {
            new object[] { 80, 4, "23734745", HttpStatusCode.OK, true, "PIN_OK" },
            new object[] { 80, 4, "23734745", HttpStatusCode.OK, true, "PIN_OK"},
            new object[] { 80, 4, "11111111", HttpStatusCode.OK, true, "PIN_OK" },

            new object[] { 80, 4, "11222333", HttpStatusCode.OK, false, "" },
            new object[] { 80, 4, "12345678", HttpStatusCode.OK, false, "" },
            new object[] { 80, 4, "11111112", HttpStatusCode.OK, false,"" }
        };
    }
}
