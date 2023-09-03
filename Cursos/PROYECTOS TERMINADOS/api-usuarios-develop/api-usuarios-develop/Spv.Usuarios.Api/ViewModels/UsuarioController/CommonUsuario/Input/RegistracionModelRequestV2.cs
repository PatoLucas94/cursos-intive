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
    /// Registración model v2
    /// </summary>
    public class RegistracionModelRequestV2
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public long IdPersona { get; set; }

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
        /// Número de Documento
        /// </summary>
        [FromHeader(Name = ParameterNames.NroDocumento), DomainValidation(typeof(NroDocumento))]
        [JsonProperty(PropertyName = ParameterNames.NroDocumento)]
        public string NroDocumento { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Usuario)]
        public string Usuario { get; set; }

        /// <summary>
        /// Clave
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Clave)]
        public string Clave { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto clave y usuario ofuscados
        /// </summary>
        public override string ToString()
        {
            return $"{{ {ParameterNames.IdPersona}:'{IdPersona}', {ParameterNames.IdPais}:'" +
                    $"{IdPais}', {ParameterNames.IdTipoDocumento}:'{IdTipoDocumento}', {ParameterNames.NroDocumento}:'"+
                    $" {NroDocumento}', {ParameterNames.Usuario}:'************', {ParameterNames.Clave}:'************' }}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        public IRequestBody<RegistracionModelInputV2> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(
                new RegistracionModelInputV2
                {
                    PersonId = IdPersona,
                    DocumentCountryId = IdPais,
                    DocumentTypeId = IdTipoDocumento,
                    DocumentNumber = NroDocumento,
                    UserName = Usuario,
                    Password = Clave
                }, allowedChannels);
        }
    }
}
