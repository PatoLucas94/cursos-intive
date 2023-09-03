﻿namespace Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input
{
    /// <summary>
    /// ApiUsuariosValidacionExistenciaModelInput
    /// </summary>
    public class ApiUsuariosValidacionExistenciaModelInput
    {
        /// <summary>
        /// Identificador de País.
        /// </summary>
        public int id_pais { get; set; }

        /// <summary>
        /// Identificador de Tipo de Documento.
        /// </summary>
        public int id_tipo_documento { get; set; }

        /// <summary>
        /// Nro de documento
        /// </summary>
        public string nro_documento { get; set; }
    }
}