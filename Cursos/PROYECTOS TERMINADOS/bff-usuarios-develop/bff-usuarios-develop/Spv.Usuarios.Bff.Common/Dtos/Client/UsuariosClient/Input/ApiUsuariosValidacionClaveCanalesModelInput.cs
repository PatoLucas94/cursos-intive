namespace Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input
{
    public class ApiUsuariosValidacionClaveCanalesModelInput
    {
        /// <summary>
        /// Tipo de Documento
        /// </summary>
        public int id_tipo_documento { get; set; }

        /// <summary>
        /// Numero de Documento
        /// </summary>
        public string nro_documento { get; set; }

        /// <summary>
        /// Clave de Canales
        /// </summary>
        public string clave_canales { get; set; }
    }
}
