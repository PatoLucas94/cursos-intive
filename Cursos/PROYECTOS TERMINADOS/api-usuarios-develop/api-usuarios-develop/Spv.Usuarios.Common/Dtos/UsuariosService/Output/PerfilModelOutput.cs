using System;

namespace Spv.Usuarios.Common.Dtos.UsuariosService.Output
{
    /// <summary>
    /// ProfileModelOutput
    /// </summary>
    public class PerfilModelOutput
    {
        /// <summary>
        /// LastLogon
        /// </summary>
        public DateTime? LastLogon { get; set; }

        /// <summary>
        /// PersonId
        /// </summary>
        public long? PersonId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// DocumentNumber
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// DocumentType
        /// </summary>
        public int DocumentType { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public int Country { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// LastPasswordChange
        /// </summary>
        public DateTime? LastPasswordChange { get; set; }

        /// <summary>
        /// PasswordExpiryDate
        /// </summary>
        public DateTime? PasswordExpiryDate { get; set; }

        /// <summary>
        /// IsEmployee
        /// </summary>
        public bool? IsEmployee { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Migrated
        /// </summary>
        public bool Migrated { get; set; }

        /// <summary>
        /// Canal
        /// </summary>
        public string Canal { get; set; }

        /// <summary>
        /// UserStatusId
        /// </summary>
        public int UserStatusId { get; set; }
    }
}
