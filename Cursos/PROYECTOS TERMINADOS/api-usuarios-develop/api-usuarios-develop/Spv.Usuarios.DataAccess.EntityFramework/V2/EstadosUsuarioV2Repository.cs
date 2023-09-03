using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.EntityFramework.V2
{
    public class EstadosUsuarioV2Repository : GenericRepository<EstadosUsuarioV2>, IEstadosUsuarioV2Repository
    {
        public EstadosUsuarioV2Repository(GenericDbContextV2 genericDbContextV2) : base(genericDbContextV2)
        {
        }
    }
}
