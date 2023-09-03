using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Api.ViewModels.DynamicImagesController.Input;
using Spv.Usuarios.Api.ViewModels.DynamicImagesController.Output;
using Spv.Usuarios.Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Api.Controllers.v2._0
{
    /// <summary>
    /// DynamicImagesController
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route(RouteRoot + "/dynamicImages")]
    public class DynamicImagesController : ApiControllerBase<IDynamicImagesService>
    {
        private readonly IAllowedChannels _allowedChannels;
        private const string RouteRoot = "v{version:apiVersion}";

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="dynamicImagesService"></param>
        /// <param name="logger"></param>
        /// <param name="allowedChannels"></param>
        public DynamicImagesController(
            IDynamicImagesService dynamicImagesService,
            ILogger<IDynamicImagesService> logger,
            IAllowedChannels allowedChannels
        ) : base(dynamicImagesService, logger)
        {
            _allowedChannels = allowedChannels;
        }

        /// <summary>
        /// ObtenerImagesLogin
        /// </summary>
        /// <returns></returns>
        [HttpGet("obtener-images-login")]
        [SwaggerOperation(Summary = "Obtener imagenes para login", Tags = new[] { "DynamicImages" })]
        [ProducesResponseType(typeof(ImageLoginModelResponse), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ObtenerImagesLogin([FromQuery] ApiHeaders apiHeaders) => await ExecuteAsync(
            Service.ObtenerImagesLoginAsync,
            ImageLoginModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
            ImageLoginModelResponse.FromAsync
        );
    }
}
