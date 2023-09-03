using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.EntityFramework
{
    public class HistorialClaveUsuariosRepository : GenericRepository<HistorialClaveUsuarios>, IHistorialClaveUsuariosRepository
    {
        public HistorialClaveUsuariosRepository(GenericDbContext genericDbContext) : base(genericDbContext)
        {
        }

        public Task<List<HistorialClaveUsuarios>> ObtenerHistorialClavesUsuarioByUserIdAsync(int userId)
        {
            return Get(h => h.UserId == userId,
                o => o.OrderByDescending(h => h.PasswordHistoryId))
                .ToListAsync();
        }
    }
}
