using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Input;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.ClaveService
{
    public class GenerarClaveSmsAsyncTest
    {
        // Datos Correctos
        private const int GeneracionTokenOkPersonaId = 1;
        private const string GeneracionTokenOkTelefono = "1122233344";

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static Bff.Service.ClaveService CrearClaveService(IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2, IApiNotificacionesRepository apiNotificacionesRepository)
        {
            var loggerMock = new Mock<ILogger<Bff.Service.ClaveService>>();

            return new Bff.Service.ClaveService(
                loggerMock.Object,
                apiUsuariosRepositoryV2,
                new Mock<IApiPersonasRepository>().Object,
                apiNotificacionesRepository);
        }

        [Fact]
        public async Task GenerarClaveOk()
        {
            // Arrange
            var apiUsuariosRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var bodyCorrecto = new GeneracionClaveSmsModelInput()
            {
                PersonId = GeneracionTokenOkPersonaId.ToString(),
                Telefono = GeneracionTokenOkTelefono
            };

            var expectedResultCorrecto = new ApiNotificacionesCrearYEnviarTokenModelOutput
            {
                id_notificacion = 1,
                id_persona = GeneracionTokenOkPersonaId,
                estado = ApiNotificacionesHelper.CreacionTokenSmsEstadoEnviado,
                identificador = Guid.NewGuid().ToString()
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(expectedResultCorrecto, new JsonSerializerOptions()), Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            apiNotificacionesRepository.Setup(m =>
                    m.CrearYEnviarTokenAsync(It.IsAny<ApiNotificacionesCrearYEnviarTokenModelInput>()))
                    .ReturnsAsync(expectedResponse);

            var sut = CrearClaveService(apiUsuariosRepositoryV2.Object, apiNotificacionesRepository.Object);

            // Act
            var resultado = await sut.GenerarClaveSmsAsync(Headers.ToRequestBody(bodyCorrecto));

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status202Accepted);
        }

        [Fact]
        public void GenerarClaveError()
        {
            // Arrange
            var apiUsuariosRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var errorResponse = new ApiNotificacionesErrorResponse(
                "timestamp",
                (int) HttpStatusCode.BadRequest,
                "Error de prueba",
                "Mensaje de prueba",
                "");

            var bodyCorrecto = new GeneracionClaveSmsModelInput()
            {
                PersonId = GeneracionTokenOkPersonaId.ToString(),
                Telefono = GeneracionTokenOkTelefono
            };

            var expectedErrorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions()), Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            apiNotificacionesRepository.Setup(m => m.CrearYEnviarTokenAsync(It.IsAny<ApiNotificacionesCrearYEnviarTokenModelInput>()))
                .ReturnsAsync(expectedErrorResponse);

            var sut = CrearClaveService(apiUsuariosRepositoryV2.Object, apiNotificacionesRepository.Object);

            // Act
            var resultado = sut.GenerarClaveSmsAsync(Headers.ToRequestBody(bodyCorrecto));

            // Assert
            resultado.Result.IsOk.Should().Be(false);
            resultado.Result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            resultado.Exception?.InnerException?.Message.Should().Be(ClaveServiceEvents.ExceptionCallingGeneracionClaveSms.Name);
        }
    }
}
