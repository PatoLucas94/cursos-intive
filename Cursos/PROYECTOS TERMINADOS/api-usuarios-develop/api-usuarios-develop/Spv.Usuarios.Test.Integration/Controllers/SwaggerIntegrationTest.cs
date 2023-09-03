using FluentAssertions;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Controllers
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class SwaggerIntegrationTest
    {
        private readonly ServerFixture _server;

        public SwaggerIntegrationTest(ServerFixture server)
        {
            _server = server;
        }

        [Fact]
        public void SwaggerTest()
        {
            var httpResponseMessage = _server
                .HttpServer
                .HttpClient
                .GetAsync("/swagger/v1/swagger.json")
                .ConfigureAwait(false);

            httpResponseMessage.Should().NotBeNull();
        }
    }
}
