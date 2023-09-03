using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Service;
using Spv.Usuarios.Bff.Test.Unit.Service.Helpers;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.UsuarioService
{
    public class RegistrarAsyncTest
    {
        private const string PersonIdOk = "1";
        private const int DocCountryId = 80;
        private const int DocTypeId = 4;
        private const string DocNumber = "23734745";

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private readonly ApiPersonasCreacionEmailModelOutput EmailResponse = new ApiPersonasCreacionEmailModelOutput
        {
            canal_creacion = string.Empty,
            canal_modificacion = string.Empty,
            cargo_interlocutor = string.Empty,
            confiable = true,
            dado_de_baja = true,
            direccion = string.Empty,
            etiquetas = null,
            fecha_creacion = string.Empty,
            fecha_modificacion = string.Empty,
            id = 1,
            nombre_interlocutor = string.Empty,
            origen_contacto = string.Empty,
            principal = true,
            score = 1,
            status_code = HttpStatusCode.OK,
            ultima_verificacion_positiva = null,
            usuario_creacion = string.Empty,
            usuario_modificacion = string.Empty
        };

        private readonly ApiPersonasCreacionTelefonoModelOutput TelefonoResponse =
            new ApiPersonasCreacionTelefonoModelOutput
            {
                canal_creacion = string.Empty,
                canal_modificacion = string.Empty,
                cargo_interlocutor = string.Empty,
                confiable = true,
                dado_de_baja = true,
                etiquetas = null,
                fecha_creacion = string.Empty,
                fecha_modificacion = string.Empty,
                id = 1,
                nombre_interlocutor = string.Empty,
                origen_contacto = string.Empty,
                principal = true,
                score = 1,
                ultima_verificacion_positiva = null,
                usuario_creacion = string.Empty,
                usuario_modificacion = string.Empty,
                codigo_area = 1,
                compania = string.Empty,
                numero = string.Empty,
                pais = 1,
                ddi = string.Empty,
                ddn = string.Empty,
                doble_factor = true,
                es_geografico = true,
                fecha_alta_no_llame = string.Empty,
                fecha_baja_no_llame = string.Empty,
                interno = 1,
                normalizado = true,
                no_llame = string.Empty,
                numero_local = 1,
                prefijo_telefonico_pais = 1,
                tipo_telefono = string.Empty,
                status_code = HttpStatusCode.OK
            };

        private readonly RegistracionModelInput DatosRequestCorrectos = new RegistracionModelInput
        {
            PersonId = PersonIdOk,
            DocumentCountryId = DocCountryId,
            DocumentTypeId = DocTypeId,
            DocumentNumber = DocNumber,
            UserName = "rcbertoldo",
            Password = "Info1212",
            Email = "email@email.com",
            Phone = "3511112233",
            SmsValidated = true,
            ChannelKey = "12345678"
        };

        private static UsuariosService CrearUsuarioService(
            IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2,
            IApiPersonasRepository apiPersonasRepository,
            IApiNotificacionesRepository apiNotificacionesRepository
        )
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();

            return new UsuariosService(
                loggerMock.Object,
                new Mock<IApiUsuariosRepository>().Object,
                apiUsuariosRepositoryV2,
                apiPersonasRepository,
                new Mock<IApiCatalogoRepository>().Object,
                apiNotificacionesRepository,
                MapperProfile.GetAppProfile(),
                new Mock<IApiScoreOperacionesRepository>().Object,
                new MemoryCache(new MemoryCacheOptions()),
                new Mock<IBackgroundJobClient>().Object
            );
        }

        [Fact]
        public async Task RegistrarAsyncOK()
        {
            // Arrange
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var perfil = new ApiUsuariosPerfilModelOutputV2
            {
                nro_documento = DocNumber,
                tipo_documento = DocTypeId,
                pais = DocCountryId
            };

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba", "")
                }
            };

            var persona = new ApiPersonasFisicaInfoModelOutput
            {
                numero_documento = "23734745",
                tipo_documento = 4,
                pais_documento = 80,
                id = 1
            };

            var json = JsonConvert.SerializeObject(response);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            apiPersonasRepository
                .Setup(m => m.ObtenerInfoPersonaFisicaAsync(It.IsAny<string>()))
                .ReturnsAsync(persona);

            var jsonPerfil = JsonConvert.SerializeObject(perfil);
            var dataPerfil = new StringContent(jsonPerfil, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2
                .Setup(m => m.ObtenerPerfilAsync(It.IsAny<long>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = dataPerfil });

            apiPersonasRepository
                .Setup(m => m.CrearEmailAsync(It.IsAny<string>(), It.IsAny<ApiPersonasCreacionEmailModelInput>()))
                .ReturnsAsync(EmailResponse);

            apiPersonasRepository
                .Setup(m => m.CrearTelefonoDobleFactorAsync(It.IsAny<string>(),
                    It.IsAny<ApiPersonasCreacionTelefonoModelInput>()))
                .ReturnsAsync(TelefonoResponse);

            apiPersonasRepository
                .Setup(m => m.VerificarTelefonoAsync(It.IsAny<long>(),
                    It.IsAny<ApiPersonasVerificacionTelefonoModelInput>()))
                .ReturnsAsync(new ApiPersonasVerificacionTelefonoModelOutput { status_code = HttpStatusCode.OK });

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = data });

            apiUsuarioRepositoryV2
                .Setup(m => m.RegistrarUsuarioV2Async(It.IsAny<ApiUsuariosRegistracionV2ModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Created });

            apiUsuarioRepositoryV2
                .Setup(m => m.InhabilitarClaveCanalesAsync(It.IsAny<ApiUsuariosInhabilitacionClaveCanalesModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarClaveCanalesAsync(It.IsAny<ApiUsuariosValidacionClaveCanalesModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var datosRequestCorrectos = new RegistracionModelInput
            {
                PersonId = PersonIdOk,
                DocumentCountryId = DocCountryId,
                DocumentTypeId = DocTypeId,
                DocumentNumber = DocNumber,
                UserName = "rcbertoldo",
                Password = "Info1212",
                Email = "email@email.com",
                Phone = "3511112233",
                SmsValidated = true,
                ChannelKey = "12345678"
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object);

            // Act
            var resultado =
                await sut.RegistrarAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Fact]
        public async Task RegistrarAsyncClaveCanales()
        {
            // Arrange
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var perfil = new ApiUsuariosPerfilModelOutputV2
            {
                nro_documento = DocNumber,
                tipo_documento = DocTypeId,
                pais = DocCountryId
            };

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba", "")
                }
            };

            var persona = new ApiPersonasFisicaInfoModelOutput
            {
                numero_documento = "23734745",
                tipo_documento = 4,
                pais_documento = 80,
                id = 1
            };

            var json = JsonConvert.SerializeObject(response);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            apiPersonasRepository
                .Setup(m => m.ObtenerInfoPersonaFisicaAsync(It.IsAny<string>()))
                .ReturnsAsync(persona);

            var jsonPerfil = JsonConvert.SerializeObject(perfil);
            var dataPerfil = new StringContent(jsonPerfil, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2
                .Setup(m => m.ObtenerPerfilAsync(It.IsAny<long>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = dataPerfil });

            apiPersonasRepository
                .Setup(m => m.CrearEmailAsync(It.IsAny<string>(), It.IsAny<ApiPersonasCreacionEmailModelInput>()))
                .ReturnsAsync(EmailResponse);

            apiPersonasRepository
                .Setup(m => m.CrearTelefonoDobleFactorAsync(It.IsAny<string>(),
                    It.IsAny<ApiPersonasCreacionTelefonoModelInput>()))
                .ReturnsAsync(TelefonoResponse);

            apiPersonasRepository
                .Setup(m => m.VerificarTelefonoAsync(It.IsAny<long>(),
                    It.IsAny<ApiPersonasVerificacionTelefonoModelInput>()))
                .ReturnsAsync(new ApiPersonasVerificacionTelefonoModelOutput { status_code = HttpStatusCode.OK });

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = data });

            apiUsuarioRepositoryV2
                .Setup(m => m.RegistrarUsuarioV2Async(It.IsAny<ApiUsuariosRegistracionV2ModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Created });

            apiUsuarioRepositoryV2
                .Setup(m => m.InhabilitarClaveCanalesAsync(It.IsAny<ApiUsuariosInhabilitacionClaveCanalesModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarClaveCanalesAsync(It.IsAny<ApiUsuariosValidacionClaveCanalesModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized });

            var datosRequestCorrectos = new RegistracionModelInput
            {
                PersonId = PersonIdOk,
                DocumentCountryId = DocCountryId,
                DocumentTypeId = DocTypeId,
                DocumentNumber = DocNumber,
                UserName = "rcbertoldo",
                Password = "Info1212",
                Email = "email@email.com",
                Phone = "3511112233",
                SmsValidated = true,
                ChannelKey = "12345678"
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object);

            // Act
            var resultado =
                await sut.RegistrarAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(false);
            resultado.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task RegistrarAsyncConflictUYU()
        {
            // Arrange
            var docNumber = "23734745";
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var persona = new ApiPersonasFisicaInfoModelOutput
            {
                numero_documento = docNumber,
                tipo_documento = 4,
                pais_documento = 80
            };

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba", "")
                }
            };

            var responseConflictUYU = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError(ErrorConstants.CodigoUsuarioYaUtilizado, "", "", "El usuario ya fue utilizado.", "")
                }
            };

            apiPersonasRepository
                .Setup(m => m.ObtenerInfoPersonaFisicaAsync(It.IsAny<string>()))
                .ReturnsAsync(persona);

            apiPersonasRepository
                .Setup(m => m.CrearEmailAsync(It.IsAny<string>(), It.IsAny<ApiPersonasCreacionEmailModelInput>()))
                .ReturnsAsync(EmailResponse);

            apiPersonasRepository
                .Setup(m => m.CrearTelefonoDobleFactorAsync(It.IsAny<string>(),
                    It.IsAny<ApiPersonasCreacionTelefonoModelInput>()))
                .ReturnsAsync(TelefonoResponse);

            apiPersonasRepository
                .Setup(m => m.VerificarTelefonoAsync(It.IsAny<long>(),
                    It.IsAny<ApiPersonasVerificacionTelefonoModelInput>()))
                .ReturnsAsync(new ApiPersonasVerificacionTelefonoModelOutput { status_code = HttpStatusCode.OK });

            var json = JsonConvert.SerializeObject(response);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Conflict, Content = data });

            apiUsuarioRepositoryV2
                .Setup( m => m.ValidarClaveCanalesAsync( It.IsAny<ApiUsuariosValidacionClaveCanalesModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var dataConflictUYU = new StringContent(json, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2
                .Setup(m => m.RegistrarUsuarioV2Async(It.IsAny<ApiUsuariosRegistracionV2ModelInput>()))
                .ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Conflict,
                        Content = dataConflictUYU
                    }
                );

            apiUsuarioRepositoryV2
                .Setup(m => m.InhabilitarClaveCanalesAsync(It.IsAny<ApiUsuariosInhabilitacionClaveCanalesModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object);

            // Act
            var resultado = await sut.RegistrarAsync(
                Headers.ToRequestBody(DatosRequestCorrectos)
            );

            // Assert
            resultado.IsOk.Should().Be(false);
            resultado.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        }
    }
}
