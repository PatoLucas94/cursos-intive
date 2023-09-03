using Newtonsoft.Json;

namespace Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Input
{
    public class BiometriaAutenticacionModelInput
    {
        [JsonProperty(PropertyName = "id_persona")]
        public long IdPersona { get; set; }

        [JsonProperty(PropertyName = "datos_biometricos")]
        public DatosBiometricosModelInput DatosBiometricos { get; set; }
    }
}
