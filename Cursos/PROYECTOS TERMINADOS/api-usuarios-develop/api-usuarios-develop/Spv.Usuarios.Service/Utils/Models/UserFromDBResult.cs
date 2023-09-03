using Spv.Usuarios.Domain.Interfaces;

namespace Spv.Usuarios.Service.Utils.Models
{
    public class UserFromDbResult
    {
        public IUsuario Usuario { get; set; }
        public string UsernameEncrypted { get; set; }
        public bool FromNewVersionDb { get; set; }
    }
}
