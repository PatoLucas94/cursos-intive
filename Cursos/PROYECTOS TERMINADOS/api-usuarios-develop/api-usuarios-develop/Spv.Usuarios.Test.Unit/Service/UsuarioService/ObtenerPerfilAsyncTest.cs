using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Service;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Spv.Usuarios.Test.Unit.Service.Helpers;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.UsuarioService
{
    public class ObtenerPerfilAsyncTest
    {
        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Perfil(string usuario, bool isOk, int statusCode)
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserName == usuario);
            usuarioRepository.Setup(m => m.ObtenerPerfilUsuarioAsync(It.IsAny<string>())).ReturnsAsync(usuarioMock);

            var datosRequestCorrectos = new PerfilModelInput
            {
                UserName = usuario
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.ObtenerPerfilAsync(Headers.ToRequestBody(datosRequestCorrectos,
                AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Fact]
        public async Task PerfilCuandoObtenerPerfilAsyncThrowsUnaExcepcion()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            usuarioRepository.Setup(m => m.ObtenerPerfilUsuarioAsync(It.IsAny<string>()))
                .Throws(new Exception("Excepcion no controlada"));

            var datosRequestCorrectos = new PerfilModelInput
            {
                UserName = UsuarioTest0
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.ObtenerPerfilAsync(Headers.ToRequestBody(datosRequestCorrectos,
                    AllowedChannelsBuilder.CrearAllowedChannels())));

            // Assert
            resultado.Message.Should().Be("Excepcion no controlada");
        }

        private static UsuariosService CrearUsuarioService(IUsuarioRepository usuarioRepository)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var auditoriaLogV2Repository = new Mock<IAuditoriaLogV2Repository>();

            var encryptionMock = new Mock<IEncryption>();

            var helperDbServerMock = new Mock<IHelperDbServer>();
            helperDbServerMock.Setup(m => m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServer { Now = DateTime.Now });

            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();
            helperDbServerMockV2.Setup(m => m.ObtenerFechaAsync())
                .ReturnsAsync(new FechaDbServerV2 { Now = DateTime.Now });

            var auditoriaMock = new Mock<IAuditoriaRepository>();
            var personasRepository = new Mock<IPersonasRepository>();
            var datosUsuarioRepository = new Mock<IDatosUsuarioRepository>();
            var nsbtRepository = new Mock<INsbtRepository>();
            var tDesRepository = new Mock<ITDesEncryption>();
            var historialClaveUsuariosV2Repository = new Mock<IHistorialClaveUsuariosV2Repository>();
            var historialUsuarioUsuariosV2Repository = new Mock<IHistorialUsuarioUsuariosV2Repository>();
            var historialClaveUsuariosRepository = new Mock<IHistorialClaveUsuariosRepository>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();
            var btaRepository = new Mock<IBtaRepository>();

            return new UsuariosService(
                loggerMock.Object,
                usuarioRepository,
                usuarioV2Repository.Object,
                auditoriaMock.Object,
                auditoriaLogV2Repository.Object,
                configuracionesService.Object,
                configuracionesV2Service.Object,
                encryptionMock.Object,
                helperDbServerMock.Object,
                helperDbServerMockV2.Object,
                personasRepository.Object,
                datosUsuarioRepository.Object,
                nsbtRepository.Object,
                tDesRepository.Object,
                historialClaveUsuariosV2Repository.Object,
                historialUsuarioUsuariosV2Repository.Object,
                historialClaveUsuariosRepository.Object,
                reglaValidacionV2Repository.Object,
                MapperProfile.GetAppProfile(),
                new Mock<IDistributedCache>().Object,
                btaRepository.Object
            );
        }

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { UsuarioTest0, true, StatusCodes.Status200OK },
                new object[] { UsuarioNoExiste, false, StatusCodes.Status404NotFound },
            };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "HBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        private const string UsuarioTest0 = "Usuario0";
        private const string UsuarioNoExiste = "UsuarioNoExiste";

        private static readonly List<Usuario> Usuarios = new List<Usuario>
        {
            new Usuario
            {
                UserId = 0,
                UserName = UsuarioTest0,
                Name = UsuarioTest0,
                LastName = UsuarioTest0,
                CustomerNumber = "0800435125487",
                Password = "PasswordEncriptado",
                UserStatusId = 0,
                UserData = new DatosUsuario
                {
                    PersonId = "PersonIdTest",
                    Mail = "test@test.com"
                }
            }
        };
    }
}
