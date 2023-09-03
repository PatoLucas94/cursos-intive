using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.ViewModels.ReCaptchaController.CommonCaptcha.Input;
using Spv.Usuarios.Bff.ViewModels.ReCaptchaController.CommonCaptcha.Output;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <summary>
    /// CaptchaController
    /// </summary>
    [ApiController]
    [Route(RouteRoot + "recaptcha")]
    public class ReCaptchaController : ApiControllerBase<IReCaptchaService>
    {
        private const string RouteRoot = AppConstants.BasePath;
        private const string ReCaptchaTag = "ReCaptcha";

        /// <summary>
        /// CaptchaController
        /// </summary>
        /// <param name="recaptchaService"></param>
        /// <param name="logger"></param>
        public ReCaptchaController(IReCaptchaService recaptchaService, ILogger<IReCaptchaService> logger)
            : base(recaptchaService, logger)
        {

        }

        /// <summary>
        /// Validacion
        /// </summary>
        /// <returns></returns>
        [HttpPost("validacion")]
        [SwaggerOperation(Summary = "Validar comportamiento de usuario", Tags = new[] { ReCaptchaTag })]
        [ProducesResponseType(typeof(ReCaptchaValidacionModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Validacion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] ReCaptchaValidacionModelRequest validacionModelRequest)
        {
            return await ExecuteAsync(
                Service.ValidarAsync,
                validacionModelRequest.ToRequestBody(apiHeaders),
                ReCaptchaValidacionModelResponse.FromAsync);
        }
    }
}
