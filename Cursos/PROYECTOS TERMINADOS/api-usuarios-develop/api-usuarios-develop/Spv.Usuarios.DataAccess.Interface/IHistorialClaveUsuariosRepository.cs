using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.Interface
{
    public interface IHistorialClaveUsuariosRepository : IGenericRepository<HistorialClaveUsuarios>
    {
        Task<List<HistorialClaveUsuarios>> ObtenerHistorialClavesUsuarioByUserIdAsync(int userId);
    }
}
