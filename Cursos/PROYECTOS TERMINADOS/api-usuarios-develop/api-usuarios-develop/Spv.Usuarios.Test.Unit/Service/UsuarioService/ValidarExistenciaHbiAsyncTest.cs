using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
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
    public class ValidarExistenciaHbiAsyncTest
    {
        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { NombreUsuarioTest1, true, StatusCodes.Status200OK },
                new object[] { NombreUsuarioTest2, true, StatusCodes.Status409Conflict }
            };

        private const string NombreUsuarioTest1 = "Usuario99999";
        private const string NombreUsuarioTest2 = "UsuarioTest2";

        private const long PersonId0 = 0;

        private static readonly Usuario UsuarioMock = new Usuario
        {
            UserId = 0,
            UserName = NombreUsuarioTest1,
            Password = "clave_encriptada0",
            UserStatusId = 3,
            UserData = new DatosUsuario
            {
                PersonId = PersonId0.ToString()
            },
            DocumentNumber = "11111111",
            DocumentCountryId = "80",
            DocumentTypeId = 4
        };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "HBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        private static UsuariosService CrearUsuarioService(
            IUsuarioRepository usuarioRepository,
            IUsuarioV2Repository usuarioV2Repository)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var auditoriaLogV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var encryptionMock = new Mock<IEncryption>();

            var helperDbServerMock = new Mock<IHelperDbServer>();
            helperDbServerMock.Setup(m =>
                m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServer { Now = DateTime.Now });

            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();
            helperDbServerMockV2.Setup(m =>
                m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServerV2 { Now = DateTime.Now });

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
                usuarioV2Repository,
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

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ValidarExistenciaNombreUsuarioExistente(string nomUsuario, bool isOk, int statusCode)
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioAsync(
                        NombreUsuarioTest1))
                .ReturnsAsync(UsuarioMock);

            var datosRequestCorrectos = new ValidacionExistenciaHbiModelInput
            {
                UserName = nomUsuario
            };

            var sut = CrearUsuarioService(usuarioRepository.Object, usuarioV2Repository.Object);

            // Act
            var resultado = await sut.ValidarExistenciaHbiAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            if (statusCode == StatusCodes.Status200OK)
            {
                var res = resultado.Match(
                    a => a.Payload,
                    a => new ValidacionExistenciaHbiModelOutput(),
                    a => new ValidacionExistenciaHbiModelOutput());


                res.ExisteUsuario.Should().BeTrue();
            }

            isOk.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ValidarExistenciaNombreUsuarioNoExistente(string nomUsuario, bool isOk, int statusCode)
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioAsync(
                        NombreUsuarioTest2))
                .ReturnsAsync(UsuarioMock);

            var datosRequestCorrectos = new ValidacionExistenciaHbiModelInput
            {
                UserName = nomUsuario
            };

            var sut = CrearUsuarioService(usuarioRepository.Object, usuarioV2Repository.Object);

            // Act
            var resultado = await sut.ValidarExistenciaHbiAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            if (statusCode == StatusCodes.Status200OK)
            {
                var res = resultado.Match(
                    a => a.Payload,
                    a => new ValidacionExistenciaHbiModelOutput(),
                    a => new ValidacionExistenciaHbiModelOutput());


                res.ExisteUsuario.Should().BeFalse();
            }

            isOk.Should().BeTrue();
        }

        [Fact]
        public async Task ValidarExistenciaThrowException()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();

            var datosRequestCorrectos = new ValidacionExistenciaHbiModelInput
            {
                UserName = NombreUsuarioTest2
            };

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioAsync(
                        It.IsAny<string>()))
                .Throws(new Exception("Excepción no controlada"));

            var sut = CrearUsuarioService(usuarioRepository.Object, usuarioV2Repository.Object);

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.ValidarExistenciaHbiAsync(
                    Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels())));

            // Assert
            resultado.Message.Should().Be("Excepción no controlada");
        }
    }
}
