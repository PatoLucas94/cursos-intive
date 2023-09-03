using Newtonsoft.Json;

namespace Spv.Usuarios.Bff.ViewModels.BiometriaController.CommonBiometria.Input
{
    /// <summary>
    /// DatosBiometricosModelRequest
    /// </summary>
    public class DatosBiometricosModelRequest
    {
        /// <summary>
        /// MejorImagenFacial
        /// </summary>
        [JsonProperty(PropertyName = "mejor_imagen_facial")]
        public string MejorImagenFacial { get; set; }

        /// <summary>
        /// PlantillaFacialExtendida
        /// </summary>
        [JsonProperty(PropertyName = "plantilla_facial_extendida")]
        public string PlantillaFacialExtendida { get; set; }
    }
}
