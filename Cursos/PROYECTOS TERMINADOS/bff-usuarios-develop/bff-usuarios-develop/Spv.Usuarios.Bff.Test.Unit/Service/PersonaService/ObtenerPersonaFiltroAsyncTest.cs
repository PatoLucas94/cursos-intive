using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.CatalogoClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Service;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.PersonaService
{
    public class ObtenerPersonaFiltroAsyncTest
    {
        // Persona correcta
        private const int PersonaCorrectaLeId = 16484507;
        private const int PersonaCorrectaLcId = 16804753;
        private const int PersonaCorrectaPassportId1 = 15520026;
        private const int PersonaCorrectaPassportId2 = 16705747;
        // Números de Documento Duplicados
        private const string NroDocDuplicadoLeLc = "1011781";
        private const string NroDocDuplicadoPasaporte = "3949";
        // Persona incorrecta
        private const string PersonaIncorrectaNroDocumento = "33222111";

        private static readonly List<ApiPersonasFiltroModelOutput> Personas = new List<ApiPersonasFiltroModelOutput>
        {
            new ApiPersonasFiltroModelOutput {
                id = PersonaCorrectaLeId,
                pais_documento = AppConstants.ArgentinaCodigoBantotal,
                tipo_documento = (int)TipoDocumento.Le,
                numero_documento = NroDocDuplicadoLeLc
            },
            new ApiPersonasFiltroModelOutput {
                id = PersonaCorrectaLcId,
                pais_documento = AppConstants.ArgentinaCodigoBantotal,
                tipo_documento = (int)TipoDocumento.Lc,
                numero_documento = NroDocDuplicadoLeLc
            },
            new ApiPersonasFiltroModelOutput {
                id = PersonaCorrectaPassportId1,
                pais_documento = 129,
                tipo_documento = (int)TipoDocumento.Pasaporte,
                numero_documento = NroDocDuplicadoPasaporte
            },
            new ApiPersonasFiltroModelOutput {
                id = PersonaCorrectaPassportId2,
                pais_documento = 4,
                tipo_documento = (int)TipoDocumento.Pasaporte,
                numero_documento = NroDocDuplicadoPasaporte
            }
        };

        private static readonly List<ApiPersonasFisicaInfoModelOutput> PersonasFisicas = new List<ApiPersonasFisicaInfoModelOutput>
        {
            new ApiPersonasFisicaInfoModelOutput {
                id = PersonaCorrectaLeId,
                pais_documento = AppConstants.ArgentinaCodigoBantotal,
                tipo_documento = (int)TipoDocumento.Le,
                numero_documento = NroDocDuplicadoLeLc,
                emails = new List<Email>
                {
                    new Email
                    {
                        confiable = true,
                        direccion = "confiableLC@test.com"
                    }
                },
                telefonos = new List<Telefono>
                {
                    new Telefono
                    {
                        confiable = true,
                        numero = "1345235"
                    }
                }
            },
            new ApiPersonasFisicaInfoModelOutput {
                id = PersonaCorrectaLcId,
                pais_documento = AppConstants.ArgentinaCodigoBantotal,
                tipo_documento = (int)TipoDocumento.Lc,
                numero_documento = NroDocDuplicadoLeLc,
                emails = new List<Email>
                {
                    new Email
                    {
                        confiable = true,
                        direccion = "confiableLE@test.com"
                    }
                },
                telefonos = new List<Telefono>
                {
                    new Telefono
                    {
                        confiable = true,
                        numero = "1345236"
                    }
                }
            },
            new ApiPersonasFisicaInfoModelOutput {
                id = PersonaCorrectaPassportId1,
                pais_documento = 129,
                tipo_documento = (int)TipoDocumento.Pasaporte,
                numero_documento = NroDocDuplicadoPasaporte,
                emails = new List<Email>
                {
                    new Email
                    {
                        confiable = true,
                        direccion = "confiablePasaporte1@test.com"
                    }
                },
                telefonos = new List<Telefono>
                {
                    new Telefono
                    {
                        confiable = true,
                        numero = "1345237"
                    }
                }
            },
            new ApiPersonasFisicaInfoModelOutput {
                id = PersonaCorrectaPassportId2,
                pais_documento = 4,
                tipo_documento = (int)TipoDocumento.Pasaporte,
                numero_documento = NroDocDuplicadoPasaporte,
                emails = new List<Email>
                {
                    new Email
                    {
                        confiable = true,
                        direccion = "confiablePasaporte2@test.com"
                    }
                },
                telefonos = new List<Telefono>
                {
                    new Telefono
                    {
                        confiable = true,
                        numero = "1345238"
                    }
                }
            }
        };

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[]
                {
                    NroDocDuplicadoLeLc,
                    (int)TipoDocumento.Le,
                    AppConstants.ArgentinaCodigoBantotal,
                    true,
                    StatusCodes.Status200OK,
                    false
                },
                new object[]
                {
                    NroDocDuplicadoLeLc,
                    null,
                    null,
                    true,
                    StatusCodes.Status200OK,
                    true
                },
                new object[]
                {
                    NroDocDuplicadoPasaporte,
                    null,
                    null,
                    true,
                    StatusCodes.Status200OK,
                    true
                },
                new object[]
                {
                    PersonaIncorrectaNroDocumento,
                    (int)TipoDocumento.Le,
                    AppConstants.ArgentinaCodigoBantotal,
                    false,
                    StatusCodes.Status404NotFound,
                    false
                }
            };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static PersonasService CrearPersonasService(
            IApiPersonasRepository apiPersonasRepository,
            IApiUsuariosRepositoryV2 apiUsuariosV2Repository = null)
        {
            var loggerMock = new Mock<ILogger<PersonasService>>();
            var apiCatalogoMock = new Mock<IApiCatalogoRepository>();

            var paisesMock = new List<ApiCatalogoPaisesModelOutput>
            {
                new ApiCatalogoPaisesModelOutput
                {
                    codigo = 4,
                    descripcion = "FRANCIA REPUBLICA"
                },
                new ApiCatalogoPaisesModelOutput
                {
                    codigo = 129,
                    descripcion = "MONACO, PRINCIPADO DE"
                }
            };

            apiCatalogoMock.Setup(m => m.ObtenerPaisesAsync()).ReturnsAsync(paisesMock);

            return new PersonasService(
                loggerMock.Object,
                apiPersonasRepository,
                apiUsuariosV2Repository ?? new Mock<IApiUsuariosRepositoryV2>().Object,
                apiCatalogoMock.Object);
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ObtenerPersonaFiltroAsync(
            string nroDocumento,
            int? idTipoDocumento,
            int? idPais,
            bool isOk,
            int statusCode,
            bool conflicto)
        {
            // Arrange
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiUsuariosV2Repository = new Mock<IApiUsuariosRepositoryV2>();

            var personaFisicaLeMock = PersonasFisicas.FirstOrDefault(x =>
                x.pais_documento == (idPais ?? AppConstants.ArgentinaCodigoBantotal) &&
                x.tipo_documento == (idTipoDocumento ?? (int)TipoDocumento.Le) &&
                x.numero_documento == nroDocumento);

            var personaFisicaLcMock = PersonasFisicas.FirstOrDefault(x =>
                x.pais_documento == AppConstants.ArgentinaCodigoBantotal &&
                x.tipo_documento == (int)TipoDocumento.Lc &&
                x.numero_documento == NroDocDuplicadoLeLc);

            var personaFisicaPasaporte1Mock = PersonasFisicas.FirstOrDefault(x =>
                x.pais_documento == 129 &&
                x.tipo_documento == (int)TipoDocumento.Pasaporte &&
                x.numero_documento == NroDocDuplicadoPasaporte);
            
            var personaFisicaPasaporte2Mock = PersonasFisicas.FirstOrDefault(x =>
                x.pais_documento == 4 &&
                x.tipo_documento == (int)TipoDocumento.Pasaporte &&
                x.numero_documento == NroDocDuplicadoPasaporte);

            // Persona
            apiPersonasRepository.Setup(m =>
                    m.ObtenerPersonaFiltroAsync(nroDocumento))
                .ReturnsAsync(Personas.FindAll(x => x.numero_documento == nroDocumento));

            // Persona Física
            apiPersonasRepository.Setup(m =>
                    m.ObtenerInfoPersonaFisicaAsync(PersonaCorrectaLeId.ToString()))
                .ReturnsAsync(personaFisicaLeMock);

            apiPersonasRepository.Setup(m =>
                    m.ObtenerInfoPersonaFisicaAsync(PersonaCorrectaLcId.ToString()))
                .ReturnsAsync(personaFisicaLcMock);
            
            apiPersonasRepository.Setup(m =>
                    m.ObtenerInfoPersonaFisicaAsync(PersonaCorrectaPassportId1.ToString()))
                .ReturnsAsync(personaFisicaPasaporte1Mock);

            apiPersonasRepository.Setup(m =>
                    m.ObtenerInfoPersonaFisicaAsync(PersonaCorrectaPassportId2.ToString()))
                .ReturnsAsync(personaFisicaPasaporte2Mock);

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba", "")
                }
            };

            var json = JsonConvert.SerializeObject(response);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            apiUsuariosV2Repository.Setup(m =>
                    m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = data });

            var datosRequestCorrectos = new PersonaFiltroInput
            {
                NumeroDocumento = nroDocumento,
                TipoDocumento = idTipoDocumento,
                IdPais = idPais
            };

            var sut = CrearPersonasService(apiPersonasRepository.Object, apiUsuariosV2Repository.Object);

            // Act
            var resultado = await sut.ObtenerPersonaFiltroAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);

            if (conflicto)
            {
                var payload = resultado.Match(
                    result => result.Payload,
                    clientError => new PersonaModelOutput(),
                       serverError => new PersonaModelOutput());

                if (payload.TiposDocumento?.Count > 0)
                {
                    payload.TiposDocumento.Any(td => 
                        td.codigo == (int)TipoDocumento.Le && td.descripcion == AppConstants.LibretaEnrolamiento).Should().BeTrue();

                    payload.TiposDocumento.Any(td =>
                        td.codigo == (int)TipoDocumento.Lc && td.descripcion == AppConstants.LibretaCivica).Should().BeTrue();
                }

                if (payload.Paises?.Count > 0)
                {
                    payload.Paises.Any(p => p.codigo == 129).Should().BeTrue();

                    payload.Paises.Any(p => p.codigo == 4).Should().BeTrue();
                }
            }
        }
    }
}
