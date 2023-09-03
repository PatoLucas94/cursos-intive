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
    /// ValidacionExistenciaHbiModelRequest
    /// </summary>
    public class ValidacionExistenciaHbiModelRequest
    {

        /// <summary>
        /// Nombre de Usuario
        /// </summary>
        [FromHeader(Name = ParameterNames.UserName), DomainValidation(typeof(CredUsuarioOriginal))]
        [JsonProperty(PropertyName = ParameterNames.UserName)]
        public string UserName { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<ValidacionExistenciaHbiModelInput> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(new ValidacionExistenciaHbiModelInput
            {
                UserName = UserName
            }, allowedChannels);
        }
    }
}
