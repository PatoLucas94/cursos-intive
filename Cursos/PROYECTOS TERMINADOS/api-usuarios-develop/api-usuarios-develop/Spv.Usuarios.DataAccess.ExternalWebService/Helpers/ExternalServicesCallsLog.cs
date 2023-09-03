using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.DataAccess.ExternalWebService.Helpers
{
    public static class ExternalServicesCallsLog<T>
    {
        private const string MessageFormat = "Response information from {ExternalApiName}: " +
            "StatusCode: {SatatusCode} " +
            "AbsoluteUri: {AbsoluteUri} " +
            "Method: {Method} " +
            "RequestBody: {RequestString} " +
            "ResponseBody: {ResponseString}";

        public static void LogDebug(ILogger<T> logger, string externalApiName, string statusCode, string absoluteUri, string method, string responseBody, string requestBody = null)
        {
            logger.LogDebug(MessageFormat, externalApiName, statusCode, absoluteUri, method, requestBody, responseBody);
        }
    }
}
