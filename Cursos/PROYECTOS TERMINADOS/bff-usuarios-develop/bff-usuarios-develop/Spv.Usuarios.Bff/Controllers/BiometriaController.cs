using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.ViewModels.BiometriaController.CommonBiometria.Input;
using Spv.Usuarios.Bff.ViewModels.BiometriaController.CommonBiometria.Output;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <summary>
    ///  BiometriaController
    /// </summary>
    [ApiController]
    [Route(RouteRoot + "biometria")]
    public class BiometriaController : ApiControllerBase<IBiometriaService>
    {
        private const string RouteRoot = AppConstants.BasePath;
        private const string BiometriaTag = "Biometria";

        /// <summary>
        ///  BiometriaController
        /// </summary>
        public BiometriaController(
            ILogger<IBiometriaService> logger,
            IBiometriaService biometriaService,
            IMapper mapper
        ) : base(biometriaService, logger, mapper)
        {
        }

        /// <summary>
        /// Autenticación
        /// </summary>
        /// <returns></returns>
        [HttpPost("autenticacion")]
        [SwaggerOperation(Summary = "Autenticar un usuario con biometria", Tags = new[] { BiometriaTag })]
        [ProducesResponseType(typeof(BiometriaAutenticacionModelResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> Autenticacion(
            [FromQuery] ApiHeaders apiHeaders,
            [FromBody] BiometriaAutenticacionModelRequest autenticacionModelRequest
        ) => await ExecuteAsync(
            Service.AutenticarAsync,
            autenticacionModelRequest.ToRequestBody(apiHeaders, Mapper),
            BiometriaAutenticacionModelResponse.FromAsync
        );
    }
}
