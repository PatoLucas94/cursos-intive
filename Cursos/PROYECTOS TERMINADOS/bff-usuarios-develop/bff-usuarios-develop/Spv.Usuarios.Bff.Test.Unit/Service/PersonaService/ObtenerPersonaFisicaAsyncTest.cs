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
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Input;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Service;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.PersonaService
{
    public class ObtenerPersonaFisicaAsyncTest
    {
        // Persona correcta
        private const int PersonaCorrectaId = 1;
        private const int PersonaCorrectaIdPais = 1;
        private const int PersonaCorrectaIdTipoDocumento = 4;
        private const string PersonaCorrectaNroDocumento = "11222333";
        // Persona correcta sin email ni teléfono confiable
        private const int PersonaCorrectaSinEmailTelefonoId = 2;
        private const int PersonaCorrectaSinEmailTelefonoIdPais = 5;
        private const int PersonaCorrectaSinEmailTelefonoIdTipoDocumento = 2;
        private const string PersonaCorrectaSinEmailTelefonoNroDocumento = "8971243";
        // Persona correcta sin datos de persona física
        private const int PersonaFisicaIncorrectaId = 1;
        private const int PersonaFisicaIncorrectaIdPais = 5;
        private const int PersonaFisicaIncorrectaIdTipoDocumento = 1;
        private const string PersonaFisicaIncorrectaNroDocumento = "1231455";
        // Persona incorrecta
        private const int PersonaIncorrectaIdPais = 2;
        private const int PersonaIncorrectaIdTipoDocumento = 3;
        private const string PersonaIncorrectaNroDocumento = "33222111";

        public static IEnumerable<object[]> Datos =>
        new List<object[]>
        {
            new object[]
            {
                PersonaCorrectaIdPais, 
                PersonaCorrectaIdTipoDocumento, 
                PersonaCorrectaNroDocumento, 
                true, 
                StatusCodes.Status200OK
            },
            new object[]
            {
                PersonaCorrectaSinEmailTelefonoIdPais, 
                PersonaCorrectaSinEmailTelefonoIdTipoDocumento, 
                PersonaCorrectaSinEmailTelefonoNroDocumento, 
                true, 
                StatusCodes.Status200OK
            },
            new object[]
            {
                PersonaIncorrectaIdPais, 
                PersonaIncorrectaIdTipoDocumento, 
                PersonaIncorrectaNroDocumento, 
                false, 
                StatusCodes.Status404NotFound
            }
        };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static readonly List<ApiPersonasFisicaInfoModelOutput> PersonasFisicas = new List<ApiPersonasFisicaInfoModelOutput>
        {
            new ApiPersonasFisicaInfoModelOutput {
                id = PersonaCorrectaId,
                pais_documento = PersonaCorrectaIdPais,
                tipo_documento = PersonaCorrectaIdTipoDocumento,
                numero_documento = PersonaCorrectaNroDocumento,
                emails = new List<Email>
                {
                    new Email
                    {
                        confiable = true,
                        direccion = "confiable@test.com"
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
                id = PersonaCorrectaSinEmailTelefonoId,
                pais_documento = PersonaCorrectaSinEmailTelefonoIdPais,
                tipo_documento = PersonaCorrectaSinEmailTelefonoIdTipoDocumento,
                numero_documento = PersonaCorrectaSinEmailTelefonoNroDocumento,
                emails = new List<Email>
                {
                    new Email
                    {
                        confiable = false,
                        direccion = "noconfiable@test.com"
                    }
                },
                telefonos = new List<Telefono>
                {
                    new Telefono
                    {
                        confiable = false,
                        numero = "7890101112"
                    }
                }
            }
        };

        private static readonly List<ApiPersonaModelOutput> Personas = new List<ApiPersonaModelOutput>
        {
            new ApiPersonaModelOutput {
                id = PersonaCorrectaId
            },
            new ApiPersonaModelOutput {
                id = PersonaFisicaIncorrectaId
            },
            new ApiPersonaModelOutput {
                id = PersonaCorrectaSinEmailTelefonoId
            }
        };

        private static PersonasService CrearPersonasService(
            IApiPersonasRepository apiPersonasRepository, 
            IApiUsuariosRepositoryV2 usuariosV2Repository = null)
        {
            var loggerMock = new Mock<ILogger<PersonasService>>();
            var catalogoMock = new Mock<IApiCatalogoRepository>();

            return new PersonasService(
                loggerMock.Object,
                apiPersonasRepository,
                usuariosV2Repository ?? new Mock<IApiUsuariosRepositoryV2>().Object,
                catalogoMock.Object);
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task PersonaFisica(int idPais, int idTipoDocumento, string nroDocumento, bool isOk, int statusCode)
        {
            // Arrange
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiUsuariosV2Repository = new Mock<IApiUsuariosRepositoryV2>();

            var personaFisicaMock = PersonasFisicas.FirstOrDefault(x =>
                x.pais_documento == idPais &&
                x.tipo_documento == idTipoDocumento &&
                x.numero_documento == nroDocumento);

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba", "")
                }
            };

            var json = JsonConvert.SerializeObject(response);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Persona
            apiPersonasRepository.Setup(m => 
                    m.ObtenerPersonaAsync(PersonaCorrectaNroDocumento, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Personas.FirstOrDefault(x => x.id == PersonaCorrectaId));
            apiPersonasRepository.Setup(m => 
                    m.ObtenerPersonaAsync(PersonaFisicaIncorrectaNroDocumento, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Personas.FirstOrDefault(x => x.id == PersonaFisicaIncorrectaId));
            apiPersonasRepository.Setup(m => 
                    m.ObtenerPersonaAsync(PersonaCorrectaSinEmailTelefonoNroDocumento, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Personas.FirstOrDefault(x => x.id == PersonaCorrectaSinEmailTelefonoId));
            apiPersonasRepository.Setup(m => 
                    m.ObtenerPersonaAsync(PersonaIncorrectaNroDocumento, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult<ApiPersonaModelOutput>(null));

            // Persona Física
            apiPersonasRepository.Setup(m => 
                    m.ObtenerInfoPersonaFisicaAsync(PersonaCorrectaId.ToString()))
                .ReturnsAsync(personaFisicaMock);
            apiPersonasRepository.Setup(m => 
                    m.ObtenerInfoPersonaFisicaAsync(PersonaCorrectaSinEmailTelefonoId.ToString()))
                .ReturnsAsync(personaFisicaMock);

            apiUsuariosV2Repository.Setup(m => 
                    m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = data });

            var datosRequestCorrectos = new PersonaModelInput
            {
                IdPais = idPais,
                IdTipoDocumento = idTipoDocumento,
                NumeroDocumento = nroDocumento
            };

            var sut = CrearPersonasService(apiPersonasRepository.Object, apiUsuariosV2Repository.Object);

            // Act
            var resultado = await sut.ObtenerPersonaFisicaAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Fact]
        public void PerfilCuandoObtenerPerfilAsyncThrowsUnaException()
        {
            // Arrange
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            apiPersonasRepository.Setup(m => 
                    m.ObtenerPersonaAsync(PersonaFisicaIncorrectaNroDocumento, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Personas.FirstOrDefault(x => x.id == PersonaFisicaIncorrectaId));

            var datosRequestCorrectos = new PersonaModelInput
            {
                IdPais = PersonaFisicaIncorrectaIdPais,
                IdTipoDocumento = PersonaFisicaIncorrectaIdTipoDocumento,
                NumeroDocumento = PersonaFisicaIncorrectaNroDocumento
            };

            var sut = CrearPersonasService(apiPersonasRepository.Object);

            // Act
            var resultado = sut.ObtenerPersonaFisicaAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.Should().NotBeNull();
            resultado.Status.Should().Be(TaskStatus.Faulted);
            resultado.Exception?.InnerException?.Message.Should().Be(MessageConstants.PersonaFisicaInexistente(PersonaFisicaIncorrectaId));
        }
    }
}
