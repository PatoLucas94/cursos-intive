using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.DataAccess.EntityFramework;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class CambioDeClaveIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        private readonly ConfiguracionRepository _configuracionRepository;

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostCambiarClaveUsuario(_uriBase, 
                new CambioDeClaveModelRequestV2 { IdPersona = 12345678, NuevaClave = "4132" })
        };

        private static ServiceRequest PostCambiarClaveUsuario(Uri uriBase, CambioDeClaveModelRequestV2 body)
        {
            var uri = new Uri(uriBase, ApiUris.CambiarClaveUsuarioV2);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public CambioDeClaveIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;

            var dbHbi = server.HttpServer.TestServer.Services.GetRequiredService<GenericDbContext>();

            _configuracionRepository = new ConfiguracionRepository(dbHbi);
        }

        [Fact]
        public async Task CambiarClaveUsuarioConCambioDeCantidadDeHistorialDeCambiosDeClaveAsync()
        {
            // Arrange
            // History:
            // .- Info1214
            // .- Info1213
            // .- Info1212 y Actual
            // CantidadDeHistorialDeCambiosDeClave = 3
            var body = new CambioDeClaveModelRequestV2
            {
                IdPersona = 13131313,
                NuevaClave = "Info1211" // 2+VDuq5HVtS6C91POaPM/lY5DFOuvt2rDTpPrDEvVcg
            };

            var request = PostCambiarClaveUsuario(_uriBase, body);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.OK);

            // Arrange
            var cantidadDeHistorialDeCambiosDeClave = _configuracionRepository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeUsers,
                AppConstants.CantidadDeHistorialDeCambiosDeClave);
            cantidadDeHistorialDeCambiosDeClave.Result.Value = "2";

            await _configuracionRepository.SaveChangesAsync();

            // History:
            // .- Info1212
            // .- Info1214
            // .- Info1213
            // .- Info1212
            // Actual -> Info1211
            // CantidadDeHistorialDeCambiosDeClave = 2
            body = new CambioDeClaveModelRequestV2
            {
                IdPersona = 13131313,
                NuevaClave = "Info1214" // ZnZibQpuudsAf+wJNhcwtcDwqbz3qQ3/YI8rcV9mOnA
            };

            request = PostCambiarClaveUsuario(_uriBase, body);

            // Act
            sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.OK);

            // Arrange
            // History:
            // .- Info1211
            // .- Info1212
            // .- Info1214
            // .- Info1213
            // .- Info1212
            // Actual -> Info1214
            // CantidadDeHistorialDeCambiosDeClave = 2
            body = new CambioDeClaveModelRequestV2
            {
                IdPersona = 13131313,
                NuevaClave = "Info1211" // 2+VDuq5HVtS6C91POaPM/lY5DFOuvt2rDTpPrDEvVcg
            };
            
            request = PostCambiarClaveUsuario(_uriBase, body);

            // Act
            sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Conflict);

            // Restoring Configuration
            cantidadDeHistorialDeCambiosDeClave = _configuracionRepository.ObtenerConfiguracion(
                AppConstants.ConfigurationTypeUsers,
                AppConstants.CantidadDeHistorialDeCambiosDeClave);
            cantidadDeHistorialDeCambiosDeClave.Result.Value = "3";

            await _configuracionRepository.SaveChangesAsync();
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task CambiarClaveUsuarioAsync(long idPersona, string nuevaClave, HttpStatusCode httpStatusCode)
        {
            // Arrange
            var body = new CambioDeClaveModelRequestV2
            {
                IdPersona = idPersona,
                NuevaClave = nuevaClave
            };

            var request = PostCambiarClaveUsuario(_uriBase, body);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);
        }

        public static IEnumerable<object[]> Datos =>
        new List<object[]>
        {
            new object[] { 12345678, "1432", HttpStatusCode.OK },
            new object[] { 12345678, "2341", HttpStatusCode.Conflict }, // Clave Ya Utilizada
            new object[] { 12345678, "231", HttpStatusCode.BadRequest }, // Longitud de 4
            new object[] { 12345678, "231a", HttpStatusCode.BadRequest }, // Clave Alfanumérica
            new object[] { 12345678, "1234", HttpStatusCode.BadRequest }, // Clave Consecutiva Ascendente
            new object[] { 12345678, "4321", HttpStatusCode.BadRequest }, // Clave Consecutiva Descendente
            new object[] { 12345678, "1111", HttpStatusCode.BadRequest }, // Clave Números Iguales
            new object[] { 31423142, "1432", HttpStatusCode.NotFound }, // Usuario Inexistente
            new object[] { 13131313, "Info121x", HttpStatusCode.OK },
            new object[] { 13131313, "Info1212", HttpStatusCode.Conflict }, // Clave Ya Utilizada
            new object[] { 13131313, "231a", HttpStatusCode.BadRequest }, // Longitud mínima de 8
            new object[] { 13131313, "12345678nnnnnnn1", HttpStatusCode.BadRequest }, // Longitud máxima de 15
        };
    }
}
