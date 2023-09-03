using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class CambiarEstadoIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        private WireMockHelper WireMockHelper { get; }

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PatchCambiarEstado(
                _uriBase,
                new CambioEstadoModelRequest
                {
                    PersonId = 1,
                    EstadoId = 3
                }
            ),
            PatchCambiarEstado(
                _uriBase,
                new CambioEstadoModelRequest
                {
                    PersonId = 10,
                    EstadoId = 1
                }
            )
        };

        private static ServiceRequest PatchCambiarEstado(Uri uriBase, CambioEstadoModelRequest request)
        {
            var uri = new Uri(uriBase, ApiUris.CambiarEstadoV2);

            return ServiceRequest.Patch(uri.AbsoluteUri, request);
        }

        public CambiarEstadoIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task CambiarEstadoRouteParams(long idPersona, byte estatusId, HttpStatusCode httpStatusCode)
        {
            // Arrange
            var request = PatchCambiarEstado(
                _uriBase,
                new CambioEstadoModelRequest
                {
                    PersonId = idPersona,
                    EstadoId = estatusId,
                }
            );

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                {
                    var response = await sut.Content.ReadAsAsync<object>();
                    response.Should().NotBeNull();
                    break;
                }
            }

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        public static IEnumerable<object[]> Datos => new List<object[]>
        {
            new object[] { 0, (byte)UserStatus.Blocked, HttpStatusCode.BadRequest },
            new object[] { 14155917, (byte)UserStatus.Inactive, HttpStatusCode.OK },
            new object[] { 10002, (byte)UserStatus.Inactive, HttpStatusCode.Conflict },
            new object[] { 678, (byte)UserStatus.Suspended, HttpStatusCode.NotFound }
        };
    }
}