using System;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output
{
    /// <summary>
    /// ApiUsuariosPerfilModelOutputV2
    /// </summary>
    public class ApiUsuariosPerfilModelOutputV2
    {
        /// <summary>
        /// ultimo_login
        /// </summary>
        public DateTime? ultimo_login { get; set; }

        /// <summary>
        /// id_persona
        /// </summary>
        public long id_persona { get; set; }

        /// <summary>
        /// id_usuario
        /// </summary>
        public int id_usuario { get; set; }

        /// <summary>
        /// nro_documento
        /// </summary>
        public string nro_documento { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// nombre
        /// </summary>
        public string nombre { get; set; }

        /// <summary>
        /// apellido
        /// </summary>
        public string apellido { get; set; }

        /// <summary>
        /// tipo_documento
        /// </summary>
        public int tipo_documento { get; set; }

        /// <summary>
        /// pais
        /// </summary>
        public int pais { get; set; }

        /// <summary>
        /// genero
        /// </summary>
        public string genero { get; set; }

        /// <summary>
        /// fecha_ultimo_cambio_clave
        /// </summary>
        public DateTime? fecha_ultimo_cambio_clave { get; set; }

        /// <summary>
        /// fecha_vencimiento_clave
        /// </summary>
        public DateTime? fecha_vencimiento_clave { get; set; }

        /// <summary>
        /// user_status_id
        /// </summary>
        public int user_status_id { get; set; }
    }
}
