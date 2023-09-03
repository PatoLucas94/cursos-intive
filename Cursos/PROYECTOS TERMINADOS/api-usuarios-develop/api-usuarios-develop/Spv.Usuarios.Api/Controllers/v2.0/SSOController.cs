using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Api.ViewModels.SSOController.Output;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Api.Controllers.v2._0
{
    /// <summary>
    /// SsoController
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route(RouteRoot + "/sso")]
    public class SsoController : ApiControllerBase<ISsoService>
    {
        private readonly IAllowedChannels _allowedChannels;
        private const string RouteRoot = "v{version:apiVersion}";

        /// <summary>
        /// SsoController
        /// </summary>
        /// <param name="usuarioService"></param>
        /// <param name="logger"></param>
        /// <param name="allowedChannels"></param>
        /// <param name="mapper"></param>
        public SsoController(
            ISsoService usuarioService,
            ILogger<ISsoService> logger,
            IAllowedChannels allowedChannels,
            IMapper mapper
        ) : base(usuarioService, logger, mapper)
        {
            _allowedChannels = allowedChannels;
        }

        /// <summary>
        /// Autenticación
        /// </summary>
        /// <returns></returns>
        [HttpPost("autenticacion")]
        [SwaggerOperation(Summary = "Autenticar un usuario", Tags = new[] { "SSO" })]
        [ProducesResponseType(typeof(TokenModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Autenticacion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] AutenticacionModelRequestV2 autenticacionModelRequest
        ) => await ExecuteAsync(
            Service.AutenticarAsync,
            autenticacionModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
            TokenModelResponse.FromAsync
        );

        /// <summary>
        /// Obtener nuevo token
        /// </summary>
        /// <returns></returns>
        [HttpGet("actualizar-token")]
        [SwaggerOperation(Summary = "Obtener un nuevo token de acceso", Tags = new[] { "SSO" })]
        [ProducesResponseType(typeof(TokenModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> RefreshAccessToken(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery(Name = ParameterNames.RefreshToken)]
            string refreshToken
        ) => await ExecuteAsync(
            Service.RefreshAccessTokenAsync,
            apiHeaders.ToRequestBody(refreshToken, _allowedChannels),
            TokenModelResponse.FromAsync
        );

        /// <summary>
        /// VerificarToken
        /// </summary>
        /// <returns></returns>
        [HttpGet("verificar-token")]
        [SwaggerOperation(Summary = "Verificar autenticidad de un token", Tags = new[] { "SSO" })]
        [ProducesResponseType(typeof(IReadOnlyDictionary<string, object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> VerificarToken(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery(Name = ParameterNames.AccessToken)]
            string accessToken
        ) => await ExecuteAsync(
            Service.VerificarTokenAsync,
            apiHeaders.ToRequestBody(accessToken, _allowedChannels),
            async (response) => await response
        );

        /// <summary>
        /// CerrarSesion
        /// </summary>
        /// <returns></returns>
        [HttpGet("cerrar-sesion")]
        [SwaggerOperation(Summary = "Cerrar sesión", Tags = new[] { "SSO" })]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> CerrarSesion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery(Name = ParameterNames.RefreshToken)]
            string refreshToken
        ) => await ExecuteAsync(Service.CerrarSesionAsync, apiHeaders.ToRequestBody(refreshToken, _allowedChannels));
    }
}
