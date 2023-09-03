using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Api.Common.Attributes;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// ActualizarPersonIdModelRequest
    /// </summary>
    public class ActualizarPersonIdModelRequest
    {
        /// <summary>
        /// Identificador de País.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPais), DomainValidation(typeof(IdPais))]
        [JsonProperty(PropertyName = ParameterNames.IdPais)]
        public int IdPais { get; set; }

        /// <summary>
        /// Identificador de Tipo de Documento.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdTipoDocumento), DomainValidation(typeof(IdTipoDocumento))]
        [JsonProperty(PropertyName = ParameterNames.IdTipoDocumento)]
        public int IdTipoDocumento { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [FromHeader(Name = ParameterNames.NroDocumento), DomainValidation(typeof(NroDocumento))]
        [JsonProperty(PropertyName = ParameterNames.NroDocumento)]
        public string NroDocumento { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<ActualizarPersonIdModelInput> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(new ActualizarPersonIdModelInput
            {
                DocumentCountryId = IdPais,
                DocumentNumber = NroDocumento,
                DocumentTypeId = IdTipoDocumento
            }, allowedChannels);
        }
    }
}
