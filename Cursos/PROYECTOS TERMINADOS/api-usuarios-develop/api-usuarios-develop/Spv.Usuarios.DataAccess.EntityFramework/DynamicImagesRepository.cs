using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.EntityFramework
{
    public class DynamicImagesRepository : GenericRepository<DynamicImages>, IDynamicImagesRepository
    {
        public DynamicImagesRepository(GenericDbContext genericDbContext) : base(genericDbContext)
        {
        }

        public Task<DynamicImages> ObtenerImages(int id)
        {
            return Get(h => h.Id == id
            ).FirstOrDefaultAsync();
        }
    }
}
