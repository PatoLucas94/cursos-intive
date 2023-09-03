using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.RhSsoService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.RhSsoService.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Test.Unit.Service.Helpers;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.RhSsoService
{
    public class AutenticacionAsyncTest
    {
        private const string DocumentNumberOk = "123456";
        private const string DocumentNumberNotFound = "654321";

        private const string UserName = "UserTest01";
        private const string Password = "1423";

        private static readonly TokenModelOutput TokenResponse = new TokenModelOutput
        {
            AccessToken = "some.random.access.token",
            ExpiresIn = 1234,
            RefreshExpiresIn = 4321,
            RefreshToken = "some.random.refresh.token"
        };

        public static IEnumerable<object[]> Datos => new List<object[]>
        {
            new object[] { DocumentNumberOk, true, StatusCodes.Status200OK }
        };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static Bff.Service.RhSsoService CrearRhSsoService(IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2)
        {
            var loggerMock = new Mock<ILogger<Bff.Service.RhSsoService>>();
            var backgroundJobClient = new Mock<IBackgroundJobClient>();

            return new Bff.Service.RhSsoService(
                loggerMock.Object,
                apiUsuariosRepositoryV2,
                MapperProfile.GetAppProfile(),
                backgroundJobClient.Object
            );
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task AutenticacionAsync(string documentNumber, bool isOk, int statusCode)
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            apiUsuarioRepositoryV2.Setup(m =>
                    m.AutenticacionAsync(
                        It.Is<ApiUsuariosAutenticacionV2ModelInput>(p =>
                            p.nro_documento == DocumentNumberOk
                        )
                    )
                )
                .ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(
                            JsonConvert.SerializeObject(TokenResponse),
                            Encoding.UTF8,
                            "application/json"
                        )
                    }
                );

            apiUsuarioRepositoryV2.Setup(m =>
                    m.AutenticacionAsync(
                        It.Is<ApiUsuariosAutenticacionV2ModelInput>(p =>
                            p.nro_documento == DocumentNumberNotFound
                        )
                    )
                )
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound });

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UserName,
                Password = Password,
                DocumentNumber = documentNumber
            };

            var sut = CrearRhSsoService(apiUsuarioRepositoryV2.Object);

            // Act
            var resultado = await sut.AutenticarAsync(
                Headers.ToRequestBody(datosRequestCorrectos)
            );

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
        }

        [Fact]
        public async Task AutenticacionAsyncThrowsException()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();

            apiUsuarioRepositoryV2.Setup(m =>
                    m.AutenticacionAsync(It.IsAny<ApiUsuariosAutenticacionV2ModelInput>())
                )
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            apiUsuarioRepositoryV2.Setup(m =>
                    m.AutenticacionAsync(
                        It.Is<ApiUsuariosAutenticacionV2ModelInput>(p =>
                            p.nro_documento == DocumentNumberOk
                        )
                    )
                )
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

            var datosRequestCorrectos = new AutenticacionModelInput
            {
                UserName = UserName,
                Password = Password,
                DocumentNumber = DocumentNumberOk
            };

            var sut = CrearRhSsoService(apiUsuarioRepositoryV2.Object);

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.AutenticarAsync(Headers.ToRequestBody(datosRequestCorrectos))
            );

            // Assert
            resultado.Message.Should().Be("Test Exception");
        }
    }
}
