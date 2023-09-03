using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.SofttokenClient.Output;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Service;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Spv.Usuarios.Bff.ViewModels.SoftToken.Input;
using Spv.Usuarios.Bff.ViewModels.SoftToken.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <summary>
    ///  SoftTokenController
    /// </summary>
    [ApiController]
    [Route(RouteRoot + "softToken")]
    public class SoftTokenController : ApiControllerBase<ISoftTokenService>
    {
        private const string RouteRoot = AppConstants.BasePath;
        private const string SoftTokenTag = "Soft Token";

        /// <summary>
        ///  Constructor TyCController
        /// </summary>
        public SoftTokenController(
            ILogger<SoftTokenService> logger,
            ISoftTokenService softTokenService) : base(softTokenService, logger)
        {

        }

        /// <summary>
        ///  Método para obtener SoftToken habilitado
        /// </summary>
        /// <param name="apiHeaders"></param>
        /// <param name="SoftTokenHabilitadoRequest"></param>
        /// <returns></returns>
        [HttpGet("habilitado")]
        [SwaggerOperation(Summary = "verifica que el usuario tenga el token habilitado", Tags = new[] { SoftTokenTag })]
        [ProducesResponseType(typeof(List<ApiSoftTokenModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Habilitado(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery] SoftTokenModelRequest SoftTokenHabilitadoRequest)
        {
            return await ExecuteAsync(
                Service.SoftTokenHabilitadoAsync,
                SoftTokenHabilitadoRequest.ToRequestBody(apiHeaders),
                SoftTokenModelResponse.FromAsync);
        }

        /// <summary>
        ///  Método para obtener SoftToken Valido
        /// </summary>
        /// <param name="apiHeaders"></param>
        /// <param name="SoftTokenValidoRequest"></param>
        /// <returns></returns>
        [HttpPost("validacion")]
        [SwaggerOperation(Summary = "verifica que el token sea valido", Tags = new[] { SoftTokenTag })]
        [ProducesResponseType(typeof(List<ApiSoftTokenModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Validacion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] SoftTokenValidoModelRequest SoftTokenValidoRequest)
        {
            return await ExecuteAsync(
                Service.SoftTokenValidoAsync,
                SoftTokenValidoRequest.ToRequestBody(apiHeaders),
                SoftTokenModelResponse.FromAsync);
        }
    }
}
