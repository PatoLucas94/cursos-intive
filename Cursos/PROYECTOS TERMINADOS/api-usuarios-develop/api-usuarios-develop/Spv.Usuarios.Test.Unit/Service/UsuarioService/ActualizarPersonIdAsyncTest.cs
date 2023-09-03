using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Dtos.PersonaService.Output;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Service;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Spv.Usuarios.Test.Unit.Service.Helpers;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.UsuarioService
{
    public class ActualizarPersonIdAsyncTest
    {
        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { 80, 4, DocUsuarioTest1, true, StatusCodes.Status200OK, Times.Once() },
                new object[] { 80, 4, DocUsuarioTest3, false, StatusCodes.Status404NotFound, Times.Never() }
            };

        private const string DocUsuarioTest1 = "12345678";
        private const string DocUsuarioTest3 = "25896314";
        private const long PersonId0 = 0;

        private static readonly Usuario UsuarioMock = new Usuario
        {
            UserId = 0,
            UserName = "username0",
            Password = "clave_encriptada0",
            UserStatusId = 3,
            UserData = new DatosUsuario
            {
                PersonId = PersonId0.ToString()
            },
            DocumentNumber = DocUsuarioTest1,
            DocumentCountryId = "80",
            DocumentTypeId = 4
        };

        private static UsuariosService CrearUsuarioService(
            IUsuarioRepository usuarioRepository,
            IPersonasRepository personasRepository)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var auditoriaLogV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();
            var encryptionMock = new Mock<IEncryption>();

            var helperDbServerMock = new Mock<IHelperDbServer>();
            helperDbServerMock.Setup(m =>
                m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServer { Now = DateTime.Now });

            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();
            helperDbServerMockV2.Setup(m =>
                m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServerV2 { Now = DateTime.Now });

            var auditoriaMock = new Mock<IAuditoriaRepository>();

            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();

            var datosUsuarioRepository = new Mock<IDatosUsuarioRepository>();
            var nsbtRepository = new Mock<INsbtRepository>();
            var tDesRepository = new Mock<ITDesEncryption>();
            var historialClaveUsuariosV2Repository = new Mock<IHistorialClaveUsuariosV2Repository>();
            var historialUsuarioUsuariosV2Repository = new Mock<IHistorialUsuarioUsuariosV2Repository>();
            var historialClaveUsuariosRepository = new Mock<IHistorialClaveUsuariosRepository>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();
            var btaRepository = new Mock<IBtaRepository>();

            return new UsuariosService(
                loggerMock.Object,
                usuarioRepository,
                usuarioV2Repository.Object,
                auditoriaMock.Object,
                auditoriaLogV2Repository.Object,
                configuracionesService.Object,
                configuracionesV2Service.Object,
                encryptionMock.Object,
                helperDbServerMock.Object,
                helperDbServerMockV2.Object,
                personasRepository,
                datosUsuarioRepository.Object,
                nsbtRepository.Object,
                tDesRepository.Object,
                historialClaveUsuariosV2Repository.Object,
                historialUsuarioUsuariosV2Repository.Object,
                historialClaveUsuariosRepository.Object,
                reglaValidacionV2Repository.Object,
                MapperProfile.GetAppProfile(),
                new Mock<IDistributedCache>().Object,
                btaRepository.Object
            );
        }

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "HBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ActualizarPersonId(int idPais, int idTipoDocumento, string nroDocumento, bool isOk,
            int statusCode, Times times)
        {
            // Arrange
            var personasRepository = new Mock<IPersonasRepository>();
            var usuarioRepository = new Mock<IUsuarioRepository>();

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        DocUsuarioTest1))
                .ReturnsAsync(UsuarioMock);

            personasRepository.Setup(m => m.ObtenerPersona(nroDocumento, idTipoDocumento, idPais, null, null))
                .ReturnsAsync(new PersonaModelResponse { id = 1 });

            var datosRequestCorrectos = new ActualizarPersonIdModelInput
            {
                DocumentCountryId = idPais,
                DocumentNumber = nroDocumento,
                DocumentTypeId = idTipoDocumento
            };

            var sut = CrearUsuarioService(usuarioRepository.Object, personasRepository.Object);

            // Act
            var resultado = await sut.ActualizarPersonIdAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);

            personasRepository.Verify(x => x.ObtenerPersona(nroDocumento, idTipoDocumento, idPais, null, null), times);
        }
    }
}
