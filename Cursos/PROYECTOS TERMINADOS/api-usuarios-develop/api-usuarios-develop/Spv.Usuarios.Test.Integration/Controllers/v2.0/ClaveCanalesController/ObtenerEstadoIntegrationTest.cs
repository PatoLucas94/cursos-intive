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
    public class ObtenerEstadoIntegrationTest : ControllerIntegrationTest
    {
        protected override IEnumerable<ServiceRequest> AllRequests => new[]
{
            PostAutenticacion(
                _uriBase,
                new ObtenerEstadoModelRequest
                {
                    IdTipoDocumento = 1,
                    NumeroDocumento = "nroDocumento"
                })
        };

        private static ServiceRequest PostAutenticacion(Uri uriBase, ObtenerEstadoModelRequest obtenerEstadoModelRequest)
        {
            var uri = new Uri(uriBase, ApiUris.ObtenerEstadoV2);

            return ServiceRequest.Post(uri.AbsoluteUri, obtenerEstadoModelRequest);
        }

        private readonly Uri _uriBase;

        public ObtenerEstadoIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ObtenerEstado(
            int idTipoDocumento,
            string nroDocumento,
            HttpStatusCode httpStatusCode,
            string mensaje = null
        )
        {
            // Arrange
            var autenticacionModelRequestV2 = new ObtenerEstadoModelRequest
            {
                IdTipoDocumento = idTipoDocumento,
                NumeroDocumento = nroDocumento,
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
                    new object[]
                    {
                        80,
                        "254547857",
                        HttpStatusCode.Unauthorized,
                        ErrorCode.ClaveDeCanalesInexistente.ErrorDescription
                    }, // Clave Inexistente
                    new object[]
                    {
                        80,
                        "12345",
                        HttpStatusCode.Unauthorized,
                        ErrorCode.ClaveDeCanalesInactiva.ErrorDescription
                    }, // Clave Inactiva
                    new object[]
                    {
                        80,
                        "101112",
                        HttpStatusCode.Unauthorized,
                        ErrorCode.ClaveDeCanalesBloqueada.ErrorDescription
                    }, // Clave Invalida Con Bloqueo Clave
                    new object[]
                    {
                        80,
                        "131415",
                        HttpStatusCode.Unauthorized,
                        ErrorCode.ClaveDeCanalesExpirada.ErrorDescription
                    }, // Clave expirada
                    new object[]
                    {
                        80,
                        "192021",
                        HttpStatusCode.Unauthorized,
                        ErrorCode.ClaveDeCanalesBloqueada.ErrorDescription
                    }, // Clave valida pero con mas de 3 intentos
                    new object[]
                    {
                        80,
                        "161718",
                        HttpStatusCode.Accepted
                    }, // Clave correcta
                };
    }
}
