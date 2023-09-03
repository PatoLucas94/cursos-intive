using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// Perfil
    /// </summary>
    public class PerfilModelRequest
    {
        /// <summary>
        /// Nombre usuario
        /// </summary>
        [FromRoute(Name = "usuario"), JsonPropertyName("usuario"), StringLength(50, ErrorMessage = "El campo Usuario, permite una longitud máxima de {1} caracteres. ")]
        public string Usuario { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        public IRequestBody<PerfilModelInput> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(
                new PerfilModelInput
                {
                    UserName = Usuario
                }, allowedChannels);
        }
    }
}
