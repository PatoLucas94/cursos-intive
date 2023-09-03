using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Spv.Usuarios.Api.Swagger
{
    /// <summary>
    /// SwaggerUiSettings Class
    /// </summary>
    public static class SwaggerUiSettings
    {
        /// <summary>
        /// Configuración de la UI de swagger
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="options"></param>
        public static void SwaggerOptionUi(this IApiVersionDescriptionProvider provider, SwaggerUIOptions options)
        {
            options.SwaggerEndpoint($"../swagger/v2/swagger.json", "latest (v2.0)");
            options.SwaggerEndpoint($"../swagger/v1/swagger.json", "HBI legacy (v1.0)");

            //Colapsa la sección Models cuando se renderiza la UI
            options.DefaultModelsExpandDepth(0);

            //Establece el path de inicio de la UI de swagger
            options.RoutePrefix = "swagger";
        }
    }
}
