using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Spv.Usuarios.Common.Configurations;
using Spv.Usuarios.Test.Infrastructure;
using Stubble.Core.Builders;
using Swashbuckle.AspNetCore.Swagger;
using Xunit;

namespace Spv.Usuarios.ApiConnect
{
    public sealed class ApiConnect : IDisposable
    {
        private readonly ServerMock _httpTestServer;
        private readonly string _apiConnectTemplatePath;
        private readonly string _apiConnectGeneratedFilePath;

        public ApiConnect()
        {
            _httpTestServer = new ServerMock();

            var configuration = _httpTestServer.TestServer.Host.Services.GetRequiredService<IOptions<ApiConnectConfigurationOptions>>();
            _apiConnectTemplatePath = configuration.Value.ApiConnectTemplatePath;
            _apiConnectGeneratedFilePath = configuration.Value.ApiConnectGeneratedFilePath;
        }

        [Fact]
        public void Should_UpdateApiConnectCorrectly()
        {
            var openApiSpecification = GenerateSwagger(OpenApiSpecVersion.OpenApi2_0);

            var fullYaml = ApplyTemplate(openApiSpecification);

            ExportToFile(fullYaml);

            File.Exists(_apiConnectGeneratedFilePath).Should().Be(true, "El archivo debe ser creado.");

            File.GetLastWriteTime(_apiConnectGeneratedFilePath).Should()
                .BeAfter(DateTime.Now.AddSeconds(-30), "El archivo debe ser creado recientemente");
        }

        [Fact]
        public void Should_ApiConnectConfigurationOptionsExists()
        {
            // Arrange - Act
            var sut = _httpTestServer.TestServer.Host.Services.GetRequiredService<IOptions<ApiConnectConfigurationOptions>>();

            // Assert
            sut.Value.ApiConnectTemplatePath.Should().NotBeNullOrEmpty();
            sut.Value.ApiConnectGeneratedFilePath.Should().NotBeNullOrEmpty();
        }

        private string ApplyTemplate(string yamlSpecification)
        {
            var stubble = new StubbleBuilder().Build();

            var template = File.ReadAllText(_apiConnectTemplatePath);

            var dataHash = new Dictionary<string, object> { { "APIC_SWAGGER_DATA", yamlSpecification } };

            var output = stubble.Render(template, dataHash);

            return output;
        }

        private void ExportToFile(string fullYaml)
        {
            if (File.Exists(_apiConnectGeneratedFilePath))
                File.Delete(_apiConnectGeneratedFilePath);

            File.WriteAllText(_apiConnectGeneratedFilePath, fullYaml);
        }

        public void Dispose()
        {
            _httpTestServer?.Dispose();
        }

        private string GenerateSwagger(OpenApiSpecVersion openApiVersion)
        {
            var openApiDocument = new List<OpenApiDocument>();

            var provider = _httpTestServer.TestServer.Host.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            var sw = (ISwaggerProvider)_httpTestServer.TestServer.Host.Services.GetService(typeof(ISwaggerProvider));

            foreach (var description in provider.ApiVersionDescriptions)
            {
                var apiVersionName = description.GroupName;

                var doc = sw.GetSwagger(apiVersionName, null, "/");

                openApiDocument.Add(doc);
            }

            var openApiDocumentMerged = new OpenApiDocument
            {
                Paths = new OpenApiPaths(), 
                Tags = new List<OpenApiTag>(), 
                Components = new OpenApiComponents()
            };

            foreach (var swaggerDocument in openApiDocument)
            {
                openApiDocumentMerged.Info = swaggerDocument.Info;

                if (swaggerDocument.Tags != null)
                {
                    foreach (var tag in swaggerDocument.Tags)
                    {
                        openApiDocumentMerged.Tags.Add(tag);
                    }
                }

                foreach (var schema in swaggerDocument.Components.Schemas)
                {
                    if(!openApiDocumentMerged.Components.Schemas.ContainsKey(schema.Key))
                    {
                        openApiDocumentMerged.Components.Schemas.Add(schema);
                    }
                }

                foreach (var path in swaggerDocument.Paths)
                {
                    openApiDocumentMerged.Paths.Add(path.Key, path.Value);
                }

                if (swaggerDocument.Components.Responses != null)
                {
                    openApiDocumentMerged.Components.Responses = new ConcurrentDictionary<string, OpenApiResponse>();

                    foreach (var response in swaggerDocument.Components.Responses)
                    {
                        openApiDocumentMerged.Components.Responses.Add(response);
                    }
                }

                if (swaggerDocument.Extensions != null)
                {
                    foreach (var extension in swaggerDocument.Extensions)
                    {
                        openApiDocumentMerged.Extensions.Add(extension.Key, extension.Value);
                    }
                }

                if (swaggerDocument.Components.Parameters != null)
                {
                    openApiDocumentMerged.Components.Parameters = new Dictionary<string, OpenApiParameter>();

                    foreach (var parameter in swaggerDocument.Components.Parameters)
                    {
                        openApiDocumentMerged.Components.Parameters.Add(parameter);
                    }
                }
            }

            return openApiDocumentMerged.SerializeAsYaml(openApiVersion);
        }
    }
}
