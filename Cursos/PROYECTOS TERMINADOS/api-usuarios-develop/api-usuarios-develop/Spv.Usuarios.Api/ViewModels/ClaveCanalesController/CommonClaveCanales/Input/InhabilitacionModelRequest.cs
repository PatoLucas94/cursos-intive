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
    /// Inhabilitación Clave Canales Model
    /// </summary>
    [JsonObject(Title = "InhabilitacionModelRequest")]
    public class InhabilitacionModelRequest
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
        /// Clave de Canales
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.ClaveCanales)]
        [Required(ErrorMessage = "El campo Clave de Canales es obligatorio.")]
        [Range(0, 99999999, ErrorMessage = "El campo Clave de Canales, admite solo números.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El campo Clave de Canales, permite una longitud de {1} dígitos.")]
        public string ClaveCanales { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto con clave de canales ofuscada
        /// </summary>
        public override string ToString()
        {
            return $"{{ {ParameterNames.IdTipoDocumento}:'{IdTipoDocumento}', {ParameterNames.NroDocumento}:'" +
                $"{NumeroDocumento}', {ParameterNames.ClaveCanales}:'************' }}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<InhabilitacionModelInput> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(
                new InhabilitacionModelInput
                {
                    DocumentTypeId = IdTipoDocumento,
                    DocumentNumber = NumeroDocumento,
                    ChannelKey = ClaveCanales
                }, allowedChannels);
        }
    }
}
