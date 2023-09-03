using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.UsuarioController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ValidacionExistenciaHbiIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        // Usuario inexistente

        private static readonly string nombreUsuarioInexistente1 = "rcbertoldo01";

        // Usuario existente

        private static readonly string nombreUsuarioExistente1 = "rcbertoldo";

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[]
                {
                    nombreUsuarioInexistente1,
                    false,
                    HttpStatusCode.OK,
                    HttpStatusCode.OK
                },
                new object[]
                {
                    nombreUsuarioExistente1,
                    true,
                    HttpStatusCode.OK,
                    HttpStatusCode.OK
                },
            };
        public WireMockHelper WireMockHelper { get; set; }

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();
        public ValidacionExistenciaHbiIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        private static ServiceRequest ValidarExistenciaHbi(
            Uri uriBase, ValidacionExistenciaHbiModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.ValidacionExistenciaHbi());

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ValidarExistenciaHbiAsync(
            string nombreUsuario,
            bool existeUsuario,
            HttpStatusCode httpStatusCode,
            HttpStatusCode statusCodeValidarExistencia)
        {
            // Arrange
            var pathValidacionExistenciaHbi = $"{ApiUsuariosUris.ValidarExistenciaHbi()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistenciaHbi))
                .WithTitle($"{statusCodeValidarExistencia}_validar_existencia_hbi")
                .RespondWith(WireMockHelper.Json(
                    new ValidacionExistenciaHbiModelOutput
                    {
                        ExisteUsuario = existeUsuario
                    }));

            var request = ValidarExistenciaHbi(
                _uriBase, 
                new ValidacionExistenciaHbiModelRequest
                {
                    NombreUsuario = nombreUsuario
                });

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaHbiOkAsync()
        {
            // Arrange
            var pathValidacionExistenciaHbi = $"{ApiUsuariosUris.ValidarExistenciaHbi()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistenciaHbi))
                .WithTitle($"{HttpStatusCode.OK}_validar_existencia_hbi")
                .RespondWith(WireMockHelper.Json(
                    new ApiUsuariosValidacionExistenciaHbiModelOutput
                    {
                        existe_usuario = false
                    }));

            // Act
            var request = ValidarExistenciaHbi(
                _uriBase,
                new ValidacionExistenciaHbiModelRequest
                {
                    NombreUsuario = nombreUsuarioInexistente1
                });

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaHbiModelResponse>();

            result.Should().NotBeNull();
            result.ExisteUsuario.Should().BeFalse();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
