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
    public class ValidarClaveSmsAsyncTest
    {
        // Datos Correctos
        private const int ValidacionTokenOkPersonaId = 1;
        private const string ValidacionTokenOkIdentificador = "047cadc2-49d0-447a-9460-dc27ce67baa0";
        private const string ValidacionTokenOkClaveSms = "123456";

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
        public async Task ValidarClaveOk()
        {
            // Arrange
            var apiUsuariosRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var body = new ApiNotificacionesValidarTokenModelOutput
            {
                estado = ApiNotificacionesHelper.ValidacionTokenSmsEstadoUtilizado
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
            expectedResponse.Content = new StringContent(JsonSerializer.Serialize(body, new JsonSerializerOptions()), Encoding.UTF8, MediaTypeNames.Application.Json);

            apiNotificacionesRepository.Setup(m => m.ValidarTokenAsync(It.IsAny<ApiNotificacionesValidarTokenModelInput>()))
                .ReturnsAsync(expectedResponse);

            var sut = CrearClaveService(apiUsuariosRepositoryV2.Object, apiNotificacionesRepository.Object);

            var bodyCorrecto = new ValidacionClaveSmsModelInput()
            {
                PersonId = ValidacionTokenOkPersonaId.ToString(),
                Identificador = ValidacionTokenOkIdentificador,
                ClaveSms = ValidacionTokenOkClaveSms
            };

            // Act
            var resultado = await sut.ValidarClaveSmsAsync(Headers.ToRequestBody(bodyCorrecto));

            // Assert
            resultado.StatusCode.Should().Be(StatusCodes.Status202Accepted);
        }

        [Fact]
        public void ValidarClaveError()
        {
            // Arrange
            var apiUsuariosRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var errorBody = new ApiNotificacionesErrorResponse
            {
                error = "Error de prueba",
                message = "Mensaje de prueba",
                path = "",
                status = (int)HttpStatusCode.BadRequest,
                timestamp = "timestamp"
            };

            var expectedErrorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            expectedErrorResponse.Content = new StringContent(JsonSerializer.Serialize(errorBody, new JsonSerializerOptions()), Encoding.UTF8, MediaTypeNames.Application.Json);

            apiNotificacionesRepository.Setup(m => m.ValidarTokenAsync(It.IsAny<ApiNotificacionesValidarTokenModelInput>()))
                .ReturnsAsync(expectedErrorResponse);

            var sut = CrearClaveService(apiUsuariosRepositoryV2.Object, apiNotificacionesRepository.Object);

            var bodyCorrecto = new ValidacionClaveSmsModelInput()
            {
                PersonId = ValidacionTokenOkPersonaId.ToString(),
                Identificador = ValidacionTokenOkIdentificador,
                ClaveSms = ValidacionTokenOkClaveSms
            };

            // Act
            var resultado = sut.ValidarClaveSmsAsync(Headers.ToRequestBody(bodyCorrecto));

            // Assert
            resultado.Result.IsOk.Should().Be(false);
            resultado.Result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            resultado.Exception?.InnerException?.Message.Should().Be(ClaveServiceEvents.ExceptionCallingValidacionClaveSms.Name);
        }
    }
}
