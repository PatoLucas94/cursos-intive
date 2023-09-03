using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Spv.Usuarios.Common.Configurations;
using Spv.Usuarios.Common.Constants;

namespace Spv.Usuarios.Api.Handlers
{
    /// <summary>
    /// Handler for Header Propagation
    /// </summary>
    public class HeaderPropagationMessageHandler : DelegatingHandler
    {
        private readonly HeaderPropagationOptions _options;
        private readonly IHttpContextAccessor _contextAccessor;
        /// <summary>
        /// HeaderPropagationMessageHandler
        /// </summary>
        /// <param name="options"></param>
        /// <param name="contextAccessor"></param>
        public HeaderPropagationMessageHandler(HeaderPropagationOptions options, IHttpContextAccessor contextAccessor)
        {
            _options = options;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (_contextAccessor.HttpContext != null)
            {
                foreach (var headerName in _options.HeaderNames)
                {
                    // Get the incoming header value
                    var headerValue = _contextAccessor.HttpContext.Request.Headers[headerName];
                    if (StringValues.IsNullOrEmpty(headerValue))
                    {
                        continue;
                    }

                    if (headerName.Equals(HeaderNames.ChannelHeaderName))
                    {
                        var channelHeaderValuesInUppercase = headerValue.ToArray().Select(value => value.ToUpperInvariant()).ToList();

                        request.Headers.TryAddWithoutValidation(headerName, channelHeaderValuesInUppercase);
                    }
                    else
                    {
                        request.Headers.TryAddWithoutValidation(headerName, (string[])headerValue);
                    }
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
