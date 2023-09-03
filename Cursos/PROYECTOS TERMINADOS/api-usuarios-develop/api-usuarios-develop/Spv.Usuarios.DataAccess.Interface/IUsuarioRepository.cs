using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.Interface
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario> ObtenerUsuarioByPersonIdAsync(long personId);

        Task<Usuario> ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
            string countryId,
            int documentTypeId,
            string documentNumber
        );

        Task<Usuario> ObtenerUsuarioAsync(string userName);

        Task<Usuario> ObtenerUsuarioAsync(string userName, string documentNumber);

        Task<Usuario> ObtenerUsuarioAsync(int documentTypeId, string documentNumber);

        Task<Usuario> ObtenerPerfilUsuarioAsync(string userName);
    }
}
