namespace Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input
{
    public class ApiUsuariosRegistracionV2ModelInput
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        public string id_persona { get; set; }

        /// <summary>
        /// Identificador de País.
        /// </summary>
        public int id_pais { get; set; }

        /// <summary>
        /// Identificador de Tipo de Documento.
        /// </summary>
        public int id_tipo_documento { get; set; }

        /// <summary>
        /// Número de Documento
        /// </summary>
        public string nro_documento { get; set; }

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
