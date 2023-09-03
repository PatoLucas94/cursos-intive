using System;
using System.Diagnostics;

namespace Spv.Usuarios.Test.Infrastructure
{
    public sealed class ServerFixture : IDisposable
    {
        public ServerMock HttpServer { get; }
        public WireMockHelper WireMock { get; set; }

        public ServerFixture()
        {
            Debug.Write("ServerFixture Constructor - Se ejecuta una sola vez antes de la ejecución de los test.");

            HttpServer = new ServerMock();
            WireMock = new WireMockHelper(HttpServer.UrlHostMock, HttpServer.PortMock);
        }

        public void Dispose()
        {
            Debug.Write("Disposes only once per test.");
            WireMock?.Stop();
            WireMock?.Dispose();
            HttpServer?.Dispose();
        }
    }
}
