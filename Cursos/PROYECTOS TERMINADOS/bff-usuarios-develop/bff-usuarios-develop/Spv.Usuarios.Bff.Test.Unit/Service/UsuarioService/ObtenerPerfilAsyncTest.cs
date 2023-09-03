using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Service;
using Spv.Usuarios.Bff.Test.Unit.Service.Helpers;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.UsuarioService
{
    public class ObtenerPerfilAsyncTest
    {
        // Usuario correcto
        private const string UsuarioCorrectoId = "PersonId1";
        private const string UsuarioCorrectoNombre = "UsuarioCorrectoNombre";
        private const string UsuarioIncorrectoNombre = "UsuarioIncorrectoNombre";
        private const string UsuarioIncorrectoId = "PersonIncorrectaId1";

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { UsuarioCorrectoNombre, true, StatusCodes.Status200OK },
                new object[] { UsuarioIncorrectoNombre, false, StatusCodes.Status404NotFound },
            };

        private static readonly List<ApiUsuariosPerfilModelOutput> Usuarios = new List<ApiUsuariosPerfilModelOutput>
        {
            new ApiUsuariosPerfilModelOutput
            {
                ultimo_login = DateTime.MinValue,
                id_persona = "PersonId1",
                id_usuario = 1,
                email = "test@test.com",
                nombre = "Usuario1",
                apellido = "Test1"
            },
            new ApiUsuariosPerfilModelOutput
            {
                ultimo_login = DateTime.MinValue,
                id_persona = "PersonId2",
                id_usuario = 1,
                email = "test@test.com",
                nombre = "Usuario2",
                apellido = "Test2"
            },
        };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static UsuariosService CrearUsuarioService(
            IApiUsuariosRepository apiUsuariosRepository,
            IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2
        )
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();

            return new UsuariosService(
                loggerMock.Object,
                apiUsuariosRepository,
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
        public async Task Perfil(string nombreUsuario, bool isOk, int statusCode)
        {
            // Arrange
            var apiUsuarioRepository = new Mock<IApiUsuariosRepository>();
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            apiUsuarioRepository.Setup(m =>
                    m.ObtenerPerfilAsync(UsuarioCorrectoNombre))
                .ReturnsAsync(Usuarios.FirstOrDefault(x => x.id_persona == UsuarioCorrectoId));

            apiUsuarioRepository.Setup(m =>
                    m.ObtenerPerfilAsync(UsuarioIncorrectoNombre))
                .ReturnsAsync(Usuarios.FirstOrDefault(x => x.id_persona == UsuarioIncorrectoId));

            var datosRequestCorrectos = new PerfilModelInput
            {
                UserName = nombreUsuario
            };

            var sut = CrearUsuarioService(apiUsuarioRepository.Object, apiUsuarioRepositoryV2.Object);

            // Act
            var resultado = await sut.ObtenerPerfilAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }
    }
}
