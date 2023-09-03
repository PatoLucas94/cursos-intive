using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Spv.Usuarios.Bff.Swagger
{
    /// <summary>
    /// SwaggerOperationFilter
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = context.ApiDescription.ParameterDescriptions.First(p =>
                    string.Equals(p.Name, parameter.Name, StringComparison.InvariantCultureIgnoreCase));
                var routeInfo = description.RouteInfo;

                parameter.Description ??= description.ModelMetadata?.Description;

                if (routeInfo == null)
                {
                    continue;
                }

                parameter.Required |= !routeInfo.IsOptional;
            }
        }
    }
}
