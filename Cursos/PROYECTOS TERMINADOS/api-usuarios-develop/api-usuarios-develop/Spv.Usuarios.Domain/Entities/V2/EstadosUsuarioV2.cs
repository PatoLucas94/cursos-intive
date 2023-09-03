using System.Collections.Generic;

namespace Spv.Usuarios.Domain.Entities.V2
{
    public class EstadosUsuarioV2
    {
        public byte UserStatusId { get; set; }
        public string Description { get; set; }

        public List<UsuarioV2> Users { get; set; }
    }
}
