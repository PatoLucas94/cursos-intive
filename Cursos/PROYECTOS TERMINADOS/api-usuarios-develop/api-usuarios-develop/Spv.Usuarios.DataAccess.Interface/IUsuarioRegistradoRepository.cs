using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.Interface
{
    public interface IUsuarioRegistradoRepository : IGenericRepository<UsuarioRegistrado>
    {
        Task<UsuarioRegistrado> ObtenerUsuarioRegistradoAsync(int documentTypeId, string documentNumber);
    }
}
