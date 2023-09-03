using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using Correlate.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Common.Constants;

namespace Spv.Usuarios.Api.Clients
{
    /// <summary>
    /// HttpClientService Extension for Service Injection
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HttpClientServiceExtension
    {
        /// <summary>
        /// Main Http Configuration Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void AddHttpClientConfiguration(this IServiceCollection services, IConfiguration config)
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            services.AddHttpClient(ExternalServicesNames.ApiPersonas, c =>
                {
                    c.BaseAddress = new Uri(config.GetValue<string>($"{ExternalServicesNames.ApiPersonas}:Url"));
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                    c.DefaultRequestHeaders.Add(HeaderNames.IbmClientId,
                        config.GetValue<string>("ApiUsuarios:IbmClientId"));
                    c.DefaultRequestHeaders.Add(HeaderNames.ContentTypeOptions,
                        config.GetValue<string>("ApiUsuarios:ContentTypeOptions"));
                })
                .CorrelateRequests()
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new HttpClientHandler
                    {
                        AllowAutoRedirect = false,
                        AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                    };
                    handler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, errors) => true;
                    return handler;
                })
                .AddPolicyHandler(PollyPolicyBuilder.BuildRetryPolicy());

            services.AddHttpClient(ExternalServicesNames.Sso, c =>
                    c.BaseAddress = new Uri(config.GetValue<string>($"{ExternalServicesNames.Sso}:AuthUrl"))
                ).CorrelateRequests()
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new HttpClientHandler
                    {
                        AllowAutoRedirect = false,
                        AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                    };
                    handler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, errors) => true;
                    return handler;
                })
                .AddPolicyHandler(PollyPolicyBuilder.BuildRetryPolicy());

                services.AddHttpClient(ExternalServicesNames.BTA, c =>
                {
                    c.BaseAddress = new Uri(config.GetValue<string>($"{ExternalServicesNames.BTA}:BtaBasePath"));
                })
                .CorrelateRequests()
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new HttpClientHandler
                    {
                        AllowAutoRedirect = false,
                        AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                    };
                    handler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, errors) => true;
                    return handler;
                })
                .AddPolicyHandler(PollyPolicyBuilder.BuildRetryPolicy());
        }
    }
}
