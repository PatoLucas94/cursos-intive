using System.Diagnostics.CodeAnalysis;
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
    /// Validacion Persona Model
    /// </summary>
    [JsonObject(Title = "ValidacionModelRequest")]
    [ExcludeFromCodeCoverage]
    public class PersonaModelRequest
    {
        /// <summary>
        /// Identificador de país
        /// </summary>
        [FromQuery(Name = ParameterNames.IdPais)]
        public int IdPais { get; set; }

        /// <summary>
        /// Identificador de tipo de documento
        /// </summary>
        [FromQuery(Name = ParameterNames.IdTipoDocumento)]
        public int IdTipoDocumento { get; set; }

        /// <summary>
        /// Identificador de número de documento
        /// </summary>
        [FromQuery(Name = ParameterNames.NroDocumento), DomainValidation(typeof(NroDocumento))]
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto
        /// </summary>
        public override string ToString()
        {
            return "{ id_pais: '" + IdPais + "', id_tipo_documento: '" + IdTipoDocumento + "', numero_documento: '" +
                   NumeroDocumento + "'}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<PersonaModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new PersonaModelInput
                {
                    IdPais = IdPais,
                    IdTipoDocumento = IdTipoDocumento,
                    NumeroDocumento = NumeroDocumento
                });
        }
    }
}
