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

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class PerfilMigradoIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        private WireMockHelper WireMockHelper { get; }

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            GetPerfilMigrado(_uriBase, 1),
            GetPerfilMigrado(_uriBase, -1)
        };

        private static ServiceRequest GetPerfilMigrado(Uri uriBase, long idPersona)
        {
            var uri = new Uri(uriBase, ApiUris.PerfilMigradoV2(idPersona));

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        private static ServiceRequest GetPerfilMigrado(
            Uri uriBase,
            string documentNumber,
            int documentCountryId,
            int documentTypeId
        )
        {
            var uri = new Uri(uriBase, ApiUris.PerfilMigradoV2(documentNumber, documentCountryId, documentTypeId));

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        public PerfilMigradoIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task PerfilMigradoRouteParams(long idPersona, HttpStatusCode httpStatusCode)
        {
            // Arrange
            var request = GetPerfilMigrado(_uriBase, idPersona);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            switch (httpStatusCode)
            {
                case HttpStatusCode.OK when idPersona == 14155917:
                {
                    var response = await sut.Content.ReadAsAsync<PerfilModelResponseV2>();

                    response.LastLogon.Should().BeMoreThan(TimeSpan.MinValue);
                    response.PersonId.Should().Be(14155917);
                    response.UserId.Should().BeGreaterThan(0);
                    response.DocumentNumber.Should().Be("12345678");
                    response.FirstName.Should().Be("RICARDO CRISTIAN");
                    response.LastName.Should().Be("BERTOLDO");
                    response.Email.Should().Be("holamundo@gmail.com");
                    response.Country.Should().Be(80);
                    response.DocumentType.Should().Be(4);
                    response.Gender.Should().Be("M");
                    break;
                }
                case HttpStatusCode.OK when idPersona == 852:
                {
                    var response = await sut.Content.ReadAsAsync<PerfilModelResponseV2>();

                    response.LastLogon.Should().BeMoreThan(TimeSpan.MinValue);
                    response.PersonId.Should().Be(852);
                    response.UserId.Should().BeGreaterThan(0);
                    response.DocumentNumber.Should().Be("11222333");
                    response.FirstName.Should().Be("Usuario");
                    response.LastName.Should().Be("Test1");
                    response.Email.Should().Be("test1@test.com");
                    response.Country.Should().Be(80);
                    response.DocumentType.Should().Be(4);
                    response.Gender.Should().Be("F");
                    break;
                }
            }

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Theory]
        [MemberData(nameof(DatosPayload))]
        public async Task PerfilMigrado(
            string documentNumber,
            int documentCountryId,
            int documentTypeId,
            HttpStatusCode httpStatusCode
        )
        {
            // Arrange
            var request = GetPerfilMigrado(_uriBase, documentNumber, documentCountryId, documentTypeId);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        public static IEnumerable<object[]> Datos => new List<object[]>
        {
            new object[] { 0, HttpStatusCode.BadRequest },
            new object[] { 4, HttpStatusCode.OK },
            new object[] { 10002, HttpStatusCode.OK },
            new object[] { 999, HttpStatusCode.NotFound }
        };

        public static IEnumerable<object[]> DatosPayload => new List<object[]>
        {
            new object[] { "", 0, 0, HttpStatusCode.BadRequest },
            new object[] { "12345678", 80, 4, HttpStatusCode.OK },
            new object[] { "21234567", 80, 4, HttpStatusCode.OK },
            new object[] { "999", 80, 6, HttpStatusCode.NotFound }
        };
    }
}