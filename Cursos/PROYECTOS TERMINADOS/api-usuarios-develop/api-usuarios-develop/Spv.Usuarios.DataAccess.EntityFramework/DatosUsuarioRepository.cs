using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.EntityFramework
{
    public class DatosUsuarioRepository : GenericRepository<DatosUsuario>, IDatosUsuarioRepository
    {
        public DatosUsuarioRepository(GenericDbContext genericDbContext) : base(genericDbContext)
        {
        }
    }
}
