using System;
using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.Constants
{
    [ExcludeFromCodeCoverage]
    public static class HeaderNames
    {
        private static bool AreEqual(string a, string b) =>
            a?.Equals(b, StringComparison.InvariantCultureIgnoreCase) ?? b is null;

        public static bool IsOneOf(string name)
        {
            return AreEqual(ChannelHeaderName, name)
                   || AreEqual(UserHeaderName, name)
                   || AreEqual(ApplicationHeaderName, name)
                   || AreEqual(RequestIdHeaderName, name)
                   || AreEqual(GateWayHeaderName, name);
        }

        public const string XCorrelationIdName = "X-Correlation-ID";

        public const string ChannelHeaderName = "X-Canal";

        public const string UserHeaderName = "X-Usuario";

        public const string ApplicationHeaderName = "X-Aplicacion";

        public const string RequestIdHeaderName = "X-Request-Id";

        public const string IbmClientId = "X-IBM-Client-Id";

        public const string GateWayHeaderName = "X-GateWay";

        public const string DeviceId = "X-DeviceId";
    }
}
