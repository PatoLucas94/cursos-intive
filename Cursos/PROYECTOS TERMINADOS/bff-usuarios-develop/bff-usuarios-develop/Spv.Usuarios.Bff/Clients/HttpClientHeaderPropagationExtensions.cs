using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Filters;

namespace Spv.Usuarios.Bff.Clients
{
    /// <summary>
    /// Extension for Header Propagation Middleware
    /// </summary>
    public static class HeaderPropagationExtensions
    {
        /// <summary>
        /// Allows Header propagation for set headers in the configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static void AddHeaderPropagation(
            this IServiceCollection services,
            Action<HeaderPropagationOptions> configure
        )
        {
            services.AddHttpContextAccessor();
            services.Configure(configure);
            services.TryAddEnumerable(
                ServiceDescriptor
                    .Singleton<IHttpMessageHandlerBuilderFilter, HeaderPropagationMessageHandlerBuilderFilter>()
            );
        }
    }
}
