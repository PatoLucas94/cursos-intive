using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// ObtenerUsuarioModelRequest
    /// </summary>
    public class ObtenerUsuarioModelRequest
    {
        /// <summary>
        /// Número de Documento
        /// </summary>
        [FromQuery(Name = ParameterNames.NroDocumento)]
        [JsonProperty(PropertyName = ParameterNames.NroDocumento)]
        [Required(ErrorMessage = "El campo Número de Documento es obligatorio.")]
        [StringLength(20, MinimumLength = 3,
            ErrorMessage = "El campo Nro_Documento, permite un rango de entre {2} y {1} dígitos.")]
        public string NroDocumento { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [FromQuery(Name = ParameterNames.Usuario)]
        [JsonProperty(PropertyName = ParameterNames.Usuario)]
        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo Usuario, permite una longitud máxima de {1} caracteres. ")]
        public string Usuario { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<ObtenerUsuarioModelInput> ToRequestBody(
            ApiHeaders headers,
            IAllowedChannels allowedChannels
        ) => headers?.ToRequestBody(
            new ObtenerUsuarioModelInput
            {
                NumeroDocumento = NroDocumento,
                Usuario = Usuario
            },
            allowedChannels
        );
    }
}
