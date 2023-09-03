using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.EntityFramework.V2
{
    public class ConfiguracionV2Repository : GenericRepository<ConfiguracionV2>, IConfiguracionV2Repository
    {
        public ConfiguracionV2Repository(GenericDbContextV2 genericDbContextV2) : base(genericDbContextV2)
        {
        }

        public Task<ConfiguracionV2> ObtenerConfiguracion(string tipo, string nombre)
        {
            return Filter(c => c.Type == tipo && c.Name == nombre).FirstOrDefaultAsync();
        }
    }
}
