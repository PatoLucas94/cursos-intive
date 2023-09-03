using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Api.ViewModels.ConfiguracionesController.Input;
using Spv.Usuarios.Api.ViewModels.ConfiguracionesController.Output;
using Spv.Usuarios.Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Api.Controllers.v2._0
{
    /// <summary>
    /// ConfiguracionesController
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route(RouteRoot + "/configuraciones")]
    public class ConfiguracionesController : ApiControllerBase<IConfiguracionesService>
    {
        private readonly IAllowedChannels _allowedChannels;
        private const string RouteRoot = "v{version:apiVersion}";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuracionService"></param>
        /// <param name="logger"></param>
        /// <param name="allowedChannels"></param>
        /// <param name="mapper"></param>
        public ConfiguracionesController(
            IConfiguracionesService configuracionService,
            ILogger<IConfiguracionesService> logger,
            IAllowedChannels allowedChannels,
            IMapper mapper) : base(configuracionService, logger, mapper)
        {
            _allowedChannels = allowedChannels;
        }

        /// <summary>
        /// TerminosYCondicionesHabilitado
        /// </summary>
        /// <returns></returns>
        [HttpGet("terminos-condiciones-habilitado")]
        [SwaggerOperation(Summary = "Terminos y Condiciones Habilitado ", Tags = new[] { "Configuracion" })]
        [ProducesResponseType(typeof(TerminosYCondicionesHabilitadoModelResponse), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> TerminosYCondicionesHabilitado([FromQuery] ApiHeaders apiHeaders) =>
            await ExecuteAsync(
                Service.ObtenerConfiguracionTerminosYCondicionesHabilitadoAsync,
                TerminosYCondicionesHabilitadoModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
                TerminosYCondicionesHabilitadoModelResponse.FromAsync
            );

        /// <summary>
        /// LoginHabilitado
        /// </summary>
        /// <returns></returns>
        [HttpGet("login-habilitado")]
        [SwaggerOperation(Summary = "login habilitado", Tags = new[] { "Configuracion" })]
        [ProducesResponseType(typeof(LoginHabilitadoModelResponse), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> LoginHabilitado([FromQuery] ApiHeaders apiHeaders) => await ExecuteAsync(
            Service.ObtenerConfiguracionLoginHabilitadoAsync,
            LoginHabilitadoModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
            LoginHabilitadoModelResponse.FromAsync
        );

        /// <summary>
        /// LoginHabilitado
        /// </summary>
        /// <returns></returns>
        [HttpGet("mensaje-default-login-deshabilitado")]
        [SwaggerOperation(Summary = "mensaje default login deshabilitado", Tags = new[] { "Configuracion" })]
        [ProducesResponseType(typeof(LoginMessageModelResponse), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> LoginDefaultMenssage([FromQuery] ApiHeaders apiHeaders) => await ExecuteAsync(
            Service.ObtenerConfiguracionLoginDefaultMenssageAsync,
            LoginMessageModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
            LoginMessageModelResponse.FromAsync
        );

        /// <summary>
        /// LoginHabilitado
        /// </summary>
        /// <returns></returns>
        [HttpGet("mensaje-login-deshabilitado")]
        [SwaggerOperation(Summary = "mensaje login deshabilitado", Tags = new[] { "Configuracion" })]
        [ProducesResponseType(typeof(LoginMessageModelResponse), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> LoginMenssage([FromQuery] ApiHeaders apiHeaders) => await ExecuteAsync(
            Service.ObtenerConfiguracionLoginMenssageAsync,
            LoginMessageModelRequest.ToRequestBody(apiHeaders, _allowedChannels),
            LoginMessageModelResponse.FromAsync
        );
    }
}
