using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Utils;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Helpers;
using Spv.Usuarios.Bff.Service;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Spv.Usuarios.Bff.ViewModels.TyCController.CommonTyC.Input;
using Spv.Usuarios.Bff.ViewModels.TyCController.CommonTyC.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <summary>
    ///  TyCController
    /// </summary>
    [ApiController]
    [Route(RouteRoot + "tyc")]
    public class TyCController : ApiControllerBase<ITyCService>
    {
        private const string RouteRoot = AppConstants.BasePath;
        private const string TyCTag = "Términos y Condiciones";

        /// <summary>
        ///  Constructor TyCController
        /// </summary>
        public TyCController(ILogger<TyCService> logger, ITyCService tycService, IMapper mapper)
            : base(tycService, logger, mapper)
        {
        }

        /// <summary>
        ///  Método para obtener TyC vigente desde api-tyc
        /// </summary>
        /// <param name="apiHeaders"></param>
        /// <returns></returns>
        [HttpGet("vigente")]
        [SwaggerOperation(Summary = "Términos y Condiciones Vigente", Tags = new[] { TyCTag })]
        [ProducesResponseType(typeof(List<ApiTyCVigenteModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Vigente([FromQuery] ApiHeaders apiHeaders) => await ExecuteAsync(
            Service.ObtenerVigenteAsync,
            apiHeaders.ToRequestBody(string.Empty),
            VigenteModelResponse.FromAsync
        );

        /// <summary>
        ///  Método para obtener TyC aceptados desde api-tyc
        /// </summary>
        /// <param name="apiHeaders"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("aceptados/{personId}")]
        [SwaggerOperation(Summary = "Términos y Condiciones Aceptados", Tags = new[] { TyCTag })]
        [ProducesResponseType(typeof(List<ApiTyCVigenteModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> AceptadosByPersonaId(
            [FromQuery] ApiHeaders apiHeaders,
            [FromRoute] string personId
        ) => await ExecuteAsync(
            Service.ObtenerAceptadosByPersonIdAsync,
            apiHeaders.ToRequestBody(personId),
            AceptadosModelResponse.FromAsync
        );

        /// <summary>
        ///  Método para obtener TyC aceptados desde api-tyc
        /// </summary>
        /// <param name="apiHeaders"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("aceptados")]
        [SwaggerOperation(Summary = "Términos y Condiciones Aceptados", Tags = new[] { TyCTag })]
        [ProducesResponseType(typeof(List<ApiTyCVigenteModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Aceptados(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery] TyCPersonaModelRequest request
        ) => await ExecuteAsync(
            Service.ObtenerAceptadosAsync,
            request.ToRequestBody(apiHeaders),
            AceptadosModelResponse.FromAsync
        );

        /// <summary>
        ///  Método para limpiar cache de api-tyc
        /// </summary>
        /// <returns></returns>
        [HttpGet("invalidar-cache")]
        [SwaggerOperation(Summary = "Invalidar TyC cache", Tags = new[] { TyCTag })]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public IActionResult InvalidarCache([FromServices] IMemoryCache memoryCache) =>
            Ok($"{MemoryCacheHelper.CleanCache(memoryCache, Cache.ApiTyC.Key)} registros eliminados del cache");
    }
}
