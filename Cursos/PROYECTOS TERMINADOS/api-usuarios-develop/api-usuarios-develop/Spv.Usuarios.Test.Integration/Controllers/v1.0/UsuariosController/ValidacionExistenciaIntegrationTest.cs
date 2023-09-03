using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v1._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ValidacionExistenciaHbiIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostValidarExistenciaUsuario(_uriBase, 
                new ValidacionExistenciaHbiModelRequest { UserName = "rcbertoldo" })
        };

        private static ServiceRequest PostValidarExistenciaUsuario(Uri uriBase, ValidacionExistenciaHbiModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.ValidacionExistenciaUsuario);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public ValidacionExistenciaHbiIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ValidarExistenciaUsuario(string userName, HttpStatusCode httpStatusCode)
        {
            // Arrange
            var body = new ValidacionExistenciaHbiModelRequest
            {
                UserName = userName
            };

            var request = PostValidarExistenciaUsuario(_uriBase, body);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);
            var response = await sut.Content.ReadAsAsync<ValidacionExistenciaHbiModelResponse>();

            if (response.ExisteUsuario)
            {
                response.ExisteUsuario.Should().BeTrue();
            } else
            {
                response.ExisteUsuario.Should().BeFalse();
            }

        }

        public static IEnumerable<object[]> Datos =>
        new List<object[]>
        {
            new object[] { "UsuarioTest1", HttpStatusCode.OK },
            new object[] { "UsuarioTest2", HttpStatusCode.OK},
            new object[] { "UsuarioTest3", HttpStatusCode.OK },
            new object[] { "Usuario99999", HttpStatusCode.OK}
        };
    }
}
