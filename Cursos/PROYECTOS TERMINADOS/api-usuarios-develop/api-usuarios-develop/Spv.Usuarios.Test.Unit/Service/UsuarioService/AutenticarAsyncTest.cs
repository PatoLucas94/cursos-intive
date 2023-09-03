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
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Common.Errors;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Domain.Services;
using Spv.Usuarios.Service;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Spv.Usuarios.Test.Unit.Service.Helpers;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.UsuarioService
{
    public class AutenticarAsyncTest
    {
        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Credenciales(string usuario, string clave, bool isOk, int statusCode)
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserName == usuario);
            usuarioRepository.Setup(m => m.ObtenerUsuarioAsync(It.IsAny<string>())).ReturnsAsync(usuarioMock);

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = usuario,
                Password = clave
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Fact]
        public async Task UsuarioIncorrecto()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(new AutenticacionModelInput(), AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var message = resultado.Match(a =>
                    a.Payload.ToString(),
                a => a.Message,
                a => a.Exception.Message);

            message.Should().Be(ErrorCode.UsuarioIncorrecto.ErrorDescription);
        }

        [Fact]
        public async Task UsuarioBloqueado()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserName == UsuarioTest2);
            usuarioRepository.Setup(m =>
                    m.ObtenerUsuarioAsync(It.IsAny<string>()))
                .ReturnsAsync(usuarioMock);

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest2,
                Password = ClaveCorrecta
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var message = resultado.Match(
                a => a.Payload.ToString(),
                a => a.Message,
                a => a.Exception.Message);

            message.Should().Be(ErrorCode.UsuarioBloqueado.ErrorDescription);
        }

        [Fact]
        public async Task UsuarioInactivo()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserName == UsuarioTest3);
            usuarioRepository.Setup(m => m.ObtenerUsuarioAsync(It.IsAny<string>())).ReturnsAsync(usuarioMock);

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest3,
                Password = ClaveCorrecta
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var message = resultado.Match(a =>
                    a.Payload.ToString(),
                a => a.Message,
                a => a.Exception.Message);

            message.Should().Be(ErrorCode.UsuarioInactivo.ErrorDescription);
        }

        [Fact]
        public async Task UsuarioConClaveVencida()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserName == UsuarioTest4);
            usuarioRepository.Setup(m =>
                m.ObtenerUsuarioAsync(It.IsAny<string>())).ReturnsAsync(usuarioMock);

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveCorrecta
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeTrue();
            resultado.StatusCode.Should().Be(StatusCodes.Status202Accepted);
        }

        [Fact]
        public async Task UsuarioSuspendido()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserName == UsuarioTest5);
            usuarioRepository.Setup(m =>
                m.ObtenerUsuarioAsync(It.IsAny<string>())).ReturnsAsync(usuarioMock);

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveCorrecta
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var respuesta = ((IClientErrorResponse<AutenticacionModelOutput>)resultado);
            respuesta.Message.Should().Be(MessageConstants.UsuarioSuspendido);
            respuesta.InternalCode.Should().Be(ErrorConstants.CodigoUsuarioSuspendido);
            respuesta.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task EstadoDeUsuarioNoControlado()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserName == UsuarioTest0);
            usuarioRepository.Setup(m => m.ObtenerUsuarioAsync(It.IsAny<string>())).ReturnsAsync(usuarioMock);

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest0,
                Password = ClaveCorrecta
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task EnAutenticarAsyncCuandoGetHashThrowsUnaException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var auditoriaLogV2Repository = new Mock<IAuditoriaLogV2Repository>();

            var configuracionMock = Configuraciones.FirstOrDefault();

            if (!int.TryParse(configuracionMock?.Value, out var cantidadDiasParaForzarCambioDeClave))
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

            var encryptionMock = new Mock<IEncryption>();
            encryptionMock.Setup(m =>
                m.GetHash(It.IsAny<string>())).Throws(new Exception(MessageConstants.NoSeEncontroSecretKey));

            var helperDbServerMock = new Mock<IHelperDbServer>();
            helperDbServerMock.Setup(m => m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServer { Now = DateTime.Now });

            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();
            helperDbServerMockV2.Setup(m => m.ObtenerFechaAsync())
                .ReturnsAsync(new FechaDbServerV2 { Now = DateTime.Now });

            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserName == DatosRequestCorrectos.UserName);
            usuarioRepository.Setup(m => m.ObtenerUsuarioAsync(It.IsAny<string>())).ReturnsAsync(usuarioMock);

            var auditoriaMock = new Mock<IAuditoriaRepository>();

            var personasRepository = new Mock<IPersonasRepository>();

            var datosUsuarioRepository = new Mock<IDatosUsuarioRepository>();
            var nsbtRepository = new Mock<INsbtRepository>();
            var tDesRepository = new Mock<ITDesEncryption>();
            var historialClaveUsuariosRepository = new Mock<IHistorialClaveUsuariosRepository>();
            var historialClaveUsuariosV2Repository = new Mock<IHistorialClaveUsuariosV2Repository>();
            var historialUsuarioUsuariosV2Repository = new Mock<IHistorialUsuarioUsuariosV2Repository>();
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
                    Headers.ToRequestBody(new AutenticacionModelInput(),
                        AllowedChannelsBuilder.CrearAllowedChannels())));

            // Assert
            resultado.Message.Should().Be(MessageConstants.NoSeEncontroSecretKey);
        }

        [Theory]
        [MemberData(nameof(DatosClaveVencida))]
        public async Task ClaveVencida(
            double? cantidadDiasLastPasswordChange,
            bool esperado,
            ConfiguracionV2 configuracion = null)
        {
            // Arrange
            var now = DateTime.Now;
            var usuarioRepository = new Mock<IUsuarioRepository>();

            var sut = CrearUsuarioService(usuarioRepository.Object, configuracion);

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
        public async Task LoginNoExitosoClaveIncorrectaSinBloqueoDeUsuario()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = new Usuario
            {
                UserId = 1,
                UserName = UsuarioTest1,
                Password = ClaveEncriptadaValida,
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10),
                UserData = new DatosUsuario
                {
                    PersonId = "1"
                }
            };

            usuarioRepository.Setup(m =>
                    m.ObtenerUsuarioAsync(It.IsAny<string>()))
                .ReturnsAsync(usuarioMock);

            var datosRequestIncorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveIncorrecta
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioMock.LoginAttempts.Should().Be(1);
            usuarioMock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioMock.LoginAttempts.Should().Be(2);
            usuarioMock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task LoginNoExitosoClaveIncorrectaSinBloqueoDeUsuarioConSegundoIntentoCorrecto()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = new Usuario
            {
                UserId = 1,
                UserName = UsuarioTest1,
                Password = ClaveEncriptadaValida,
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10),
                UserData = new DatosUsuario
                {
                    PersonId = "1"
                }
            };

            usuarioRepository.Setup(m =>
                    m.ObtenerUsuarioAsync(It.IsAny<string>()))
                .ReturnsAsync(usuarioMock);

            var datosRequestIncorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveIncorrecta
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioMock.LoginAttempts.Should().Be(1);
            usuarioMock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Arrange
            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveCorrecta
            };

            // Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioMock.LoginAttempts.Should().Be(0);
            usuarioMock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeTrue();
            resultado.StatusCode.Should().Be(StatusCodes.Status202Accepted);
        }

        [Fact]
        public async Task LoginNoExitosoClaveIncorrectaSinBloqueoDeUsuarioConUsuarioYaBloqueado()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = new Usuario
            {
                UserId = 2,
                UserName = UsuarioTest2,
                Password = ClaveEncriptadaValida,
                UserStatusId = (byte)UserStatus.Blocked,
                LoginAttempts = 3,
                LastPasswordChange = DateTime.Now.AddDays(-10),
                UserData = new DatosUsuario
                {
                    UserId = 2,
                    PersonId = "2"
                }
            };

            usuarioRepository.Setup(m =>
                    m.ObtenerUsuarioAsync(It.IsAny<string>()))
                .ReturnsAsync(usuarioMock);

            var datosRequestIncorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveIncorrecta
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioMock.LoginAttempts.Should().Be(3);
            usuarioMock.UserStatusId.Should().Be((byte)UserStatus.Blocked);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioMock.LoginAttempts.Should().Be(3);
            usuarioMock.UserStatusId.Should().Be((byte)UserStatus.Blocked);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task LoginNoExitosoClaveIncorrectaSinBloqueoDeUsuarioConUsuarioInactivo()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = new Usuario
            {
                UserId = 3,
                UserName = UsuarioTest3,
                Password = ClaveEncriptadaValida,
                UserStatusId = (byte)UserStatus.Inactive,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10),
                UserData = new DatosUsuario
                {
                    UserId = 3,
                    PersonId = "3"
                }
            };

            usuarioRepository.Setup(m =>
                    m.ObtenerUsuarioAsync(It.IsAny<string>()))
                .ReturnsAsync(usuarioMock);

            var datosRequestIncorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveIncorrecta
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioMock.LoginAttempts.Should().Be(0);
            usuarioMock.UserStatusId.Should().Be((byte)UserStatus.Inactive);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioMock.LoginAttempts.Should().Be(0);
            usuarioMock.UserStatusId.Should().Be((byte)UserStatus.Inactive);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task LoginNoExitosoClaveIncorrectaConBloqueoDeUsuario()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioMock = new Usuario
            {
                UserId = 1,
                UserName = UsuarioTest1,
                Password = ClaveEncriptadaValida,
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10),
                UserData = new DatosUsuario
                {
                    PersonId = "1"
                }
            };

            usuarioRepository.Setup(m =>
                    m.ObtenerUsuarioAsync(It.IsAny<string>()))
                .ReturnsAsync(usuarioMock);

            var datosRequestIncorrectos = new AutenticacionModelInput
            {
                UserName = UsuarioTest1,
                Password = ClaveIncorrecta
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioMock.LoginAttempts.Should().Be(1);
            usuarioMock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioMock.LoginAttempts.Should().Be(2);
            usuarioMock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Act
            resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioMock.LoginAttempts.Should().Be(3);
            usuarioMock.UserStatusId.Should().Be((byte)UserStatus.Blocked);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        private static UsuariosService CrearUsuarioService(IUsuarioRepository usuarioRepository,
            ConfiguracionV2 configuracionMock = null)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var auditoriaLogV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();

            configuracionMock ??= Configuraciones.FirstOrDefault();

            if (!int.TryParse(configuracionMock?.Value, out var cantidadDiasParaForzarCambioDeClave))
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

            configuracionesService.Setup(m =>
                    m.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync())
                .ReturnsAsync(3);

            configuracionesV2Service.Setup(m =>
                    m.ObtenerConfiguracionCantidadDeIntentosDeLoginAsync())
                .ReturnsAsync(5);

            var encryptionMock = new Mock<IEncryption>();
            encryptionMock.Setup(m => m.GetHash(ClaveCorrecta)).Returns(ClaveEncriptadaValida);
            encryptionMock.Setup(m => m.GetHash(ClaveIncorrecta)).Returns(ClaveEncriptadaInvalida);

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

        #region Datos

        private const string ClaveEncriptadaValida = "PasswordEncriptado";
        private const string ClaveEncriptadaInvalida = "PasswordEncriptadoInvalido";
        private const string UsuarioTest0 = "Usuario0";
        private const string UsuarioTest1 = "Usuario1";
        private const string UsuarioTest2 = "Usuario2";
        private const string UsuarioTest3 = "Usuario3";
        private const string UsuarioTest4 = "Usuario4";
        private const string UsuarioTest5 = "Usuario5";
        private const string UsuarioNoExiste = "UsuarioNoExiste";
        private const string ClaveCorrecta = "ClaveCorrecta";
        private const string ClaveIncorrecta = "ClaveIncorrecta";

        private static readonly AutenticacionModelInput DatosRequestCorrectos = new AutenticacionModelInput
        {
            UserName = UsuarioTest1,
            Password = ClaveCorrecta
        };

        private static readonly List<Usuario> Usuarios = new List<Usuario>
        {
            new Usuario
            {
                UserId = 0,
                UserName = UsuarioTest0,
                Password = ClaveEncriptadaValida,
                UserStatusId = 0,
                UserData = new DatosUsuario
                {
                    PersonId = "0"
                }
            },
            new Usuario
            {
                UserId = 1,
                UserName = UsuarioTest1,
                Password = ClaveEncriptadaValida,
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10),
                UserData = new DatosUsuario
                {
                    PersonId = "1"
                }
            },
            new Usuario
            {
                UserId = 2,
                UserName = UsuarioTest2,
                Password = ClaveEncriptadaValida,
                UserStatusId = (byte)UserStatus.Blocked,
                UserData = new DatosUsuario
                {
                    PersonId = "2"
                }
            },
            new Usuario
            {
                UserId = 3,
                UserName = UsuarioTest3,
                Password = ClaveEncriptadaValida,
                UserStatusId = (byte)UserStatus.Inactive,
                UserData = new DatosUsuario
                {
                    PersonId = "3"
                }
            },
            new Usuario
            {
                UserId = 4,
                UserName = UsuarioTest4,
                Password = ClaveEncriptadaValida,
                UserStatusId = (byte)UserStatus.Active,
                LastPasswordChange = DateTime.Now.AddDays(-181),
                UserData = new DatosUsuario
                {
                    PersonId = "4"
                }
            },
            new Usuario
            {
                UserId = 5,
                UserName = UsuarioTest5,
                Password = ClaveEncriptadaValida,
                UserStatusId = (byte)UserStatus.Suspended,
                LastPasswordChange = DateTime.Now.AddDays(-181),
                UserData = new DatosUsuario
                {
                    PersonId = "5"
                }
            }
        };

        private static readonly List<ConfiguracionV2> Configuraciones = new List<ConfiguracionV2>
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
                new object[] { UsuarioTest1, ClaveCorrecta, true, StatusCodes.Status202Accepted },
                new object[] { UsuarioTest2, ClaveCorrecta, false, StatusCodes.Status401Unauthorized },
                new object[] { UsuarioTest3, ClaveCorrecta, false, StatusCodes.Status401Unauthorized },
                new object[] { UsuarioTest4, ClaveCorrecta, true, StatusCodes.Status202Accepted },
                new object[] { UsuarioTest1, ClaveIncorrecta, false, StatusCodes.Status401Unauthorized },
                new object[] { UsuarioNoExiste, ClaveCorrecta, false, StatusCodes.Status401Unauthorized },
                new object[] { UsuarioNoExiste, ClaveIncorrecta, false, StatusCodes.Status401Unauthorized }
            };

        public static IEnumerable<object[]> DatosClaveVencida =>
            new List<object[]>
            {
                new object[] { null, false },
                new object[] { null, false, Configuraciones[0] },
                new object[] { null, false, Configuraciones[1] },
                new object[] { 0, false },
                new object[] { 0, false, Configuraciones[0] },
                new object[] { 0, false, Configuraciones[1] },
                new object[] { -60, false },
                new object[] { -60, false, Configuraciones[0] },
                new object[] { -60, true, Configuraciones[1] },
                new object[] { -180, false },
                new object[] { -180, false, Configuraciones[0] },
                new object[] { -180, true, Configuraciones[1] },
                new object[] { -181, true },
                new object[] { -181, true, Configuraciones[0] },
                new object[] { -181, true, Configuraciones[1] },
                new object[] { -365, true },
                new object[] { -365, true, Configuraciones[0] },
                new object[] { -365, true, Configuraciones[1] }
            };

        #endregion
    }
}
