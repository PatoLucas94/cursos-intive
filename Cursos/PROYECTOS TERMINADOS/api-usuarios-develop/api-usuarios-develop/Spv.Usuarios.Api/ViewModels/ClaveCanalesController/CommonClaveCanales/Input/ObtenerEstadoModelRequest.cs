using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.ClaveCanalesService.Input;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.ClaveCanalesController.CommonClaveCanales.Input
{
    /// <summary>
    /// Obtener Estado Clave Canales Model
    /// </summary>
    public class ObtenerEstadoModelRequest
    {
        /// <summary>
        /// Tipo de Documento
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.IdTipoDocumento)]
        [Required(ErrorMessage = "El campo Tipo de Documento es obligatorio.")]
        public int IdTipoDocumento { get; set; }

        /// <summary>
        /// Numero de Documento
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.NroDocumento)]
        [Required(ErrorMessage = "El campo Número de Documento es obligatorio.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El campo Nro_Documento, permite un rango de entre {2} y {1} dígitos.")]
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto con clave de canales ofuscada
        /// </summary>
        public override string ToString()
        {
            return $"{{ {ParameterNames.IdTipoDocumento}:'{IdTipoDocumento}', {ParameterNames.NroDocumento}:'" +
                $"{NumeroDocumento}' }}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<EstadoModelInput> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(
                new EstadoModelInput
                {
                    DocumentTypeId = IdTipoDocumento,
                    DocumentNumber = NumeroDocumento
                }, allowedChannels);
        }
    }
}
