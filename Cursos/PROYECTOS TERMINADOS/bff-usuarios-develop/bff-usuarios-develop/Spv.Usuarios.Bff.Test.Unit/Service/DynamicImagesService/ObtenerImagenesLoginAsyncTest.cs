using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.DynamicImagesService
{
    public class ObtenerImagenesLoginAsyncTest
    {
        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static Bff.Service.DynamicImagesService CrearUsuarioService(
            IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2
        )
        {
            var loggerMock = new Mock<ILogger<Bff.Service.DynamicImagesService>>();

            DateTime dataTime = new DateTime();
            TimeSpan ts = new TimeSpan(0, 5, 0);
            dataTime = dataTime.Date + ts;
            var result = new DateTimeOffset(dataTime);
            var apiUsuariosHelperMock = new Mock<IApiUsuariosHelper>();
            apiUsuariosHelperMock
              .Setup(m => m.ExpirationCacheImagenesLogin())
              .Returns(result);

            var memoryCacheMock = new Mock<IMemoryCache>();

            return new Bff.Service.DynamicImagesService(
                loggerMock.Object,
                apiUsuariosRepositoryV2,
                apiUsuariosHelperMock.Object,
                memoryCacheMock.Object
            );
        }

        [Fact]
        public async Task ObtenerImagenesLoginAsyncThrowException()
        {
            // Arrange
            var apiUsuarioRepositoryV2 = new Mock<IApiUsuariosRepositoryV2>();
            apiUsuarioRepositoryV2.Setup(m =>
              m.ObtenerImagenesLoginAsync())
             .Throws(new Exception("Excepción no controlada"));

            var sut = CrearUsuarioService(apiUsuarioRepositoryV2.Object);

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.ObtenerImagenesLoginAsync(Headers.ToRequestBody(string.Empty))
            );

            // Assert
            resultado.Message.Should().Be("Excepción no controlada");
        }
    }
}
