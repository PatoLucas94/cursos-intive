using System;
using System.Collections.Generic;
using System.Linq;
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
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Output;
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
    public class CambiarCredencialesAsyncTest
    {
        private const string PersonIdOk = "1";
        private const string PersonIdConflictCredencialesYaUtilizadas = "2";
        private const string PersonIdConflictUsuarioYaUtilizado = "3";
        private const string PersonIdConflictClaveYaUtilizada = "4";
        private const string PersonIdNotFound = "5";

        private const string NewUserName = "UserTest01";
        private const string NewPassword = "1423";

        private const string CredencialesYaUtilizadasCode = "CREDYU";
        private const string ClaveYaUtilizadaCode = "CREDYU";
        private const string UsuarioYaUtilizadoCode = "CREDYU";

        private static readonly List<ApiUsuariosErrorResponse> Conflicts = new List<ApiUsuariosErrorResponse>
        {
            new ApiUsuariosErrorResponse
            {
                Detalle = CredencialesYaUtilizadasCode,
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError(CredencialesYaUtilizadasCode, "", "", "Error de prueba", "")
                }
            },
            new ApiUsuariosErrorResponse
            {
                Detalle = ClaveYaUtilizadaCode,
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError(ClaveYaUtilizadaCode, "", "", "Error de prueba", "")
                }
            },
            new ApiUsuariosErrorResponse
            {
                Detalle = UsuarioYaUtilizadoCode,
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError(UsuarioYaUtilizadoCode, "", "", "Error de prueba", "")
                }
            },
            new ApiUsuariosErrorResponse
            {
                Detalle = "NotFound",
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NotFound", "", "", "Error de prueba", "")
                }
            }
        };

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { PersonIdOk, true, StatusCodes.Status200OK },
                new object[] { PersonIdConflictCredencialesYaUtilizadas, false, StatusCodes.Status409Conflict },
                new object[] { PersonIdConflictUsuarioYaUtilizado, false, StatusCodes.Status409Conflict },
                new object[] { PersonIdConflictClaveYaUtilizada, false, StatusCodes.Status409Conflict },
                new object[] { PersonIdNotFound, false, StatusCodes.Status404NotFound }
            };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
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

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task CambioDeCredencialesAsync(string personId, bool isOk, int statusCode)
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            apiUsuarioRepositoryV2
                .Setup(
                    m => m.ValidarClaveCanalesAsync(
                        It.IsAny<ApiUsuariosValidacionClaveCanalesModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            apiUsuarioRepositoryV2
                .Setup(
                    m => m.CambiarCredencialesAsync(
                        It.Is<ApiUsuariosCambioDeCredencialesModelInput>(
                            p =>
                                p.id_persona == PersonIdOk &&
                                p.nueva_clave == NewPassword &&
                                p.nueva_clave == NewPassword
                        )))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            apiUsuarioRepositoryV2
                .Setup(
                    m => m.CambiarCredencialesAsync(
                        It.Is<ApiUsuariosCambioDeCredencialesModelInput>(
                            p =>
                                p.id_persona == PersonIdNotFound &&
                                p.nueva_clave == NewPassword &&
                                p.nueva_clave == NewPassword
                        )))
                .ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Content = new StringContent(
                            JsonConvert.SerializeObject(Conflicts.First(c => c.Detalle == "NotFound")),
                            Encoding.UTF8,
                            "application/json"
                        )
                    });

            apiUsuarioRepositoryV2
                .Setup(
                    m => m.CambiarCredencialesAsync(
                        It.Is<ApiUsuariosCambioDeCredencialesModelInput>(
                            p =>
                                p.id_persona == PersonIdConflictCredencialesYaUtilizadas &&
                                p.nueva_clave == NewPassword &&
                                p.nueva_clave == NewPassword
                        )))
                .ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Conflict,
                        Content = new StringContent(
                            JsonConvert.SerializeObject(
                                Conflicts.First(c => c.Detalle == CredencialesYaUtilizadasCode)
                            ),
                            Encoding.UTF8,
                            "application/json"
                        )
                    });

            apiUsuarioRepositoryV2
                .Setup(
                    m => m.CambiarCredencialesAsync(
                        It.Is<ApiUsuariosCambioDeCredencialesModelInput>(
                            p =>
                                p.id_persona == PersonIdConflictUsuarioYaUtilizado &&
                                p.nueva_clave == NewPassword &&
                                p.nueva_clave == NewPassword
                        )))
                .ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Conflict,
                        Content = new StringContent(
                            JsonConvert.SerializeObject(Conflicts.First(c => c.Detalle == UsuarioYaUtilizadoCode)),
                            Encoding.UTF8,
                            "application/json"
                        )
                    });

            apiUsuarioRepositoryV2
                .Setup(
                    m => m.CambiarCredencialesAsync(
                        It.Is<ApiUsuariosCambioDeCredencialesModelInput>(
                            p =>
                                p.id_persona == PersonIdConflictClaveYaUtilizada &&
                                p.nueva_clave == NewPassword &&
                                p.nueva_clave == NewPassword
                        )))
                .ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Conflict,
                        Content = new StringContent(
                            JsonConvert.SerializeObject(Conflicts.First(c => c.Detalle == ClaveYaUtilizadaCode)),
                            Encoding.UTF8,
                            "application/json"
                        )
                    });

            var perfil = new ApiUsuariosPerfilModelOutputV2
            {
                nro_documento = "12331231",
                tipo_documento = 1,
                nombre = "Ricardo",
                email = "mail@mail.com"
            };

            var jsonPerfil = JsonConvert.SerializeObject(perfil);
            var dataPerfil = new StringContent(jsonPerfil, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2
                .Setup(m => m.ObtenerPerfilAsync(It.IsAny<long>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = dataPerfil });

            apiUsuarioRepositoryV2
                .Setup(m => m.InhabilitarClaveCanalesAsync(
                    It.IsAny<ApiUsuariosInhabilitacionClaveCanalesModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var result = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"id_persona\":15338601,\"migrado\":false,\"usuario\":\"cperani\",\"id_estado_usuario\":3}")
            };

            apiNotificacionesRepository
                .Setup(m => m.EnviarEmailAsync(
                    It.IsAny<ApiNotificacionesEnviarEmailModelInput>()))
                .ReturnsAsync(result);

            var datosRequestCorrectos = new CambioDeCredencialesModelInput
            {
                NewUsername = NewUserName,
                NewPassword = NewPassword,
                PersonId = personId,
                ChannelKey = "12345678"
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object
            );

            // Act
            var resultado = await sut.ModificarCredencialesAsync(
                Headers.ToRequestBody(datosRequestCorrectos)
            );

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Fact]
        public async Task CambioDeCredencialesAsyncThrowsException()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            apiUsuarioRepositoryV2
                .Setup(
                    m => m.ValidarClaveCanalesAsync(
                        It.IsAny<ApiUsuariosValidacionClaveCanalesModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            apiUsuarioRepositoryV2
                .Setup(
                    m => m.CambiarCredencialesAsync(
                        It.Is<ApiUsuariosCambioDeCredencialesModelInput>(
                            p =>
                                p.id_persona == PersonIdOk &&
                                p.nueva_clave == NewPassword &&
                                p.nueva_clave == NewPassword
                        )))
                .Throws(new Exception("Test Exception"));

            var perfil = new ApiUsuariosPerfilModelOutputV2
            {
                nro_documento = "12331231",
                tipo_documento = 1,
                nombre = "Ricardo",
                email = "mail@mail.com"
            };

            var jsonPerfil = JsonConvert.SerializeObject(perfil);
            var dataPerfil = new StringContent(jsonPerfil, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2
                .Setup(m => m.ObtenerPerfilAsync(It.IsAny<long>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = dataPerfil });

            var datosRequestCorrectos = new CambioDeCredencialesModelInput
            {
                NewUsername = NewUserName,
                NewPassword = NewPassword,
                PersonId = PersonIdOk,
                ChannelKey = "12345678"
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object
            );

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.ModificarCredencialesAsync(Headers.ToRequestBody(datosRequestCorrectos))
            );

            // Assert
            resultado.Message.Should().Be("Test Exception");
        }

        [Fact]
        public async Task CambioDeCredencialesAsyncSinClaveCanalesOk()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            apiUsuarioRepositoryV2
                .Setup(
                    m => m.ValidarClaveCanalesAsync(
                        It.IsAny<ApiUsuariosValidacionClaveCanalesModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            apiUsuarioRepositoryV2
                .Setup(
                    m => m.CambiarCredencialesAsync(
                        It.Is<ApiUsuariosCambioDeCredencialesModelInput>(
                            p =>
                                p.id_persona == PersonIdOk &&
                                p.nueva_clave == NewPassword
                        )))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var perfil = new ApiUsuariosPerfilModelOutputV2
            {
                nro_documento = "12331231",
                tipo_documento = 1,
                nombre = "Ricardo",
                email = "mail@mail.com"
            };

            var jsonPerfil = JsonConvert.SerializeObject(perfil);
            var dataPerfil = new StringContent(jsonPerfil, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2
                .Setup(m => m.ObtenerPerfilAsync(It.IsAny<long>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = dataPerfil });

            var apiNotificacionesEnviarEmailModelOutput = new ApiNotificacionesEnviarEmailModelOutput
            {
                id = 1,
                estado = "PENDIENTE_ENVIO"
            };

            var json = JsonConvert.SerializeObject(apiNotificacionesEnviarEmailModelOutput);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            apiNotificacionesRepository
                .Setup(m => m.EnviarEmailAsync(It.IsAny<ApiNotificacionesEnviarEmailModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = data });

            var datosRequestCorrectos = new CambioDeCredencialesModelInput
            {
                NewUsername = NewUserName,
                NewPassword = NewPassword,
                PersonId = PersonIdOk
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object
            );

            // Act
            var resultado = await sut.ModificarCredencialesAsync(
                Headers.ToRequestBody(datosRequestCorrectos)
            );

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
            apiNotificacionesRepository.Verify(x =>
                    x.EnviarEmailAsync(It.IsAny<ApiNotificacionesEnviarEmailModelInput>()), Times.Once()
            );
        }
    }
}
