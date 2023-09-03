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
using Spv.Usuarios.Common.Dtos.PersonaService.Output;
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
    public class RegistrarAsyncTestV2
    {
        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[]
                {
                    IdPersonaTest1, 80, 4, "10123456", false, StatusCodes.Status409Conflict, true
                }, // UsuarioYaExiste en Legacy
                new object[]
                {
                    IdPersonaTest3, 80, 4, "10111000", false, StatusCodes.Status409Conflict, true
                }, // UsuarioYaExiste en Modelo Nuevo
                new object[] { IdPersonaTest4, 80, 4, "12111111", true, StatusCodes.Status201Created, true },
                new object[]
                {
                    IdPersonaTest5, 80, 4, "12000000", false, StatusCodes.Status400BadRequest, true
                }, // NoSeEncontroPersonaFisica
                new object[]
                {
                    IdPersonaTest6, 80, 4, "12000000", false, StatusCodes.Status400BadRequest, true
                }, // NroDocumentoNoCoincide
                new object[]
                {
                    IdPersonaTest7, 80, 4, "12000000", false, StatusCodes.Status400BadRequest, true
                }, // NroDocumentoInvalidoDePersonaFisica
                new object[]
                {
                    IdPersonaTest8, 80, 4, "1012345a", false, StatusCodes.Status400BadRequest, true
                }, // NroDocumentoInvalido
            };

        public static IEnumerable<object[]> DatosV1 =>
            new List<object[]>
            {
                new object[]
                {
                    IdPersonaTest1, 80, 4, "10123456", "User0001", false, StatusCodes.Status409Conflict, false
                }, // UsuarioYaExiste en Legacy
                new object[]
                    { IdPersonaTest4, 80, 4, "12111111", "User0002", true, StatusCodes.Status201Created, false },
                new object[]
                {
                    IdPersonaTest5, 80, 4, "12000000", "User0003", false, StatusCodes.Status400BadRequest, false
                }, // NoSeEncontroPersonaFisica
                new object[]
                {
                    IdPersonaTest6, 80, 4, "12000000", "User0004", false, StatusCodes.Status400BadRequest, false
                }, // NroDocumentoNoCoincide
                new object[]
                {
                    IdPersonaTest7, 80, 4, "12000000", "User0005", false, StatusCodes.Status400BadRequest, false
                }, // NroDocumentoInvalidoDePersonaFisica
                new object[]
                {
                    IdPersonaTest8, 80, 4, "1012345a", "User0006", false, StatusCodes.Status400BadRequest, false
                }, // NroDocumentoInvalido
                new object[]
                {
                    IdPersonaTest9, 80, 4, "10123457", Duplicad0, false, StatusCodes.Status409Conflict, false
                } // UsuarioYaUtilizado en Legacy
            };

        private static readonly List<ReglaValidacionV2> ListaReglasUsuarios = new List<ReglaValidacionV2>
        {
            new ReglaValidacionV2
            {
                ValidationRuleText = "Letras y números.",
                RegularExpression = "/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/",
                InputId = 1,
                IsRequired = true
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "Entre8y15Caracteres",
                RegularExpression = "/^[A-Za-z0-9]{8,15}$/i",
                InputId = 1,
                IsRequired = true
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "NoMasDe3CaracteresRepetidos",
                RegularExpression = @"/^(?!.*?([A-Za-z0-9])\1\1\1).+$/i",
                InputId = 1,
                IsRequired = true
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "NoMasDe3CaracteresConsecutivosAscODesc",
                RegularExpression =
                    "/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210|abcd|bcde|cdef|defg|efgh|fghi|ghij|hijk|ijkl|jklm|klmn|lmno|mnop|nopq|opqr|pqrs|qrst|rstu|stuv|tuvw|uvwx|vwxy|wxyz|zyxw|yxwv|xwvu|wvut|vuts|utsr|tsru|srqp|rqpo|qpon|ponm|onml|nmlk|mlkj|lkji|kjih|jihg|ihgf|hgfe|gfed|fedc|edcb|dcba|ABCD|BCDE|CDEF|DEFG|EFGH|FGHI|GHIJ|HIJK|IJKL|JKLM|KLMN|LMNO|MNOP|NOPQ|OPQR|PQRS|QRST|RSTU|STUV|TUVW|UVWX|VWXY|WXYZ|ZYXW|YXWV|XWVU|WVUT|VUTS|UTSR|TSRU|SRQP|RQPO|QPON|PONM|ONML|NMLK|MLKJ|LKJI|KJIH|JIHG|IHGF|HGFE|GFED|FEDC|EDCB|DCBA)).+$/i",
                InputId = 1,
                IsRequired = true
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "AlMenos1LetraMayuscula",
                RegularExpression = "/^(?=.*[A-Z])([A-Za-z0-9]+)$/",
                InputId = 1,
                IsRequired = true
            },
        };

        private static readonly List<ReglaValidacionV2> ListaReglasclave = new List<ReglaValidacionV2>
        {
            new ReglaValidacionV2
            {
                ValidationRuleText = "SoloNumeros",
                RegularExpression = "/^(?=.*[0-9])([0-9]+)$/",
                InputId = 2,
                IsRequired = true
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "Solo4Caracteres",
                RegularExpression = "/^[A-Za-z0-9]{4}$/i",
                InputId = 2,
                IsRequired = true
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "NoMasDe3NumerosRepetidos",
                RegularExpression = @"/^(?!.*?([0-9])\1\1\1).+$/",
                InputId = 2,
                IsRequired = true
            },
            new ReglaValidacionV2
            {
                ValidationRuleText = "NoMasDe3NumerosConsecutivosAscODesc",
                RegularExpression =
                    "/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210)).+$/",
                InputId = 2,
                IsRequired = true
            },
        };

        public static IEnumerable<object[]> DatosValidacionReglas =>
            new List<object[]>
            {
                new object[] { IdPersonaTest4, 80, 4, "12111111", false, StatusCodes.Status400BadRequest, true },
            };

        public static IEnumerable<object[]> DatosV1ValidacionReglas =>
            new List<object[]>
            {
                new object[] { IdPersonaTest4, 80, 4, "12111111", false, StatusCodes.Status400BadRequest, false },
            };

        private const long IdPersonaTest1 = 12345678;
        private const long IdPersonaTest3 = 11111111;
        private const long IdPersonaTest4 = 987654321;
        private const long IdPersonaTest5 = 1;
        private const long IdPersonaTest6 = 2;
        private const long IdPersonaTest7 = 3;
        private const long IdPersonaTest8 = 4;
        private const long IdPersonaTest9 = 5;

        private const string User1245 = "User1245";
        private const string Duplicad0 = "Duplicad0";

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
            },
            new Usuario
            {
                UserId = 4,
                UserName = Duplicad0,
                Password = "clave_encriptada4",
                UserStatusId = 3,
                UserData = new DatosUsuario
                {
                    PersonId = IdPersonaTest9.ToString()
                },
                DocumentCountryId = "80",
                DocumentTypeId = 4,
                DocumentNumber = "10111001",
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
                DocumentNumber = "10111000"
            }
        };

        private static readonly List<PersonaFisicaInfoModelResponse> PersonasFisicas =
            new List<PersonaFisicaInfoModelResponse>
            {
                new PersonaFisicaInfoModelResponse
                {
                    id = 12345678,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = "10123456"
                },
                new PersonaFisicaInfoModelResponse
                {
                    id = 11111111,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = "10111000"
                },
                new PersonaFisicaInfoModelResponse
                {
                    id = 2,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = "10999999"
                },
                new PersonaFisicaInfoModelResponse
                {
                    id = 987654321,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = "12111111",
                    telefonos = new List<Telefono>
                    {
                        new Telefono
                        {
                            id = 762855,
                            numero_local = 23454356,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 11,
                            numero = "1123454356",
                            ddn = "0111523454356",
                            ddi = "+54 9 11 2345 4356",
                            no_llame = "false",
                            fecha_creacion = "2021-01-07 15:12:09.1049280 -03:00",
                            canal_creacion = "HBI",
                            usuario_creacion = "18",
                            compania = "CLARO",
                            principal = true,
                            confiable = true,
                            doble_factor = true,
                            tipo_telefono = "CELULAR",
                            dado_de_baja = false,
                            ultima_verificacion_positiva = new UltimaVerificacionPositiva
                            {
                                canal_verificacion = "OBI",
                                fecha_verificacion = "2021-01-07 15:12:09.1049280 -03:00",
                                usuario_verificacion = "18"
                            }
                        }
                    },

                    emails = new List<Email>
                    {
                        new Email
                        {
                            id = 29140,
                            direccion = "asdfgh@simtlix.com",
                            fecha_creacion = "2020-11-18 17:03:45.9769930 -03:00",
                            canal_creacion = "HBI",
                            usuario_creacion = "test",
                            usuario_modificacion = null,
                            origen_contacto = "PERSONAL",
                            principal = true,
                            confiable = true,
                            dado_de_baja = false
                        }
                    },
                },
                new PersonaFisicaInfoModelResponse
                {
                    id = 3,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = "1012345a"
                },
                new PersonaFisicaInfoModelResponse
                {
                    id = 4,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = "10111000"
                },
                new PersonaFisicaInfoModelResponse
                {
                    id = 5,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = "10123457"
                }
            };

        private static readonly AuditoriaLogV2 AuditoriaLogV2Mock = new AuditoriaLogV2
        {
            AuditLogId = 1,
            Channel = "OBI",
            DateTime = DateTime.MinValue,
            UserId = 1,
            EventTypeId = (int)EventTypes.Registration,
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

        private static UsuariosService CrearUsuarioService(IUsuarioRepository usuarioRepository,
            IUsuarioV2Repository usuarioV2Repository,
            IAuditoriaLogV2Repository auditoriaLogV2Repository,
            IPersonasRepository personasRepository,
            IConfiguracionesService configuracionesService,
            IReglaValidacionV2Repository reglaValidacionV2Repository)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var encryptionMock = new Mock<IEncryption>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();

            var helperDbServerMock = new Mock<IHelperDbServer>();
            helperDbServerMock.Setup(m =>
                m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServer { Now = DateTime.MinValue });

            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();
            helperDbServerMockV2.Setup(m =>
                m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServerV2 { Now = DateTime.MinValue });

            var auditoriaMock = new Mock<IAuditoriaRepository>();

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
                configuracionesService,
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
                reglaValidacionV2Repository,
                MapperProfile.GetAppProfile(),
                new Mock<IDistributedCache>().Object,
                btaRepository.Object
            );
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Registrar(
            long idPersona,
            int documentCountryId,
            int documentTypeId,
            string documentNumber,
            bool isOk,
            int statusCode,
            bool registrationEnabled)
        {
            // Arrange
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserData.PersonId == idPersona.ToString());
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Mock = UsuarioV2.FirstOrDefault(x => x.PersonId == idPersona);
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var personasRepositoryMock = PersonasFisicas.FirstOrDefault(x => x.id == idPersona);
            var personasRepository = new Mock<IPersonasRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();

            configuracionesService.Setup(m => m
                    .ObtenerConfiguracionRegistracionNuevoModeloEstaHabilitadaAsync())
                .ReturnsAsync(registrationEnabled);

            personasRepository.Setup(m => m
                    .ObtenerInfoPersonaFisica(idPersona, null, null))
                .ReturnsAsync(personasRepositoryMock);

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        documentCountryId.ToString().PadLeft(3, '0'),
                        documentTypeId,
                        documentNumber))
                .ReturnsAsync(usuarioMock);

            usuarioV2Repository.Setup(m => m
                    .ObtenerUsuarioByPersonIdAsync(idPersona))
                .ReturnsAsync(usuarioV2Mock);

            reglaValidacionV2Repository.Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync(
                    It.IsAny<int>(),
                    1))
                .ReturnsAsync(ListaReglasUsuarios);

            reglaValidacionV2Repository.Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync(
                    It.IsAny<int>(),
                    2))
                .ReturnsAsync(ListaReglasclave);

            auditoriaV2Repository.Setup(m => m
                    .SaveAuditLogAsync(
                        It.IsAny<int>(),
                        It.IsAny<EventTypes>(),
                        It.IsAny<EventResults>(),
                        It.IsAny<string>(),
                        It.IsAny<FechaDbServerV2>(),
                        It.IsAny<string>()))
                .ReturnsAsync(AuditoriaLogV2Mock);

            var datosRequestCorrectos = new RegistracionModelInputV2
            {
                PersonId = idPersona,
                UserName = User1245,
                Password = "1245",
                DocumentCountryId = documentCountryId,
                DocumentTypeId = documentTypeId,
                DocumentNumber = documentNumber,
            };

            var sut = CrearUsuarioService(
                usuarioRepository.Object,
                usuarioV2Repository.Object,
                auditoriaV2Repository.Object,
                personasRepository.Object,
                configuracionesService.Object,
                reglaValidacionV2Repository.Object);

            // Act
            var resultado = await sut.RegistrarV2Async(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            auditoriaV2Repository.Verify(x => x.SaveAuditLogAsync(It.IsAny<int>(),
                EventTypes.Registration,
                It.IsAny<EventResults>(),
                It.IsAny<string>(),
                It.IsAny<FechaDbServerV2>(),
                It.IsAny<string>()), Times.Once);
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Theory]
        [MemberData(nameof(DatosV1))]
        public async Task RegistrarV1(
            long idPersona,
            int documentCountryId,
            int documentTypeId,
            string documentNumber,
            string userName,
            bool isOk,
            int statusCode,
            bool registrationEnabled)
        {
            // Arrange
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserData.PersonId == idPersona.ToString());
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var auditoriaRepository = new Mock<IAuditoriaRepository>();
            var personasRepositoryMock = PersonasFisicas.FirstOrDefault(x => x.id == idPersona);
            var personasRepository = new Mock<IPersonasRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();

            configuracionesService.Setup(m => m
                    .ObtenerConfiguracionRegistracionNuevoModeloEstaHabilitadaAsync())
                .ReturnsAsync(registrationEnabled);

            personasRepository.Setup(m => m
                    .ObtenerInfoPersonaFisica(idPersona, null, null))
                .ReturnsAsync(personasRepositoryMock);

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        documentCountryId.ToString().PadLeft(3, '0'),
                        documentTypeId,
                        documentNumber))
                .ReturnsAsync(usuarioMock);

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioAsync(Duplicad0))
                .ReturnsAsync(usuarioMock);

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioAsync(documentTypeId, documentNumber))
                .ReturnsAsync(usuarioMock);

            auditoriaRepository.Setup(m => m
                .SaveRegistrationAuditAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>()));

            var ListaReglasUsuarios = new List<ReglaValidacionV2>
            {
                new ReglaValidacionV2
                {
                    RegularExpression = "/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/",
                    InputId = (int)Input.Usuario,
                    ValidationRulePriority = 1
                },
                new ReglaValidacionV2
                {
                    RegularExpression = "/^.{8,15}$/i",
                    InputId = 1,
                    ValidationRulePriority = 2
                }
            };

            reglaValidacionV2Repository.Setup(m =>
                    m.ObtenerReglasValidacionActivasByModelAndInputAsync((int)Model.HomeBankingIndividuo,
                        (int)Input.Usuario))
                .ReturnsAsync(ListaReglasUsuarios);

            var reglaPassword4 = new ReglaValidacionV2
            {
                RegularExpression = "/^.{8,14}$/i",
                InputId = (int)Input.Clave,
                ValidationRulePriority = 4
            };

            var ListaReglasPassword = new List<ReglaValidacionV2>
            {
                new ReglaValidacionV2
                {
                    RegularExpression = @"/^(.*(\d+.*\d+).*)$/",
                    InputId = (int)Input.Clave,
                    ValidationRulePriority = 2
                },
                new ReglaValidacionV2
                {
                    RegularExpression = "/^(?=.*[A-Z]).*$/",
                    InputId = (int)Input.Clave,
                    ValidationRulePriority = 3
                },
                new ReglaValidacionV2
                {
                    RegularExpression = "/^.{8,14}$/i",
                    InputId = (int)Input.Clave,
                    ValidationRulePriority = 4
                },
                new ReglaValidacionV2
                {
                    RegularExpression = @"/^[A-Za-z0-9.\-_]+$/",
                    InputId = (int)Input.Clave,
                    ValidationRulePriority = 1
                }
            };

            reglaValidacionV2Repository.Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync(1, 2))
                .ReturnsAsync(ListaReglasPassword);

            var datosRequestCorrectos = new RegistracionModelInputV2
            {
                PersonId = idPersona,
                UserName = userName,
                Password = "P12456712",
                DocumentCountryId = documentCountryId,
                DocumentTypeId = documentTypeId,
                DocumentNumber = documentNumber,
            };

            var sut = CrearUsuarioService(
                usuarioRepository.Object,
                usuarioV2Repository.Object,
                auditoriaV2Repository.Object,
                personasRepository.Object,
                configuracionesService.Object,
                reglaValidacionV2Repository.Object);

            // Act
            var resultado = await sut.RegistrarV2Async(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            auditoriaV2Repository.Verify(x => x.SaveAuditLogAsync(It.IsAny<int>(),
                It.IsAny<EventTypes>(),
                It.IsAny<EventResults>(),
                It.IsAny<string>(),
                It.IsAny<FechaDbServerV2>(),
                It.IsAny<string>()), Times.Once);
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Fact]
        public async Task RegistrarThrowException()
        {
            // Arrange
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserData.PersonId == It.IsAny<string>());
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Mock = UsuarioV2.FirstOrDefault(x => x.PersonId == It.IsAny<long>());
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var personasRepository = new Mock<IPersonasRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();

            configuracionesService.Setup(m => m
                    .ObtenerConfiguracionRegistracionNuevoModeloEstaHabilitadaAsync())
                .ReturnsAsync(true);

            personasRepository.Setup(m => m
                    .ObtenerInfoPersonaFisica(It.IsAny<long>(), null, null))
                .Throws(new Exception("Excepción no controlada"));

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(usuarioMock);

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

            reglaValidacionV2Repository
                .Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<ReglaValidacionV2>());

            var datosRequestCorrectos = new RegistracionModelInputV2
            {
                PersonId = 123456,
                UserName = User1245,
                Password = "1245",
                DocumentCountryId = 80,
                DocumentTypeId = 4,
                DocumentNumber = "12345678",
            };

            var sut = CrearUsuarioService(
                usuarioRepository.Object,
                usuarioV2Repository.Object,
                auditoriaV2Repository.Object,
                personasRepository.Object,
                configuracionesService.Object,
                reglaValidacionV2Repository.Object);

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.RegistrarV2Async(Headers.ToRequestBody(datosRequestCorrectos,
                    AllowedChannelsBuilder.CrearAllowedChannels())));

            // Assert
            resultado.Message.Should().Be("Excepción no controlada");
        }

        [Theory]
        [MemberData(nameof(DatosValidacionReglas))]
        public async Task RegistrarValidarReglasCaso1(
            long idPersona,
            int documentCountryId,
            int documentTypeId,
            string documentNumber,
            bool isOk,
            int statusCode,
            bool registrationEnabled)
        {
            // Arrange
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserData.PersonId == idPersona.ToString());
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Mock = UsuarioV2.FirstOrDefault(x => x.PersonId == idPersona);
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var personasRepositoryMock = PersonasFisicas.FirstOrDefault(x => x.id == idPersona);
            var personasRepository = new Mock<IPersonasRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();

            configuracionesService.Setup(m => m
                    .ObtenerConfiguracionRegistracionNuevoModeloEstaHabilitadaAsync())
                .ReturnsAsync(registrationEnabled);

            personasRepository.Setup(m => m
                    .ObtenerInfoPersonaFisica(idPersona, null, null))
                .ReturnsAsync(personasRepositoryMock);

            var reglaUsuario5 = new ReglaValidacionV2
            {
                RegularExpression = "/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/",
                InputId = 1,
                ValidationRulePriority = 5,
                ValidationRuleText = "5"
            };

            var ListaReglasUsuarios = new List<ReglaValidacionV2>
            {
                new ReglaValidacionV2
                {
                    RegularExpression = "/^(?=.*[A-Z])([A-Za-z0-9]+)$/",
                    InputId = (int)Input.Usuario,
                    ValidationRulePriority = 1,
                    ValidationRuleText = "1",
                    IsRequired = true
                },
                new ReglaValidacionV2
                {
                    RegularExpression =
                        "/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210|abcd|bcde|cdef|defg|efgh|fghi|ghij|hijk|ijkl|jklm|klmn|lmno|mnop|nopq|opqr|pqrs|qrst|rstu|stuv|tuvw|uvwx|vwxy|wxyz|zyxw|yxwv|xwvu|wvut|vuts|utsr|tsru|srqp|rqpo|qpon|ponm|onml|nmlk|mlkj|lkji|kjih|jihg|ihgf|hgfe|gfed|fedc|edcb|dcba|ABCD|BCDE|CDEF|DEFG|EFGH|FGHI|GHIJ|HIJK|IJKL|JKLM|KLMN|LMNO|MNOP|NOPQ|OPQR|PQRS|QRST|RSTU|STUV|TUVW|UVWX|VWXY|WXYZ|ZYXW|YXWV|XWVU|WVUT|VUTS|UTSR|TSRU|SRQP|RQPO|QPON|PONM|ONML|NMLK|MLKJ|LKJI|KJIH|JIHG|IHGF|HGFE|GFED|FEDC|EDCB|DCBA)).+$/i",
                    InputId = (int)Input.Usuario,
                    ValidationRulePriority = 2,
                    ValidationRuleText = "2",
                    IsRequired = true
                },
                new ReglaValidacionV2
                {
                    RegularExpression = @"/^(?!.*?([A-Za-z0-9])\1\1\1).+$/i",
                    InputId = (int)Input.Usuario,
                    ValidationRulePriority = 3,
                    ValidationRuleText = "3",
                    IsRequired = true
                },
                new ReglaValidacionV2
                {
                    RegularExpression = "/^[A-Za-z0-9]{8,15}$/i",
                    InputId = (int)Input.Usuario,
                    ValidationRulePriority = 4,
                    ValidationRuleText = "4",
                    IsRequired = true
                }
            };

            reglaValidacionV2Repository
                .Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync((int)Model.BaseUnicaUsuarios,
                    (int)Input.Usuario)).ReturnsAsync(ListaReglasUsuarios);

            var ListaReglasPassword = new List<ReglaValidacionV2>
            {
                new ReglaValidacionV2
                {
                    RegularExpression =
                        "/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210)).+$/",
                    InputId = (int)Input.Clave,
                    ValidationRulePriority = 1,
                    ValidationRuleText = "1",
                    IsRequired = true
                },
                new ReglaValidacionV2
                {
                    RegularExpression = @"/^(?!.*?([0-9])\1\1\1).+$/",
                    InputId = (int)Input.Clave,
                    ValidationRulePriority = 2,
                    ValidationRuleText = "2",
                    IsRequired = true
                },
                new ReglaValidacionV2
                {
                    RegularExpression = "/^[A-Za-z0-9]{4}$/i",
                    InputId = (int)Input.Clave,
                    ValidationRulePriority = 3,
                    ValidationRuleText = "3",
                    IsRequired = true
                },
                new ReglaValidacionV2
                {
                    RegularExpression = "/^(?=.*[0-9])([0-9]+)$/",
                    InputId = (int)Input.Clave,
                    ValidationRulePriority = 4,
                    ValidationRuleText = "4",
                    IsRequired = true
                }
            };

            reglaValidacionV2Repository
                .Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync((int)Model.BaseUnicaUsuarios,
                    (int)Input.Clave)).ReturnsAsync(ListaReglasPassword);

            var datosRequestUserIvalido = new RegistracionModelInputV2
            {
                PersonId = idPersona,
                UserName = "1234",
                Password = "4444a",
                DocumentCountryId = documentCountryId,
                DocumentTypeId = documentTypeId,
                DocumentNumber = documentNumber,
            };

            var sut = CrearUsuarioService(
                usuarioRepository.Object,
                usuarioV2Repository.Object,
                auditoriaV2Repository.Object,
                personasRepository.Object,
                configuracionesService.Object,
                reglaValidacionV2Repository.Object);

            // Act
            var resultado = await sut.RegistrarV2Async(
                Headers.ToRequestBody(datosRequestUserIvalido, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            auditoriaV2Repository.Verify(x => x.SaveAuditLogAsync(It.IsAny<int>(),
                EventTypes.Registration,
                It.IsAny<EventResults>(),
                It.IsAny<string>(),
                It.IsAny<FechaDbServerV2>(),
                It.IsAny<string>()), Times.Once);
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Theory]
        [MemberData(nameof(DatosValidacionReglas))]
        public async Task RegistrarValidarReglasCaso2(
            long idPersona,
            int documentCountryId,
            int documentTypeId,
            string documentNumber,
            bool isOk,
            int statusCode,
            bool registrationEnabled)
        {
            // Arrange
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserData.PersonId == idPersona.ToString());
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Mock = UsuarioV2.FirstOrDefault(x => x.PersonId == idPersona);
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var personasRepositoryMock = PersonasFisicas.FirstOrDefault(x => x.id == idPersona);
            var personasRepository = new Mock<IPersonasRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();

            configuracionesService.Setup(m => m
                    .ObtenerConfiguracionRegistracionNuevoModeloEstaHabilitadaAsync())
                .ReturnsAsync(registrationEnabled);

            personasRepository.Setup(m => m
                    .ObtenerInfoPersonaFisica(idPersona, null, null))
                .ReturnsAsync(personasRepositoryMock);

            var reglaUsuario1 = new ReglaValidacionV2
            {
                RegularExpression = "/^(?=.*[A-Z])([A-Za-z0-9]+)$/",
                InputId = 1,
                ValidationRulePriority = 1,
                ValidationRuleText = "1",
                IsRequired = true
            };
            var reglaUsuario2 = new ReglaValidacionV2
            {
                RegularExpression =
                    "/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210|abcd|bcde|cdef|defg|efgh|fghi|ghij|hijk|ijkl|jklm|klmn|lmno|mnop|nopq|opqr|pqrs|qrst|rstu|stuv|tuvw|uvwx|vwxy|wxyz|zyxw|yxwv|xwvu|wvut|vuts|utsr|tsru|srqp|rqpo|qpon|ponm|onml|nmlk|mlkj|lkji|kjih|jihg|ihgf|hgfe|gfed|fedc|edcb|dcba)).+$/i",
                InputId = 1,
                ValidationRulePriority = 2,
                ValidationRuleText = "2",
                IsRequired = true
            };
            var reglaUsuario3 = new ReglaValidacionV2
            {
                RegularExpression = @"/^(?!.*?([A-Za-z0-9])\1\1\1).+$/i",
                InputId = 1,
                ValidationRulePriority = 3,
                ValidationRuleText = "3",
                IsRequired = true
            };
            var reglaUsuario4 = new ReglaValidacionV2
            {
                RegularExpression = "/^[A-Za-z0-9]{8,15}$/i",
                InputId = 1,
                ValidationRulePriority = 4,
                ValidationRuleText = "4",
                IsRequired = true
            };

            var ListaReglasUsuarios = new List<ReglaValidacionV2>();
            ListaReglasUsuarios.Add(reglaUsuario1);
            ListaReglasUsuarios.Add(reglaUsuario2);
            ListaReglasUsuarios.Add(reglaUsuario3);
            ListaReglasUsuarios.Add(reglaUsuario4);

            reglaValidacionV2Repository
                .Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync((int)Model.BaseUnicaUsuarios,
                    (int)Input.Usuario)).ReturnsAsync(ListaReglasUsuarios);

            var reglaPassword1 = new ReglaValidacionV2
            {
                RegularExpression =
                    "/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210)).+$/",
                InputId = 2,
                ValidationRulePriority = 1,
                ValidationRuleText = "1",
                IsRequired = true
            };
            var reglaPassword2 = new ReglaValidacionV2
            {
                RegularExpression = @"/^(?!.*?([0-9])\1\1\1).+$/",
                InputId = 2,
                ValidationRulePriority = 2,
                ValidationRuleText = "2",
                IsRequired = true
            };
            var reglaPassword3 = new ReglaValidacionV2
            {
                RegularExpression = "/^[A-Za-z0-9]{4}$/i",
                InputId = 2,
                ValidationRulePriority = 3,
                ValidationRuleText = "3",
                IsRequired = true
            };
            var reglaPassword4 = new ReglaValidacionV2
            {
                RegularExpression = "/^(?=.*[0-9])([0-9]+)$/",
                InputId = 2,
                ValidationRulePriority = 4,
                ValidationRuleText = "4",
                IsRequired = true
            };

            var ListaReglasPassword = new List<ReglaValidacionV2>();
            ListaReglasPassword.Add(reglaPassword1);
            ListaReglasPassword.Add(reglaPassword2);
            ListaReglasPassword.Add(reglaPassword3);
            ListaReglasPassword.Add(reglaPassword4);

            reglaValidacionV2Repository
                .Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync((int)Model.BaseUnicaUsuarios,
                    (int)Input.Clave)).ReturnsAsync(ListaReglasPassword);

            var datosRequestUserIvalido = new RegistracionModelInputV2
            {
                PersonId = idPersona,
                UserName = "Userrrrrr12",
                Password = "1234",
                DocumentCountryId = documentCountryId,
                DocumentTypeId = documentTypeId,
                DocumentNumber = documentNumber,
            };

            var sut = CrearUsuarioService(
                usuarioRepository.Object,
                usuarioV2Repository.Object,
                auditoriaV2Repository.Object,
                personasRepository.Object,
                configuracionesService.Object,
                reglaValidacionV2Repository.Object);

            // Act
            var resultado = await sut.RegistrarV2Async(
                Headers.ToRequestBody(datosRequestUserIvalido, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            auditoriaV2Repository.Verify(x => x.SaveAuditLogAsync(It.IsAny<int>(),
                EventTypes.Registration,
                It.IsAny<EventResults>(),
                It.IsAny<string>(),
                It.IsAny<FechaDbServerV2>(),
                It.IsAny<string>()), Times.Once);
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Theory]
        [MemberData(nameof(DatosV1ValidacionReglas))]
        public async Task RegistrarValidarReglasCaso3(
            long idPersona,
            int documentCountryId,
            int documentTypeId,
            string documentNumber,
            bool isOk,
            int statusCode,
            bool registrationEnabled)
        {
            // Arrange
            var usuarioMock = Usuarios.FirstOrDefault(x => x.UserData.PersonId == idPersona.ToString());
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Mock = UsuarioV2.FirstOrDefault(x => x.PersonId == idPersona);
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var personasRepositoryMock = PersonasFisicas.FirstOrDefault(x => x.id == idPersona);
            var personasRepository = new Mock<IPersonasRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();

            configuracionesService.Setup(m => m
                    .ObtenerConfiguracionRegistracionNuevoModeloEstaHabilitadaAsync())
                .ReturnsAsync(registrationEnabled);

            personasRepository.Setup(m => m
                    .ObtenerInfoPersonaFisica(idPersona, null, null))
                .ReturnsAsync(personasRepositoryMock);

            var reglaUsuario1 = new ReglaValidacionV2
            {
                RegularExpression = "/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/",
                InputId = 1,
                ValidationRulePriority = 1,
                ValidationRuleText = "4",
                IsRequired = true
            };
            var reglaUsuario2 = new ReglaValidacionV2
            {
                RegularExpression = "/^.{8,15}$/i",
                InputId = 1,
                ValidationRulePriority = 2,
                ValidationRuleText = "4",
                IsRequired = true
            };


            var ListaReglasUsuarios = new List<ReglaValidacionV2>();
            ListaReglasUsuarios.Add(reglaUsuario1);
            ListaReglasUsuarios.Add(reglaUsuario2);

            reglaValidacionV2Repository
                .Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync((int)Model.HomeBankingIndividuo,
                    (int)Input.Usuario)).ReturnsAsync(ListaReglasUsuarios);

            var reglaPassword1 = new ReglaValidacionV2
            {
                RegularExpression = @"/^[A-Za-z0-9.\-_]+$/",
                InputId = 2,
                ValidationRulePriority = 1,
                ValidationRuleText = "4",
                IsRequired = true
            };
            var reglaPassword2 = new ReglaValidacionV2
            {
                RegularExpression = @"/^(.*(\d+.*\d+).*)$/",
                InputId = 2,
                ValidationRulePriority = 2,
                ValidationRuleText = "4",
                IsRequired = true
            };
            var reglaPassword3 = new ReglaValidacionV2
            {
                RegularExpression = "/^(?=.*[A-Z]).*$/",
                InputId = 2,
                ValidationRulePriority = 3,
                ValidationRuleText = "4",
                IsRequired = true
            };
            var reglaPassword4 = new ReglaValidacionV2
            {
                RegularExpression = "/^.{8,14}$/i",
                InputId = 2,
                ValidationRulePriority = 4,
                ValidationRuleText = "4",
                IsRequired = true
            };

            var ListaReglasPassword = new List<ReglaValidacionV2>();
            ListaReglasPassword.Add(reglaPassword1);
            ListaReglasPassword.Add(reglaPassword2);
            ListaReglasPassword.Add(reglaPassword3);
            ListaReglasPassword.Add(reglaPassword4);

            reglaValidacionV2Repository
                .Setup(m => m.ObtenerReglasValidacionActivasByModelAndInputAsync((int)Model.HomeBankingIndividuo,
                    (int)Input.Clave)).ReturnsAsync(ListaReglasPassword);

            var datosRequestUserIvalido = new RegistracionModelInputV2
            {
                PersonId = idPersona,
                UserName = "User",
                Password = "pass",
                DocumentCountryId = documentCountryId,
                DocumentTypeId = documentTypeId,
                DocumentNumber = documentNumber,
            };

            var sut = CrearUsuarioService(
                usuarioRepository.Object,
                usuarioV2Repository.Object,
                auditoriaV2Repository.Object,
                personasRepository.Object,
                configuracionesService.Object,
                reglaValidacionV2Repository.Object);

            // Act
            var resultado = await sut.RegistrarV2Async(
                Headers.ToRequestBody(datosRequestUserIvalido, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            auditoriaV2Repository.Verify(x => x.SaveAuditLogAsync(It.IsAny<int>(),
                EventTypes.Registration,
                It.IsAny<EventResults>(),
                It.IsAny<string>(),
                It.IsAny<FechaDbServerV2>(),
                It.IsAny<string>()), Times.Once);
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }
    }
}
