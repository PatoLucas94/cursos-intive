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
    /// Validacion Clave Sms Model
    /// </summary>
    [JsonObject(Title = "ValidacionClaveSmsModelRequest")]
    public class ValidacionClaveSmsModelRequest
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public string IdPersona { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [FromHeader(Name = ParameterNames.Identificador), DomainValidation(typeof(Guid))]
        [JsonProperty(PropertyName = ParameterNames.Identificador)]
        public string Identificador { get; set; }

        /// <summary>
        /// Clave SMS
        /// </summary>
        [FromHeader(Name = ParameterNames.ClaveSms), DomainValidation(typeof(ClaveSms))]
        [JsonProperty(PropertyName = ParameterNames.ClaveSms)]
        public string ClaveSms { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto con clave_sms ofuscada
        /// </summary>
        public override string ToString()
        {
            return "{ id_persona: '" + IdPersona + "', clave_sms: '********' }";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        public IRequestBody<ValidacionClaveSmsModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new ValidacionClaveSmsModelInput
                {
                    PersonId = IdPersona,
                    Identificador = Identificador,
                    ClaveSms = ClaveSms
                });
        }
    }
}
