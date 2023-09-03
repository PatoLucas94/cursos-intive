using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Service;
using Spv.Usuarios.Bff.Test.Unit.Service.Helpers;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.UsuarioService
{
    public class ObtenerPerfilV2AsyncTest
    {
        private const long IdPersonaCorrecto = 1447;
        private const long IdPersonaIncorrecto = 1111;

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { IdPersonaCorrecto, true, StatusCodes.Status200OK },
                new object[] { IdPersonaIncorrecto, false, StatusCodes.Status404NotFound },
            };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static UsuariosService CrearUsuarioService(IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();

            return new UsuariosService(
                loggerMock.Object,
                new Mock<IApiUsuariosRepository>().Object,
                apiUsuariosRepositoryV2,
                new Mock<IApiPersonasRepository>().Object,
                new Mock<IApiCatalogoRepository>().Object,
                new Mock<IApiNotificacionesRepository>().Object,
                MapperProfile.GetAppProfile(),
                new Mock<IApiScoreOperacionesRepository>().Object,
                new MemoryCache(new MemoryCacheOptions()),
                new Mock<IBackgroundJobClient>().Object
            );
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Perfil(long idPersona, bool isOk, int statusCode)
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            var result = new ApiUsuariosPerfilModelOutputV2
            {
                ultimo_login = DateTime.MinValue,
                id_persona = IdPersonaCorrecto,
                nro_documento = "11223344",
                tipo_documento = 1,
                email = "test@test.com",
                nombre = "Usuario1",
                apellido = "Test1",
                genero = "masculino",
                pais = 80,
                fecha_ultimo_cambio_clave = DateTime.MinValue,
                fecha_vencimiento_clave = DateTime.MinValue
            };

            var result2 = new ApiUsuariosPerfilModelOutputV2
            {
                ultimo_login = DateTime.MinValue,
                id_persona = IdPersonaCorrecto,
                nro_documento = "11223344",
                tipo_documento = 1,
                email = "test@test.com",
                nombre = "Usuario1",
                apellido = "Test1",
                genero = "masculino",
                pais = 80,
                fecha_ultimo_cambio_clave = DateTime.MinValue,
                fecha_vencimiento_clave = DateTime.MinValue
            };

            var jsonPerfil = JsonConvert.SerializeObject(result);
            var dataPerfil = new StringContent(jsonPerfil, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2
                .Setup(m => m.ObtenerPerfilAsync(IdPersonaCorrecto))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = dataPerfil });

            var jsonPerfil2 = JsonConvert.SerializeObject(result2);
            var dataPerfil2 = new StringContent(jsonPerfil2, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2
                .Setup(m => m.ObtenerPerfilAsync(IdPersonaIncorrecto))
                .ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = dataPerfil2
                    }
                );

            var datosRequestCorrectos = new PerfilModelInputV2
            {
                IdPersona = idPersona
            };

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object);

            // Act
            var resultado = await sut.ObtenerPerfilAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Fact]
        public async Task PerfilThrowException()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            
            apiUsuarioRepositoryV2.Setup(m =>
                    m.ObtenerPerfilAsync(It.IsAny<long>()))
                .Throws(new Exception("Excepción no controlada"));

            var datosRequestCorrectos = new PerfilModelInputV2
            {
                IdPersona = IdPersonaCorrecto
            };

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object);

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.ObtenerPerfilAsync(Headers.ToRequestBody(datosRequestCorrectos))
            );

            // Assert
            resultado.Message.Should().Be("Excepción no controlada");
        }
    }
}
