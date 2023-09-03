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
    /// Validacion Clave Canales Model
    /// </summary>
    [JsonObject(Title = "ValidacionClaveCanalesIdPersonaModelRequest")]
    public class ValidacionClaveCanalesIdPersonaModelRequest
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public string IdPersona { get; set; }


        /// <summary>
        /// Clave de Canales
        /// </summary>
        [FromHeader(Name = ParameterNames.ClaveCanales), DomainValidation(typeof(ClaveCanales))]
        [JsonProperty(PropertyName = ParameterNames.ClaveCanales)]
        public string ClaveCanales { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto con clave ofuscada
        /// </summary>
        public override string ToString()
        {
            return "{ id_persona: '" + IdPersona + "', clave_canales: '********' }";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<ValidacionClaveCanalesIdPersonaModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new ValidacionClaveCanalesIdPersonaModelInput
                {
                    PersonId = IdPersona,
                    ChannelKey = ClaveCanales
                });
        }
    }
}
