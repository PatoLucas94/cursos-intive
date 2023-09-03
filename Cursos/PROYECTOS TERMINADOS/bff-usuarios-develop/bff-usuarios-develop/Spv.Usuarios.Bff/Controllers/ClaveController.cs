using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Input;
using Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Output;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <summary>
    /// ClaveController
    /// </summary>
    [ApiController]
    [Route(RouteRoot + "claves")]
    public class ClaveController : ApiControllerBase<IClaveService>
    {
        private const string RouteRoot = AppConstants.BasePath;
        private const string ClavesTag = "Claves";

        /// <summary>
        /// UsuariosController
        /// </summary>
        /// <param name="usuarioService"></param>
        /// <param name="logger"></param>
        public ClaveController(IClaveService usuarioService, ILogger<IClaveService> logger)
            : base(usuarioService, logger)
        {

        }

        /// <summary>
        /// Validación de Clave de Canales
        /// </summary>
        /// <returns></returns>
        [HttpPost("canales/validacion")]
        [SwaggerOperation(Summary = "Validar clave de canales", Tags = new[] { ClavesTag })]
        [ProducesResponseType(typeof(ValidacionClaveCanalesModelResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ValidacionClaveCanales(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ValidacionClaveCanalesModelRequest validacionModelRequest)
        {
            return await ExecuteAsync(
                Service.ValidarClaveCanalesAsync, 
                validacionModelRequest.ToRequestBody(apiHeaders),
                ValidacionClaveCanalesModelResponse.FromAsync);
        }

        /// <summary>
        /// Validación de Clave de Canales por ID persona
        /// </summary>
        /// <returns></returns>
        [HttpPost("canales/validacion-idpersona")]
        [SwaggerOperation(Summary = "Validar clave de canales por id persona", Tags = new[] { ClavesTag })]
        [ProducesResponseType(typeof(ValidacionClaveCanalesModelResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ValidacionClaveCanalesIdPersona(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ValidacionClaveCanalesIdPersonaModelRequest validacionModelRequest)
        {
            return await ExecuteAsync(
                Service.ValidarClaveCanalesIdPersonaAsync,
                validacionModelRequest.ToRequestBody(apiHeaders),
                ValidacionClaveCanalesModelResponse.FromAsync);
        }

        /// <summary>
        /// Generación de Clave SMS
        /// </summary>
        /// <returns></returns>
        [HttpPost("sms")]
        [SwaggerOperation(Summary = "Generar clave Sms", Tags = new[] { ClavesTag })]
        [ProducesResponseType(typeof(GeneracionClaveSmsModelResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> GeneracionClaveSms(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] GeneracionClaveSmsModelRequest generacionModelRequest)
        {
            return await ExecuteAsync(
                Service.GenerarClaveSmsAsync, 
                generacionModelRequest.ToRequestBody(apiHeaders),
                GeneracionClaveSmsModelResponse.FromAsync);
        }

        /// <summary>
        /// Validación de Clave SMS
        /// </summary>
        /// <returns></returns>
        [HttpPost("sms/validacion")]
        [SwaggerOperation(Summary = "Validar clave Sms", Tags = new[] { ClavesTag })]
        [ProducesResponseType(typeof(ValidacionClaveSmsModelResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ValidacionClaveSms(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ValidacionClaveSmsModelRequest validacionModelRequest)
        {
            return await ExecuteAsync(
                Service.ValidarClaveSmsAsync, 
                validacionModelRequest.ToRequestBody(apiHeaders),
                ValidacionClaveSmsModelResponse.FromAsync);
        }

        /// <summary>
        /// Obtener Estado
        /// </summary>
        /// <returns></returns>
        [HttpPost("canales/obtener-estado")]
        [SwaggerOperation(Summary = "Validar clave de canales", Tags = new[] { ClavesTag })]
        [ProducesResponseType(typeof(ObtenerEstadoModelResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ObtenerEstado(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ObtenerEstadoModelRequest obtenerEstadoModelRequest)
        {
            return await ExecuteAsync(Service.ObtenerEstadoAsync, obtenerEstadoModelRequest.ToRequestBody(apiHeaders),
                ObtenerEstadoModelResponse.FromAsync);
        }
    }
}
