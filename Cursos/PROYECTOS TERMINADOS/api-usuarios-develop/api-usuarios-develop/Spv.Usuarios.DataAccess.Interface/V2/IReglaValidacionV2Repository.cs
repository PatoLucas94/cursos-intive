using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.Interface.V2
{
    public interface IReglaValidacionV2Repository : IGenericRepository<ReglaValidacionV2>
    {
        Task<List<ReglaValidacionV2>> ObtenerReglasValidacionActivasByModelAndInputAsync(int modelId, int inputId);
    }
}