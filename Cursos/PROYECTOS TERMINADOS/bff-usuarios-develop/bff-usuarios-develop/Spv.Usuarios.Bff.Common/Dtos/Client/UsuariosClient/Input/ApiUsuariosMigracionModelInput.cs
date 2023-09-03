namespace Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input
{
    public class ApiUsuariosMigracionModelInput
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        public string id_persona { get; set; }
        
        /// <summary>
        /// Usuario
        /// </summary>
        public string usuario { get; set; }

        /// <summary>
        /// Clave
        /// </summary>
        public string clave { get; set; }
    }
}
