using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v1._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class PerfilIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            GetPerfil(_uriBase, "UsuarioTest1"),
            GetPerfil(_uriBase, "UsuarioNoExistente")
        };

        private static ServiceRequest GetPerfil(Uri uriBase, string usuario)
        {
            var uri = new Uri(uriBase, ApiUris.Perfil_v1(usuario));

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        public PerfilIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Perfil(string usuario, HttpStatusCode httpStatusCode)
        {
            // Arrange
            var request = GetPerfil(_uriBase, usuario);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            if (httpStatusCode == HttpStatusCode.OK)
            {
                var response = await sut.Content.ReadAsAsync<PerfilModelResponse>();

                response.LastName.Should().Be("Test8");
                response.LastLogon.Should().Be(DateTime.MinValue);
                response.PersonId.Should().Be("10008");
                response.UserId.Should().Be(8);
                response.Email.Should().Be("test8@test.com");
                response.FirstName.Should().Be("Usuario");
            }
        }

        public static IEnumerable<object[]> Datos =>
        new List<object[]>
        {
            new object[] { "", HttpStatusCode.NotFound },
            new object[] { null, HttpStatusCode.NotFound },
            new object[] { "UsuarioTest8", HttpStatusCode.OK},
            new object[] { "UsuarioNoExistente", HttpStatusCode.NotFound}
        };
    }
}
