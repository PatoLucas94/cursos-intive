using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Service;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Spv.Usuarios.Bff.ViewModels.RhSsoController.CommonRhSso.Input;
using Spv.Usuarios.Bff.ViewModels.RhSsoController.CommonRhSso.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <summary>
    ///  RhSsoController
    /// </summary>
    [ApiController]
    [Route(RouteRoot + "rh-sso")]
    public class RhSsoController : ApiControllerBase<IRhSsoService>
    {
        private const string RouteRoot = AppConstants.BasePath;
        private const string RhSsoTag = "RedHat-SSO";

        /// <summary>
        ///  Constructor RhSsoController
        /// </summary>
        public RhSsoController(ILogger<RhSsoService> logger, IRhSsoService rhSsoService, IMapper mapper)
            : base(rhSsoService, logger, mapper)
        {
        }

        /// <summary>
        /// Autenticación
        /// </summary>
        /// <returns></returns>
        [HttpPost("autenticacion")]
        [SwaggerOperation(Summary = "Autenticar un usuario", Tags = new[] { RhSsoTag })]
        [ProducesResponseType(typeof(TokenModelResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Autenticacion(
            [FromQuery] ApiHeadersDeviceId apiHeaders,
            [FromBody] AutenticacionModelRequest autenticacionModelRequest
        ) => await ExecuteAsync(
            Service.AutenticarAsync,
            autenticacionModelRequest.ToRequestBody(apiHeaders),
            TokenModelResponse.FromAsync
        );
    }
}
