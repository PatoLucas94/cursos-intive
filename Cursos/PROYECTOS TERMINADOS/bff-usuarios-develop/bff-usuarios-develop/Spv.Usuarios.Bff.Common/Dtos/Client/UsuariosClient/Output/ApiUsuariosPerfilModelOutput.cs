using System;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output
{
    public class ApiUsuariosPerfilModelOutput
    {
        /// <summary>
        /// LastLogon
        /// </summary>
        public DateTime? ultimo_login { get; set; }

        /// <summary>
        /// PersonId
        /// </summary>
        public string id_persona { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int id_usuario { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public string nombre { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        public string apellido { get; set; }
    }
}
