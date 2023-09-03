using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.Interface.V2
{
    public interface IDynamicImagesRepository : IGenericRepository<DynamicImages>
    {
        Task<DynamicImages> ObtenerImages(int id);
    }
}
