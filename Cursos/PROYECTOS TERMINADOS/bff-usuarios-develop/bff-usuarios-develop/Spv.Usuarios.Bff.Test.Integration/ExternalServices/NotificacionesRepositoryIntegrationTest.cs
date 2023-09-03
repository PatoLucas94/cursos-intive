using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Output;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class NotificacionesRepositoryIntegrationTest
    {
        private readonly IApiNotificacionesRepository _notificacionesRepository;

        public WireMockHelper WireMockHelper { get; set; }

        public Dictionary<string, string> ErrorHeaders = new Dictionary<string, string>
        {
            { HeaderNames.UserHeaderName , "ErrorUser" },
            { HeaderNames.ChannelHeaderName , "ErrorChannel" }
        };

        public Dictionary<string, string> SuccessHeaders = new Dictionary<string, string>
        {
            { HeaderNames.UserHeaderName , "SuccessUser" },
            { HeaderNames.ChannelHeaderName , "SuccessChannel" }
        };

        public NotificacionesRepositoryIntegrationTest(ServerFixture server)
        {
            var notificacionesRepository = server.HttpServer.TestServer.Services.GetRequiredService<IApiNotificacionesRepository>();

            _notificacionesRepository = notificacionesRepository;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task CrearYEnviarTokenOkAsync()
        {
            // Arrange
            var path = $"{ApiNotificacionesUris.CrearYEnviarToken()}";

            var identificador = Guid.NewGuid().ToString();

            var expectedResult = new ApiNotificacionesCrearYEnviarTokenModelOutput
            {
                id_notificacion = 1,
                id_persona = 1,
                identificador = identificador,
                estado = ApiNotificacionesHelper.CreacionTokenSmsEstadoEnviado,
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Notificaciones-CrearYEnviarToken-Ok")
                .RespondWith(WireMockHelper.Json(expectedResult));

            // Act
            var response = await _notificacionesRepository.CrearYEnviarTokenAsync(
                            new ApiNotificacionesCrearYEnviarTokenModelInput());

            var responseStream = await response.Content.ReadAsStreamAsync();
            var crearYEnviarTokenOutput = await JsonSerializer.DeserializeAsync<ApiNotificacionesCrearYEnviarTokenModelOutput>(responseStream);

            // Assert
            crearYEnviarTokenOutput.id_notificacion.Should().Be(1);
            crearYEnviarTokenOutput.id_persona.Should().Be(1);
            crearYEnviarTokenOutput.identificador.Should().Be(identificador);
            crearYEnviarTokenOutput.estado.Should().Be(ApiNotificacionesHelper.CreacionTokenSmsEstadoEnviado);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task CrearYEnviarTokenErrorAsync()
        {
            // Arrange
            var path = $"{ApiNotificacionesUris.CrearYEnviarToken()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Notificaciones-CrearYEnviarToken-Error")
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            // Act
            var result = await _notificacionesRepository.CrearYEnviarTokenAsync(
                            new ApiNotificacionesCrearYEnviarTokenModelInput());

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarTokenOkAsync()
        {
            // Arrange
            var path = $"{ApiNotificacionesUris.ValidarToken()}";

            var expectedResult = new ApiNotificacionesValidarTokenModelOutput
            {
                estado = ApiNotificacionesHelper.ValidacionTokenSmsEstadoUtilizado
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Notificaciones-ValidarToken-Ok")
                .RespondWith(WireMockHelper.Json(expectedResult));

            // Act
            var response = await _notificacionesRepository.ValidarTokenAsync(
                            new ApiNotificacionesValidarTokenModelInput());

            var responseStream = await response.Content.ReadAsStreamAsync();
            var validarTokenOutput = await JsonSerializer.DeserializeAsync<ApiNotificacionesCrearYEnviarTokenModelOutput>(responseStream);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            validarTokenOutput.estado.Should().Be(ApiNotificacionesHelper.ValidacionTokenSmsEstadoUtilizado);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarTokenErrorAsync()
        {
            // Arrange
            var path = $"{ApiNotificacionesUris.ValidarToken()}";

            var timestamp = DateTime.Now;
            var errorResponse = new ApiNotificacionesErrorResponse
            {
                timestamp = timestamp.ToString(CultureInfo.InvariantCulture),
                error = "Titulo del error",
                message = "Mensaje del error",
                path = "path",
                status = 500
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Notificaciones-ValidarToken-Error")
                .RespondWith(WireMockHelper.RespondWithBadRequest(errorResponse));

            // Act
            var response = await _notificacionesRepository.ValidarTokenAsync(
                            new ApiNotificacionesValidarTokenModelInput());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task EnviarEmailOkAsync()
        {
            // Arrange
            var path = $"{ApiNotificacionesUris.EnviarEmail()}";

            var expectedResult = new ApiNotificacionesEnviarEmailModelOutput()
            {
                id = 1,
                estado = ApiNotificacionesHelper.EnvioEmailEstadoEnviado
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Notificaciones-EnviarEmail-Ok")
                .RespondWith(WireMockHelper.Json(expectedResult));

            // Act
            var response = await _notificacionesRepository.EnviarEmailAsync(
                new ApiNotificacionesEnviarEmailModelInput());

            var responseStream = await response.Content.ReadAsStreamAsync();
            var enviarEmailOutput = await JsonSerializer.DeserializeAsync<ApiNotificacionesEnviarEmailModelOutput>(responseStream);

            // Assert
            enviarEmailOutput.id.Should().Be(1);
            enviarEmailOutput.estado.Should().Be(ApiNotificacionesHelper.EnvioEmailEstadoEnviado);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task EnviarEmailErrorAsync()
        {
            // Arrange
            var path = $"{ApiNotificacionesUris.EnviarEmail()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("Notificaciones-EnviarEmail-Error")
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            // Act
            var result = await _notificacionesRepository.EnviarEmailAsync(
                new ApiNotificacionesEnviarEmailModelInput());

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
