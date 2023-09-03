using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace Spv.Usuarios.Bff.Clients
{
    /// <summary>
    /// Main Polly Policy Builder
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class PollyPolicyBuilderExternalApi
    {
        /// <summary>
        /// Retry Policy for general HttpClient usage
        /// </summary>
        /// <returns></returns>
        public static IAsyncPolicy<HttpResponseMessage> BuildRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, context) =>
                    {
                        Log.Logger.Error("Retry StatusCode:{StatusCode}, Url: {RequestUri}",
                            exception?.Result?.StatusCode, exception?.Result?.RequestMessage.RequestUri);
                    });
        }

        /// <summary>
        /// Retry Policy for general HttpClient usage
        /// </summary>
        /// <returns></returns>
        public static IAsyncPolicy<HttpResponseMessage> BuildRetryPolicyScoreOperaciones()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(0, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, context) =>
                    {
                        Log.Logger.Error("Retry StatusCode:{StatusCode}, Url: {RequestUri}",
                            exception?.Result?.StatusCode, exception?.Result?.RequestMessage.RequestUri);
                    });
        }
    }
}
