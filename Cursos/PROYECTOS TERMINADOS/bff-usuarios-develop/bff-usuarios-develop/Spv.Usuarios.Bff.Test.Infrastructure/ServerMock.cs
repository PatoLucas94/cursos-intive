using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spv.Usuarios.Bff.Test.Infrastructure
{
    public sealed class ServerMock : IDisposable
    {
        private IWebHostBuilder WebHostBuilder { get; }
        private IConfigurationRoot Configuration { get; set; }

        private readonly MemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

        public HttpClient HttpClient { get; }
        public TestServer TestServer { get; }

        public string UrlHostMock { get; private set; }
        public string PortMock { get; private set; }

        public ServerMock()
        {
            WebHostBuilder = CreateWebHostBuilder();
            TestServer = new TestServer(WebHostBuilder);
            HttpClient = TestServer.CreateClient();
        }

        private IWebHostBuilder CreateWebHostBuilder()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.IntegrationTest.json", optional: true, reloadOnChange: true)
                .Build();

            var hostBuilder = WebHost.CreateDefaultBuilder().UseConfiguration(Configuration).UseStartup<Startup>();

            //Override services that was defined in startup class
            hostBuilder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                services.AddHangfire(config =>
                    config.UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseMemoryStorage()
                );

                services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(IMemoryCache)));
                services.AddSingleton<IMemoryCache>(_memoryCache);
            });

            FillConfiguration();

            return hostBuilder;
        }

        private void FillConfiguration()
        {
            PortMock = Configuration.GetValue<string>("PortMock");
            UrlHostMock = Configuration.GetValue<string>("UrlHostMock");
        }

        public void Dispose()
        {
            HttpClient.Dispose();
        }

        public void ClearCache() => _memoryCache.Compact(1.0);
    }
}
