using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.GoogleClient.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.ReCaptchaController.CommonCaptcha.Input;
using Spv.Usuarios.Bff.ViewModels.ReCaptchaController.CommonCaptcha.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.ReCaptchaController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ValidarTokenIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        public WireMockHelper WireMockHelper { get; set; }

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest PostValidarToken(Uri uriBase, ReCaptchaValidacionModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.ValidacionTokenCaptcha());

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public ValidarTokenIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ValidarAsync(
            string tokenInput, 
            string actionInput,
            HttpStatusCode expectedResponseCodeFromGoogle,
            ApiGoogleValidarTokenCaptchaV3ModelOutput expectedResponseFromGoogle,
            HttpStatusCode expectedResponseCodeFromApi,
            ErrorCode expectedErrorCode = null)
        {
            // Arrange
            var validarTokenUri = ApiGoogleUris.ValidarTokenCaptchaV3();

            var requestBody = new ReCaptchaValidacionModelRequest
            {
                Token = tokenInput,
                Action = actionInput
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(
                    validarTokenUri, 
                    new Dictionary<string, string> { { HeaderNames.RequestIdHeaderName, tokenInput } }))
                .WithTitle("ValidarAsync-ValidarTokenCaptchaV3")
                .RespondWith(WireMockHelper.RespondWithStatusCode(expectedResponseCodeFromGoogle, expectedResponseFromGoogle));

            var request = PostValidarToken(_uriBase, requestBody);
            
            // Act
            var sut = await SendAsync(request, tokenInput);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(expectedResponseCodeFromApi);

            if(expectedResponseCodeFromApi == HttpStatusCode.OK)
            {
                var response = await sut.Content.ReadAsAsync<ReCaptchaValidacionModelResponse>();

                response.Score.Should().Be(expectedResponseFromGoogle.score);
                response.Success.Should().Be(expectedResponseFromGoogle.success);
            }

            if (expectedResponseCodeFromApi == HttpStatusCode.Unauthorized)
            {
                var error = await sut.Content.ReadAsAsync<ErrorDetailModel>();

                error.Errors.First().Code.Should().Be(expectedErrorCode?.Code);
                error.Errors.First().Detail.Should().Be(expectedErrorCode?.ErrorDescription);
            }

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        public const string ValidAction = "ValidAction";
        public const string InValidAction = "InValidAction";

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[]
                {
                    "token_usuario_no_sospechoso",
                    ValidAction,
                    HttpStatusCode.OK,
                    new ApiGoogleValidarTokenCaptchaV3ModelOutput { score = 0.9, success = true, action = ValidAction },
                    HttpStatusCode.OK
                },
                new object[]
                {
                    "token_usuario_sospechoso",
                    ValidAction,
                    HttpStatusCode.OK,
                    new ApiGoogleValidarTokenCaptchaV3ModelOutput { score = 0.3, success = true, action = ValidAction },
                    HttpStatusCode.OK
                },
                new object[]
                {
                    "token_validado_anteriormente",
                    ValidAction,
                    HttpStatusCode.OK,
                    new ApiGoogleValidarTokenCaptchaV3ModelOutput { success = false, action = ValidAction },
                    HttpStatusCode.Unauthorized,
                    ErrorCode.ReCaptchaValidacionFallida
                },
                new object[]
                {
                    "Action_Invalido",
                    InValidAction,
                    HttpStatusCode.OK,
                    new ApiGoogleValidarTokenCaptchaV3ModelOutput { success = true, action = ValidAction },
                    HttpStatusCode.Unauthorized,
                    ErrorCode.ReCaptchaActionInvalido
                },
                new object[]
                {
                    "error_al_consumir_servicio",
                    InValidAction,
                    HttpStatusCode.InternalServerError,
                    new ApiGoogleValidarTokenCaptchaV3ModelOutput(),
                    HttpStatusCode.InternalServerError,
                }
            };
    }
}
