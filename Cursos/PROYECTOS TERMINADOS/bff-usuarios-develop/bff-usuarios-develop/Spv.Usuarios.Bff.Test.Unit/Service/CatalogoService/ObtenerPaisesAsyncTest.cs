using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Bff.Common.Dtos.Client.CatalogoClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.CatalogoService
{
    public class ObtenerPaisesAsyncTest
    {
        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static Bff.Service.CatalogoService CrearCatalogoService(IApiCatalogoRepository apiCatalogoRepository)
        {
            var loggerMock = new Mock<ILogger<Bff.Service.CatalogoService>>();

            return new Bff.Service.CatalogoService(
                loggerMock.Object,
                apiCatalogoRepository);
        }

        [Fact]
        public async Task PaisesOk()
        {
            // Arrange
            var apiCatalogoRepository = new Mock<IApiCatalogoRepository>();

            var paisesMock = new List<ApiCatalogoPaisesModelOutput>
            {
                new ApiCatalogoPaisesModelOutput
                {
                    codigo = 1,
                    descripcion = "Pais 1"
                },
                new ApiCatalogoPaisesModelOutput
                {
                    codigo = 2,
                    descripcion = "Pais 2"
                }
            };

            var sut = CrearCatalogoService(apiCatalogoRepository.Object);

            apiCatalogoRepository.Setup(m => m.ObtenerPaisesAsync()).ReturnsAsync(paisesMock);

            // Act
            var resultado = await sut.ObtenerPaisesAsync(Headers.ToRequestBody(new PaisesModelInput()));

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);

            var payload = resultado.Match(result => result.Payload, clientError => new List<PaisModelOutput>(),
                serverError => new List<PaisModelOutput>());

            payload.Count.Should().Be(2);
        }

        [Fact]
        public async Task PaisesError()
        {
            // Arrange
            var apiCatalogoRepository = new Mock<IApiCatalogoRepository>();

            var sut = CrearCatalogoService(apiCatalogoRepository.Object);

            // Act
            var resultado = await sut.ObtenerPaisesAsync(Headers.ToRequestBody(new PaisesModelInput()));

            // Assert
            resultado.IsOk.Should().Be(false);
            resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = resultado.Match(a => a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            message.Should().Be(ErrorCode.PaisesInexistentes.ErrorDescription);
        }

        [Fact]
        public async Task PaisesDevuelveException()
        {
            // Arrange
            var apiCatalogoRepository = new Mock<IApiCatalogoRepository>();

            var sut = CrearCatalogoService(apiCatalogoRepository.Object);

            apiCatalogoRepository.Setup(m => m.ObtenerPaisesAsync())
                .Throws(new Exception("Test Exception"));

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.ObtenerPaisesAsync(Headers.ToRequestBody(new PaisesModelInput())));

            // Assert
            resultado.Message.Should().Be("Test Exception");
        }
    }
}
