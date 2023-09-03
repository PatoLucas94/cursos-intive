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
    public class ObtenerTiposDocumentoAsyncTest
    {
        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XRequestId = "1"
        };

        private static Usuarios.Bff.Service.CatalogoService CrearCatalogoService(IApiCatalogoRepository apiCatalogoRepository)
        {
            var loggerMock = new Mock<ILogger<Usuarios.Bff.Service.CatalogoService>>();

            return new Usuarios.Bff.Service.CatalogoService(
                loggerMock.Object,
                apiCatalogoRepository);
        }

        [Fact]
        public async Task TiposDcumentoOk()
        {
            // Arrange
            var apiCatalogoRepository = new Mock<IApiCatalogoRepository>();

            var paisesMock = new List<ApiCatalogoTiposDocumentoModelOutput>
            {
                new ApiCatalogoTiposDocumentoModelOutput
                {
                    codigo = 1,
                    descripcion = "Tipo Doc 1",
                    tipoPersonaQueAplica = "J"
                },
                new ApiCatalogoTiposDocumentoModelOutput
                {
                    codigo = 2,
                    descripcion = "Tipo Doc 2",
                    tipoPersonaQueAplica = "F"
                },
                new ApiCatalogoTiposDocumentoModelOutput
                {
                    codigo = 3,
                    descripcion = "Tipo Doc 3",
                    tipoPersonaQueAplica = "J"
                },
                new ApiCatalogoTiposDocumentoModelOutput
                {
                    codigo = 4,
                    descripcion = "Tipo Doc 4",
                    tipoPersonaQueAplica = "F"
                }
            };

            var sut = CrearCatalogoService(apiCatalogoRepository.Object);

            apiCatalogoRepository.Setup(m => m.ObtenerTiposDocumentoAsync()).ReturnsAsync(paisesMock);

            // Act
            var resultado = await sut.ObtenerTiposDocumentoAsync(Headers.ToRequestBody(new TiposDocumentoModelInput()));

            // Assert
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);

            var payload = resultado.Match(result => result.Payload, clientError => new List<TipoDocumentoModelOutput>(), serverError => new List<TipoDocumentoModelOutput>());

            payload.Count.Should().Be(2);
        }

        [Fact]
        public async Task TiposDcumentoError()
        {
            // Arrange
            var apiCatalogoRepository = new Mock<IApiCatalogoRepository>();

            var sut = CrearCatalogoService(apiCatalogoRepository.Object);

            // Act
            var resultado = await sut.ObtenerTiposDocumentoAsync(Headers.ToRequestBody(new TiposDocumentoModelInput()));

            // Assert
            resultado.IsOk.Should().Be(false);
            resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = resultado.Match(a => a.Payload.ToString(), a => a.Message, a => a.Exception.Message);

            message.Should().Be(ErrorCode.TiposDocumentoInexistentes.ErrorDescription);
        }

        [Fact]
        public async Task TiposDcumentoDevuelveException()
        {
            // Arrange
            var apiCatalogoRepository = new Mock<IApiCatalogoRepository>();

            var sut = CrearCatalogoService(apiCatalogoRepository.Object);

            apiCatalogoRepository.Setup(m => m.ObtenerTiposDocumentoAsync()).Throws(new Exception("Test Exception"));

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() => sut.ObtenerTiposDocumentoAsync(Headers.ToRequestBody(new TiposDocumentoModelInput())));

            // Assert
            resultado.Message.Should().Be("Test Exception");
        }
    }
}
