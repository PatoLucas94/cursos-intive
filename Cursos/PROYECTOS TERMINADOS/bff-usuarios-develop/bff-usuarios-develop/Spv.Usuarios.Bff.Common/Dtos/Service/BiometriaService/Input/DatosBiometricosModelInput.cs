using Newtonsoft.Json;

namespace Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Input
{
    public class DatosBiometricosModelInput
    {
        [JsonProperty(PropertyName = "mejor_imagen_facial")]
        public string MejorImagenFacial { get; set; }

        [JsonProperty(PropertyName = "plantilla_facial_extendida")]
        public string PlantillaFacialExtendida { get; set; }
    }
}
