using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.EntityFramework
{
    public class ConfiguracionRepository : GenericRepository<Configuracion>, IConfiguracionRepository
    {
        public ConfiguracionRepository(GenericDbContext genericDbContext) : base(genericDbContext)
        {
        }

        public Task<Configuracion> ObtenerConfiguracion(string tipo, string nombre)
        {
            return Filter(c => c.Type == tipo && c.Name == nombre).FirstOrDefaultAsync();
        }
    }
}