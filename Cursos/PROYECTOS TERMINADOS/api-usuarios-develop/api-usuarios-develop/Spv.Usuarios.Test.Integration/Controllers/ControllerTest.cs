using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Spv.Usuarios.Test.Integration.Utils;

namespace Spv.Usuarios.Test.Integration.Controllers
{
    public class ServiceRequest
    {
        private readonly HttpMethod _httpMethod;
        private readonly string _uri;
        private readonly object _body;

        private ServiceRequest(HttpMethod httpMethod, string uri, object body)
        {
            _httpMethod = httpMethod;
            _uri = uri;
            _body = body;
        }

        public static ServiceRequest Get(string uri)
        {
            return new ServiceRequest(HttpMethod.Get, uri, null);
        }

        public static ServiceRequest Post(string uri, object body = null)
        {
            return new ServiceRequest(HttpMethod.Post, uri, body);
        }

        public static ServiceRequest Patch(string uri, object body = null) =>
            new ServiceRequest(HttpMethod.Patch, uri, body);

        public async Task<HttpResponseMessage> SendToAsync(HttpClient client)
        {
            var request = new HttpRequestMessage(_httpMethod, _uri);
            if (_body != null)
            {
                request.Content = new JsonContent(_body);
            }

            return await client.SendAsync(request);
        }

        public override string ToString()
        {
            return $"{_httpMethod} {_uri}";
        }
    }

    public abstract class ControllerTest : TestIntegracion
    {
        protected abstract IEnumerable<ServiceRequest> AllRequests { get; }
    }
}