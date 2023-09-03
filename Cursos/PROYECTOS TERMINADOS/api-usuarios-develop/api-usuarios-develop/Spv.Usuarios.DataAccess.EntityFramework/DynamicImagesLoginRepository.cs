using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.EntityFramework
{
    public class DynamicImagesLoginRepository : GenericRepository<DynamicImagesLogin>, IDynamicImagesLoginRepository
    {
        public DynamicImagesLoginRepository(GenericDbContext genericDbContext) : base(genericDbContext)
        {
        }

        public Task<List<DynamicImagesLogin>> ObtenerImagesLogin(bool habilitado)
        {
            return Get(h => h.Habilitada == habilitado
                   ).ToListAsync();
        }
    }
}
