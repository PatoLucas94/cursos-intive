using Newtonsoft.Json;

namespace Spv.Usuarios.Bff.ViewModels.BiometriaController.CommonBiometria.Output
{
    public class IdentificacionDigitalResponse
    {
        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("documento_vigente")]
        public bool? DocumentoVigente { get; set; }

        [JsonProperty("porcentaje_similitud_renaper")]
        public decimal? PorcentajeSimilitudRenaper { get; set; }

        [JsonProperty("codigo_respuesta_renaper")]
        public string CodigoRespuestaRenaper { get; set; }

        [JsonProperty("id_tramite")]
        public int? IdTramite { get; set; }

        [JsonProperty("mensaje_error_renaper")]
        public string MensajeErrorRenaper { get; set; }
    }
}
