using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Serilog;
using Spv.Usuarios.Common.Configurations;
using Spv.Usuarios.Domain.Exceptions;

namespace Spv.Usuarios.Api.Swagger
{
    /// <summary>
    /// SwaggerDefaultValues
    /// </summary>
    public static class SwaggerGenSettings
    {
        /// <summary>
        /// AddSwaggerGen
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerGenForService(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    // resuelve el servicio IApiVersionDescriptionProvider
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    //Agrega un documento swagger para cada versión de API descubierta
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, services));
                    }

                    //integramos xml comments
                    if (XmlCommentsFilePath() != null)
                    {
                        options.IncludeXmlComments(XmlCommentsFilePath());
                    }

                    options.OperationFilter<SwaggerOperationFilter>();
                    options.EnableAnnotations();
                    options.CustomSchemaIds(type => type.FullName);
                });
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, IServiceCollection services)
        {
            try
            {
                var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "VERSION");

                if (File.Exists(fileName))
                {
                    var release = File.ReadLines(fileName).First();

                    var openApiInfoConfiguration = services.BuildServiceProvider()
                        .GetRequiredService<IOptions<OpenApiInfoConfigurationOptions>>();

                    var info = new OpenApiInfo
                    {
                        Title = openApiInfoConfiguration.Value.Title,
                        Version = $"{description.ApiVersion}",
                        Description = $"Release: {release}",
                        Contact = new OpenApiContact { Email = openApiInfoConfiguration.Value.Contact },
                        TermsOfService = new Uri(openApiInfoConfiguration.Value.TermsOfService)
                    };

                    //Api Connect 
                    if (!string.IsNullOrEmpty(openApiInfoConfiguration.Value.IbmName))
                    {
                        info.Extensions.Add("x-ibm-name", new OpenApiString(openApiInfoConfiguration.Value.IbmName));
                    }

                    if (description.IsDeprecated) info.Description += " Esta versión de API esta deprecated";

                    return info;
                }

                Log.Error("No se encontró el archivo de version de la api");
                throw new BusinessException("No se encontró el archivo de version de la api", 0);

            }
            catch (Exception ex)
            {
                Log.Error("Error creando configuración para OpenApi" + ex.Message);
                throw new BusinessException("Error creando configuración para OpenApi: " + ex.Message, 0);
            }
        }

        private static string XmlCommentsFilePath()
        {
            var path = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            return path;
        }
    }
}
