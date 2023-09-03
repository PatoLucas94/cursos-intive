using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Api.ViewModels.ClaveCanalesController.CommonClaveCanales.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Errors;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.ClaveCanalesController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ValidacionIntegrationTest : ControllerIntegrationTest
    {
        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostAutenticacion(
                _uriBase,
                new ValidacionModelRequest
                {
                    IdTipoDocumento = 1,
                    ClaveCanales = "clavecanales",
                    NumeroDocumento = "nroDocumento"
                })
        };

        private static ServiceRequest PostAutenticacion(Uri uriBase, ValidacionModelRequest validacionModelRequest)
        {
            var uri = new Uri(uriBase, ApiUris.ClaveCanalesValidacionV2);

            return ServiceRequest.Post(uri.AbsoluteUri, validacionModelRequest);
        }

        private readonly Uri _uriBase;

        public ValidacionIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task Validacion(
            int idTipoDocumento,
            string claveCanales,
            string nroDocumento,
            HttpStatusCode httpStatusCode,
            string mensaje = null
        )
        {
            // Arrange
            var autenticacionModelRequestV2 = new ValidacionModelRequest
            {
                IdTipoDocumento = idTipoDocumento,
                NumeroDocumento = nroDocumento,
                ClaveCanales = claveCanales
            };

            var request = PostAutenticacion(_uriBase, autenticacionModelRequestV2);

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
        }

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
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
                    "11111111",
                    "6789",
                    HttpStatusCode.Unauthorized,
                    ErrorCode.ClaveDeCanalesIncorrecta.ErrorDescription
                }, // Clave Invalida Sin Bloqueo Clave
                new object[]
                {
                    80,
                    "11111111",
                    "101112",
                    HttpStatusCode.Unauthorized,
                    ErrorCode.ClaveDeCanalesBloqueada.ErrorDescription
                }, // Clave Invalida Con Bloqueo Clave
                new object[]
                {
                    80,
                    "11111111",
                    "131415",
                    HttpStatusCode.Unauthorized,
                    ErrorCode.ClaveDeCanalesExpirada.ErrorDescription
                }, // Clave expirada
                new object[]
                {
                    80,
                    "11111111",
                    "192021",
                    HttpStatusCode.Unauthorized,
                    ErrorCode.ClaveDeCanalesBloqueada.ErrorDescription
                }, // Clave valida pero con mas de 3 intentos
                new object[]
                {
                    80,
                    "11111111",
                    "161718",
                    HttpStatusCode.Accepted
                }, // Clave correcta
            };
    }
}