using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.Interface.V2
{
    public interface IHelperDbServerV2
    {
        Task<FechaDbServerV2> ObtenerFechaAsync();
    }
}
