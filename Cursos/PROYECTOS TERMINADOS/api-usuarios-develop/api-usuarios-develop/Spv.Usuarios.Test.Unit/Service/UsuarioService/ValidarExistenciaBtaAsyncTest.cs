using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.NSBTClient;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Service;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;
using Spv.Usuarios.Test.Unit.Common.Builders;
using Spv.Usuarios.Test.Unit.Service.Helpers;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Service.UsuarioService
{
    public class ValidarExistenciaBtaAsyncTest
    {
        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                // idPais, idTipoDocumento, nroDocumento,               bool isOk | string registrado | bool claveBt | int statusCode
                new object[] { 80, 4, DocUsuarioInexistente, true, BtaConstants.NO, false, StatusCodes.Status200OK },
                new object[] { 80, 4, DocUsuarioInexistenteConClaveBta, true, BtaConstants.NO, true, StatusCodes.Status200OK },
                new object[] { 80, 4, DocUsuarioV2Test1, true, BtaConstants.BUU, false, StatusCodes.Status200OK },
                new object[] { 80, 4, DocUsuarioV1Test1, true, BtaConstants.HBI, false, StatusCodes.Status200OK },
                new object[] { 80, 4, DocUsuarioV1Test2, true, BtaConstants.HBI, true, StatusCodes.Status200OK }
            };

        private const string DocUsuarioV2Test1 = "87654321";
        private const string DocUsuarioV1Test1 = "12345678";
        private const string DocUsuarioV1Test2 = "12345679";
        private const string DocUsuarioInexistente = "25896314";
        private const string DocUsuarioInexistenteConClaveBta = "84625930";

        private const long PersonId0 = 0;
        private const long PersonId1 = 1;
        private const long PersonId2 = 2;

        private static readonly Usuario UsuarioV1Test1Mock = new Usuario
        {
            UserId = 0,
            UserName = "username0",
            Password = "clave_encriptada0",
            UserStatusId = 0,
            UserData = new DatosUsuario
            {
                PersonId = PersonId0.ToString()
            },
            DocumentNumber = DocUsuarioV1Test1,
            DocumentCountryId = "80",
            DocumentTypeId = 4
        };

        private static readonly Usuario UsuarioV1Test2Mock = new Usuario
        {
            UserId = 0,
            UserName = "username1",
            Password = "clave_encriptada0",
            UserStatusId = 0,
            UserData = new DatosUsuario
            {
                PersonId = PersonId1.ToString()
            },
            DocumentNumber = DocUsuarioV1Test2,
            DocumentCountryId = "80",
            DocumentTypeId = 4
        };

        private static readonly UsuarioV2 UsuarioV2Mock = new UsuarioV2
        {
            UserId = 1,
            Username = "usernameV2",
            Password = "clave_encriptada1",
            UserStatusId = (byte)UserStatus.Active,
            LoginAttempts = 0,
            LastPasswordChange = DateTime.Now.AddDays(-10),
            DocumentNumber = DocUsuarioV2Test1,
            DocumentCountryId = 80,
            DocumentTypeId = 4,
            PersonId = PersonId2
        };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "HBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        private static UsuariosService CrearUsuarioService(
            IUsuarioRepository usuarioRepository,
            IUsuarioV2Repository usuarioV2Repository,
            IBtaRepository btaRepository = null)
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

            var personasRepository = new Mock<IPersonasRepository>();

            var datosUsuarioRepository = new Mock<IDatosUsuarioRepository>();
            var tDesRepository = new Mock<ITDesEncryption>();
            var historialClaveUsuariosV2Repository = new Mock<IHistorialClaveUsuariosV2Repository>();
            var historialUsuarioUsuariosV2Repository = new Mock<IHistorialUsuarioUsuariosV2Repository>();
            var historialClaveUsuariosRepository = new Mock<IHistorialClaveUsuariosRepository>();
            var reglaValidacionV2Repository = new Mock<IReglaValidacionV2Repository>();
            var nsbtRepository = new Mock<INsbtRepository>();

            return new UsuariosService(
                loggerMock.Object,
                usuarioRepository,
                usuarioV2Repository,
                auditoriaMock.Object,
                auditoriaLogV2Repository.Object,
                configuracionesService.Object,
                configuracionesV2Service.Object,
                encryptionMock.Object,
                helperDbServerMock.Object,
                helperDbServerMockV2.Object,
                personasRepository.Object,
                datosUsuarioRepository.Object,
                nsbtRepository.Object,
                tDesRepository.Object,
                historialClaveUsuariosV2Repository.Object,
                historialUsuarioUsuariosV2Repository.Object,
                historialClaveUsuariosRepository.Object,
                reglaValidacionV2Repository.Object,
                MapperProfile.GetAppProfile(),
                new Mock<IDistributedCache>().Object,
                btaRepository
            );
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ValidarExistenciaBta(
            int idPais,
            int idTipoDocumento,
            string nroDocumento,
            bool isOk,
            string registrado,
            bool claveBt,
            int statusCode)
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var btaRepository = new Mock<IBtaRepository>();

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        DocUsuarioV1Test1))
                .ReturnsAsync(UsuarioV1Test1Mock);

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        DocUsuarioV1Test2))
                .ReturnsAsync(UsuarioV1Test2Mock);

            usuarioV2Repository.Setup(m => m
                    .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        DocUsuarioV2Test1))
                .ReturnsAsync(UsuarioV2Mock);

            btaRepository.Setup(m => m.ObtenerToken()).ReturnsAsync(new TokenBtaModelOutput { SessionToken = "djf3248" });

            btaRepository.Setup(m => m.ObtenerPinAsync(
                DocUsuarioV1Test1,
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()
                )).ReturnsAsync(new ObtenerPinModelOutput { DatosPIN = new DatosPin {pin = null } });

            btaRepository.Setup(m => m.ObtenerPinAsync(
                DocUsuarioInexistente,
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()
                )).ReturnsAsync(new ObtenerPinModelOutput { DatosPIN = new DatosPin { pin = null } });

            btaRepository.Setup(m => m.ObtenerPinAsync(
                DocUsuarioV1Test2,
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()
            )).ReturnsAsync(new ObtenerPinModelOutput { DatosPIN = new DatosPin { pin = "2d13e55d0374fe25b5fc9b08546b3c68" } });

            btaRepository.Setup(m => m.ObtenerPinAsync(
                DocUsuarioV2Test1,
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()
            )).ReturnsAsync(new ObtenerPinModelOutput { DatosPIN = new DatosPin { pin = "" } });

            btaRepository.Setup(m => m.ObtenerPinAsync(
                DocUsuarioInexistenteConClaveBta,
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()
            )).ReturnsAsync(new ObtenerPinModelOutput { DatosPIN = new DatosPin { pin = "2d13e55d0374fe25b5fc9b08546b3c68" } });

            var datosRequestCorrectos = new ValidacionExistenciaBtaModelInput
            {
                DocumentCountryId = idPais,
                DocumentNumber = nroDocumento,
                DocumentTypeId = idTipoDocumento
            };

            var sut = CrearUsuarioService(usuarioRepository.Object, usuarioV2Repository.Object, btaRepository.Object);

            // Act
            var resultado = await sut.ValidarExistenciaBtaAsync(
                Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels()));

            var respuesta = resultado.Match(
                a => a.Payload,
                a => new ValidacionExistenciaBtaModelOutput(),
                a => new ValidacionExistenciaBtaModelOutput());

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);
            respuesta.Registrado.Should().Be(registrado);
            respuesta.ClaveBt.Should().Be(claveBt);
        }

        [Fact]
        public async Task ValidarExistenciaBtaThrowException()
        {
            // Arrange
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();

            var datosRequestCorrectos = new ValidacionExistenciaBtaModelInput
            {
                DocumentCountryId = 80,
                DocumentNumber = "12345678",
                DocumentTypeId = 4
            };

            usuarioRepository.Setup(m => m
                    .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(UsuarioV1Test1Mock);

            usuarioV2Repository.Setup(m => m
                    .ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .Throws(new Exception("Excepción no controlada"));

            var sut = CrearUsuarioService(usuarioRepository.Object, usuarioV2Repository.Object);

            // Act
            var resultado = await Assert.ThrowsAsync<Exception>(() =>
                sut.ValidarExistenciaBtaAsync(
                    Headers.ToRequestBody(datosRequestCorrectos, AllowedChannelsBuilder.CrearAllowedChannels())));

            // Assert
            resultado.Message.Should().Be("Excepción no controlada");
        }
    }
}
