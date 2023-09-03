using System;

namespace Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output
{
    /// <summary>
    /// PerfilModelOutputV2
    /// </summary>
    public class PerfilModelOutputV2
    {
        /// <summary>
        /// UltimoLogin
        /// </summary>
        public DateTime? UltimoLogin { get; set; }

        /// <summary>
        /// IdPersona
        /// </summary>
        public long? IdPersona { get; set; }

        /// <summary>
        /// IdUsuario
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// NumeroDocumento
        /// </summary>
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido
        /// </summary>
        public string Apellido { get; set; }

        /// <summary>
        /// TipoDocumento
        /// </summary>
        public int TipoDocumento { get; set; }

        /// <summary>
        /// Pais
        /// </summary>
        public int Pais { get; set; }

        /// <summary>
        /// Genero
        /// </summary>
        public string Genero { get; set; }

        /// <summary>
        /// LastPasswordChange
        /// </summary>
        public DateTime? FechaUltimoCambioClave { get; set; }

        /// <summary>
        /// PasswordExpiryDate
        /// </summary>
        public DateTime? FechaVencimientoClave { get; set; }

        /// <summary>
        /// IdStatusUsuario
        /// </summary>
        public int IdStatusUsuario { get; set; }
    }
}
