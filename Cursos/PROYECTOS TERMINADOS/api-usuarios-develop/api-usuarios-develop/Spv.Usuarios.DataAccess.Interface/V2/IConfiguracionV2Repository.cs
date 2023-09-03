using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.Interface.V2
{
    public interface IConfiguracionV2Repository : IGenericRepository<ConfiguracionV2>
    {
        Task<ConfiguracionV2> ObtenerConfiguracion(string tipo, string nombre);
    }
}
