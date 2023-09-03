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
    /// Perfil Migrado
    /// </summary>
    public class PerfilMigradoModelRequest
    {
        /// <summary>
        /// Identificador de País.
        /// </summary>
        [FromQuery(Name = ParameterNames.IdPais)]
        [JsonProperty(PropertyName = ParameterNames.IdPais)]
        public int IdPais { get; set; }

        /// <summary>
        /// Identificador de Tipo de Documento.
        /// </summary>
        [FromQuery(Name = ParameterNames.IdTipoDocumento)]
        [JsonProperty(PropertyName = ParameterNames.IdTipoDocumento)]
        public int IdTipoDocumento { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [Required]
        [FromQuery(Name = ParameterNames.NroDocumento), DomainValidation(typeof(NroDocumento))]
        [JsonProperty(PropertyName = ParameterNames.NroDocumento)]
        public string NroDocumento { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<PerfilMigradoModelInput> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
            => headers?.ToRequestBody(
                new PerfilMigradoModelInput
                {
                    DocumentCountryId = IdPais,
                    DocumentNumber = NroDocumento,
                    DocumentTypeId = IdTipoDocumento
                },
                allowedChannels
            );

        /// <summary>
        /// AddRequestBody
        /// </summary>
        /// <returns></returns>
        public static IRequestBody<PerfilMigradoModelInput> AddRequestBody(
            ApiHeaders headers,
            IAllowedChannels allowedChannels,
            long id_persona
        ) => headers?.ToRequestBody(new PerfilMigradoModelInput(id_persona), allowedChannels);
    }
}