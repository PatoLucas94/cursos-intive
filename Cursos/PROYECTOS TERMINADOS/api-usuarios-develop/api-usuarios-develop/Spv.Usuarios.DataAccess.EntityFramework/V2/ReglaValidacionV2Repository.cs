using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.EntityFramework.V2
{
    public class ReglaValidacionV2Repository : GenericRepository<ReglaValidacionV2>, IReglaValidacionV2Repository
    {
        public ReglaValidacionV2Repository(GenericDbContextV2 genericDbContextV2) : base(genericDbContextV2)
        {
        }

        public Task<List<ReglaValidacionV2>> ObtenerReglasValidacionActivasByModelAndInputAsync(int modelId, int inputId)
        {
            return Get(r => r.ModelId == modelId && r.InputId == inputId && r.IsActive == true,
                o => o.OrderByDescending(v => v.ValidationRulePriority))
                .ToListAsync();
        }
    }
}