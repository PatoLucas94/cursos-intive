namespace Spv.Usuarios.Bff.Common.Dtos.Client.ClaveCliente.Input
{
    public class ApiUsuariosObtenerEstadoClaveCanalesModelInput
    {
        /// <summary>
        /// Tipo de Documento
        /// </summary>
        public int id_tipo_documento { get; set; }

        /// <summary>
        /// Numero de Documento
        /// </summary>
        public string nro_documento { get; set; }
    }
}
