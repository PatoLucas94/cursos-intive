using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Exceptions;
using Spv.Usuarios.Bff.Service;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Spv.Usuarios.Bff.ViewModels.PersonaController.CommonPersona.Input;
using Spv.Usuarios.Bff.ViewModels.PersonaController.CommonPersona.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace Spv.Usuarios.Bff.Controllers
{
    /// <summary>
    /// PersonaController
    /// </summary>
    [ApiController]
    [Route(RouteRoot + "personas")]
    public class PersonaController : ApiControllerBase<IPersonasService>
    {
        private const string RouteRoot = AppConstants.BasePath;
        private const string PersonasTag = "Personas";

        /// <summary>
        /// Constructor PersonaController
        /// </summary>
        public PersonaController(
            ILogger<PersonasService> logger,
            IPersonasService personasService
            ) : base(personasService, logger)
        {

        }

        /// <summary>
        /// Persona Física
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Obtener Persona Física", Tags = new[] { PersonasTag })]
        [ProducesResponseType(typeof(PersonaModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> PersonaFisica(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery] PersonaModelRequest personaModelRequest)
        {
            return await ExecuteAsync(
                Service.ObtenerPersonaFisicaAsync,
                personaModelRequest.ToRequestBody(apiHeaders),
                PersonaModelResponse.FromAsync);
        }

        /// <summary>
        /// Persona filtro nro. Documento
        /// </summary>
        /// <returns></returns>
        [HttpGet("filtro")]
        [SwaggerOperation(Summary = "Obtener Personas no registradas", Tags = new[] { PersonasTag })]
        [ProducesResponseType(typeof(PersonaModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailModel), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> PersonaFiltro(
            [FromQuery] ApiHeaders apiHeaders,
            [FromQuery] PersonaFiltroModelRequest personaFiltroModelRequest)
        {
            return await ExecuteAsync(
                Service.ObtenerPersonaFiltroAsync,
                personaFiltroModelRequest.ToRequestBody(apiHeaders),
                PersonaModelResponse.FromAsync);
        }
    }
}
