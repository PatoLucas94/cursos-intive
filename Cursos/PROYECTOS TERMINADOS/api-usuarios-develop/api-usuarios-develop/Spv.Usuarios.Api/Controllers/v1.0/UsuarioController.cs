using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output;
using Spv.Usuarios.Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Api.Controllers.v1._0
{
    /// <summary>
    /// UsuarioController
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route(RouteRoot)]
    public class UsuarioController : ApiControllerBase<IUsuariosService>
    {
        private readonly IAllowedChannels _allowedChannels;
        private const string RouteRoot = "v{version:apiVersion}";

        /// <summary>
        /// UsuarioController
        /// </summary>
        /// <param name="usuarioService"></param>
        /// <param name="logger"></param>
        /// <param name="allowedChannels"></param>
        public UsuarioController(IUsuariosService usuarioService, ILogger<IUsuariosService> logger, IAllowedChannels allowedChannels) 
            : base(usuarioService, logger)
        {
            _allowedChannels = allowedChannels;
        }

        /// <summary>
        /// Autenticación
        /// </summary>
        /// <returns></returns>
        [HttpPost("usuarios/autenticacion")]
        [SwaggerOperation(Summary = "Autenticar un usuario", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Autenticacion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] AutenticacionModelRequest autenticacionModelRequest)
        {
            return await ExecuteAsync(Service.AutenticarAsync, autenticacionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                AutenticacionModelResponse.FromAsync);
        }

        /// <summary>
        /// Perfil
        /// </summary>
        /// <returns></returns>
        [HttpGet("usuarios/{usuario}/perfil")]
        [SwaggerOperation(Summary = "Perfil de usuario", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(PerfilModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Perfil(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery] PerfilModelRequest perfilModelRequest)
        {
            return await ExecuteAsync(Service.ObtenerPerfilAsync, perfilModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                PerfilModelResponse.FromAsync);
        }

        /// <summary>
        /// Registración
        /// </summary>
        /// <returns></returns>
        [HttpPost("usuarios/registracion")]
        [SwaggerOperation(Summary = "Registrar usuario", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(RegistracionModelResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Registracion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] RegistracionModelRequest registracionModelRequest)
        {
            return await ExecuteAsync(Service.RegistrarV1Async, registracionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                RegistracionModelResponse.FromAsync);
        }

        /// <summary>
        /// Validación de existencia de nombre de usuario registrado en HBI Legacy
        /// </summary>
        /// <returns></returns>
        [HttpPost("usuarios/validacion-existencia")]
        [SwaggerOperation(Summary = "Validar usuario ya registrado en hbi legacy", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(ValidacionExistenciaHbiModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ValidacionHbi(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ValidacionExistenciaHbiModelRequest validacionModelRequest)
        {
            return await ExecuteAsync(Service.ValidarExistenciaHbiAsync, validacionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                ValidacionExistenciaHbiModelResponse.FromAsync);
        }
    }
}
