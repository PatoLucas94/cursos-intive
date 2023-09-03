using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.Interface
{
    public interface IHelperDbServer
    {
        Task<FechaDbServer> ObtenerFechaAsync();
    }
}
