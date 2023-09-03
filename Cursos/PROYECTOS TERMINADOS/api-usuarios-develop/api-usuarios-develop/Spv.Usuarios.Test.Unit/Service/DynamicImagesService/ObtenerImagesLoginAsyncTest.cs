using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.DynamicImagesService
{
    public class ObtenerImagesLoginAsyncTest
    {
        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "OBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        private static Spv.Usuarios.Service.DynamicImagesService CrearDynamicImagesService(
            IDynamicImagesLoginRepository dynamicImagesLoginRepository,
            IDynamicImagesRepository dynamicImagesRepository)
        {
            return new Spv.Usuarios.Service.DynamicImagesService(
                dynamicImagesLoginRepository,
                dynamicImagesRepository
            );
        }

        [Fact]
        public async Task ObtenerImagesLoginAsync()
        {
            // Arrange
            var dynamicImagesLoginRepository = new Mock<IDynamicImagesLoginRepository>();
            var dynamicImagesRepository = new Mock<IDynamicImagesRepository>();

            var sut = CrearDynamicImagesService(
                dynamicImagesLoginRepository.Object,
                dynamicImagesRepository.Object);

            var listaImagesLogin = new List<DynamicImagesLogin>
            {
                new DynamicImagesLogin
                {
                    Id = 1,
                    Nombre = "Imagen1",
                    IdImagen = 1
                },
                new DynamicImagesLogin
                {
                    Id = 2,
                    Nombre = "Imagen2",
                    IdImagen = 2
                }
            };

            var imagen1 = new DynamicImages
            {
                Id = 1,
                Nombre = "Imagen1"
            };

            var imagen2 = new DynamicImages
            {
                Id = 2,
                Nombre = "Imagen2"
            };

            dynamicImagesLoginRepository.Setup(m => m.ObtenerImagesLogin(It.IsAny<bool>()))
           .ReturnsAsync(listaImagesLogin);

            dynamicImagesRepository.Setup(m => m.ObtenerImages(1))
           .ReturnsAsync(imagen1);

            dynamicImagesRepository.Setup(m => m.ObtenerImages(2))
           .ReturnsAsync(imagen2);

            // Act
            var resultado = await sut.ObtenerImagesLoginAsync(
                Headers.ToRequestBody(true, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            dynamicImagesLoginRepository.Verify(x => x.ObtenerImagesLogin(It.IsAny<bool>()), Times.Once);
            dynamicImagesRepository.Verify(x => x.ObtenerImages(It.IsAny<int>()), Times.Exactly(2));
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task ObtenerImagesLoginAsyncVacio()
        {
            // Arrange
            var dynamicImagesLoginRepository = new Mock<IDynamicImagesLoginRepository>();
            var dynamicImagesRepository = new Mock<IDynamicImagesRepository>();

            var sut = CrearDynamicImagesService(
                dynamicImagesLoginRepository.Object,
                dynamicImagesRepository.Object);

            var listaImagesLogin = new List<DynamicImagesLogin>
            {
            };

            var imagen1 = new DynamicImages
            {
                Id = 1,
                Nombre = "Imagen1"
            };

            var imagen2 = new DynamicImages
            {
                Id = 2,
                Nombre = "Imagen2"
            };

            dynamicImagesLoginRepository.Setup(m => m.ObtenerImagesLogin(It.IsAny<bool>()))
           .ReturnsAsync(listaImagesLogin);

            dynamicImagesRepository.Setup(m => m.ObtenerImages(1))
           .ReturnsAsync(imagen1);

            dynamicImagesRepository.Setup(m => m.ObtenerImages(2))
           .ReturnsAsync(imagen2);

            // Act
            var resultado = await sut.ObtenerImagesLoginAsync(
                Headers.ToRequestBody(true, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            dynamicImagesLoginRepository.Verify(x => x.ObtenerImagesLogin(It.IsAny<bool>()), Times.Once);
            dynamicImagesRepository.Verify(x => x.ObtenerImages(It.IsAny<int>()), Times.Never);
            resultado.IsOk.Should().Be(true);
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
