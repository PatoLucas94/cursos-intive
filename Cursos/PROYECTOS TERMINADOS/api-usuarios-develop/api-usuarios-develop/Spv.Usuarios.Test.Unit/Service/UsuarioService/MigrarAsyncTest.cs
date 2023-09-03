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
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Service;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Spv.Usuarios.Test.Unit.Service.Helpers;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.UsuarioService
{
    public class MigrarAsyncTest
    {
        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { IdPersonaTest1, true, StatusCodes.Status201Created },
                //   new object[] { IdPersonaTest2, false, StatusCodes.Status404NotFound },
                new object[] { IdPersonaTest3, false, StatusCodes.Status409Conflict }
            };

        private const long IdPersonaTest1 = 12345678;
        private const long IdPersonaTest2 = 25896314;
        private const long IdPersonaTest3 = 11111111;

        private static readonly List<ReglaValidacionV2> ListaReglasUsuarios = new List<ReglaValidacionV2>
        {
            new ReglaValidacionV2
            {
                ValidationRuleText = "Letras y números.",
                RegularExpression = "/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/",
                InputId = 1
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "Entre8y15Caracteres",
                RegularExpression = "/^[A-Za-z0-9]{8,15}$/i",
                InputId = 1
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "NoMasDe3CaracteresRepetidos",
                RegularExpression = @"/^(?!.*?([A-Za-z0-9])\1\1\1).+$/i",
                InputId = 1
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "NoMasDe3CaracteresConsecutivosAscODesc",
                RegularExpression =
                    "/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210|abcd|bcde|cdef|defg|efgh|fghi|ghij|hijk|ijkl|jklm|klmn|lmno|mnop|nopq|opqr|pqrs|qrst|rstu|stuv|tuvw|uvwx|vwxy|wxyz|zyxw|yxwv|xwvu|wvut|vuts|utsr|tsru|srqp|rqpo|qpon|ponm|onml|nmlk|mlkj|lkji|kjih|jihg|ihgf|hgfe|gfed|fedc|edcb|dcba|ABCD|BCDE|CDEF|DEFG|EFGH|FGHI|GHIJ|HIJK|IJKL|JKLM|KLMN|LMNO|MNOP|NOPQ|OPQR|PQRS|QRST|RSTU|STUV|TUVW|UVWX|VWXY|WXYZ|ZYXW|YXWV|XWVU|WVUT|VUTS|UTSR|TSRU|SRQP|RQPO|QPON|PONM|ONML|NMLK|MLKJ|LKJI|KJIH|JIHG|IHGF|HGFE|GFED|FEDC|EDCB|DCBA)).+$/i",
                InputId = 1
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "AlMenos1LetraMayuscula",
                RegularExpression = "/^(?=.*[A-Z])([A-Za-z0-9]+)$/",
                InputId = 1
            },
        };

        private static readonly List<ReglaValidacionV2> ListaReglasclave = new List<ReglaValidacionV2>
        {
            new ReglaValidacionV2
            {
                ValidationRuleText = "SoloNumeros",
                RegularExpression = "/^(?=.*[0-9])([0-9]+)$/",
                InputId = 2
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "Solo4Caracteres",
                RegularExpression = "/^[A-Za-z0-9]{4}$/i",
                InputId = 2
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "NoMasDe3NumerosRepetidos",
                RegularExpression = @"/^(?!.*?([0-9])\1\1\1).+$/",
                InputId = 2
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "NoMasDe3NumerosConsecutivosAscODesc",
                RegularExpression =
                    "/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210)).+$/",
                InputId = 2
            },
        };

        private static readonly List<Usuario> Usuarios = new List<Usuario>
        {
            new Usuario
            {
                UserId = 0,
                UserName = "username0",
                Password = "clave_encriptada0",
                UserStatusId = 3,
                UserData = new DatosUsuario
                {
                    PersonId = IdPersonaTest1.ToString()
                },
                DocumentCountryId = "80",
                DocumentTypeId = 4,
                DocumentNumber = "10123456",
                LastLogon = DateTime.Now.AddDays(-1),
                CreatedDate = DateTime.Now.AddDays(-10)
            },
            new Usuario
            {
                UserId = 3,
                UserName = "username3",
                Password = "clave_encriptada3",
                UserStatusId = 3,
                UserData = new DatosUsuario
                {
                    PersonId = IdPersonaTest3.ToString()
                },
                DocumentCountryId = "80",
                DocumentTypeId = 4,
                DocumentNumber = "10111000",
                LastLogon = DateTime.Now.AddDays(-1),
                CreatedDate = DateTime.Now.AddDays(-10)
            }
        };

        private static readonly List<UsuarioV2> UsuarioV2 = new List<UsuarioV2>
        {
            new UsuarioV2
            {
                UserId = 3,
                PersonId = IdPersonaTest3,
                Username = "username3",
                Password = "clave_encriptada3",
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10),
                DocumentNumber = "87654321"
            }
        };

        private static readonly AuditoriaLogV2 AuditoriaLogV2Mock = new AuditoriaLogV2
        {
            AuditLogId = 1,
            Channel = "OBI",
            DateTime = DateTime.MinValue,
            UserId = 1,
            EventTypeId = (int)EventTypes.Migration,
            EventResultId = (int)EventResults.Ok,
            ExtendedInfo = "extended_info"
        };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "OBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        private static UsuariosService CrearUsuarioService(
            IUsuarioRepository usuarioRepository,
            IUsuarioV2Repository usuarioV2Repository,
            IAuditoriaLogV2Repository auditoriaLogV2Repository,
            IReglaValidacionV2Repository reglaValidacionV2Repository)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var encryptionMock = new Mock<IEncryption>();

            var helperDbServerMock = new Mock<IHelperDbServer>();
            helperDbServerMock.Setup(m =>
                m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServer { Now = DateTime.MinValue });

            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();
            helperDbServerMockV2.Setup(m =>
                m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServerV2 { Now = DateTime.MinValue });

            var auditoriaMock = new Mock<IAuditoriaRepository>();

            var personasRepository = new Mock<IPersonasRepository>();

            var datosUsuarioRepository = new Mock<IDatosUsuarioRepository>();
            var nsbtRepository = new Mock<INsbtRepository>();
            var tDesRepository = new Mock<ITDesEncryption>();
            var historialClaveUsuariosV2Repository = new Mock<IHistorialClaveUsuariosV2Repository>();
            var historialUsuarioUsuariosV2Repository = new Mock<IHistorialUsuarioUsuariosV2Repository>();
            var historialClaveUsuariosRepository = new Mock<IHistorialClaveUsuariosRepository>();
            var btaRepository = new Mock<IBtaRepository>();

            return new UsuariosService(
                loggerMock.Object,
                usuarioRepository,
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
                reglaValidacionV2Repository,
                MapperProfile.GetAppProfile(),
                new Mock<IDistributedCache>().Object,
                btaRepository.Object
            );
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Migrar(long idPersona, bool isOk, int statusCode)
        {
            // Arrange
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserData.PersonId == idPersona.ToString());
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Mock = UsuarioV2.FirstOrDefault(x => x.PersonId == idPersona);
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();

            reglaValidacionV2Repository.Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync(
                    It.IsAny<int>(),
                    1))
                .ReturnsAsync(ListaReglasUsuarios);

            reglaValidacionV2Repository.Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync(
                    It.IsAny<int>(),
                    2))
                .ReturnsAsync(ListaReglasclave);

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioByPersonIdAsync(idPersona))
                .ReturnsAsync(usuarioMock);

            usuarioV2Repository.Setup(m => m
                    .ObtenerUsuarioByPersonIdAsync(idPersona))
                .ReturnsAsync(usuarioV2Mock);

            auditoriaV2Repository.Setup(m => m
                    .SaveAuditLogAsync(
                        It.IsAny<int>(),
                        It.IsAny<EventTypes>(),
                        It.IsAny<EventResults>(),
                        It.IsAny<string>(),
                        It.IsAny<FechaDbServerV2>(),
                        It.IsAny<string>()))
                .ReturnsAsync(AuditoriaLogV2Mock);

            var datosRequestCorrectos = new MigracionModelInput
            {
                IdPersona = idPersona,
                UserName = "UserName1",
                Password = "1010"
            };

            var sut = CrearUsuarioService(usuarioRepository.Object, usuarioV2Repository.Object,
                auditoriaV2Repository.Object, reglaValidacionV2Repository.Object);

            // Act
            var resultado = await sut.MigrarAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Fact]
        public async Task MigrarThrowException()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Mock = UsuarioV2.FirstOrDefault(x => x.PersonId == It.IsAny<long>());
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();

            reglaValidacionV2Repository.Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync(
                    It.IsAny<int>(),
                    1))
                .ReturnsAsync(ListaReglasUsuarios);

            reglaValidacionV2Repository.Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync(
                    It.IsAny<int>(),
                    2))
                .ReturnsAsync(ListaReglasclave);

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioByPersonIdAsync(It.IsAny<long>()))
                .Throws(new Exception("Excepción no controlada"));

            usuarioV2Repository.Setup(m => m
                    .ObtenerUsuarioByPersonIdAsync(It.IsAny<long>()))
                .ReturnsAsync(usuarioV2Mock);

            auditoriaV2Repository.Setup(m => m
                    .SaveAuditLogAsync(
                        It.IsAny<int>(),
                        It.IsAny<EventTypes>(),
                        It.IsAny<EventResults>(),
                        It.IsAny<string>(),
                        It.IsAny<FechaDbServerV2>(),
                        It.IsAny<string>()))
                .ReturnsAsync(AuditoriaLogV2Mock);

            var datosRequestCorrectos = new MigracionModelInput
            {
                IdPersona = 123456,
                UserName = "UserName",
                Password = "Password"
            };

            var sut = CrearUsuarioService(usuarioRepository.Object, usuarioV2Repository.Object,
                auditoriaV2Repository.Object, reglaValidacionV2Repository.Object);

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.MigrarAsync(Headers.ToRequestBody(datosRequestCorrectos,
                    AllowedChannelsBuilder.CrearAllowedChannels())));

            // Assert
            resultado.Message.Should().Be("Excepción no controlada");
        }
    }
}
