using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Api.Handlers;
using Spv.Usuarios.Common.Configurations;

namespace Spv.Usuarios.Api.Filters
{
    /// <summary>
    /// HeaderPropagationMessageHandlerBuilderFilter class
    /// </summary>
    public class HeaderPropagationMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly HeaderPropagationOptions _options;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="contextAccessor"></param>
        public HeaderPropagationMessageHandlerBuilderFilter(IOptions<HeaderPropagationOptions> options, IHttpContextAccessor contextAccessor)
        {
            _options = options.Value;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            return builder =>
            {
                builder.AdditionalHandlers.Add(new HeaderPropagationMessageHandler(_options, _contextAccessor));
                next(builder);
            };
        }
    }
}
