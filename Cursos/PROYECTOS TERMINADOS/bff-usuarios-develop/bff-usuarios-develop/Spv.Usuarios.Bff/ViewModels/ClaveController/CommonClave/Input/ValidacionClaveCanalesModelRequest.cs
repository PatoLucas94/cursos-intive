using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Input;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Input
{
    /// <summary>
    /// Validacion Clave Canales Model
    /// </summary>
    [JsonObject(Title = "ValidacionCanalesModelRequest")]
    public class ValidacionClaveCanalesModelRequest
    {
        /// <summary>
        /// Tipo de Documento
        /// </summary>
        [JsonProperty(PropertyName = "id_tipo_documento")]
        [Required(ErrorMessage = "El campo Tipo de Documento es obligatorio.")]
        public int IdTipoDocumento { get; set; }

        /// <summary>
        /// Numero de Documento
        /// </summary>
        [JsonProperty(PropertyName = "nro_documento")]
        [Required(ErrorMessage = "El campo Número de Documento es obligatorio.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El campo Nro_Documento, permite un rango de entre {2} y {1} dígitos.")]
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Clave de Canales
        /// </summary>
        [JsonProperty(PropertyName = "clave_canales")]
        [Required(ErrorMessage = "El campo Clave de Canales es obligatorio.")]
        [Range(0, 99999999, ErrorMessage = "El campo Clave de Canales, admite solo números.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El campo Clave de Canales, permite una longitud de {1} dígitos.")]
        public string ClaveCanales { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto con clave ofuscada
        /// </summary>
        public override string ToString()
        {
            return "{ id_tipo_documento: '" + IdTipoDocumento + "', numero_documento: '" + NumeroDocumento + "', clave_canales: '********' }";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<ValidacionClaveCanalesModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new ValidacionClaveCanalesModelInput
                {
                    DocumentTypeId = IdTipoDocumento,
                    DocumentNumber = NumeroDocumento,
                    ChannelKey = ClaveCanales
                });
        }
    }
}
