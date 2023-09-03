using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Input;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.ClaveService
{
    public class ValidarClaveCanalesAsyncTest
    {
        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static Bff.Service.ClaveService CrearUsuarioService(IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2, IApiNotificacionesRepository apiNotificacionesRepository)
        {
            var loggerMock = new Mock<ILogger<Bff.Service.ClaveService>>();

            return new Bff.Service.ClaveService(
                loggerMock.Object,
                apiUsuariosRepositoryV2,
                new Mock<IApiPersonasRepository>().Object,
                apiNotificacionesRepository);
        }

        [Fact]
        public async Task ValidarOk()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            apiUsuarioRepositoryV2.Setup(m => m.ValidarClaveCanalesAsync(It.IsAny<ApiUsuariosValidacionClaveCanalesModelInput>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object, apiNotificacionesRepository.Object);

            // Act
            var resultado = await sut.ValidarClaveCanalesAsync(Headers.ToRequestBody(new ValidacionClaveCanalesModelInput()));

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status202Accepted);
        }

        [Fact]
        public void ValidarError()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            var apiNotificacionesRepository = new Mock<IApiNotificacionesRepository>();

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba", "")
                }
            };

            var json = JsonConvert.SerializeObject(response);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            apiUsuarioRepositoryV2.Setup(m => m.ValidarClaveCanalesAsync(It.IsAny<ApiUsuariosValidacionClaveCanalesModelInput>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = data });

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object, apiNotificacionesRepository.Object);

            // Act
            var resultado = sut.ValidarClaveCanalesAsync(Headers.ToRequestBody(new ValidacionClaveCanalesModelInput()));

            // Assert
            resultado.Result.IsOk.Should().Be(false);
            resultado.Result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            resultado.Exception?.InnerException?.Message.Should().Be(ClaveServiceEvents.ExceptionCallingValidacionClaveCanales.Name);
        }
    }
}
