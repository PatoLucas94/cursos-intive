using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class CambioDeCredencialesIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostCambiarCredencialesUsuario(_uriBase,
                new CambioDeCredencialesModelRequestV2
                {
                    IdPersona = 12345678,
                    NuevoUsuario = "rcbertoldo1",
                    NuevaClave = "4132"
                })
        };

        private static ServiceRequest PostCambiarCredencialesUsuario(
            Uri uriBase, 
            CambioDeCredencialesModelRequestV2 body)
        {
            var uri = new Uri(uriBase, ApiUris.CambiarCredencialesUsuarioV2);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public CambioDeCredencialesIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task CambiarCredencialesUsuario(
            long idPersona, 
            string nuevoUsuario, 
            string nuevaClave, 
            HttpStatusCode httpStatusCode,
            string mensaje = null)
        {
            // Arrange
            var body = new CambioDeCredencialesModelRequestV2
            {
                IdPersona = idPersona,
                NuevoUsuario = nuevoUsuario,
                NuevaClave = nuevaClave
            };

            var request = PostCambiarCredencialesUsuario(_uriBase, body);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            if(sut.StatusCode == HttpStatusCode.Conflict)
            {
                var errorDetailModel = await sut.Content.ReadAsAsync<ErrorDetailModel>();

                var error = errorDetailModel.Errors.First();

                error.Detail.Should().Be(mensaje);
            }
        }

        public static IEnumerable<object[]> Datos =>
        new List<object[]>
        {
            new object[] { 12345678, "Rcb3rt0ld0", "0258", HttpStatusCode.OK },
            new object[] { 12345678, "Rcb3rt0ld0", "2341", HttpStatusCode.Conflict, MessageConstants.CredencialesYaUtilizadas }, // Credenciales Ya Utilizadas
            new object[] { 12345678, "Rcbertoldo", "2413", HttpStatusCode.Conflict, MessageConstants.UsuarioYaUtilizado }, // Usuario Ya Utilizado
            new object[] { 12345678, "rcbertoldo1", "2341", HttpStatusCode.Conflict, MessageConstants.ClaveYaUtilizada }, // Clave Ya Utilizada
            new object[] { 12345678, "rcbertoldo", "231a", HttpStatusCode.BadRequest }, // Clave Alfanumérica
            new object[] { 12345678, "rcbertoldo", "1234", HttpStatusCode.BadRequest }, // Clave Consecutiva Ascendente
            new object[] { 12345678, "rcbertoldo", "4321", HttpStatusCode.BadRequest }, // Clave Consecutiva Descendente
            new object[] { 12345678, "rcbertoldo", "1111", HttpStatusCode.BadRequest }, // Clave Números Iguales
            new object[] { 31423142, "rcbertoldo1", "1432", HttpStatusCode.NotFound }, // Usuario Inexistente
        };
    }
}
