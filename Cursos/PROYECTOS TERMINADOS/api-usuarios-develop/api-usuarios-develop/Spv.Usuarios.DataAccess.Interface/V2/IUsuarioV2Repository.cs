using System.Linq;
using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Domain.Enums;

namespace Spv.Usuarios.DataAccess.Interface.V2
{
    public interface IUsuarioV2Repository : IGenericRepository<UsuarioV2>
    {
        Task<UsuarioV2> ObtenerUsuarioByPersonIdAsync(long personId);

        Task<UsuarioV2> ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
            int documentCountryId,
            int documentTypeId,
            string documentNumber
        );

        Task<UsuarioV2[]> ObtenerUsuariosByCountryIdDocumentTypeIdDocumentNumberAsync(
            int documentCountryId,
            int documentTypeId,
            string documentNumber
        );

        IQueryable<UsuarioV2> ObtenerUsuarioByDocumentNumber(string documentNumber);

        Task<bool> CambiarEstadoAsync(long personId, UserStatus userStatus);

        Task<UsuarioV2> ObtenerUsuarioAsync(string username, string documentNumber);
    }
}
