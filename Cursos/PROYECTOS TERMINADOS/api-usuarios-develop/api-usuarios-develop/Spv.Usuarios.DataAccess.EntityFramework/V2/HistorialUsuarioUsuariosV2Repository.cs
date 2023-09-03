using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.EntityFramework.V2
{
    public class HistorialUsuarioUsuariosV2Repository : GenericRepository<HistorialUsuarioUsuariosV2>, IHistorialUsuarioUsuariosV2Repository
    {
        public HistorialUsuarioUsuariosV2Repository(GenericDbContextV2 genericDbContext) : base(genericDbContext)
        {
        }

        public Task<List<HistorialUsuarioUsuariosV2>> ObtenerHistorialNombresUsuarioByUserId(int userId)
        {
            return Get(h => h.UserId == userId,
                o => o.OrderByDescending(h => h.UsernameHistoryId))
                .ToListAsync();
        }
    }
}
