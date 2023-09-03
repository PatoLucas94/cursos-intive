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
using Spv.Usuarios.Common.Errors;
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
    public class CambiarClaveAsyncTest
    {
        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { IdPersonaTest1, "4132", true, StatusCodes.Status200OK },
                new object[] { IdPersonaTest2, "1423", false, StatusCodes.Status404NotFound },
            };

        public static IEnumerable<object[]> Datos2 =>
            new List<object[]>
            {
                new object[] { IdPersonaTest5, "4132" },
            };

        public static IEnumerable<object[]> Datos3 =>
            new List<object[]>
            {
                new object[] { IdPersonaTest4, "4132", true, StatusCodes.Status200OK },
                new object[] { IdPersonaTest2, "1423", false, StatusCodes.Status404NotFound },
            };

        private const long IdPersonaTest1 = 12345678;
        private const long IdPersonaTest2 = 25896314;
        private const long IdPersonaTest3 = 14141414;
        private const long IdPersonaTest4 = 14141413;
        private const long IdPersonaTest5 = 14141415;

        private const string clave1 = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8";

        private static readonly UsuarioV2 UsuarioV2Mock = new UsuarioV2
        {
            UserId = 1,
            PersonId = IdPersonaTest1,
            Username = "username1",
            Password = "clave_encriptada1",
            UserStatusId = (byte)UserStatus.Active,
            LoginAttempts = 0,
            LastPasswordChange = DateTime.Now.AddDays(-10),
            DocumentNumber = "87654321"
        };

        private static readonly UsuarioV2 Usuario2V2Mock = new UsuarioV2
        {
            UserId = 1,
            PersonId = IdPersonaTest4,
            Username = "username1",
            Password = clave1,
            UserStatusId = (byte)UserStatus.Active,
            LoginAttempts = 0,
            LastPasswordChange = DateTime.Now.AddDays(-10),
            DocumentNumber = "87654321"
        };

        private static readonly UsuarioV2 Usuario3V2Mock = new UsuarioV2
        {
            UserId = 1,
            PersonId = IdPersonaTest5,
            Username = "username1",
            Password = clave1,
            UserStatusId = (byte)UserStatus.Active,
            LoginAttempts = 0,
            LastPasswordChange = DateTime.Now.AddDays(-10),
            DocumentNumber = "87654321"
        };

        private static readonly AuditoriaLogV2 AuditoriaLogV2Mock = new AuditoriaLogV2
        {
            AuditLogId = 1,
            Channel = "OBI",
            DateTime = DateTime.MinValue,
            UserId = 1,
            EventTypeId = (int)EventTypes.PasswordChange,
            EventResultId = (int)EventResults.Ok,
            ExtendedInfo = "extended_info"
        };

        private static readonly List<Usuario> UsuariosHbiMock = new List<Usuario>
        {
            new Usuario
            {
                CustomerNumber =
                    $"0{UsuarioV2Mock.DocumentCountryId}0{UsuarioV2Mock.DocumentTypeId}{UsuarioV2Mock.DocumentNumber}",
                UserName = UsuarioV2Mock.PersonId.ToString(),
                Password = UsuarioV2Mock.Password,
                UserStatusId = UsuarioV2Mock.UserStatusId,
                Name = "UsuarioUT",
                LastName = "ApellidoUT",
                LastPasswordChange = UsuarioV2Mock.LastPasswordChange,
                CreatedDate = UsuarioV2Mock.CreatedDate,
                DocumentCountryId = "080",
                DocumentTypeId = 4,
                DocumentNumber = UsuarioV2Mock.DocumentNumber,
                IsEmployee = false,
                MobileEnabled = false,
                FullControl = true,
                LoginAttempts = 0,
                UserData = new DatosUsuario { PersonId = IdPersonaTest1.ToString() }
            },
            new Usuario
            {
                UserName = UsuarioV2Mock.PersonId.ToString(),
                UserStatusId = UsuarioV2Mock.UserStatusId,
                Password = UsuarioV2Mock.Password,
                Name = "UsuarioBTA",
                LastName = "ApellidoBTA",
                CreatedDate = DateTime.Now.AddDays(-10),
                LastPasswordChange = UsuarioV2Mock.LastPasswordChange,
                UserData = new DatosUsuario { PersonId = IdPersonaTest3.ToString() }
            }
        };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "OBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        private static readonly ApiHeaders HeadersBta = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "BTA",
            XUsuario = "user",
            XRequestId = "2"
        };

        private static UsuariosService CrearUsuarioService(
            IUsuarioV2Repository usuarioV2Repository,
            IAuditoriaLogV2Repository auditoriaLogV2Repository,
            IUsuarioRepository usuarioHbiRepository = null)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var encryptionMock = new Mock<IEncryption>();

            var dateTimeNow = DateTime.Now;

            var helperDbServerMock = new Mock<IHelperDbServer>();
            helperDbServerMock.Setup(m => m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServer { Now = dateTimeNow });

            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();
            helperDbServerMockV2.Setup(m =>
                m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServerV2 { Now = dateTimeNow });

            var auditoriaMock = new Mock<IAuditoriaRepository>();

            encryptionMock.Setup(m => m.GetHash("clave_encriptada")).Returns(clave1);

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
                usuarioHbiRepository ?? usuarioRepository.Object,
                usuarioV2Repository,
                auditoriaMock.Object,
                auditoriaLogV2Repository,
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
        public async Task CambiarClave(long idPersona, string nuevaClave, bool isOk, int statusCode)
        {
            // Arrange 
            var usuarioHbiRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var usuarioHbiMock = UsuariosHbiMock.FirstOrDefault(x =>
                x.UserData.PersonId == IdPersonaTest1.ToString());

            usuarioHbiRepository.Setup(m =>
                    m.ObtenerUsuarioByPersonIdAsync(IdPersonaTest1))
                .ReturnsAsync(usuarioHbiMock);

            usuarioV2Repository.Setup(m => m
                    .ObtenerUsuarioByPersonIdAsync(IdPersonaTest1))
                .ReturnsAsync(UsuarioV2Mock);

            auditoriaV2Repository.Setup(m => m
                    .SaveAuditLogAsync(
                        It.IsAny<int>(),
                        It.IsAny<EventTypes>(),
                        It.IsAny<EventResults>(),
                        It.IsAny<string>(),
                        It.IsAny<FechaDbServerV2>(),
                        It.IsAny<string>()))
                .ReturnsAsync(AuditoriaLogV2Mock);

            var datosRequestCorrectos = new CambioDeClaveModelInputV2
            {
                PersonId = idPersona,
                NewPassword = nuevaClave
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object, auditoriaV2Repository.Object,
                usuarioHbiRepository.Object);

            // Act
            var resultado = await sut.ModificarClaveAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);

            if (statusCode.Equals(StatusCodes.Status200OK))
            {
                UsuarioV2Mock.LastPasswordChange?.Date.Should().Be(DateTime.Today);
                UsuarioV2Mock.LastPasswordChange.Should().Be(usuarioHbiMock?.LastPasswordChange);
            }
        }

        [Theory]
        [MemberData(nameof(Datos3))]
        public async Task CambiarClaveValidarClaveActual(long idPersona, string nuevaClave, bool isOk, int statusCode)
        {
            // Arrange
            var usuarioHbiRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var usuarioHbiMock = UsuariosHbiMock.FirstOrDefault(x =>
                x.UserData.PersonId == IdPersonaTest4.ToString());

            usuarioHbiRepository.Setup(m =>
                    m.ObtenerUsuarioByPersonIdAsync(IdPersonaTest4))
                .ReturnsAsync(usuarioHbiMock);

            usuarioV2Repository.Setup(m => m
                    .ObtenerUsuarioByPersonIdAsync(IdPersonaTest4))
                .ReturnsAsync(Usuario2V2Mock);

            auditoriaV2Repository.Setup(m => m
                    .SaveAuditLogAsync(
                        It.IsAny<int>(),
                        It.IsAny<EventTypes>(),
                        It.IsAny<EventResults>(),
                        It.IsAny<string>(),
                        It.IsAny<FechaDbServerV2>(),
                        It.IsAny<string>()))
                .ReturnsAsync(AuditoriaLogV2Mock);

            var datosRequestCorrectos = new CambioDeClaveModelInputV2
            {
                PersonId = idPersona,
                NewPassword = nuevaClave,
                CurrentPasword = "clave_encriptada"
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object, auditoriaV2Repository.Object,
                usuarioHbiRepository.Object);

            // Act
            var resultado = await sut.ModificarClaveAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Theory]
        [MemberData(nameof(Datos2))]
        public async Task CambiarClaveValidarClaveActualNoValida(long idPersona, string nuevaClave)
        {
            // Arrange
            var usuarioHbiRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var usuarioHbiMock = UsuariosHbiMock.FirstOrDefault(x =>
                x.UserData.PersonId == IdPersonaTest5.ToString());

            usuarioHbiRepository.Setup(m =>
                    m.ObtenerUsuarioByPersonIdAsync(IdPersonaTest5))
                .ReturnsAsync(usuarioHbiMock);

            usuarioV2Repository.Setup(m => m
                    .ObtenerUsuarioByPersonIdAsync(IdPersonaTest5))
                .ReturnsAsync(Usuario3V2Mock);

            auditoriaV2Repository.Setup(m => m
                    .SaveAuditLogAsync(
                        It.IsAny<int>(),
                        It.IsAny<EventTypes>(),
                        It.IsAny<EventResults>(),
                        It.IsAny<string>(),
                        It.IsAny<FechaDbServerV2>(),
                        It.IsAny<string>()))
                .ReturnsAsync(AuditoriaLogV2Mock);

            var datosRequestCorrectos = new CambioDeClaveModelInputV2
            {
                PersonId = idPersona,
                NewPassword = nuevaClave,
                CurrentPasword = "clave_no_valida"
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object, auditoriaV2Repository.Object,
                usuarioHbiRepository.Object);

            // Act
            var resultado = await sut.ModificarClaveAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var message = resultado.Match(a =>
                a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            message.Should().Be(ErrorCode.ClaveActualNoCoincide.ErrorDescription);
        }

        [Fact]
        public async Task UsuarioInexistente()
        {
            // Arrange
            var usuarioHbiRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();

            var sut = CrearUsuarioService(usuarioV2Repository.Object, auditoriaV2Repository.Object,
                usuarioHbiRepository.Object);

            // Act
            var resultado = await sut.ModificarClaveAsync(
                Headers.ToRequestBody(new CambioDeClaveModelInputV2(), AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = resultado.Match(a =>
                a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            message.Should().Be(ErrorCode.UsuarioInexistente.ErrorDescription);
        }

        [Fact]
        public async Task OperacionNoHabilitada_XCanalBTA_UsuarioHBI()
        {
            // Arrange
            var usuarioHbiRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var usuarioHbiMock = UsuariosHbiMock.FirstOrDefault(x =>
                x.UserData.PersonId == IdPersonaTest3.ToString());

            usuarioHbiRepository.Setup(m =>
                    m.ObtenerUsuarioByPersonIdAsync(IdPersonaTest3))
                .ReturnsAsync(usuarioHbiMock);

            var datosRequestCorrectos = new CambioDeClaveModelInputV2
            {
                PersonId = IdPersonaTest3,
                NewPassword = "Info1212"
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object, auditoriaV2Repository.Object,
                usuarioHbiRepository.Object);

            // Act
            var resultado = await sut.ModificarClaveAsync(
                HeadersBta.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var message = resultado.Match(a =>
                a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            message.Should().Be(ErrorCode.OperacionNoHabilitada.ErrorDescription);
        }
    }
}
