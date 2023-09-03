using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ValidacionExistenciaIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostValidarExistenciaUsuario(_uriBase, 
                new ValidacionExistenciaModelRequest { IdTipoDocumento = 4, NroDocumento = "12345678", IdPais = 80 })
        };

        private static ServiceRequest PostValidarExistenciaUsuario(Uri uriBase, ValidacionExistenciaModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.ValidacionExistenciaUsuarioV2);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public ValidacionExistenciaIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ValidarExistenciaUsuario(int idPais, int idTipoDocumento, string nroDocumento, HttpStatusCode httpStatusCode)
        {
            // Arrange
            var body = new ValidacionExistenciaModelRequest
            {
                IdPais = idPais,
                NroDocumento = nroDocumento,
                IdTipoDocumento = idTipoDocumento
            };

            var request = PostValidarExistenciaUsuario(_uriBase, body);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);
        }

        public static IEnumerable<object[]> Datos =>
        new List<object[]>
        {
            new object[] { 80, 4, "11222333", HttpStatusCode.OK },
            new object[] { 80, 4, "12345678", HttpStatusCode.OK },
            new object[] { 80, 4, "11111111", HttpStatusCode.NotFound }
        };
    }
}
