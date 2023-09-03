using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Test.Infrastructure;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers
{
    public abstract class ControllerIntegrationTest : ControllerTest
    {
        private readonly ServerFixture _server;

        protected ControllerIntegrationTest(ServerFixture server)
        {
            _server = server;
            _server.HttpServer.ClearCache();
        }

        protected async Task<HttpResponseMessage> SendAsync(
            ServiceRequest request,
            string requestId = "dae34444a44d4e4f",
            Dictionary<string, string> headers = null
        )
        {
            var client = _server.HttpServer.HttpClient;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, requestId);

            if (headers == null)
                return await request.SendToAsync(client);

            foreach (var item in headers)
                client.DefaultRequestHeaders.Add(item.Key, item.Value);

            return await request.SendToAsync(client);
        }
    }
}
