using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.Interface.V2
{
    public interface IHistorialClaveUsuariosV2Repository : IGenericRepository<HistorialClaveUsuariosV2>
    {
        Task<List<HistorialClaveUsuariosV2>> ObtenerHistorialClavesUsuarioByUserIdAsync(int userId);
    }
}
