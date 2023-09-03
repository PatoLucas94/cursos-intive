using System;

namespace Spv.Usuarios.Common.Dtos.UsuariosService.Output
{
    public class AutenticacionClaveNumericaModelOutput
    {
        public long? IdPersona { get; set; }
        public string EstadoPassword { get; set; }
        public DateTime? FechaExpiracionPassword{ get; set; }
    }
}
