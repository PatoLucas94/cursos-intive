using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Attributes;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// ValidacionExistenciaModelRequest
    /// </summary>
    public class ValidacionExistenciaModelRequest
    {
        /// <summary>
        /// Número de Documento
        /// </summary>
        [FromQuery(Name = ParameterNames.NroDocumento), DomainValidation(typeof(NroDocumento))]
        [JsonProperty(PropertyName = ParameterNames.NroDocumento)]
        public string NroDocumento { get; set; }

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
        public IRequestBody<ValidacionExistenciaModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new ValidacionExistenciaModelInput
                {
                    NumeroDocumento = NroDocumento,
                    TipoDocumento = IdTipoDocumento,
                    IdPais = IdPais
                });
        }
    }
}
