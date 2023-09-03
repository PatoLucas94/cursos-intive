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
using Spv.Usuarios.Common.Constants;
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
    public class AutenticarAsyncTestV2
    {
        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Credenciales(string usuario, string clave, string nroDocumento, bool isOk, int statusCode)
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            usuarioV2Repository.Setup(m =>
                m.ObtenerUsuarioByDocumentNumber(NroDocumentoCorrecto)).Returns(Usuarios.AsQueryable);

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = usuario,
                Password = clave,
                DocumentNumber = nroDocumento
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Theory]
        [MemberData(nameof(DatosClaveVencida))]
        public async Task ClaveVencida(double? cantidadDiasLastPasswordChange, bool esperado,
            ConfiguracionV2 configuracion = null)
        {
            // Arrange
            var now = DateTime.Now;
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();

            var sut = CrearUsuarioService(usuarioV2Repository.Object, configuracion);

            DateTime? lastPasswordChange = null;

            if (cantidadDiasLastPasswordChange != null)
            {
                lastPasswordChange = now.AddDays((double)cantidadDiasLastPasswordChange);
            }

            // Act
            var resultado = await sut.ClaveVencidaAsync(lastPasswordChange, now);

            // Assert
            resultado.Should().Be(esperado);
        }

        [Fact]
        public async Task UsuarioIncorrecto()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(new AutenticacionModelInput(), AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var message = resultado.Match(a =>
                a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            message.Should().Be(ErrorCode.UsuarioIncorrecto.ErrorDescription);
        }

        [Fact]
        public async Task UsuarioBloqueado()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            usuarioV2Repository.Setup(m =>
                m.ObtenerUsuarioByDocumentNumber(It.IsAny<string>())).Returns(Usuarios.AsQueryable);

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest2,
                Password = ClaveCorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task UsuarioInactivo()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            usuarioV2Repository.Setup(m =>
                m.ObtenerUsuarioByDocumentNumber(It.IsAny<string>())).Returns(Usuarios.AsQueryable);

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest3,
                Password = ClaveCorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(Headers.ToRequestBody(datosRequestCorrectos,
                AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task UsuarioConClaveVencida()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            usuarioV2Repository.Setup(m =>
                m.ObtenerUsuarioByDocumentNumber(It.IsAny<string>())).Returns(Usuarios.AsQueryable);

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest4,
                Password = ClaveCorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeTrue();
            resultado.StatusCode.Should().Be(StatusCodes.Status202Accepted);
        }

        [Fact]
        public async Task EstadoDeUsuarioNoControlado()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            usuarioV2Repository.Setup(m =>
                m.ObtenerUsuarioByDocumentNumber(It.IsAny<string>())).Returns(Usuarios.AsQueryable);

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest0,
                Password = ClaveCorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(Headers.ToRequestBody(datosRequestCorrectos,
                AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task EnAutenticarAsyncCuandoGetHashThrowsException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var auditoriaLogV2Repository = new Mock<IAuditoriaLogV2Repository>();

            var configuracionV2Mock = ConfiguracionesV2.FirstOrDefault(x =>
                x.Name == AppConstants.DiasParaForzarCambioDeClaveKey);
            if (!int.TryParse(configuracionV2Mock?.Value, out var cantidadDiasParaForzarCambioDeClave))
            {
                configuracionesService.Setup(m =>
                        m.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync())
                    .ReturnsAsync(cantidadDiasParaForzarCambioDeClave);
            }
            else
            {
                configuracionesService.Setup(m =>
                        m.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync())
                    .ReturnsAsync(180);
            }

            configuracionV2Mock = ConfiguracionesV2.FirstOrDefault(x =>
                x.Name == AppConstants.CantidadDeIntentosDeLoginKey);
            if (!int.TryParse(configuracionV2Mock?.Value, out var cantidadDeIntentosDeLoginKeyV2))
            {
                configuracionesService.Setup(m =>
                        m.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync())
                    .ReturnsAsync(cantidadDeIntentosDeLoginKeyV2);
            }
            else
            {
                configuracionesService.Setup(m =>
                        m.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync())
                    .ReturnsAsync(5);
            }

            var configuracionMock = Configuraciones.FirstOrDefault(x =>
                x.Name == AppConstants.CantidadDeIntentosDeLoginKey);
            if (!int.TryParse(configuracionMock?.Value, out var cantidadDeIntentosDeLoginKey))
            {
                configuracionesService.Setup(m =>
                        m.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync())
                    .ReturnsAsync(cantidadDeIntentosDeLoginKey);
            }
            else
            {
                configuracionesService.Setup(m =>
                        m.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync())
                    .ReturnsAsync(3);
            }

            var encryptionMock = new Mock<IEncryption>();
            encryptionMock.Setup(m =>
                    m.GetHash(It.IsAny<string>()))
                .Throws(new Exception(MessageConstants.NoSeEncontroSecretKey));

            var helperDbServerMock = new Mock<IHelperDbServer>();
            helperDbServerMock.Setup(m =>
                    m.ObtenerFechaAsync())
                .ReturnsAsync(new FechaDbServer { Now = DateTime.Now });

            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();
            helperDbServerMockV2.Setup(m =>
                    m.ObtenerFechaAsync())
                .ReturnsAsync(new FechaDbServerV2 { Now = DateTime.Now });

            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            usuarioV2Repository.Setup(m =>
                    m.ObtenerUsuarioByDocumentNumber(It.IsAny<string>()))
                .Returns(Usuarios.AsQueryable);

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

            var sut = new UsuariosService(
                loggerMock.Object,
                usuarioRepository.Object,
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

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.AutenticarAsync(
                    Headers.ToRequestBody(new AutenticacionModelInput { DocumentNumber = "123" },
                        AllowedChannelsBuilder.CrearAllowedChannels())));

            // Assert
            resultado.Message.Should().Be(MessageConstants.NoSeEncontroSecretKey);
        }

        [Fact]
        public async Task LoginNoExitosoClaveCorrectaNroDocumentoIncorrecto()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();

            var datosRequestIncorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveCorrecta,
                DocumentNumber = NroDocumentoIncorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task LoginNoExitosoClaveIncorrectaSinBloqueoDeUsuario()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var usuarioV2Mock = new UsuarioV2
            {
                UserId = 1,
                Username = UsuarioTest1,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10)
            };

            var usuarios = new List<UsuarioV2> { usuarioV2Mock };

            usuarioV2Repository.Setup(m =>
                m.ObtenerUsuarioByDocumentNumber(It.IsAny<string>())).Returns(usuarios.AsQueryable);

            var datosRequestIncorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveIncorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(1);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(2);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task LoginNoExitosoClaveIncorrectaSinBloqueoDeUsuarioConSegundoIntentoCorrecto()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var usuarioV2Mock = new UsuarioV2
            {
                UserId = 1,
                Username = UsuarioTest1,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10)
            };

            var usuarios = new List<UsuarioV2> { usuarioV2Mock };

            usuarioV2Repository.Setup(m =>
                m.ObtenerUsuarioByDocumentNumber(It.IsAny<string>())).Returns(usuarios.AsQueryable);

            var datosRequestIncorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveIncorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(1);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Arrange
            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveCorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            // Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(0);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeTrue();
            resultado.StatusCode.Should().Be(StatusCodes.Status202Accepted);
        }

        [Fact]
        public async Task LoginNoExitosoClaveIncorrectaSinBloqueoDeUsuarioConUsuarioYaBloqueado()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var usuarioV2Mock = new UsuarioV2
            {
                UserId = 2,
                Username = UsuarioTest2,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Blocked,
                LoginAttempts = 3,
                LastPasswordChange = DateTime.Now.AddDays(-10)
            };

            var usuarios = new List<UsuarioV2> { usuarioV2Mock };

            usuarioV2Repository.Setup(m =>
                m.ObtenerUsuarioByDocumentNumber(It.IsAny<string>())).Returns(usuarios.AsQueryable);

            var datosRequestIncorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveIncorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(3);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Blocked);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(3);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Blocked);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task LoginNoExitosoClaveIncorrectaSinBloqueoDeUsuarioConUsuarioInactivo()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var usuarioV2Mock = new UsuarioV2
            {
                UserId = 3,
                Username = UsuarioTest3,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Inactive,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10)
            };

            var usuarios = new List<UsuarioV2> { usuarioV2Mock };

            usuarioV2Repository.Setup(m =>
                m.ObtenerUsuarioByDocumentNumber(It.IsAny<string>())).Returns(usuarios.AsQueryable);

            var datosRequestIncorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveIncorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(0);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Inactive);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(0);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Inactive);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task LoginNoExitosoClaveIncorrectaConBloqueoDeUsuario()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var usuarioV2Mock = new UsuarioV2
            {
                UserId = 1,
                Username = UsuarioTest1,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10)
            };

            var usuarios = new List<UsuarioV2> { usuarioV2Mock };

            usuarioV2Repository.Setup(m =>
                m.ObtenerUsuarioByDocumentNumber(It.IsAny<string>())).Returns(usuarios.AsQueryable);

            var datosRequestIncorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveIncorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(1);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(2);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(3);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(4);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(5);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Blocked);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        private static UsuariosService CrearUsuarioService(
            IUsuarioV2Repository usuarioV2Repository,
            ConfiguracionV2 configuracionV2Mock = null)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var auditoriaLogV2Repository = new Mock<IAuditoriaLogV2Repository>();

            configuracionV2Mock ??= ConfiguracionesV2.FirstOrDefault(x =>
                x.Name.Equals(AppConstants.DiasParaForzarCambioDeClaveKey));

            if (!int.TryParse(configuracionV2Mock?.Value, out var cantidadDiasParaForzarCambioDeClave))
            {
                configuracionesV2Service.Setup(m =>
                        m.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync())
                    .ReturnsAsync(cantidadDiasParaForzarCambioDeClave);
            }
            else
            {
                configuracionesV2Service.Setup(m =>
                        m.ObtenerConfiguracionDiasParaForzarCambioDeClaveAsync())
                    .ReturnsAsync(180);
            }

            configuracionV2Mock ??= ConfiguracionesV2.FirstOrDefault(x =>
                x.Name.Equals(AppConstants.CantidadDeIntentosDeLoginKey));

            if (!int.TryParse(configuracionV2Mock?.Value, out var cantidadDeIntentosDeLoginKeyV2))
            {
                configuracionesService.Setup(m =>
                        m.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync())
                    .ReturnsAsync(cantidadDeIntentosDeLoginKeyV2);
            }
            else
            {
                configuracionesV2Service.Setup(m =>
                        m.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync())
                    .ReturnsAsync(5);
            }

            var encryptionMock = new Mock<IEncryption>();
            encryptionMock.Setup(m => m.GetHash(ClaveCorrecta)).Returns(ClaveEncriptadaValida);
            encryptionMock.Setup(m => m.GetHash(ClaveIncorrecta)).Returns(ClaveEncriptadaInvalida);

            encryptionMock.Setup(m => m.GetHash(UsuarioTest0)).Returns<string>(x => x);
            encryptionMock.Setup(m => m.GetHash(UsuarioTest1)).Returns<string>(x => x);
            encryptionMock.Setup(m => m.GetHash(UsuarioTest2)).Returns<string>(x => x);
            encryptionMock.Setup(m => m.GetHash(UsuarioTest3)).Returns<string>(x => x);
            encryptionMock.Setup(m => m.GetHash(UsuarioTest4)).Returns<string>(x => x);

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

        #region Datos

        private const string ClaveEncriptadaValida = "PasswordEncriptado";
        private const string ClaveEncriptadaInvalida = "PasswordEncriptadoInvalido";
        private const string UsuarioTest0 = "Usuario0";
        private const string UsuarioTest1 = "Usuario1";
        private const string UsuarioTest2 = "Usuario2";
        private const string UsuarioTest3 = "Usuario3";
        private const string UsuarioTest4 = "Usuario4";
        private const string UsuarioNoExiste = "UsuarioNoExiste";
        private const string ClaveCorrecta = "ClaveCorrecta";
        private const string ClaveIncorrecta = "ClaveIncorrecta";
        private const string NroDocumentoCorrecto = "NroDocumentoCorrecto";
        private const string NroDocumentoIncorrecto = "NroDocumentoIncorrecto";

        private static readonly List<UsuarioV2> Usuarios = new List<UsuarioV2>
        {
            new UsuarioV2
            {
                UserId = 0,
                Username = UsuarioTest0,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                UserStatusId = 0
            },
            new UsuarioV2
            {
                UserId = 1,
                Username = UsuarioTest1,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10)
            },
            new UsuarioV2
            {
                UserId = 2,
                Username = UsuarioTest2,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Blocked
            },
            new UsuarioV2
            {
                UserId = 3,
                Username = UsuarioTest3,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Inactive
            },
            new UsuarioV2
            {
                UserId = 4,
                Username = UsuarioTest4,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Active,
                LastPasswordChange = DateTime.Now.AddDays(-181)
            }
        };

        private static readonly List<Configuracion> Configuraciones = new List<Configuracion>
        {
            new Configuracion
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.CantidadDeIntentosDeLoginKey,
                Value = "3"
            }
        };

        private static readonly List<ConfiguracionV2> ConfiguracionesV2 = new List<ConfiguracionV2>
        {
            new ConfiguracionV2
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "180"
            },
            new ConfiguracionV2
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "Error"
            },
            new ConfiguracionV2
            {
                ConfigurationId = 2,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.CantidadDeIntentosDeLoginKey,
                Value = "5"
            }
        };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "HBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { UsuarioTest1, ClaveCorrecta, NroDocumentoCorrecto, true, StatusCodes.Status202Accepted },
                new object[]
                    { UsuarioTest2, ClaveCorrecta, NroDocumentoCorrecto, false, StatusCodes.Status401Unauthorized },
                new object[]
                    { UsuarioTest3, ClaveCorrecta, NroDocumentoCorrecto, false, StatusCodes.Status401Unauthorized },
                new object[] { UsuarioTest4, ClaveCorrecta, NroDocumentoCorrecto, true, StatusCodes.Status202Accepted },
                new object[]
                    { UsuarioTest1, ClaveCorrecta, NroDocumentoIncorrecto, false, StatusCodes.Status401Unauthorized },
                new object[]
                    { UsuarioTest1, ClaveIncorrecta, NroDocumentoCorrecto, false, StatusCodes.Status401Unauthorized },
                new object[]
                    { UsuarioTest1, ClaveIncorrecta, NroDocumentoIncorrecto, false, StatusCodes.Status401Unauthorized },
                new object[]
                    { UsuarioNoExiste, ClaveCorrecta, NroDocumentoCorrecto, false, StatusCodes.Status401Unauthorized },
                new object[]
                {
                    UsuarioNoExiste, ClaveCorrecta, NroDocumentoIncorrecto, false, StatusCodes.Status401Unauthorized
                },
                new object[]
                {
                    UsuarioNoExiste, ClaveIncorrecta, NroDocumentoCorrecto, false, StatusCodes.Status401Unauthorized
                },
                new object[]
                {
                    UsuarioNoExiste, ClaveIncorrecta, NroDocumentoIncorrecto, false, StatusCodes.Status401Unauthorized
                },
            };

        public static IEnumerable<object[]> DatosClaveVencida =>
            new List<object[]>
            {
                new object[] { null, false },
                new object[] { null, false, ConfiguracionesV2[0] },
                new object[] { null, false, ConfiguracionesV2[1] },
                new object[] { 0, false },
                new object[] { 0, false, ConfiguracionesV2[0] },
                new object[] { 0, false, ConfiguracionesV2[1] },
                new object[] { -60, false },
                new object[] { -60, false, ConfiguracionesV2[0] },
                new object[] { -60, true, ConfiguracionesV2[1] },
                new object[] { -180, false },
                new object[] { -180, false, ConfiguracionesV2[0] },
                new object[] { -180, true, ConfiguracionesV2[1] },
                new object[] { -181, true },
                new object[] { -181, true, ConfiguracionesV2[0] },
                new object[] { -181, true, ConfiguracionesV2[1] },
                new object[] { -365, true },
                new object[] { -365, true, ConfiguracionesV2[0] },
                new object[] { -365, true, ConfiguracionesV2[1] }
            };

        #endregion
    }
}
