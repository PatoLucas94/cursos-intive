using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Dtos.PersonaService.Output;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
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
    public class ObtenerUsuarioAsyncTest
    {
        public static IEnumerable<object[]> Datos => new List<object[]>
        {
            new object[] { "username0", DocUsuarioTest1, true, StatusCodes.Status200OK },
            new object[] { "username1", DocUsuarioTest2, true, StatusCodes.Status200OK }
        };

        private const string DocUsuarioTest1 = "12345678";
        private const string DocUsuarioTest2 = "87654321";

        private const long PersonId0 = 0;
        private const long PersonId1 = 1;

        private static readonly Usuario UsuarioMock = new Usuario
        {
            UserId = 0,
            UserName = "username0",
            Password = "clave_encriptada0",
            UserStatusId = 3,
            UserData = new DatosUsuario
            {
                PersonId = PersonId0.ToString()
            },
            DocumentNumber = DocUsuarioTest1,
            DocumentCountryId = "80",
            DocumentTypeId = 4
        };

        private static readonly UsuarioV2 UsuarioV2Mock = new UsuarioV2
        {
            UserId = 1,
            Username = "username1",
            Password = "clave_encriptada1",
            UserStatusId = (byte)UserStatus.Active,
            LoginAttempts = 0,
            LastPasswordChange = DateTime.Now.AddDays(-10),
            DocumentNumber = DocUsuarioTest2,
            DocumentCountryId = 80,
            DocumentTypeId = 4,
            PersonId = PersonId1
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
            IUsuarioV2Repository usuarioV2Repository,
            IPersonasRepository personasRepository
        )
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var auditoriaLogV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var encryptionMock = new Mock<IEncryption>();
            var helperDbServerMock = new Mock<IHelperDbServer>();
            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();
            var auditoriaMock = new Mock<IAuditoriaRepository>();
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
                personasRepository,
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
        public async Task ObtenerUsuario(string username, string nroDocumento, bool isOk, int statusCode)
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var personasRepository = new Mock<IPersonasRepository>();

            usuarioRepository.Setup(m => m.ObtenerUsuarioAsync(username, nroDocumento)).ReturnsAsync(UsuarioMock);
            usuarioV2Repository.Setup(m => m.ObtenerUsuarioAsync(username, nroDocumento)).ReturnsAsync(UsuarioV2Mock);

            personasRepository.Setup(m =>
                    m.ObtenerInfoPersonaFisica(
                        It.IsAny<long>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()
                    )
                )
                .ReturnsAsync(new PersonaFisicaInfoModelResponse());

            var datosRequestCorrectos = new ObtenerUsuarioModelInput
            {
                Usuario = username,
                NumeroDocumento = nroDocumento
            };

            var sut = CrearUsuarioService(
                usuarioRepository.Object,
                usuarioV2Repository.Object,
                personasRepository.Object
            );

            // Act
            var resultado = await sut.ObtenerUsuarioAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels())
            );

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);

            if (statusCode == StatusCodes.Status200OK)
            {
                var res = resultado.Match(
                    a => a.Payload,
                    a => new PerfilModelOutput(),
                    a => new PerfilModelOutput()
                );

                if (res.Migrated)
                {
                    res.PersonId.Should().Be(UsuarioV2Mock.PersonId);
                    res.Migrated.Should().BeTrue();
                }
                else
                {
                    res.PersonId.ToString().Should().Be(UsuarioMock.UserData.PersonId);
                    res.Migrated.Should().BeFalse();
                }
            }
        }

        [Fact]
        public async Task ObtenerUsuario_UsuarioMigrado()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var personasRepository = new Mock<IPersonasRepository>();

            var datosRequestCorrectos = new ObtenerUsuarioModelInput
            {
                Usuario = "username0",
                NumeroDocumento = "87654321"
            };

            usuarioRepository.Setup(m => m.ObtenerUsuarioAsync(It.IsAny<string>(), DocUsuarioTest2))
                .ReturnsAsync(UsuarioMock);

            usuarioV2Repository.Setup(m => m.ObtenerUsuarioAsync(It.IsAny<string>(), DocUsuarioTest2))
                .ReturnsAsync(UsuarioV2Mock);

            personasRepository.Setup(m =>
                    m.ObtenerInfoPersonaFisica(
                        It.IsAny<long>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()
                    )
                )
                .ReturnsAsync(new PersonaFisicaInfoModelResponse());

            var sut = CrearUsuarioService(
                usuarioRepository.Object,
                usuarioV2Repository.Object,
                personasRepository.Object
            );

            // Act
            var resultado = await sut.ObtenerUsuarioAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels())
            );

            var res = resultado.Match(
                a => a.Payload,
                a => new PerfilModelOutput(),
                a => new PerfilModelOutput());

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);

            res.PersonId.Should().Be(UsuarioV2Mock.GetPersonId());
            res.Migrated.Should().Be(true);
        }

        [Fact]
        public async Task ObtenerUsuario_UsuarioNoMigrado()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var personasRepository = new Mock<IPersonasRepository>();

            var datosRequestCorrectos = new ObtenerUsuarioModelInput
            {
                Usuario = "username1",
                NumeroDocumento = "12345678"
            };

            usuarioRepository.Setup(m => m.ObtenerUsuarioAsync(It.IsAny<string>(), DocUsuarioTest1))
                .ReturnsAsync(UsuarioMock);

            personasRepository.Setup(m =>
                    m.ObtenerInfoPersonaFisica(
                        It.IsAny<long>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()
                    )
                )
                .ReturnsAsync(new PersonaFisicaInfoModelResponse());

            var sut = CrearUsuarioService(
                usuarioRepository.Object,
                usuarioV2Repository.Object,
                personasRepository.Object
            );

            // Act
            var resultado = await sut.ObtenerUsuarioAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels())
            );

            var res = resultado.Match(
                a => a.Payload,
                a => new PerfilModelOutput(),
                a => new PerfilModelOutput()
            );

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);

            res.PersonId.Should().Be(UsuarioMock.GetPersonId());
            res.Migrated.Should().Be(false);
        }

        [Fact]
        public async Task ObtenerUsuario_ThrowException()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var personasRepository = new Mock<IPersonasRepository>();

            var datosRequestCorrectos = new ObtenerUsuarioModelInput
            {
                Usuario = "username1",
                NumeroDocumento = "12345678"
            };

            usuarioRepository.Setup(m => m.ObtenerUsuarioAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(UsuarioMock);

            usuarioV2Repository.Setup(m => m.ObtenerUsuarioAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("Excepción no controlada"));

            var sut = CrearUsuarioService(
                usuarioRepository.Object,
                usuarioV2Repository.Object,
                personasRepository.Object
            );

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.ObtenerUsuarioAsync(
                    Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels())
                )
            );

            // Assert
            resultado.Message.Should().Be("Excepción no controlada");
        }
    }
}
