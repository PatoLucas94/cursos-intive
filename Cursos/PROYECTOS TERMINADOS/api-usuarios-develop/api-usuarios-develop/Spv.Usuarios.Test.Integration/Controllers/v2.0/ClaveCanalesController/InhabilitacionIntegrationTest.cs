using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Api.ViewModels.ClaveCanalesController.CommonClaveCanales.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Errors;
using Spv.Usuarios.DataAccess.EntityFramework;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.ClaveCanalesController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class InhabilitacionIntegrationTest : ControllerIntegrationTest
    {
        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostInhabilitacion(
                _uriBase,
                new InhabilitacionModelRequest
                {
                    IdTipoDocumento = 1,
                    ClaveCanales = "clavecanales",
                    NumeroDocumento = "nroDocumento"
                })
        };

        private static ServiceRequest PostInhabilitacion(
            Uri uriBase,
            InhabilitacionModelRequest inhabilitacionModelRequest
        )
        {
            var uri = new Uri(uriBase, ApiUris.ClaveCanalesInhabilitacionV2);

            return ServiceRequest.Post(uri.AbsoluteUri, inhabilitacionModelRequest);
        }

        private readonly ServerFixture _server;
        private readonly Uri _uriBase;

        private readonly UsuarioRegistradoRepository _usuarioRegistradoRepository;

        public InhabilitacionIntegrationTest(ServerFixture server) : base(server)
        {
            _server = server;
            _uriBase = _server.HttpServer.TestServer.BaseAddress;

            var db = _server.HttpServer.TestServer.Services.GetRequiredService<GenericDbContext>();

            _usuarioRegistradoRepository = new UsuarioRegistradoRepository(db);
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Inhabilitacion(
            int idTipoDocumento,
            string claveCanales,
            string nroDocumento,
            HttpStatusCode httpStatusCode,
            string mensaje = null
        )
        {
            // Arrange
            var inhabilitacionModelRequestV2 = new InhabilitacionModelRequest
            {
                IdTipoDocumento = idTipoDocumento,
                NumeroDocumento = nroDocumento,
                ClaveCanales = claveCanales
            };

            var request = PostInhabilitacion(_uriBase, inhabilitacionModelRequestV2);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            if (!string.IsNullOrWhiteSpace(mensaje) && httpStatusCode == HttpStatusCode.Unauthorized)
            {
                var errorDetailModel = await sut.Content.ReadAsAsync<ErrorDetailModel>();

                var error = errorDetailModel.Errors.First();

                error.Detail.Should().Be(mensaje);
            }

            if (httpStatusCode == HttpStatusCode.OK)
            {
                var result = await _usuarioRegistradoRepository.ObtenerUsuarioRegistradoAsync(
                    idTipoDocumento,
                    nroDocumento
                );
                result.Active.Should().BeFalse();
            }
        }

        public static IEnumerable<object[]> Datos => new List<object[]>
        {
            // idTipoDocumento - claveCanales - nroDocumento - HttpStatusCode
            // Unauthorized
            new object[]
            {
                1,
                "11111111",
                "Inexistente",
                HttpStatusCode.InternalServerError,
                ErrorCode.ClaveDeCanalesInexistente.ErrorDescription
            }, // Inexistente
            new object[]
            {
                80,
                "11111111",
                "12345",
                HttpStatusCode.Unauthorized,
                ErrorCode.ClaveDeCanalesInactiva.ErrorDescription
            }, // Clave Inactiva
            new object[]
            {
                80,
                "11111110",
                "252627",
                HttpStatusCode.Unauthorized,
                ErrorCode.ClaveDeCanalesIncorrecta.ErrorDescription
            }, // Clave incorrecta
            new object[]
            {
                80,
                "11111111",
                "222324",
                HttpStatusCode.OK
            }, // Clave correcta
        };
    }
}