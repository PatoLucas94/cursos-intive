using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Correlate.AspNetCore;
using Correlate.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spv.Usuarios.Api.Clients;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Api.Filters;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.Middleware;
using Spv.Usuarios.Api.Swagger;
using Spv.Usuarios.Common.Configurations;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.DataAccess.EntityFramework;
using Spv.Usuarios.DataAccess.EntityFramework.V2;
using Spv.Usuarios.DataAccess.ExternalWebService;
using Spv.Usuarios.DataAccess.ExternalWebService.Helpers;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Service;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;

namespace Spv.Usuarios.Api
{
    /// <summary>
    /// Startup
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(
                    new ProducesResponseTypeAttribute(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)
                );
                options.Filters.Add(
                    new ProducesResponseTypeAttribute(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)
                );
                options.Filters.Add(
                    new ProducesResponseTypeAttribute(
                        typeof(ErrorDetailModel),
                        StatusCodes.Status500InternalServerError
                    )
                );
                options.Filters.Add(typeof(ModelStateValidateAttribute));
                options.Filters.Add(typeof(ExceptionsAttribute));
            }).AddNewtonsoftJson();

            #region Mapper

            services.AddAutoMapper(typeof(Startup).Assembly);

            #endregion

            #region Services for entity framework

            services.AddDbContext<GenericDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:HbiConnection"]));

            services.AddDbContext<GenericDbContextV2>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:CdigConnection"]));

            #endregion

            #region Correlation Ids

            services.AddCorrelate(options => options.RequestHeaders = new[] { HeaderNames.XCorrelationIdName });

            #endregion

            #region Servicio de Logs Request/Response

            services.AddTransient<RequestResponseLoggingMiddleware>();

            #endregion

            #region Servicio Errores x convencion

            services.AddTransient<IInvalidResponseBuilder, InvalidResponseBuilder>();

            #endregion

            #region Versionado de la Api

            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddApiVersioning(
                options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.ReportApiVersions = true;
                });

            #endregion

            #region Configuracion ApiBehaviorOptions

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressConsumesConstraintForFormFileParameters = true;
            });

            #endregion

            #region Propagacion de Headers

            // Global header propagation for any HttpClient that comes from HttpClientFactory
            services.AddHeaderPropagation(options =>
            {
                options.HeaderNames.Add(HeaderNames.UserHeaderName);
                options.HeaderNames.Add(HeaderNames.ChannelHeaderName);
                options.HeaderNames.Add(HeaderNames.ApplicationHeaderName);
                options.HeaderNames.Add(HeaderNames.RequestIdHeaderName);
            });

            #endregion

            #region Open Api (swagger)

            services.AddSwaggerGenForService();
            services.AddSwaggerGenNewtonsoftSupport();

            #endregion

            #region HealthChecks

            services.AddHealthChecks()
                .AddSqlServer(
                    Configuration["ConnectionStrings:HbiConnection"],
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "ready" }, name: "SQL Server Health Check");

            services.AddHealthChecksUI().AddInMemoryStorage();

            #endregion

            #region Soporte para atributos Newtonsoft

            services.AddSwaggerGenNewtonsoftSupport();

            #endregion

            #region IOption

            services.Configure<ApiConnectConfigurationOptions>(Configuration.GetSection("ApiConnect"));
            services.Configure<OpenApiInfoConfigurationOptions>(Configuration.GetSection("OpenApiInfo"));
            services.Configure<ApiUsuariosConfigurationOptions>(Configuration.GetSection("ApiUsuarios"));
            services.Configure<ValidChannelsConfigurationOptions>(Configuration.GetSection("ApiUsuarios"));
            services.Configure<ApiPersonasConfigurationsOptions>(Configuration.GetSection("ApiPersonas"));
            services.Configure<NsbtConfigurationOptions>(Configuration.GetSection("NSBT_WS"));
            services.Configure<SsoConfigurationOptions>(Configuration.GetSection("SSO"));
            services.Configure<BtaConfigurationsOptions>(Configuration.GetSection("BTA"));

            #endregion

            #region HttpClient Configurations

            services.AddHttpClientConfiguration(Configuration);

            #endregion

            #region Configuration Injection Dependency

            services.AddSingleton<Func<DateTime>>(prov => () => DateTime.Now);
            services.AddTransient<IHelperDbServer, HelperDbServer>();
            services.AddTransient<IHelperDbServerV2, HelperDbServerV2>();

            // configurations
            services.AddTransient<IEncryption, Encryption>();
            services.AddTransient<ITDesEncryption, TDesEncryption>();
            services.AddSingleton<IAllowedChannels, AllowedChannels>();
            services.AddSingleton<IApiPersonasHelper, ApiPersonasHelper>();
            services.AddSingleton<IBtaHelper, BtaHelper>();

            // repositories
            services.AddTransient<IConfiguracionRepository, ConfiguracionRepository>();
            services.AddTransient<IAuditoriaRepository, AuditoriaRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IUsuarioRegistradoRepository, UsuarioRegistradoRepository>();
            services.AddTransient<IDatosUsuarioRepository, DatosUsuarioRepository>();
            services.AddTransient<IHistorialClaveUsuariosRepository, HistorialClaveUsuariosRepository>();

            services.AddTransient<IConfiguracionV2Repository, ConfiguracionV2Repository>();
            services.AddTransient<IEstadosUsuarioV2Repository, EstadosUsuarioV2Repository>();
            services.AddTransient<IUsuarioV2Repository, UsuarioV2Repository>();
            services.AddTransient<IAuditoriaLogV2Repository, AuditoriaLogV2Repository>();
            services.AddTransient<IHistorialClaveUsuariosV2Repository, HistorialClaveUsuariosV2Repository>();
            services.AddTransient<IHistorialUsuarioUsuariosV2Repository, HistorialUsuarioUsuariosV2Repository>();
            services.AddTransient<IReglaValidacionV2Repository, ReglaValidacionV2Repository>();
            services.AddTransient<IDynamicImagesRepository, DynamicImagesRepository>();
            services.AddTransient<IDynamicImagesLoginRepository, DynamicImagesLoginRepository>();
            services.AddTransient<IBtaRepository, BtaRepository>();

            // services
            services.AddScoped<IConfiguracionesService, ConfiguracionesService>();
            services.AddScoped<IConfiguracionesV2Service, ConfiguracionesV2Service>();
            services.AddTransient<IUsuariosService, UsuariosService>();
            services.AddTransient<IClaveCanalesService, ClaveCanalesService>();
            services.AddTransient<ISsoService, SsoService>();
            services.AddTransient<IDynamicImagesService, DynamicImagesService>();

            // external services
            services.AddTransient<IPersonasRepository, PersonasRepository>();
            services.AddTransient<INsbtRepository, NsbtRepository>();
            services.AddTransient<ISsoRepository, SsoRepository>();

            #endregion

            #region DistributedSqlServerCache

            if (Configuration.GetValue<bool>("UseDistributedMemoryCache"))
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddDistributedSqlServerCache(options =>
                {
                    options.ConnectionString = Configuration["ConnectionStrings:CdigConnection"];
                    options.SchemaName = "dbo";
                    options.TableName = "Cache";
                });
            }

            #endregion
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCorrelate();

            app.UseRequestResponseLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger(c => { c.SerializeAsV2 = true; });

            app.UseSwaggerUI(provider.SwaggerOptionUi);

            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("health/ready", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("ready"),
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    },
                    ResponseWriter = (httpContext, result) =>
                    {
                        httpContext.Response.ContentType = "application/json";

                        var json = new JObject(
                            new JProperty("OverallStatus", result.Status.ToString()),
                            new JProperty("TotalChecksDuration", result.TotalDuration.ToString("c")),
                            new JProperty("DependencyHealthChecks", new JObject(result.Entries.Select(dicItem =>
                                new JProperty(dicItem.Key, new JObject(
                                    new JProperty("Status", dicItem.Value.Status.ToString()),
                                    new JProperty("Description", dicItem.Value.Description),
                                    new JProperty("Duration", dicItem.Value.Duration),
                                    new JProperty("Data", new JObject(dicItem.Value.Data.Select(
                                        p => new JProperty(p.Key, p.Value))))))))));
                        return httpContext.Response.WriteAsync(
                            json.ToString(Formatting.Indented));
                    },
                    AllowCachingResponses = false
                });

                endpoints.MapHealthChecks("/health-ui", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            // URL: HealthChecks-ui
            app.UseHealthChecksUI();

            #region Remover solo para TEST

            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<GenericDbContext>();
            context.Database.EnsureCreated();

            var contextV2 = serviceScope.ServiceProvider.GetRequiredService<GenericDbContextV2>();
            contextV2.Database.EnsureCreated();

            #endregion
        }
    }
}
