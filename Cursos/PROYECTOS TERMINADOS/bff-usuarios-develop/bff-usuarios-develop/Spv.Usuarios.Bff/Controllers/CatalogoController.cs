using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Service;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.ViewModels.CatalogoController.CommonCatalogo.Input;
using Spv.Usuarios.Bff.ViewModels.CatalogoController.CommonCatalogo.Output;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <summary>
    ///  CatalogoController
    /// </summary>
    [ApiController]
    [Route(RouteRoot + "catalogo")]
    public class CatalogoController : ApiControllerBase<ICatalogoService>
    {
        private const string RouteRoot = AppConstants.BasePath;
        private const string CatalogoTag = "Catálogo";

        /// <summary>
        ///  Constructor CatalogoController
        /// </summary>
        public CatalogoController(
            ILogger<CatalogoService> logger,
            ICatalogoService catalogoService) : base(catalogoService, logger)
        {

        }

        /// <summary>
        ///  Método para obtener países desde api-personas-catalogo
        /// </summary>
        /// <param name="apiHeaders"></param>
        /// <returns></returns>
        [HttpGet("paises")]
        [SwaggerOperation(Summary = "Paises", Tags = new[] { CatalogoTag })]
        [ProducesResponseType(typeof(List<PaisesModelResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Paises(
            [FromQuery] ApiHeaders apiHeaders)
        {
            return await ExecuteAsync(
                Service.ObtenerPaisesAsync, 
                new PaisesModelRequest().ToRequestBody(apiHeaders),
                PaisesModelResponse.FromAsync);
        }

        /// <summary>
        ///  Método para obtener la lista de tipos de documento desde api-personas-catalogo
        /// </summary>
        /// <param name="apiHeaders"></param>
        /// <returns></returns>
        [HttpGet("tipos-documento")]
        [SwaggerOperation(Summary = "Tipos Documento", Tags = new[] { CatalogoTag })]
        [ProducesResponseType(typeof(List<TiposDocumentoModelResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> TiposDocumento(
            [FromQuery] ApiHeaders apiHeaders)
        {
            return await ExecuteAsync(
                Service.ObtenerTiposDocumentoAsync, 
                new TiposDocumentoModelRequest().ToRequestBody(apiHeaders),
                TiposDocumentoModelResponse.FromAsync);
        }
    }
}
