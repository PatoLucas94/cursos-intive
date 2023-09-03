using System;
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
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Test.Unit.Service.Helpers;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.UsuarioService
{
    public class RecuperarUsuarioAsyncTest
    {
        private static Bff.Service.UsuariosService CrearUsuarioService(
            IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2,
            IApiPersonasRepository apiPersonasRepository,
            IApiNotificacionesRepository apiNotificacionesRepository
        )
        {
            var loggerMock = new Mock<ILogger<Bff.Service.UsuariosService>>();

            return new Bff.Service.UsuariosService(
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

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        [Fact]
        public async Task RecuperarUsuarioAsyncOk()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var persona = new ApiPersonasFiltroModelOutput
            {
                id = 12334444,
                numero_documento = "12331231",
                tipo_documento = 1
            };

            var listaPersona = new List<ApiPersonasFiltroModelOutput>();
            listaPersona.Add(persona);

            apiPersonasRepository
                .Setup(m => m.ObtenerPersonaFiltroAsync(It.IsAny<string>()))
                .ReturnsAsync(listaPersona);

            apiPersonasRepository
                .Setup(m => m.ObtenerInfoPersonaFisicaAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApiPersonasFisicaInfoModelOutput());

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

            var result = new HttpResponseMessage
            {
                Content = new StringContent(
                    "{\"id_persona\":15338601,\"migrado\":false,\"usuario\":\"cperani\",\"id_estado_usuario\":3}")
            };

            apiUsuarioRepositoryV2
                .Setup(m => m.ActualizarPersonIdAsync(It.IsAny<ApiUsuariosActualizarPersonIdModelInput>()))
                .ReturnsAsync(result);

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(result);

            apiNotificacionesRepository
                .Setup(m => m.EnviarEmailAsync(It.IsAny<ApiNotificacionesEnviarEmailModelInput>()))
                .ReturnsAsync(result);

            var datosRequestCorrectos = new RecuperarUsuarioModelInput
            {
                NumeroDocumento = "12331231"
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object);

            // Act
            var resultado = await sut.RecuperarUsuarioAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task RecuperarUsuarioAsyncAmbigua()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var persona = new ApiPersonasFiltroModelOutput
            {
                id = 12334444,
                numero_documento = "12331231",
                tipo_documento = 1
            };

            var persona2 = new ApiPersonasFiltroModelOutput
            {
                id = 12334443,
                numero_documento = "12331231",
                tipo_documento = 4
            };

            var listaPersona = new List<ApiPersonasFiltroModelOutput>();
            listaPersona.Add(persona);
            listaPersona.Add(persona2);

            apiPersonasRepository
                .Setup(m => m.ObtenerPersonaFiltroAsync(It.IsAny<string>()))
                .ReturnsAsync(listaPersona);

            var personaFisica = new ApiPersonasFisicaInfoModelOutput();

            apiPersonasRepository
                .Setup(m => m.ObtenerInfoPersonaFisicaAsync(It.IsAny<string>()))
                .ReturnsAsync(personaFisica);

            var result = new HttpResponseMessage
            {
                Content = new StringContent(
                    "{\"id_persona\":15338601,\"migrado\":false,\"usuario\":\"cperani\",\"id_estado_usuario\":3}")
            };

            apiUsuarioRepositoryV2
                .Setup(m => m.ObtenerPerfilAsync(It.IsAny<long>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound });

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(result);

            apiUsuarioRepositoryV2
                .Setup(m => m.ActualizarPersonIdAsync(It.IsAny<ApiUsuariosActualizarPersonIdModelInput>()))
                .ReturnsAsync(result);

            apiNotificacionesRepository
                .Setup(m => m.EnviarEmailAsync(It.IsAny<ApiNotificacionesEnviarEmailModelInput>()))
                .ReturnsAsync(result);

            var datosRequestCorrectos = new RecuperarUsuarioModelInput
            {
                NumeroDocumento = "12331231"
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object);

            // Act
            var resultado = await sut.RecuperarUsuarioAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(false);
            resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task RecuperarUsuarioAsyncPersonaDesambiguada()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var persona = new ApiPersonasFiltroModelOutput
            {
                id = 12334444,
                numero_documento = "12331231",
                tipo_documento = 1,
                pais_documento = 80
            };

            var persona2 = new ApiPersonasFiltroModelOutput
            {
                id = 12334443,
                numero_documento = "12331231",
                tipo_documento = 4,
                pais_documento = 80
            };

            var listaPersona = new List<ApiPersonasFiltroModelOutput>();
            listaPersona.Add(persona);
            listaPersona.Add(persona2);

            apiPersonasRepository
                .Setup(m => m.ObtenerPersonaFiltroAsync(It.IsAny<string>()))
                .ReturnsAsync(listaPersona);

            apiPersonasRepository
                .Setup(m => m.ObtenerInfoPersonaFisicaAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApiPersonasFisicaInfoModelOutput());

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

            var result = new HttpResponseMessage
            {
                Content = new StringContent(
                    "{\"id_persona\":15338601,\"migrado\":false,\"usuario\":\"cperani\",\"id_estado_usuario\":3}")
            };

            apiUsuarioRepositoryV2
                .Setup(m => m.ActualizarPersonIdAsync(It.IsAny<ApiUsuariosActualizarPersonIdModelInput>()))
                .ReturnsAsync(result);

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(result);

            apiNotificacionesRepository
                .Setup(m => m.EnviarEmailAsync(It.IsAny<ApiNotificacionesEnviarEmailModelInput>()))
                .ReturnsAsync(result);

            var datosRequestCorrectos = new RecuperarUsuarioModelInput
            {
                NumeroDocumento = "12331231",
                TipoDocumento = 4,
                IdPais = 80
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object);

            // Act
            var resultado = await sut.RecuperarUsuarioAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task RecuperarUsuarioAsyncPersonaInexistente()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var listaPersona = new List<ApiPersonasFiltroModelOutput>();

            apiPersonasRepository
                .Setup(m => m.ObtenerPersonaFiltroAsync(It.IsAny<string>()))
                .ReturnsAsync(listaPersona);

            var personaFisica = new ApiPersonasFisicaInfoModelOutput
            {
                emails = new List<Email>
                {
                    new Email
                    {
                        confiable = true,
                        direccion = "mail@mail.com",
                        principal = true
                    }
                }
            };

            apiPersonasRepository
                .Setup(m => m.ObtenerInfoPersonaFisicaAsync(It.IsAny<string>()))
                .ReturnsAsync(personaFisica);

            var result = new HttpResponseMessage
            {
                Content = new StringContent(
                    "{\"id_persona\":15338601,\"migrado\":false,\"usuario\":\"cperani\",\"id_estado_usuario\":3}")
            };

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(result);

            apiNotificacionesRepository
                .Setup(m => m.EnviarEmailAsync(It.IsAny<ApiNotificacionesEnviarEmailModelInput>()))
                .ReturnsAsync(result);

            var datosRequestCorrectos = new RecuperarUsuarioModelInput
            {
                NumeroDocumento = "12331231"
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object);

            // Act
            var resultado = await sut.RecuperarUsuarioAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(false);
            resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task RecuperarUsuarioAsyncPersonaNoDesambiguada()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var persona = new ApiPersonasFiltroModelOutput
            {
                id = 12334444,
                numero_documento = "12331231",
                tipo_documento = 1,
                pais_documento = 80
            };

            var persona2 = new ApiPersonasFiltroModelOutput
            {
                id = 12334443,
                numero_documento = "12331231",
                tipo_documento = 4,
                pais_documento = 80
            };

            var listaPersona = new List<ApiPersonasFiltroModelOutput>();
            listaPersona.Add(persona);
            listaPersona.Add(persona2);

            apiPersonasRepository
                .Setup(m => m.ObtenerPersonaFiltroAsync(It.IsAny<string>()))
                .ReturnsAsync(listaPersona);

            var personaFisica = new ApiPersonasFisicaInfoModelOutput
            {
                emails = new List<Email>
                {
                    new Email
                    {
                        confiable = true,
                        direccion = "mail@mail.com",
                        principal = true
                    }
                }
            };

            apiPersonasRepository
                .Setup(m => m.ObtenerInfoPersonaFisicaAsync(It.IsAny<string>()))
                .ReturnsAsync(personaFisica);

            var result = new HttpResponseMessage
            {
                Content = new StringContent(
                    "{\"id_persona\":15338601,\"migrado\":false,\"usuario\":\"cperani\",\"id_estado_usuario\":3}")
            };

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(result);

            apiNotificacionesRepository
                .Setup(m => m.EnviarEmailAsync(It.IsAny<ApiNotificacionesEnviarEmailModelInput>()))
                .ReturnsAsync(result);

            var datosRequestCorrectos = new RecuperarUsuarioModelInput
            {
                NumeroDocumento = "12331231",
                TipoDocumento = 5,
                IdPais = 80
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object);

            // Act
            var resultado = await sut.RecuperarUsuarioAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(false);
            resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task RecuperarUsuarioAsyncThrowException()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var persona = new ApiPersonasFiltroModelOutput
            {
                id = 12334444,
                numero_documento = "12331231",
                tipo_documento = 1
            };

            var listaPersona = new List<ApiPersonasFiltroModelOutput>();
            listaPersona.Add(persona);

            apiPersonasRepository
                .Setup(m => m.ObtenerPersonaFiltroAsync(It.IsAny<string>()))
                .Throws(new Exception("Excepción no controlada"));

            var datosRequestCorrectos = new RecuperarUsuarioModelInput
            {
                NumeroDocumento = "12331231"
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object);


            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.RecuperarUsuarioAsync(Headers.ToRequestBody(datosRequestCorrectos)));

            // Assert
            resultado.Message.Should().Be("Excepción no controlada");
        }

        [Fact]
        public async Task RecuperarUsuarioAsyncEmailEmpty()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var persona = new ApiPersonasFiltroModelOutput
            {
                id = 12334444,
                numero_documento = "12331231",
                tipo_documento = 1
            };

            var listaPersona = new List<ApiPersonasFiltroModelOutput>();
            listaPersona.Add(persona);

            apiPersonasRepository
                .Setup(m => m.ObtenerPersonaFiltroAsync(It.IsAny<string>()))
                .ReturnsAsync(listaPersona);

            apiPersonasRepository
                .Setup(m => m.ObtenerInfoPersonaFisicaAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApiPersonasFisicaInfoModelOutput());

            var perfil = new ApiUsuariosPerfilModelOutputV2
            {
                nro_documento = "12331231",
                tipo_documento = 1
            };

            var jsonPerfil = JsonConvert.SerializeObject(perfil);
            var dataPerfil = new StringContent(jsonPerfil, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2
                .Setup(m => m.ObtenerPerfilAsync(It.IsAny<long>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = dataPerfil });

            var result = new HttpResponseMessage
            {
                Content = new StringContent(
                    "{\"id_persona\":15338601,\"migrado\":false,\"usuario\":\"cperani\",\"id_estado_usuario\":3}")
            };

            apiUsuarioRepositoryV2
                .Setup(m => m.ActualizarPersonIdAsync(It.IsAny<ApiUsuariosActualizarPersonIdModelInput>()))
                .ReturnsAsync(result);

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(result);

            apiNotificacionesRepository
                .Setup(m => m.EnviarEmailAsync(It.IsAny<ApiNotificacionesEnviarEmailModelInput>()))
                .ReturnsAsync(result);

            var datosRequestCorrectos = new RecuperarUsuarioModelInput
            {
                NumeroDocumento = "12331231"
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object);

            // Act
            var resultado = await sut.RecuperarUsuarioAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
            ((IOkResponse<ValidacionExistenciaModelOutput>)resultado).Payload.EmailSemiOfuscado.Should()
                .Be(string.Empty);
        }

        [Fact]
        public async Task RecuperarUsuarioAsyncEmailUnoCaracter()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiPersonasRepository = new Mock<IApiPersonasRepository>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();
            var email = "a@hotmail.com";

            var persona = new ApiPersonasFiltroModelOutput
            {
                id = 12334444,
                numero_documento = "12331231",
                tipo_documento = 1
            };

            var listaPersona = new List<ApiPersonasFiltroModelOutput>();
            listaPersona.Add(persona);

            apiPersonasRepository
                .Setup(m => m.ObtenerPersonaFiltroAsync(It.IsAny<string>()))
                .ReturnsAsync(listaPersona);

            apiPersonasRepository
                .Setup(m => m.ObtenerInfoPersonaFisicaAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApiPersonasFisicaInfoModelOutput());

            var perfil = new ApiUsuariosPerfilModelOutputV2
            {
                nro_documento = "12331231",
                tipo_documento = 1,
                email = email
            };

            var jsonPerfil = JsonConvert.SerializeObject(perfil);
            var dataPerfil = new StringContent(jsonPerfil, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2
                .Setup(m => m.ObtenerPerfilAsync(It.IsAny<long>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = dataPerfil });

            var result = new HttpResponseMessage
            {
                Content = new StringContent(
                    "{\"id_persona\":15338601,\"migrado\":false,\"usuario\":\"cperani\",\"id_estado_usuario\":3}")
            };

            apiUsuarioRepositoryV2
                .Setup(m => m.ActualizarPersonIdAsync(It.IsAny<ApiUsuariosActualizarPersonIdModelInput>()))
                .ReturnsAsync(result);

            apiUsuarioRepositoryV2
                .Setup(m => m.ValidarExistenciaAsync(It.IsAny<ApiUsuariosValidacionExistenciaModelInput>()))
                .ReturnsAsync(result);

            apiNotificacionesRepository
                .Setup(m => m.EnviarEmailAsync(It.IsAny<ApiNotificacionesEnviarEmailModelInput>()))
                .ReturnsAsync(result);

            var datosRequestCorrectos = new RecuperarUsuarioModelInput
            {
                NumeroDocumento = "12331231"
            };

            var sut = CrearUsuarioService(
                apiUsuarioRepositoryV2.Object,
                apiPersonasRepository.Object,
                apiNotificacionesRepository.Object);

            // Act
            var resultado = await sut.RecuperarUsuarioAsync(Headers.ToRequestBody(datosRequestCorrectos));

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
            ((IOkResponse<ValidacionExistenciaModelOutput>)resultado).Payload.EmailSemiOfuscado.Should().Be(email);
        }
    }
}
