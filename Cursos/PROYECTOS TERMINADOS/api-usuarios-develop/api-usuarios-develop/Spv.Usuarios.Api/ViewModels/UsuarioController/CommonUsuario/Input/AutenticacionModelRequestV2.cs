using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// Autenticacion Model
    /// </summary>
    [JsonObject(Title = "AutenticacionModelRequest")]
    public class AutenticacionModelRequestV2
    {
        /// <summary>
        /// Usuario
        /// </summary>
        [JsonProperty(PropertyName = "usuario")]
        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo Usuario, permite una longitud máxima de {1} caracteres. ")]
        public string Usuario { get; set; }

        /// <summary>
        /// Clave
        /// </summary>
        [JsonProperty(PropertyName = "clave")]
        [Required(ErrorMessage = "El campo Clave es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Clave, permite una longitud máxima de {1} caracteres. ")]
        public string Clave { get; set; }

        /// <summary>
        /// Numero de Documento
        /// </summary>
        [JsonProperty(PropertyName = "nro_documento")]
        [Required(ErrorMessage = "El campo Número de Documento es obligatorio.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El campo Nro_Documento, permite un rango de entre {2} y {1} dígitos.")]
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto con usuario y clave ofuscados
        /// </summary>
        public override string ToString()
        {
            return $"{{ {ParameterNames.Usuario}:'************', {ParameterNames.NroDocumento}:' " +
                $"{NumeroDocumento}', {ParameterNames.Clave}: '************' }}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<AutenticacionModelInput> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(
                new AutenticacionModelInput
                {
                    UserName = Usuario,
                    Password = Clave,
                    DocumentNumber = NumeroDocumento
                }, allowedChannels);
        }
    }
}
