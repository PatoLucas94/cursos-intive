using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
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

namespace Spv.Usuarios.Api.Controllers.v2._0
{
    /// <summary>
    /// UsuarioController
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route(RouteRoot + "/usuarios")]
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
        /// <param name="mapper"></param>
        public UsuarioController(
            IUsuariosService usuarioService,
            ILogger<IUsuariosService> logger,
            IAllowedChannels allowedChannels,
            IMapper mapper
        ) : base(usuarioService, logger, mapper)
        {
            _allowedChannels = allowedChannels;
        }

        /// <summary>
        /// Perfil
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id_persona}/perfil")]
        [SwaggerOperation(Summary = "Perfil de usuario", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(PerfilModelResponseV2), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Perfil(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery] PerfilModelRequestV2 perfilModelRequestV2
        )
        {
            return await ExecuteAsync(
                Service.ObtenerPerfilAsync,
                perfilModelRequestV2.ToRequestBody(apiHeaders, _allowedChannels),
                PerfilModelResponseV2.FromAsync
            );
        }

        /// <summary>
        /// Perfil de usuario migrado
        /// </summary>
        /// <returns></returns>
        [HttpGet("perfil-migrado")]
        [SwaggerOperation(Summary = "Perfil de usuario migrado", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(PerfilMigradoModelResponse[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> PerfilMigrado(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery] PerfilMigradoModelRequest perfilMigradoModelRequestV2
        ) => await ExecuteAsync(
            Service.ObtenerPerfilesAsync,
            perfilMigradoModelRequestV2.ToRequestBody(apiHeaders, _allowedChannels),
            PerfilMigradoModelResponse.FromListAsync
        );

        /// <summary>
        /// Perfil de usuario migrado
        /// </summary>
        /// <returns></returns>
        [HttpGet("perfil-migrado/{id_persona}")]
        [SwaggerOperation(Summary = "Perfil de usuario migrado", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(PerfilMigradoModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> PerfilMigrado([FromQuery] ApiHeaders apiHeaders, [FromRoute] long id_persona)
            => await ExecuteAsync(
                Service.ObtenerPerfilAsync,
                PerfilMigradoModelRequest.AddRequestBody(apiHeaders, _allowedChannels, id_persona),
                PerfilMigradoModelResponse.FromAsync
            );

        /// <summary>
        /// Autenticación
        /// </summary>
        /// <returns></returns>
        [HttpPost("autenticacion")]
        [SwaggerOperation(Summary = "Autenticar un usuario", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(AutenticacionModelResponseV2), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Autenticacion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] AutenticacionModelRequestV2 autenticacionModelRequest
        )
        {
            return await ExecuteAsync(
                Service.AutenticarAsync,
                autenticacionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                AutenticacionModelResponseV2.FromAsync
            );
        }

        /// <summary>
        /// Autenticación de clave numérica
        /// </summary>
        /// <returns></returns>
        [HttpPost("autenticacion/clave-numerica")]
        [SwaggerOperation(Summary = "Autenticar un usuario con clave numérica", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(AutenticacionClaveNumericaModelResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> AutenticacionClaveNumerica(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] AutenticacionClaveNumericaModelRequest autenticacionModelRequest
        )
        {
            return await ExecuteAsync(
                Service.AutenticarConClaveNumericaAsync,
                autenticacionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                AutenticacionClaveNumericaModelResponse.FromAsync
            );
        }

        /// <summary>
        /// Validación de existencia de usuario registrado
        /// </summary>
        /// <returns></returns>
        [HttpPost("validacion-existencia")]
        [SwaggerOperation(Summary = "Validar usuario ya registrado", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(ValidacionExistenciaModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Validacion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ValidacionExistenciaModelRequest validacionModelRequest
        )
        {
            return await ExecuteAsync(
                Service.ValidarExistenciaAsync,
                validacionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                ValidacionExistenciaModelResponse.FromAsync
            );
        }

        /// <summary>
        /// Validación de existencia de usuario registrado con clave BTA
        /// </summary>
        /// <returns></returns>
        [HttpPost("validacion-existencia-bta")]
        [SwaggerOperation(Summary = "Validar usuario ya registrado con clave BTA", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(ValidacionExistenciaBtaModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ValidacionBta(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ValidacionExistenciaBtaModelRequest validacionModelBtaRequest
        )
        {
            return await ExecuteAsync(
                Service.ValidarExistenciaBtaAsync,
                validacionModelBtaRequest.ToRequestBody(apiHeaders, _allowedChannels),
                ValidacionExistenciaBtaModelResponse.FromAsync
            );
        }

        /// <summary>
        /// Registración de usuario
        /// </summary>
        /// <returns></returns>
        [HttpPost("registracion")]
        [SwaggerOperation(Summary = "Registrar usuario", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(RegistracionModelResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Registracion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] RegistracionModelRequestV2 registracionModelRequest
        )
        {
            return await ExecuteAsync(
                Service.RegistrarV2Async,
                registracionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                RegistracionModelResponse.FromAsync
            );
        }

        /// <summary>
        /// Migración de usuario
        /// </summary>
        /// <returns></returns>
        [HttpPost("migracion")]
        [SwaggerOperation(Summary = "Migrar usuario", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(MigracionModelResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Migracion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] MigracionModelRequest migracionModelRequest
        )
        {
            return await ExecuteAsync(
                Service.MigrarAsync,
                migracionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                MigracionModelResponse.FromAsync
            );
        }

        /// <summary>
        /// Cambio de Clave de Usuario
        /// </summary>
        /// <returns></returns>
        [HttpPost("cambio-de-clave")]
        [SwaggerOperation(Summary = "Cambiar clave de usuario", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(CambioDeClaveModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> CambioDeClave(
            [FromQuery] ApiHeadersGateWay apiHeaders,
            [FromBody] CambioDeClaveModelRequestV2 cambioDeClaveModelRequestV2
        )
        {
            return await ExecuteAsync(
                Service.ModificarClaveAsync,
                cambioDeClaveModelRequestV2.ToRequestBody(apiHeaders, _allowedChannels),
                CambioDeClaveModelResponse.FromAsync
            );
        }

        /// <summary>
        /// Cambio de Credenciales de Usuario
        /// </summary>
        /// <returns></returns>
        [HttpPost("cambio-de-credenciales")]
        [SwaggerOperation(Summary = "Cambiar credenciales de usuario", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(CambioDeClaveModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> CambioDeCredenciales(
            [FromQuery] ApiHeadersGateWay apiHeaders,
            [FromBody] CambioDeCredencialesModelRequestV2 cambioDeCredencialesModelRequestV2
        )
        {
            return await ExecuteAsync(
                Service.ModificarCredencialesAsync,
                cambioDeCredencialesModelRequestV2.ToRequestBody(apiHeaders, _allowedChannels),
                CambioDeCredencialesModelResponse.FromAsync
            );
        }

        /// <summary>
        /// Cambio estados de usuario
        /// </summary>
        /// <returns></returns>
        [HttpPatch("cambiar-estado")]
        [SwaggerOperation(Summary = "Cambiar estados de usuario", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(CambioDeClaveModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> CambiarEstado(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] CambioEstadoModelRequest cambioEstadoModelRequest
        ) => await ExecuteAsync(
            Service.CambiarEstadoAsync,
            cambioEstadoModelRequest.ToRequestBody(apiHeaders, _allowedChannels)
        );

        /// <summary>
        /// Actualizar PersonId
        /// </summary>
        /// <returns></returns>
        [HttpPatch("actualizar-personId")]
        [SwaggerOperation(Summary = "Actualizar PersonId", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(ActualizarPersonIdModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ActualizarPersonId(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ActualizarPersonIdModelRequest validacionModelRequest
        )
        {
            return await ExecuteAsync(
                Service.ActualizarPersonIdAsync,
                validacionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                ActualizarPersonIdModelResponse.FromAsync
            );
        }

        /// <summary>
        /// Validación de existencia de usuario registrado por num. doc y nombre de usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet("obtener-usuario")]
        [SwaggerOperation(Summary = "Obtener usuario ya registrado", Tags = new[] { "Usuarios" })]
        [ProducesResponseType(typeof(ObtenerUsuarioModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> ObtenerUsuario(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery] ObtenerUsuarioModelRequest obtenerUsuarioModelRequest
        ) => await ExecuteAsync(
            Service.ObtenerUsuarioAsync,
            obtenerUsuarioModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
            ObtenerUsuarioModelResponse.FromAsync
        );
    }
}
