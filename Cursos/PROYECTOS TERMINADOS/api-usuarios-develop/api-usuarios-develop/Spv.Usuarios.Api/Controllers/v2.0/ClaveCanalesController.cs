using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Api.ViewModels.ClaveCanalesController.CommonClaveCanales.Input;
using Spv.Usuarios.Api.ViewModels.ClaveCanalesController.CommonClaveCanales.Output;
using Spv.Usuarios.Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Api.Controllers.v2._0
{
    /// <summary>
    /// ClaveCanalesController
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route(RouteRoot + "/clave-canales")]
    public class ClaveCanalesController : ApiControllerBase<IClaveCanalesService>
    {
        private readonly IAllowedChannels _allowedChannels;
        private const string RouteRoot = "v{version:apiVersion}";
        private const string Tag = "Clave Canales";

        /// <summary>
        /// ClaveCanalesController
        /// </summary>
        /// <param name="claveCanalesService"></param>
        /// <param name="logger"></param>
        /// <param name="allowedChannels"></param>
        public ClaveCanalesController(IClaveCanalesService claveCanalesService, ILogger<IClaveCanalesService> logger, IAllowedChannels allowedChannels)
            : base(claveCanalesService, logger)
        {
            _allowedChannels = allowedChannels;
        }

        /// <summary>
        /// Validación
        /// </summary>
        /// <returns></returns>
        [HttpPost("validacion")]
        [SwaggerOperation(Summary = "Validar clave de canales", Tags = new[] { Tag })]
        [ProducesResponseType(typeof(ValidacionModelResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Validacion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ValidacionModelRequest validacionModelRequest)
        {
            return await ExecuteAsync(Service.ValidarAsync, validacionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                ValidacionModelResponse.FromAsync);
        }

        /// <summary>
        /// Inhabilitación
        /// </summary>
        /// <returns></returns>
        [HttpPost("inhabilitacion")]
        [SwaggerOperation(Summary = "Inhabilitar una clave de canales", Tags = new[] { Tag })]
        [ProducesResponseType(typeof(InhabilitacionModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Inhabilitacion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] InhabilitacionModelRequest inhabilitacionModelRequest)
        {
            return await ExecuteAsync(Service.InhabilitarAsync, inhabilitacionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                InhabilitacionModelResponse.FromAsync);
        }

        /// <summary>
        /// Obtener Estado
        /// </summary>
        /// <returns></returns>
        [HttpPost("obtener-estado")]
        [SwaggerOperation(Summary = "Obtener Estado clave de canales", Tags = new[] { Tag })]
        [ProducesResponseType(typeof(ObtenerEstadoModelResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ObtenerEstado(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ObtenerEstadoModelRequest obtenerEstadoModelRequest)
        {
            return await ExecuteAsync(Service.ObtenerEstadoAsync, obtenerEstadoModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                ObtenerEstadoModelResponse.FromAsync);
        }
    }
}
