using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Correlate.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Bff.Common.Constants;

namespace Spv.Usuarios.Bff.Clients
{
    /// <summary>
    /// HttpClientService Extension for Service Injection
    /// </summary>
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

            var httpClientNames = new List<string> {
                ExternalServicesNames.ApiPersonas,
                ExternalServicesNames.ApiCatalogo,
                ExternalServicesNames.ApiUsuarios,
                ExternalServicesNames.ApiNotificaciones,
                ExternalServicesNames.ApiTyC,
                ExternalServicesNames.ApiSoftToken,
                ExternalServicesNames.Configuraciones,
                ExternalServicesNames.ApiBiometria
            };

            foreach (var httpClientName in httpClientNames)
            {
                services.AddHttpClient(httpClientName, c =>
                {
                    c.BaseAddress = new Uri(config.GetValue<string>("IbmApiConnect:Host"));
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                    c.DefaultRequestHeaders.Add(HeaderNames.IbmClientId, config.GetValue<string>("IbmApiConnect:ClientId"));
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

            services.AddHttpClient(ExternalServicesNames.ApiGoogle, c =>
            {
                c.BaseAddress = new Uri(config.GetValue<string>("ApiGoogle:Host"));
                c.DefaultRequestHeaders.Add("Accept", "application/json");
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
                    .AddPolicyHandler(PollyPolicyBuilderExternalApi.BuildRetryPolicy());

            services.AddHttpClient(ExternalServicesNames.ApiScoreOperaciones, c =>
            {
                c.BaseAddress = new Uri(config.GetValue<string>("IbmApiConnect:Host"));
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add(HeaderNames.IbmClientId, config.GetValue<string>("IbmApiConnect:ClientId"));
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
            .AddPolicyHandler(PollyPolicyBuilderExternalApi.BuildRetryPolicyScoreOperaciones());
            }
    }
}
