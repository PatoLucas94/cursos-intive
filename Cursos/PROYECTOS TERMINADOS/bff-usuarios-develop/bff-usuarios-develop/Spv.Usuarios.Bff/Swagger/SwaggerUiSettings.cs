using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Spv.Usuarios.Bff.Common.Constants;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Spv.Usuarios.Bff.Swagger
{
    /// <summary>
    /// SwaggerUiSettings Class
    /// </summary>
    public static class SwaggerUiSettings
    {
        /// <summary>
        /// Configuracion de la UI de swagger
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="options"></param>
        public static void SwaggerOptionUi(this IApiVersionDescriptionProvider provider, SwaggerUIOptions options)
        {
            //Crea un endpoint swagger (definicion) para cada versión de API descubierta
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/{AppConstants.SwaggerUrl}/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant()
                );
            }

            //Colapsa la seccion Models cuando se renderiza la UI
            options.DefaultModelsExpandDepth(0);

            //Establece el path de inicio de la UI de swagger
            options.RoutePrefix = AppConstants.SwaggerUrl;
        }
    }
}
