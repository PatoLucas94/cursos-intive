using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Helpers;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Service.Helpers
{
    public class ProcessExternalErrorTest
    {
        public ApiUsuariosErrorResponse externalResponse = new ApiUsuariosErrorResponse
        {
            Detalle = "Detalle de error de prueba",
            Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba1 ", ""),
                    new ApiUsuarioError("INC", "", "", "Error de prueba 2", "")
                }
        };

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound, HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.OK, HttpStatusCode.InternalServerError, MessageConstants.MensajeGenerico)]
        public async Task ProcessApiUsuariosErrorResponseAsync(HttpStatusCode status, HttpStatusCode expectedStatus, string expectedMessage = null)
        {
            // Arrange

            var json = JsonConvert.SerializeObject(externalResponse);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = new HttpResponseMessage { StatusCode = status, Content = data };

            // Act
            var result = await ProcessExternalError<ValidacionClaveCanalesModelOutput>.ProcessApiUsuariosErrorResponse(response);

            // Assert
            result.IsOk.Should().BeFalse();
            result.Match(result => result.Payload, clientError => ProcessClientError(clientError, expectedStatus), serverError => ProcessServerError(serverError, expectedMessage));
        }

        public ValidacionClaveCanalesModelOutput ProcessClientError(IClientErrorResponse<ValidacionClaveCanalesModelOutput> response, HttpStatusCode status)
        {
            response.StatusCode.Should().Be((int)status);
            response.IsOk.Should().BeFalse();
            if(status == HttpStatusCode.BadRequest)
            {
                response.CodesAndDetailsErrors.Should().NotBeNullOrEmpty();
                response.CodesAndDetailsErrors.Count.Should().Be(2);
            }
            return new ValidacionClaveCanalesModelOutput();
        }        
        
        public ValidacionClaveCanalesModelOutput ProcessServerError(IServerErrorResponse<ValidacionClaveCanalesModelOutput> response, string expectedMessage)
        {
            expectedMessage ??= "Detalle de error de prueba";

            response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            response.IsOk.Should().BeFalse();
            response.Exception.Message.Should().Be(expectedMessage);
            return new ValidacionClaveCanalesModelOutput();
        }
    }
}
