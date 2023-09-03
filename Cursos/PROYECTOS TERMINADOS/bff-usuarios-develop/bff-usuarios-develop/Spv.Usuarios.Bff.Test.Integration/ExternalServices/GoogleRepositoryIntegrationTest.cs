using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Bff.Common.Dtos.Client.GoogleClient.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class GoogleRepositoryIntegrationTest
    {
        private readonly IApiGoogleRepository _apiGoogleRepository;
        public WireMockHelper WireMockHelper { get; set; }

        public GoogleRepositoryIntegrationTest(ServerFixture server)
        {
            var apiGoogleRepository = server.HttpServer.TestServer.Services.GetRequiredService<IApiGoogleRepository>();

            _apiGoogleRepository = apiGoogleRepository;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task ValidacionOkAsync()
        {
            // Arrange
            var path = $"{ApiGoogleUris.ValidarTokenCaptchaV3()}";

            var responseGoogle = new ApiGoogleValidarTokenCaptchaV3ModelOutput
            {
                success = true,
                score = 0.6,
                action = "action",
                challenge_ts = DateTime.MinValue,
                hostname = "hostname"
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("ValidacionOkAsync")
                .RespondWith(WireMockHelper.RespondWithAccepted(responseGoogle));

            // Act
            var result = await _apiGoogleRepository.ReCaptchaV3ValidarTokenAsync("oiaspdfgu8hpqu43bjasdlk3j21b5i4b");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Accepted);

            await using var responseStream = await result.Content.ReadAsStreamAsync();
            var validationOutput =
                await JsonSerializer.DeserializeAsync<ApiGoogleValidarTokenCaptchaV3ModelOutput>(responseStream);

            validationOutput.success.Should().BeTrue();
            validationOutput.score.Should().Be(responseGoogle.score);
            validationOutput.action.Should().Be(responseGoogle.action);
            validationOutput.challenge_ts.Should().Be(responseGoogle.challenge_ts);
            validationOutput.hostname.Should().Be(responseGoogle.hostname);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidacionErrorAsync()
        {
            // Arrange
            var path = $"{ApiGoogleUris.ValidarTokenCaptchaV3()}";

            var responseGoogle = new ApiGoogleValidarTokenCaptchaV3ModelOutput
            {
                success = false,
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("ValidacionErrorAsync")
                .RespondWith(WireMockHelper.RespondWithAccepted(responseGoogle));

            // Act
            var result = await _apiGoogleRepository.ReCaptchaV3ValidarTokenAsync("oiaspdfgu8hpqu43bjasdlk3j21b5i4b");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Accepted);

            await using var responseStream = await result.Content.ReadAsStreamAsync();
            var validationOutput =
                await JsonSerializer.DeserializeAsync<ApiGoogleValidarTokenCaptchaV3ModelOutput>(responseStream);

            validationOutput.success.Should().BeFalse();

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
