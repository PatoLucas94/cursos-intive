﻿namespace Spv.Usuarios.Common.Dtos.UsuariosService.Input
{
    public class ValidacionExistenciaModelInput
    {
        public int DocumentCountryId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
    }
}
