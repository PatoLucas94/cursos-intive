using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.Interface.V2
{
    public interface IHistorialUsuarioUsuariosV2Repository : IGenericRepository<HistorialUsuarioUsuariosV2>
    {
        Task<List<HistorialUsuarioUsuariosV2>> ObtenerHistorialNombresUsuarioByUserId(int userId);
    }
}
