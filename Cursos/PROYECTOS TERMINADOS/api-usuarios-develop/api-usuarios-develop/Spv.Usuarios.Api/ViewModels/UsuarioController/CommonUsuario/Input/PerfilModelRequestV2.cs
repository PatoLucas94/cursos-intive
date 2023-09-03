using Microsoft.AspNetCore.Mvc;
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
    /// Perfil
    /// </summary>
    public class PerfilModelRequestV2
    {
        /// <summary>
        /// Identificador de Api-Personas del usuario
        /// </summary>
        [FromRoute(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        public long IdPersona { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        public IRequestBody<PerfilModelInputV2> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(
                new PerfilModelInputV2
                {
                    IdPersona = IdPersona
                }, allowedChannels);
        }
    }
}
