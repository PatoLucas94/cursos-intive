using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
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
    public class CambiarCredencialesAsyncTest
    {
        [Theory]
        [MemberData(nameof(Datos))]
        public async Task CambiarCredenciales(int idPersona, string nuevoUsername, string nuevaClave, bool isOk,
            int statusCode)
        {
            // Arrange
            var usuarioHbiRepository = new Mock<IUsuarioRepository>();
            var usuarioV2Repository = new Mock<IUsuarioV2Repository>();
            var auditoriaV2Repository = new Mock<IAuditoriaLogV2Repository>();
            var usuarioV2 = GetUsuarioV2Mock();
            var usuarioHbi = GetUsuarioHbiMock();

            usuarioV2Repository.Setup(m =>
                m.ObtenerUsuarioByPersonIdAsync(IdPersonaTest1)).ReturnsAsync(usuarioV2);

            usuarioHbiRepository.Setup(m =>
                m.ObtenerUsuarioByPersonIdAsync(IdPersonaTest1)).ReturnsAsync(usuarioHbi);

            auditoriaV2Repository.Setup(m =>
                m.SaveAuditLogAsync(
                    It.IsAny<int>(),
                    It.IsAny<EventTypes>(),
                    It.IsAny<EventResults>(),
                    It.IsAny<string>(),
                    It.IsAny<FechaDbServerV2>(),
                    It.IsAny<string>())).ReturnsAsync(AuditoriaLogV2Mock);

            var datosRequest = new CambioDeCredencialesModelInputV2
            {
                PersonId = idPersona,
                NewUsername = nuevoUsername,
                NewPassword = nuevaClave
            };

            var sut = CrearUsuarioService(
                usuarioV2Repository.Object,
                auditoriaV2Repository.Object,
                usuarioHbiRepository.Object);

            // Act
            var resultado = await sut.ModificarCredencialesAsync(
                Headers.ToRequestBody(
                    datosRequest,
                    AllowedChannelsBuilder.CrearAllowedChannels()));

            // Assert
            resultado.IsOk.Should().Be(isOk);
            resultado.StatusCode.Should().Be(statusCode);

            if (statusCode.Equals(StatusCodes.Status200OK))
            {
                usuarioV2.LastPasswordChange?.Date.Should().Be(DateTime.Today);
                usuarioV2.LastPasswordChange.Should().Be(usuarioHbi.LastPasswordChange);
            }
        }

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[] { IdPersonaTest2, UsuarioNuevo, ClaveNueva, false, StatusCodes.Status404NotFound },
                new object[] { IdPersonaTest1, UsuarioCorrecto, ClaveCorrecta, false, StatusCodes.Status409Conflict },
                new object[] { IdPersonaTest1, UsuarioCorrecto, ClaveNueva, false, StatusCodes.Status409Conflict },
                new object[] { IdPersonaTest1, UsuarioNuevo, ClaveCorrecta, false, StatusCodes.Status409Conflict },
                new object[] { IdPersonaTest1, UsuarioNuevo, ClaveNueva, true, StatusCodes.Status200OK }
            };

        private const long IdPersonaTest1 = 12345678;
        private const long IdPersonaTest2 = 25896314;
        private const string UsuarioCorrecto = "username";
        private const string UsuarioEncriptadoOriginal = "username_encriptado";
        private const string UsuarioNuevo = "usernameX0";
        private const string UsuarioNuevoEncriptado = "username_nuevo_encriptado";
        private const string ClaveCorrecta = "1478";
        private const string ClaveEncriptadaOriginal = "clave_encriptada";
        private const string ClaveNueva = "1423";
        private const string ClaveNuevaEncriptada = "clave_nueva_encriptada";

        private static readonly AuditoriaLogV2 AuditoriaLogV2Mock = new AuditoriaLogV2
        {
            AuditLogId = 1,
            Channel = "OBI",
            DateTime = DateTime.MinValue,
            UserId = 1,
            EventTypeId = (int)EventTypes.PasswordChange,
            EventResultId = (int)EventResults.Ok,
            ExtendedInfo = "extended_info"
        };

        private static readonly ApiHeaders Headers = new ApiHeaders
        {
            XAplicacion = "app",
            XCanal = "OBI",
            XUsuario = "user",
            XRequestId = "1"
        };

        private static UsuarioV2 GetUsuarioV2Mock()
        {
            return new UsuarioV2
            {
                UserId = 1,
                PersonId = IdPersonaTest1,
                Username = UsuarioEncriptadoOriginal,
                Password = ClaveEncriptadaOriginal,
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                LastPasswordChange = DateTime.Now.AddDays(-10),
                DocumentNumber = "87654321",
                DocumentCountryId = 80,
                DocumentTypeId = 4
            };
        }

        private static Usuario GetUsuarioHbiMock()
        {
            var usuarioV2 = GetUsuarioV2Mock();

            return new Usuario
            {
                CustomerNumber = $"0{usuarioV2.DocumentCountryId}0{usuarioV2.DocumentTypeId}{usuarioV2.DocumentNumber}",
                UserName = usuarioV2.PersonId.ToString(),
                Password = usuarioV2.Password,
                UserStatusId = usuarioV2.UserStatusId,
                Name = "UsuarioUT",
                LastName = "ApellidoUT",
                LastPasswordChange = usuarioV2.LastPasswordChange,
                CreatedDate = usuarioV2.CreatedDate,
                DocumentCountryId = $"0{usuarioV2.DocumentCountryId}",
                DocumentTypeId = usuarioV2.DocumentTypeId,
                DocumentNumber = usuarioV2.DocumentNumber,
                IsEmployee = false,
                MobileEnabled = false,
                FullControl = true,
                LoginAttempts = 0
            };
        }

        private static UsuariosService CrearUsuarioService(
            IUsuarioV2Repository usuarioV2Repository,
            IAuditoriaLogV2Repository auditoriaLogV2Repository,
            IUsuarioRepository usuarioHbiRepository = null)
        {
            var loggerMock = new Mock<ILogger<UsuariosService>>();
            var usuarioRepository = new Mock<IUsuarioRepository>();
            var configuracionesService = new Mock<IConfiguracionesService>();
            var configuracionesV2Service = new Mock<IConfiguracionesV2Service>();

            var encryptionMock = new Mock<IEncryption>();
            encryptionMock.Setup(m => m.GetHash(UsuarioCorrecto)).Returns(UsuarioEncriptadoOriginal);
            encryptionMock.Setup(m => m.GetHash(UsuarioNuevo)).Returns(UsuarioNuevoEncriptado);
            encryptionMock.Setup(m => m.GetHash(ClaveCorrecta)).Returns(ClaveEncriptadaOriginal);
            encryptionMock.Setup(m => m.GetHash(ClaveNueva)).Returns(ClaveNuevaEncriptada);

            var dateTimeNow = DateTime.Now;

            var helperDbServerMock = new Mock<IHelperDbServer>();
            helperDbServerMock.Setup(m => m.ObtenerFechaAsync()).ReturnsAsync(new FechaDbServer { Now = dateTimeNow });

            var helperDbServerMockV2 = new Mock<IHelperDbServerV2>();
            helperDbServerMockV2.Setup(m => m.ObtenerFechaAsync())
                .ReturnsAsync(new FechaDbServerV2 { Now = dateTimeNow });

            var auditoriaMock = new Mock<IAuditoriaRepository>();

            var personasRepository = new Mock<IPersonasRepository>();

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
                usuarioHbiRepository ?? usuarioRepository.Object,
                usuarioV2Repository,
                auditoriaMock.Object,
                auditoriaLogV2Repository,
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
                btaRepository.Object
            );
        }
    }
}
