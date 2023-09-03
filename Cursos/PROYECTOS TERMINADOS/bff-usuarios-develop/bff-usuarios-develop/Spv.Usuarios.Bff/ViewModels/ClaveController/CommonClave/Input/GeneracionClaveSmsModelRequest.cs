using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Attributes;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Input;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Input
{
    /// <summary>
    /// Generación Clave Sms Model
    /// </summary>
    [JsonObject(Title = "GeneracionClaveSmsModelRequest")]
    public class GeneracionClaveSmsModelRequest
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public string IdPersona { get; set; }

        /// <summary>
        /// Teléfono
        /// </summary>
        [FromHeader(Name = ParameterNames.Telefono), DomainValidation(typeof(Telefono))]
        [JsonProperty(PropertyName = ParameterNames.Telefono)]
        public string Telefono { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        public IRequestBody<GeneracionClaveSmsModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new GeneracionClaveSmsModelInput
                {
                    PersonId = IdPersona,
                    Telefono = Telefono
                });
        }
    }
}
