using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Attributes;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Input;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.PersonaController.CommonPersona.Input
{
    /// <summary>
    /// PersonaFiltroModelRequest
    /// </summary>
    public class PersonaFiltroModelRequest
    {
        /// <summary>
        /// Identificador de número de documento
        /// </summary>
        [FromQuery(Name = ParameterNames.NroDocumento), DomainValidationAttribute(typeof(NroDocumento))]
        [JsonProperty(PropertyName = ParameterNames.NroDocumento)]
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Identificador de Tipo de Documento.
        /// </summary>
        [FromQuery(Name = ParameterNames.IdTipoDocumento)]
        [JsonProperty(PropertyName = ParameterNames.IdTipoDocumento)]
        public int? IdTipoDocumento { get; set; }

        /// <summary>
        /// Identificador de País.
        /// </summary>
        [FromQuery(Name = ParameterNames.IdPais)]
        [JsonProperty(PropertyName = ParameterNames.IdPais)]
        public int? IdPais { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<PersonaFiltroInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new PersonaFiltroInput
                {
                    NumeroDocumento = NumeroDocumento,
                    TipoDocumento = IdTipoDocumento,
                    IdPais = IdPais
                });
        }
    }
}
