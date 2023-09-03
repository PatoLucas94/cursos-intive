using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Api.Filters;
using Spv.Usuarios.Api.Handlers;
using Spv.Usuarios.Common.Configurations;

namespace Spv.Usuarios.Api.Clients
{
    /// <summary>
    /// Extension for Header Propagation Middleware
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HeaderPropagationExtensions
    {
        /// <summary>
        /// Allows Header propagation for set headers in the configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddHeaderPropagation(
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
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddHeaderPropagation(
            this IHttpClientBuilder builder,
            Action<HeaderPropagationOptions> configure
        )
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.Configure(configure);
            builder.AddHttpMessageHandler((sp) =>
            {
                var options = sp.GetRequiredService<IOptions<HeaderPropagationOptions>>();
                var contextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

                return new HeaderPropagationMessageHandler(options.Value, contextAccessor);
            });

            return builder;
        }
    }
}
