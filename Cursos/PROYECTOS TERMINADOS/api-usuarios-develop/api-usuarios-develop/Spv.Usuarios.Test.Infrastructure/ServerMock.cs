using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Api;
using Spv.Usuarios.DataAccess.EntityFramework;

namespace Spv.Usuarios.Test.Infrastructure
{
    public sealed class ServerMock : IDisposable
    {
        public HttpClient HttpClient { get; }
        public TestServer TestServer { get; }
        public IWebHostBuilder WebHostBuilder { get; }
        public string UrlHostMock { get; set; }
        public string PortMock { get; set; }
        public IConfigurationRoot Configuration { get; private set; }
        public GenericDbContext GenericDbCtx { get; }
        public GenericDbContextV2 GenericDbV2Ctx { get; }

        public ServerMock()
        {
            WebHostBuilder = CreateWebHostBuilder();
            TestServer = new TestServer(WebHostBuilder);
            HttpClient = TestServer.CreateClient();

            using var scope = TestServer.Host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                GenericDbCtx = services.GetRequiredService<GenericDbContext>();
                GenericDbV2Ctx = services.GetRequiredService<GenericDbContextV2>();
                DbInitializer.Initialize(GenericDbCtx);
                DbInitializerV2.Initialize(GenericDbV2Ctx);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private IWebHostBuilder CreateWebHostBuilder()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.IntegrationTest.json", optional: true, reloadOnChange: true)
                .Build();

            var hostBuilder = WebHost.CreateDefaultBuilder()
                .UseConfiguration(Configuration)
                .UseStartup<Startup>();

            //Override services that was defined in startup class
            hostBuilder.ConfigureTestServices(services =>
            {
                var descriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<GenericDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                descriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<GenericDbContextV2>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<GenericDbContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "GenericDbContextDB"));

                services.AddDbContext<GenericDbContextV2>(options =>
                    options.UseInMemoryDatabase(databaseName: "GenericDbContextV2DB"));
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
    }
}
