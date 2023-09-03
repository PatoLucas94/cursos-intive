using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.EntityFramework.V2
{
    public class HistorialClaveUsuariosV2Repository : GenericRepository<HistorialClaveUsuariosV2>, IHistorialClaveUsuariosV2Repository
    {
        public HistorialClaveUsuariosV2Repository(GenericDbContextV2 genericDbContext) : base(genericDbContext)
        {
        }

        public Task<List<HistorialClaveUsuariosV2>> ObtenerHistorialClavesUsuarioByUserIdAsync(int userId)
        {
            return Get(h => h.UserId == userId,
                o => o.OrderByDescending(h => h.PasswordHistoryId))
                .ToListAsync();
        }
    }
}
