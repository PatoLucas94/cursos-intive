using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Utils;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Helpers;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Spv.Usuarios.Bff.ViewModels.DynamicImagesController.Input;
using Spv.Usuarios.Bff.ViewModels.DynamicImagesController.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <summary>
    /// DynamicImagesController
    /// </summary>
    [ApiController]
    [Route(RouteRoot + "dynamicImages")]
    public class DynamicImagesController : ApiControllerBase<IDynamicImagesService>
    {
        private const string RouteRoot = AppConstants.BasePath;
        private const string DynamicImagesTag = "DynamicImages";

        /// <summary>
        /// DynamicImagesController
        /// </summary>
        /// <param name="dynamicImagesService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public DynamicImagesController(IDynamicImagesService dynamicImagesService,
            ILogger<IDynamicImagesService> logger, IMapper mapper)
            : base(dynamicImagesService, logger, mapper)
        {
        }

        /// <summary>
        /// ObtenerImagenesLogin
        /// </summary>
        /// <returns></returns>
        [HttpGet("obtener-imagenes-login")]
        [SwaggerOperation(Summary = "ObtenerImagenesLogin", Tags = new[] { DynamicImagesTag })]
        [ProducesResponseType(typeof(ObtenerImagenesLoginModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ObtenerImagenesLogin([FromQuery] ApiHeaders apiHeaders)
            => await ExecuteAsync(
                Service.ObtenerImagenesLoginAsync,
                ObtenerImagenesLoginModelRequest.ToRequestBody(apiHeaders),
                ObtenerImagenesLoginModelResponse.FromAsync
            );

        /// <summary>
        ///  Método para limpiar cache de Dynamic Images
        /// </summary>
        /// <returns></returns>
        [HttpGet("invalidar-cache")]
        [SwaggerOperation(Summary = "Invalidar DynamicImages cache", Tags = new[] { DynamicImagesTag })]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public IActionResult InvalidarCache([FromServices] IMemoryCache memoryCache) =>
            Ok($"{MemoryCacheHelper.CleanCache(memoryCache, Cache.DynamicImages.Key)} registros eliminados del cache");
    }
}
