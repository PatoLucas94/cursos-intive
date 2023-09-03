using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <summary>
    /// UsuarioController
    /// </summary>
    [ApiController]
    [Route(RouteRoot + "usuarios")]
    public class UsuarioController : ApiControllerBase<IUsuarioService>
    {
        private const string RouteRoot = AppConstants.BasePath;
        private const string UsuariosTag = "Usuarios";

        /// <summary>
        /// UsuariosController
        /// </summary>
        /// <param name="usuarioService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public UsuarioController(IUsuarioService usuarioService, ILogger<IUsuarioService> logger, IMapper mapper)
            : base(usuarioService, logger, mapper)
        {
        }

        /// <summary>
        /// Perfil
        /// </summary>
        /// <returns></returns>
        [HttpGet("perfil")]
        [SwaggerOperation(Summary = "Perfil de usuario", Tags = new[] { UsuariosTag })]
        [ProducesResponseType(typeof(PerfilModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Perfil(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery] PerfilModelRequest perfilModelRequest
        ) => await ExecuteAsync(
            Service.ObtenerPerfilAsync,
            perfilModelRequest.ToRequestBody(apiHeaders),
            PerfilModelResponse.FromAsync
        );

        /// <summary>
        /// Perfil
        /// </summary>
        /// <returns></returns>
        [HttpGet("perfilV2")]
        [SwaggerOperation(Summary = "Perfil de usuario V2", Tags = new[] { UsuariosTag })]
        [ProducesResponseType(typeof(PerfilModelResponseV2), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> PerfilV2(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery] PerfilModelRequestV2 perfilModelRequest
        ) => await ExecuteAsync(
            Service.ObtenerPerfilAsync,
            perfilModelRequest.ToRequestBody(apiHeaders),
            PerfilModelResponseV2.FromAsync
        );

        /// <summary>
        /// Validación de existencia de usuario registrado en el nuevo modelo
        /// </summary>
        /// <returns></returns>
        [HttpGet("validacion-existencia")]
        [SwaggerOperation(Summary = "Validar existencia de usuario", Tags = new[] { UsuariosTag })]
        [ProducesResponseType(typeof(ValidacionExistenciaModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidacionExistenciaModelResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ValidacionExistencia(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery] ValidacionExistenciaModelRequest validacionExistenciaModelRequest
        ) => await ExecuteAsync(
            Service.ValidarExistenciaAsync,
            validacionExistenciaModelRequest.ToRequestBody(apiHeaders),
            ValidacionExistenciaModelResponse.FromAsync
        );

        /// <summary>
        /// Validación de existencia de un usuario (username) en el sistema HBI Legacy
        /// </summary>
        /// <returns></returns>
        [HttpPost("validacion-existencia-hbi")]
        [SwaggerOperation(Summary = "Validar usuario ya registrado en hbi legacy", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(ValidacionExistenciaHbiModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ValidacionExistenciaHbi(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ValidacionExistenciaHbiModelRequest validacionExistenciaHbiModelRequest
        ) => await ExecuteAsync(
            Service.ValidarExistenciaHbiAsync,
            validacionExistenciaHbiModelRequest.ToRequestBody(apiHeaders),
            ValidacionExistenciaHbiModelResponse.FromAsync
        );

        /// <summary>
        /// Registración
        /// </summary>
        /// <returns></returns>
        [HttpPost("registracion")]
        [SwaggerOperation(Summary = "Registrar usuario", Tags = new[] { UsuariosTag })]
        [ProducesResponseType(typeof(RegistracionModelResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Registracion(
            [FromQuery] ApiHeadersDeviceId apiHeaders,
            [FromBody] RegistracionModelRequest registracionModelRequest
        ) => await ExecuteAsync(
            Service.RegistrarAsync,
            registracionModelRequest.ToRequestBody(apiHeaders),
            RegistracionModelResponse.FromAsync
        );

        /// <summary>
        /// Cambio de Credenciales de Usuario
        /// </summary>
        /// <returns></returns>
        [HttpPost("cambio-de-credenciales")]
        [SwaggerOperation(Summary = "Cambiar credenciales de usuario", Tags = new[] { UsuariosTag })]
        [ProducesResponseType(typeof(CambioDeCredencialesModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> CambioDeCredenciales(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] CambioDeCredencialesModelRequest cambioDeCredencialesModelRequest
        ) => await ExecuteAsync(
            Service.ModificarCredencialesAsync,
            cambioDeCredencialesModelRequest.ToRequestBody(apiHeaders),
            CambioDeCredencialesModelResponse.FromAsync
        );

        /// <summary>
        /// Cambio de Clave
        /// </summary>
        /// <returns></returns>
        [HttpPost("cambio-de-clave")]
        [SwaggerOperation(Summary = "Cambiar clave de usuario", Tags = new[] { UsuariosTag })]
        [ProducesResponseType(typeof(CambioDeClaveModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> CambioDeClave(
            [FromQuery] ApiHeadersDeviceId apiHeaders,
            [FromBody] CambioDeClaveModelRequest cambioDeClaveModelRequest)
        {
            return await ExecuteAsync(
                Service.ModificarClaveAsync,
                cambioDeClaveModelRequest.ToRequestBody(apiHeaders),
                CambioDeClaveModelResponse.FromAsync);
        }

        /// <summary>
        /// Migración
        /// </summary>
        /// <returns></returns>
        [HttpPost("migracion")]
        [SwaggerOperation(Summary = "Migrar usuario", Tags = new[] { UsuariosTag })]
        [ProducesResponseType(typeof(MigracionModelResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Migracion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] MigracionModelRequest migracionModelRequest)
        {
            return await ExecuteAsync(
                Service.MigrarAsync,
                migracionModelRequest.ToRequestBody(apiHeaders),
                MigracionModelResponse.FromAsync);
        }

        /// <summary>
        /// Recuperar Usuario
        /// </summary>
        /// <returns></returns>
        [HttpPost("recuperar-usuario")]
        [SwaggerOperation(Summary = "Recuperar Usuario", Tags = new[] { UsuariosTag })]
        [ProducesResponseType(typeof(RecuperarUsuarioModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> RecuperarUsuario(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] RecuperarUsuarioModelRequest recuperarUsuarioModelRequest
        ) => await ExecuteAsync(
            Service.RecuperarUsuarioAsync,
            recuperarUsuarioModelRequest.ToRequestBody(apiHeaders),
            RecuperarUsuarioModelResponse.FromAsync
        );
    }
}
