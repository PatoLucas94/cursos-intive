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
using Spv.Usuarios.Common.Dtos.NSBTClient;
using Spv.Usuarios.Common.Dtos.PersonaService.Output;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
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
    public class AutenticarConClaveNumericaAsyncTest
    {
        [Theory]
        [MemberData(nameof(Datos))]
        public async Task AutenticarConClaveNumericaNuevoModelo(
            string usuario,
            int idPais,
            string clave,
            string nroDocumento,
            int idTipoDocumento,
            bool isOk,
            int statusCode,
            string estadoPassword = null,
            DateTime? passwordExpirationDate = null)
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var usuarioV2Mock = Usuarios.FirstOrDefault(x =>
                x.Username == usuario
                && x.DocumentTypeId == idTipoDocumento
                && x.DocumentCountryId == idPais
                && x.DocumentNumber == nroDocumento);

            usuarioV2Repository.Setup(m =>
                    m.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(usuarioV2Mock);

            var datosRequestCorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentTypeId = idTipoDocumento,
                DocumentCountryId = idPais,
                Password = clave,
                DocumentNumber = nroDocumento
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);

            if (isOk)
            {
                var res = resultado.Match(
                    a => a.Payload,
                    a => new AutenticacionClaveNumericaModelOutput(),
                    a => new AutenticacionClaveNumericaModelOutput());

                res.IdPersona.Should().Be(PersonId);
                res.EstadoPassword.Should().Be(estadoPassword);
                if (passwordExpirationDate != null)
                {
                    res.FechaExpiracionPassword?.Date.Should().Be(passwordExpirationDate.Value.Date);
                }
            }
        }

        [Theory]
        [MemberData(nameof(DatosNsbt))]
        public async Task AutenticarConClaveNumericaNsbt(
            PinFromNsbt response,
            bool isOk,
            int statusCode,
            string message = null,
            bool toIncrementAttemptsFail = false)
        {
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();

            var personasRepository = new Mock<IPersonasRepository>();

            var nsbtRepository = new Mock<INsbtRepository>();

            nsbtRepository.Setup(m =>
                    m.GetPinAsync(
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(response);

            if (toIncrementAttemptsFail)
            {
                nsbtRepository.Setup(m =>
                        m.IncrementLoginAttemptsAsync(
                            It.IsAny<string>(),
                            It.IsAny<int>(),
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<int>(),
                            It.IsAny<string>()))
                    .ThrowsAsync(new Exception(MessageConstants.ClaveNumericaErrorWebService));
            }

            var datosRequestCorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentTypeId = 4,
                DocumentCountryId = 80,
                Password = "Info1212",
                DocumentNumber = "12345678"
            };

            personasRepository.Setup(m =>
                    m.ObtenerPersona(
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<int>(), null, null))
                .ReturnsAsync(new PersonaModelResponse { id = 1 });

            var sut = CrearUsuarioService(usuarioV2Repository.Object, nsbtRepository.Object, personasRepository.Object);

            // Act
            if (toIncrementAttemptsFail)
            {
                var resultado = await Assert.ThrowsAsync<Exception>(
                    () => sut.AutenticarConClaveNumericaAsync(Headers.ToRequestBody(
                        datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels())));

                // Assert
                resultado.Message.Should().Be(message);
            }
            else
            {
                var resultado = await sut.AutenticarConClaveNumericaAsync(
                    Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

                // Assert
                resultado.IsOk.Should().Be(isOk);
                resultado.StatusCode.Should().Be(statusCode);

                if (isOk)
                {
                    var res = resultado.Match(
                        a => a.Payload,
                        a => new AutenticacionClaveNumericaModelOutput(),
                        a => new AutenticacionClaveNumericaModelOutput());

                    res.IdPersona.Should().Be(1);
                    res.EstadoPassword.Should().Be(message);
                    res.FechaExpiracionPassword.Should().Be(response.ExpirationDate);
                }
            }
        }

        [Fact]
        public async Task UsuarioIncorrecto()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var nsbtRepository = new Mock<INsbtRepository>();

            nsbtRepository.Setup(m =>
                    m.GetPinAsync(
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .Returns(Task.FromResult<PinFromNsbt>(null));

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(new AutenticacionClaveNumericaModelInput(),
                    AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var message = resultado.Match(
                a => a.Payload.ToString(),
                a => a.Message,
                a => a.Exception.Message);

            message.Should().Be(ErrorCode.UsuarioIncorrecto.ErrorDescription);
        }

        [Fact]
        public async Task UsuarioBloqueado()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var usuarioV2Mock = Usuarios.FirstOrDefault(x => x.Username == UsuarioTest2);

            usuarioV2Repository.Setup(m =>
                    m.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(usuarioV2Mock);

            var datosRequestCorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                Password = ClaveCorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarConClaveNumericaAsync(
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
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var usuarioV2Mock = Usuarios.FirstOrDefault(x =>
                x.Username == UsuarioTest3 && x.DocumentNumber == NroDocumentoCorrecto);

            usuarioV2Repository.Setup(m =>
                    m.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(usuarioV2Mock);

            var datosRequestCorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                Password = ClaveCorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var message = resultado.Match(
                a => a.Payload.ToString(),
                a => a.Message,
                a => a.Exception.Message);

            message.Should().Be(ErrorCode.UsuarioInactivo.ErrorDescription);
        }

        [Fact]
        public async Task UsuarioConClaveVencida()
        {
            // Arrange
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var usuarioV2Mock = Usuarios.FirstOrDefault(x =>
                x.Username == UsuarioTest4 && x.DocumentNumber == NroDocumentoCorrecto);

            usuarioV2Repository.Setup(m =>
                    m.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(usuarioV2Mock);

            var datosRequestCorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                Password = ClaveCorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarConClaveNumericaAsync(
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
            var usuarioV2Mock = Usuarios.FirstOrDefault(x =>
                x.Username == UsuarioTest0 && x.DocumentNumber == NroDocumentoCorrecto);

            usuarioV2Repository.Setup(m =>
                    m.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(usuarioV2Mock);

            var datosRequestCorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                Password = ClaveCorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var message = resultado.Match(a => a.Payload.ToString(),
                a => a.Message,
                a => a.Exception.Message);

            message.Should().Be(ErrorCode.EstadoDeUsuarioNoControlado.ErrorDescription);
        }

        [Fact]
        public async Task EnAutenticarConClaveNumericaAsyncCuandoGetHashThrowsUnaException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var auditoriaLogV2Repository = new Mock<IAuditoriaLogV2Repository>();

            var encryptionMock = new Mock<IEncryption>();
            encryptionMock.Setup(m => m.GetHash(
                    It.IsAny<string>()))
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
            var usuarioMock = Usuarios.FirstOrDefault(x => x.Username == DatosRequestCorrectos.UserName);
            usuarioV2Repository.Setup(m =>
                    m.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(usuarioMock);

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
                sut.AutenticarConClaveNumericaAsync(
                    Headers.ToRequestBody(new AutenticacionClaveNumericaModelInput { DocumentNumber = "123" },
                        AllowedChannelsBuilder.CrearAllowedChannels())));

            // Assert
            resultado.Message.Should().Be(MessageConstants.NoSeEncontroSecretKey);
        }

        [Fact]
        public async Task LoginNoExitosoClaveIncorrectaSinBloqueoDeUsuario()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioV2Repository>();
            var usuarioV2Mock = new UsuarioV2
            {
                UserId = 1,
                Username = UsuarioTest1,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10),
                DocumentTypeId = IdTipoDocumentoCorrecto,
                DocumentCountryId = IdPaisCorrecto
            };

            usuarioRepository.Setup(m =>
                    m.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(usuarioV2Mock);

            var datosRequestIncorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                Password = ClaveIncorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioRepository.Object);

            // Act
            var resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(1);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Act
            resultado = await sut.AutenticarConClaveNumericaAsync(
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
                LastPasswordChange = DateTime.Now.AddDays(-10),
                DocumentTypeId = IdTipoDocumentoCorrecto,
                DocumentCountryId = IdPaisCorrecto
            };

            usuarioV2Repository.Setup(m =>
                    m.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(usuarioV2Mock);

            var datosRequestIncorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                Password = ClaveIncorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(1);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Arrange
            var datosRequestCorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                Password = ClaveCorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            // Act
            resultado = await sut.AutenticarConClaveNumericaAsync(
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
                LastPasswordChange = DateTime.Now.AddDays(-10),
                DocumentTypeId = IdTipoDocumentoCorrecto,
                DocumentCountryId = IdPaisCorrecto
            };

            usuarioV2Repository.Setup(m =>
                    m.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(usuarioV2Mock);

            var datosRequestIncorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                Password = ClaveIncorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(3);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Blocked);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Act
            resultado = await sut.AutenticarConClaveNumericaAsync(
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
                LastPasswordChange = DateTime.Now.AddDays(-10),
                DocumentTypeId = IdTipoDocumentoCorrecto,
                DocumentCountryId = IdPaisCorrecto
            };

            usuarioV2Repository.Setup(m =>
                    m.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(usuarioV2Mock);

            var datosRequestIncorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                Password = ClaveIncorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(0);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Inactive);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Act
            resultado = await sut.AutenticarConClaveNumericaAsync(
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
                LastPasswordChange = DateTime.Now.AddDays(-10),
                DocumentTypeId = IdTipoDocumentoCorrecto,
                DocumentCountryId = IdPaisCorrecto
            };

            usuarioV2Repository.Setup(m =>
                    m.ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(usuarioV2Mock);

            var datosRequestIncorrectos = new AutenticacionClaveNumericaModelInput
            {
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                Password = ClaveIncorrecta,
                DocumentNumber = NroDocumentoCorrecto
            };

            var sut = CrearUsuarioService(usuarioV2Repository.Object);

            // Act
            var resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(1);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Act
            resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(2);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Act
            resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(3);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Act
            resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(4);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Active);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Then Act
            resultado = await sut.AutenticarConClaveNumericaAsync(
                Headers.ToRequestBody(datosRequestIncorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            usuarioV2Mock.LoginAttempts.Should().Be(5);
            usuarioV2Mock.UserStatusId.Should().Be((byte)UserStatus.Blocked);
            resultado.IsOk.Should().BeFalse();
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        private static UsuariosService CrearUsuarioService(
            IUsuarioV2Repository usuarioV2Repository,
            INsbtRepository nsbtRepository = null,
            IPersonasRepository personasRepository = null,
            Configuracion configuracionMock = null)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var auditoriaLogV2Repository = new Mock<IAuditoriaLogV2Repository>();

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

            var datosUsuarioRepository = new Mock<IDatosUsuarioRepository>();
            var tDesRepository = new Mock<ITDesEncryption>();
            var historialClaveUsuariosV2Repository = new Mock<IHistorialClaveUsuariosV2Repository>();
            var historialUsuarioUsuariosV2Repository = new Mock<IHistorialUsuarioUsuariosV2Repository>();
            var historialClaveUsuariosRepository = new Mock<IHistorialClaveUsuariosRepository>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();
            var btaRepository = new Mock<IBtaRepository>();

            tDesRepository.Setup(m => m.Encrypt("Info1212")).Returns("2d13e55d0374fe25b5fc9b08546b3c68");

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
                personasRepository ?? new Mock<IPersonasRepository>().Object,
                datosUsuarioRepository.Object,
                nsbtRepository ?? new Mock<INsbtRepository>().Object,
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

        private const string ClaveCorrecta = "ClaveCorrecta";
        private const string ClaveIncorrecta = "ClaveIncorrecta";

        private const string NroDocumentoCorrecto = "NroDocumentoCorrecto";

        private const int IdPaisCorrecto = 80;

        private const int IdTipoDocumentoCorrecto = 1;

        private const long PersonId = 888;

        private static readonly AutenticacionModelInput DatosRequestCorrectos = new AutenticacionModelInput
        {
            UserName = UsuarioTest1,
            Password = ClaveCorrecta,
            DocumentNumber = NroDocumentoCorrecto,
        };

        private static readonly List<UsuarioV2> Usuarios = new List<UsuarioV2>
        {
            new UsuarioV2
            {
                UserId = 0,
                Username = UsuarioTest0,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                UserStatusId = 0
            },
            new UsuarioV2
            {
                UserId = 1,
                PersonId = PersonId,
                Username = UsuarioTest1,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
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
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Blocked
            },
            new UsuarioV2
            {
                UserId = 3,
                Username = UsuarioTest3,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
                UserStatusId = (byte)UserStatus.Inactive
            },
            new UsuarioV2
            {
                UserId = 4,
                PersonId = PersonId,
                Username = UsuarioTest4,
                Password = ClaveEncriptadaValida,
                DocumentNumber = NroDocumentoCorrecto,
                DocumentCountryId = IdPaisCorrecto,
                DocumentTypeId = IdTipoDocumentoCorrecto,
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
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "180"
            },
            new Configuracion
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
                // string usuario, string idPais, string clave, string nroDocumento, int idTipoDocumento, bool isOk, int statusCode, string estadoPassword = null, DateTime passwordExpirationDate = null
                new object[]
                {
                    UsuarioTest1,
                    IdPaisCorrecto,
                    ClaveCorrecta,
                    NroDocumentoCorrecto,
                    IdTipoDocumentoCorrecto,
                    true,
                    StatusCodes.Status202Accepted,
                    ErrorConstants.CodigoPasswordNoExpirado,
                    DateTime.Now.AddDays(170)
                },
                new object[]
                {
                    UsuarioTest2,
                    IdPaisCorrecto,
                    ClaveCorrecta,
                    NroDocumentoCorrecto,
                    IdTipoDocumentoCorrecto,
                    false,
                    StatusCodes.Status401Unauthorized
                }, // inactivo
                new object[]
                {
                    UsuarioTest3,
                    IdPaisCorrecto,
                    ClaveCorrecta,
                    NroDocumentoCorrecto,
                    IdTipoDocumentoCorrecto,
                    false,
                    StatusCodes.Status401Unauthorized
                }, // bloqueado
                new object[]
                {
                    UsuarioTest4,
                    IdPaisCorrecto,
                    ClaveCorrecta,
                    NroDocumentoCorrecto,
                    IdTipoDocumentoCorrecto,
                    true,
                    StatusCodes.Status202Accepted,
                    ErrorConstants.CodigoPasswordExpirado,
                    DateTime.Now.AddDays(-1)
                }, // clave vencida


                new object[]
                {
                    UsuarioTest1,
                    IdPaisCorrecto,
                    ClaveIncorrecta,
                    NroDocumentoCorrecto,
                    IdTipoDocumentoCorrecto,
                    false,
                    StatusCodes.Status401Unauthorized
                },
            };

        public static IEnumerable<object[]> DatosNsbt =>
            new List<object[]>
            {
                new object[]
                {
                    new PinFromNsbt
                        { Pin = "2d13e55d0374fe25b5fc9b08546b3c68", ExpirationDate = DateTime.MaxValue.AddDays(-180) },
                    true,
                    StatusCodes.Status202Accepted,
                    ErrorConstants.CodigoPasswordNoExpirado
                }, // correcto
                new object[]
                {
                    new PinFromNsbt { Pin = "2d13e55d0374fe25b5fc9b08546b3c68", ExpirationDate = DateTime.MinValue },
                    true,
                    StatusCodes.Status202Accepted,
                    ErrorConstants.CodigoPasswordExpiradoBta
                }, // correcto pin vencido
                new object[]
                {
                    new PinFromNsbt
                    {
                        Pin = "2d13e55d0374fe25b5fc9b08546b3c68", ExpirationDate = DateTime.MaxValue.AddDays(-180),
                        Attempt = 3
                    },
                    false,
                    StatusCodes.Status401Unauthorized,
                    ErrorConstants.CodigoUsuarioBloqueado
                },
                new object[]
                {
                    new PinFromNsbt { Pin = "Info1213" },
                    false,
                    StatusCodes.Status401Unauthorized
                },
                new object[]
                {
                    new PinFromNsbt { Pin = null },
                    false,
                    StatusCodes.Status401Unauthorized
                },
                new object[]
                {
                    new PinFromNsbt
                        { Pin = "2d13e55d0374fe25b5fc9b08546b3c68", ExpirationDate = DateTime.MaxValue, Attempt = 3 },
                    false,
                    StatusCodes.Status401Unauthorized
                }, // bloqueado por intentos
                new object[]
                {
                    new PinFromNsbt
                        { Pin = "2d13e55d0374fe25b5fc9b08546b3c68", ExpirationDate = DateTime.MaxValue, Attempt = 0 },
                    false,
                    StatusCodes.Status500InternalServerError,
                    MessageConstants.ClaveNumericaErrorWebService,
                    true
                } // falla incrementar intentos bta
            };

        #endregion
    }
}
