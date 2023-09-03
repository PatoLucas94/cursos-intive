using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
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
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Service;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Spv.Usuarios.Test.Unit.Service.Helpers;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.UsuarioService
{
    public class CambiarEstadoAsyncTest
    {
        [Theory]
        [MemberData(nameof(Datos))]
        public async Task CambiarEstado(long personId, bool isOk)
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();

            usuarioV2Repository.Setup(m => m.FindAsync(It.IsAny<Expression<Func<UsuarioV2, bool>>>(), false))
                .ReturnsAsync(
                    new UsuarioV2
                    {
                        UserId = 1,
                        PersonId = personId
                    }
                );

            usuarioV2Repository.Setup(m => m.CambiarEstadoAsync(It.IsAny<long>(), It.IsAny<UserStatus>()))
                .ReturnsAsync(isOk);

            var datosRequestCorrectos = new CambioEstadoModelInput
            {
                PersonId = personId,
                EstadoId = UserStatus.Active
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.CambiarEstadoAsync(Headers.ToRequestBody(datosRequestCorrectos,
                AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
        }

        [Fact]
        public async Task CuandoCambiarEstadoAsyncThrowsUnaExcepcion()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();

            usuarioV2Repository.Setup(m => m.FindAsync(It.IsAny<Expression<Func<UsuarioV2, bool>>>(), false))
                .ReturnsAsync(
                    new UsuarioV2
                    {
                        UserId = 1,
                        PersonId = 1
                    }
                );

            usuarioV2Repository.Setup(m => m.CambiarEstadoAsync(It.IsAny<long>(), It.IsAny<UserStatus>()))
                .Throws(new Exception("Excepcion no controlada"));

            var datosRequestCorrectos = new CambioEstadoModelInput
            {
                PersonId = PersonIdTest0,
                EstadoId = UserStatus.Blocked
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.CambiarEstadoAsync(
                    Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels())
                ));

            // Assert
            resultado.Message.Should().Be("Excepcion no controlada");
        }

        private static UsuariosService CrearUsuarioService(IUsuarioV2Repository usuarioV2Repository)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var auditoriaLogV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var encryptionMock = new Mock<IEncryption>();
            var helperDbServerMock = new Mock<IHelperDbServer>();
            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();
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

            helperDbServerMock.Setup(m => m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServer { Now = DateTime.Now });

            helperDbServerMockV2.Setup(m => m.ObtenerFechaAsync())
                .ReturnsAsync(new FechaDbServerV2 { Now = DateTime.Now });

            auditoriaLogV2Repository.Setup(m => m.SaveAuditLogAsync(
                It.IsAny<int>(),
                It.IsAny<EventTypes>(),
                It.IsAny<EventResults>(),
                It.IsAny<string>(),
                It.IsAny<FechaDbServerV2>(),
                It.IsAny<string>())
            ).ReturnsAsync(new AuditoriaLogV2());

            return new UsuariosService(
                loggerMock.Object,
                usuarioRepository.Object,
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

        public static IEnumerable<object[]> Datos => new List<object[]>
        {
            new object[] { PersonIdTest0, true },
            new object[] { PersonIdNoExiste, false }
        };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "HBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        private const long PersonIdTest0 = 12345678;
        private const long PersonIdNoExiste = 0;
    }
}
