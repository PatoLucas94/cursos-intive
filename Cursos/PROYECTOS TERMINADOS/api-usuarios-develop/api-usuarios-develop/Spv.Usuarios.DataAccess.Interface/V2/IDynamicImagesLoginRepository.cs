using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.Interface.V2
{
    public interface IDynamicImagesLoginRepository : IGenericRepository<DynamicImagesLogin>
    {
        Task<List<DynamicImagesLogin>> ObtenerImagesLogin(bool habilitado);
    }
}
