using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Testing.Attributes;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.ExternalServices
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    [TestCaseOrderer("Spv.Usuarios.Common.Testing.PriorityOrderer", "Spv.Usuarios.Common")]
    public class NsbtRepositoryIntegrationTest
    {
        private readonly INsbtRepository _nsbtRepository;
        private WireMockHelper WireMockHelper { get; set; }

        public NsbtRepositoryIntegrationTest(ServerFixture server)
        {
            var nsbtRepository = server.HttpServer.TestServer.Services.GetRequiredService<INsbtRepository>();

            _nsbtRepository = nsbtRepository;
            WireMockHelper = server.WireMock;
        }

        [Fact, PriorityAttribute.TestPriority(0)]
        public async Task NsbtOkAsync()
        {
            // Arrange
            var response = "../../../ExternalServices/XmlResponses/NSBTWS/PIN_OK.xml";

            var path = $"{NsbtWsUris.Execute()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.WebService(path))
                .WithTitle("NSBT_OK")
                .RespondWith(WireMockHelper.Xml(response));

            // Act
            var result = await _nsbtRepository.GetPinAsync("080", 4, "12345678");

            // Assert
            result.Pin.Should().Be("bde05d0b5280e5ad4b088d3ce4ade517");
            result.Attempt.Should().Be(0);
            result.CountryId.Should().Be("80");
            result.IsTemporal.Should().Be("S");
            result.DocumentTypeId.Should().Be(4);
            result.DocumentNumber.Should().Be("25983770");
            result.Exists.Should().Be(false);
            result.LastLogIn.Should().Be("2008-07-03");
            result.ExpirationDate.Should().Be(new DateTime(9999, 4, 8));

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact, PriorityAttribute.TestPriority(1)]
        public async Task NsbtErrorAsync()
        {
            // Arrange
            const string response = "../../../ExternalServices/XmlResponses/NSBTWS/PIN_ERROR.xml";

            var path = $"{NsbtWsUris.Execute()}";

            if (WireMockHelper.ServiceMock != null)
            {
                WireMockHelper.ServiceMock
                    .Given(WireMockHelper.WebService(path))
                    .RespondWith(WireMockHelper.Xml(response));

                // Act
                var result = await _nsbtRepository.GetPinAsync("080", 4, "12345678");

                // Assert
                result.Should().BeNull();

                // Clean WireMock
                WireMockHelper.ServiceMock.ResetMappings();
            }
        }

        [Fact]
        public async Task LoginAttemptsOkAsync()
        {
            // Arrange
            const string response = "../../../ExternalServices/XmlResponses/NSBTWS/PIN_OK.xml";

            var path = $"{NsbtWsUris.Execute()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.WebService(path))
                .WithTitle("NSBT_ATTEMPT_OK")
                .RespondWith(WireMockHelper.Xml(response));

            // Act
            await _nsbtRepository.IncrementLoginAttemptsAsync("080", 4, "12345678", "asdfh3123jkgnf3u", 1);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task LoginAttemptsErrorAsync()
        {
            // Arrange
            const string response = "../../../ExternalServices/XmlResponses/NSBTWS/PIN_ERROR.xml";

            var path = $"{NsbtWsUris.Execute()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.WebService(path))
                .WithTitle("NSBT_ATTEMPT_ERROR")
                .RespondWith(WireMockHelper.Xml(response));

            // Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() =>
                _nsbtRepository.IncrementLoginAttemptsAsync(
                    "080",
                    4,
                    "12345678",
                    "asdfh3123jkgnf3u",
                    1)
            );

            // Assert
            ex.Message.Should().Be(MessageConstants.ClaveNumericaErrorWebService);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
