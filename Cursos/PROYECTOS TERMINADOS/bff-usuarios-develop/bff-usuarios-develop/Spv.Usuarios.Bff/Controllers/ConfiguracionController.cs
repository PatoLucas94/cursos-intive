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
using Spv.Usuarios.Bff.ViewModels.ConfiguracionController.Input;
using Spv.Usuarios.Bff.ViewModels.ConfiguracionController.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <summary>
    /// ConfiguracionController
    /// </summary>
    [ApiController]
    [Route(RouteRoot + "configuraciones")]
    public class ConfiguracionController : ApiControllerBase<IConfiguracionService>
    {
        private const string RouteRoot = AppConstants.BasePath;
        private const string ConfiguracionTag = "Configuracion";

        /// <summary>
        /// ConfiguracionController
        /// </summary>
        /// <param name="configuracionService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ConfiguracionController(IConfiguracionService configuracionService, ILogger<IConfiguracionService> logger, IMapper mapper)
            : base(configuracionService, logger, mapper)
        {
        }

        /// <summary>
        /// LoginHabilitado
        /// </summary>
        /// <returns></returns>
        [HttpGet("login-habilitado")]
        [SwaggerOperation(Summary = "Login Habilitado", Tags = new[] { ConfiguracionTag })]
        [ProducesResponseType(typeof(LoginHabilitadoModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> LoginHabilitado([FromQuery] ApiHeaders apiHeaders)
         => await ExecuteAsync(
            Service.LoginHabilitadoAsync,
            LoginHabilitadoModelRequest.ToRequestBody(apiHeaders),
            LoginHabilitadoModelResponse.FromAsync
        );
        
    }
}
