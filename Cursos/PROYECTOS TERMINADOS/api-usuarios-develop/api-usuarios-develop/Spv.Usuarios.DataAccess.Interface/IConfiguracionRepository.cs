using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.Interface
{
    public interface IConfiguracionRepository : IGenericRepository<Configuracion>
    {
        Task<Configuracion> ObtenerConfiguracion(string tipo, string nombre);
    }
}
