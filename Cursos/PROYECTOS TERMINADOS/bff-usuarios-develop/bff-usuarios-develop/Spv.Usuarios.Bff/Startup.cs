using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Correlate.AspNetCore;
using Correlate.DependencyInjection;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.Heartbeat;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spv.Usuarios.Bff.Clients;
using Spv.Usuarios.Bff.Common.Configurations;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Filters;
using Spv.Usuarios.Bff.Helpers;
using Spv.Usuarios.Bff.Middleware;
using Spv.Usuarios.Bff.Service;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.Swagger;
using Spv.Usuarios.Bff.Tasks;

namespace Spv.Usuarios.Bff
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
            IdentityModelEventSource.ShowPII = true;

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

            #region Authentication

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = Configuration["BffUsuarios:Authority"];
                    options.Audience = Configuration["BffUsuarios:Audience"];
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                    };
                });

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
            services.AddHeaderPropagation(options => { options.HeaderNames.Add(HeaderNames.RequestIdHeaderName); });

            #endregion

            #region Open Api (swagger)

            services.AddSwaggerGenForService();
            services.AddSwaggerGenNewtonsoftSupport();

            #endregion

            #region HealthChecks

            services.AddHealthChecks();

            #endregion

            #region Soporte para atributos Newtonsoft

            services.AddSwaggerGenNewtonsoftSupport();

            #endregion

            #region IOption

            services.Configure<ApiConnectConfigurationOptions>(
                Configuration.GetSection(ExternalServicesNames.ApiConnect)
            );
            services.Configure<OpenApiInfoConfigurationOptions>(
                Configuration.GetSection(ExternalServicesNames.OpenApiInfo)
            );
            services.Configure<ApiServicesHeadersConfigurationOptions>(
                Configuration.GetSection(ExternalServicesNames.ApiServices)
            );
            services.Configure<ApiPersonasConfigurationsOptions>(
                Configuration.GetSection(ExternalServicesNames.ApiPersonas)
            );
            services.Configure<ApiCatalogoConfigurationsOptions>(
                Configuration.GetSection(ExternalServicesNames.ApiCatalogo)
            );
            services.Configure<ApiUsuariosConfigurationsOptions>(
                Configuration.GetSection(ExternalServicesNames.ApiUsuarios)
            );
            services.Configure<ApiNotificacionesConfigurationsOptions>(
                Configuration.GetSection(ExternalServicesNames.ApiNotificaciones)
            );
            services.Configure<ApiTyCConfigurationsOptions>(Configuration.GetSection(ExternalServicesNames.ApiTyC));

            services.Configure<ApiSoftTokenConfigurationsOptions>(
                Configuration.GetSection(ExternalServicesNames.ApiSoftToken));

            services.Configure<ApiScoreOperacionesConfigurationsOptions>(
                Configuration.GetSection(ExternalServicesNames.ApiScoreOperaciones));

            services.Configure<ApiGoogleConfigurationOptions>(
                Configuration.GetSection(ExternalServicesNames.ApiGoogle)
            );

            services.Configure<ConfiguracionConfigurationOptions>(
                Configuration.GetSection(ExternalServicesNames.Configuraciones)
            );

            services.Configure<HangfireConfigurationOptions>(Configuration.GetSection(ExternalServicesNames.Hangfire));

            services.Configure<ApiBiometriaConfigurationsOptions>(
                Configuration.GetSection(ExternalServicesNames.ApiBiometria)
            );

            #endregion

            #region HttpClient Configurations

            services.AddHttpClientConfiguration(Configuration);

            #endregion

            #region Configuration Injection Dependency

            services.AddSingleton<Func<DateTime>>(prov => () => DateTime.Now);

            // configurations
            services.AddSingleton<IApiPersonasHelper, ApiPersonasHelper>();
            services.AddSingleton<IApiCatalogoHelper, ApiCatalogoHelper>();
            services.AddSingleton<IApiUsuariosHelper, ApiUsuariosHelper>();
            services.AddSingleton<IApiNotificacionesHelper, ApiNotificacionesHelper>();
            services.AddSingleton<IApiTyCHelper, ApiTyCHelper>();
            services.AddSingleton<IApiSoftTokenHelper, ApiSoftTokenHelper>();
            services.AddSingleton<IApiGoogleHelper, ApiGoogleHelper>();
            services.AddSingleton<IRhSsoHelper, RhSsoHelper>();
            services.AddSingleton<IApiScoreOperacionesHelper, ApiScoreOperacionesHelper>();
            services.AddSingleton<IConfiguracionHelper, ConfiguracionHelper>();
            services.AddSingleton<IApiBiometriaHelper, ApiBiometriaHelper>();

            // services
            services.AddTransient<IPersonasService, PersonasService>();
            services.AddTransient<ICatalogoService, CatalogoService>();
            services.AddTransient<IUsuarioService, UsuariosService>();
            services.AddTransient<IClaveService, ClaveService>();
            services.AddTransient<ITyCService, TyCService>();
            services.AddTransient<ISoftTokenService, SoftTokenService>();
            services.AddTransient<IReCaptchaService, ReCaptchaService>();
            services.AddTransient<IRhSsoService, RhSsoService>();
            services.AddTransient<IConfiguracionService, ConfiguracionService>();
            services.AddTransient<IDynamicImagesService, DynamicImagesService>();
            services.AddTransient<IScoreOperacionesService, ScoreOperacionesService>();
            services.AddTransient<IBiometriaService, BiometriaService>();

            // external services
            services.AddTransient<IApiPersonasRepository, ApiPersonasRepository>();
            services.AddTransient<IApiCatalogoRepository, ApiCatalogoRepository>();
            services.AddTransient<IApiUsuariosRepository, ApiUsuariosRepository>();
            services.AddTransient<IApiUsuariosRepositoryV2, ApiUsuariosRepositoryV2>();
            services.AddTransient<IApiNotificacionesRepository, ApiNotificacionesRepository>();
            services.AddTransient<IApiTyCRepository, ApiTyCRepository>();
            services.AddTransient<IApiSoftTokenRepository, ApiSoftTokenRepository>();
            services.AddTransient<IApiGoogleRepository, ApiGoogleRepository>();
            services.AddSingleton<IApiScoreOperacionesRepository, ApiScoreOperacionesRepository>();
            services.AddTransient<IApiBiometriaRepository, ApiBiometriaRepository>();

            #endregion

            #region Cors

            var configCors = Configuration["Seguridad:Origins.Cors"] ?? string.Empty;

            services.AddCors(options =>
                options.AddPolicy(
                    "obi-frontend",
                    builder => builder.WithOrigins(configCors.Split(','))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            #endregion

            #region MemoryCache

            services.AddMemoryCache();

            #endregion

            #region Hangfire

            AddHangfire(services);

            #endregion
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        /// <param name="hangfireConfiguration"></param>
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider,
            IOptions<HangfireConfigurationOptions> hangfireConfiguration
        )
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCorrelate();

            app.UseRequestResponseLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("obi-frontend");

            app.UseAuthentication();

            app.UseAuthorization();

            UseHangfireDashboard(app, hangfireConfiguration);

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
                c.RouteTemplate = $"{AppConstants.SwaggerUrl}/{{documentName}}/swagger.json";
            });

            app.UseSwaggerUI(provider.SwaggerOptionUi);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                MapHealthChecks(endpoints);
            });
        }

        private static void MapHealthChecks(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks(AppConstants.HealthUrl, new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("health"),
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
                            new JProperty("status", result.Status.ToString()),
                            new JProperty("totalChecksDuration", result.TotalDuration.ToString("c")),
                            new JProperty("version", BffHelper.LoadVersion()),
                            new JProperty(
                                "dependencyHealthChecks",
                                new JObject(result.Entries.Select(dicItem =>
                                        new JProperty(
                                            dicItem.Key,
                                            new JObject(
                                                new JProperty("status", dicItem.Value.Status.ToString()),
                                                new JProperty("description", dicItem.Value.Description),
                                                new JProperty("duration", dicItem.Value.Duration),
                                                new JProperty("data", new JObject(
                                                        dicItem.Value.Data.Select(p => new JProperty(p.Key, p.Value))
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        );

                        return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
                    },
                    AllowCachingResponses = false
                }
            );
        }

        private static void AddHangfire(IServiceCollection services)
        {
            GlobalJobFilters.Filters.Add(
                new AutomaticRetryAttribute
                {
                    Attempts = 0,
                    OnAttemptsExceeded = AttemptsExceededAction.Delete
                }
            );

            services.AddHangfire(config =>
            {
#if DEBUG
                config.UseHeartbeatPage(checkInterval: TimeSpan.FromSeconds(1));
#endif
                config.UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSQLiteStorage(
                        "hangfire",
                        new SQLiteStorageOptions
                        {
                            AutoVacuumSelected = SQLiteStorageOptions.AutoVacuum.FULL
                        }
                    );
            });

            services.AddHangfireServer();
        }

        private static void UseHangfireDashboard(
            IApplicationBuilder app,
            IOptions<HangfireConfigurationOptions> hangfireConfiguration
        )
        {
            var hangfireOptions = hangfireConfiguration.Value;

            app.UseHangfireDashboard(
                $"/{AppConstants.BasePath}jobs",
                new DashboardOptions
                {
                    Authorization = new[]
                    {
                        new BasicAuthAuthorizationFilter(
                            new BasicAuthAuthorizationFilterOptions
                            {
                                RequireSsl = false,
                                SslRedirect = false,
                                LoginCaseSensitive = true,
                                Users = new[]
                                {
                                    new BasicAuthAuthorizationUser
                                    {
                                        Login = hangfireOptions.User,
                                        PasswordClear = hangfireOptions.Password
                                    }
                                }
                            }
                        )
                    }
                }
            );

            RecurrentTasks.Run();
        }
    }
}
