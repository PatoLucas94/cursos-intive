using Newtonsoft.Json;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output
{
    /// <summary>
    /// ApiObtenerUsuariosModelOutputV2
    /// </summary>
    public class ApiUsuariosObtenerUsuariosModelOutputV2
    {
        [JsonProperty("id_persona")]
        public string IdPersona { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("apellido")]
        public string Apellido { get; set; }

        [JsonProperty("migrado")]
        public bool Migrado { get; set; }

        [JsonProperty("identificador")]
        public string Identificador { get; set; }

        [JsonProperty("canal")]
        public string Canal { get; set; }
    }
}
