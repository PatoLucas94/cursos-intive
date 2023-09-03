using Newtonsoft.Json;

namespace Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Output
{
    public class BiometriaAutenticacionModelOutput
    {
        [JsonProperty("identificacion_digital")]
        public IdentificacionDigitalOutput IdentificacionDigital { get; set; }

        [JsonProperty("status_liveness_pasivo")]
        public string StatusLivenessPasivo { get; set; }

        [JsonProperty("status_obtenido_mejor_imagen_facial")]
        public string StatusObtenidoMejorImagenFacial { get; set; }

        [JsonProperty("status_obtenido_plantilla_facial")]
        public string StatusObtenidoPlantillaFacial { get; set; }

        [JsonProperty("status_obtenido_plantilla_facial_extendida")]
        public string StatusObtenidoPlantillaFacialExtendida { get; set; }

        [JsonProperty("validacion_manual")]
        public bool? ValidacionManual { get; set; }
    }
}
