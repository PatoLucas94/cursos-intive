using System.ComponentModel.DataAnnotations;
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
    /// AutenticacionClaveNumericaModelRequest
    /// </summary>
    public class AutenticacionClaveNumericaModelRequest
    {
        /// <summary>
        /// Clave
        /// </summary>
        [JsonProperty(PropertyName = "clave")]
        [Required(ErrorMessage = "El campo Clave es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Clave, permite una longitud máxima de {1} caracteres. ")]
        public string Clave { get; set; }

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
        /// Retorna la representación en json del objeto clave ofuscados
        /// </summary>
        public override string ToString()
        {
            return $"{{ {ParameterNames.NroDocumento}:'{NroDocumento}', {ParameterNames.IdTipoDocumento}:'" +
                $"{IdTipoDocumento}', {ParameterNames.IdPais}:'{IdPais}', {ParameterNames.Clave}:'************' }}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<AutenticacionClaveNumericaModelInput> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(
                new AutenticacionClaveNumericaModelInput
                {
                    DocumentCountryId = IdPais,
                    DocumentTypeId = IdTipoDocumento,
                    Password = Clave,
                    DocumentNumber = NroDocumento
                }, allowedChannels);
        }
    }
}
