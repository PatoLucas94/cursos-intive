using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Domain.Enums;

namespace Spv.Usuarios.DataAccess.EntityFramework.V2
{
    public class UsuarioV2Repository : GenericRepository<UsuarioV2>, IUsuarioV2Repository
    {
        public UsuarioV2Repository(GenericDbContextV2 genericDbContextV2) : base(genericDbContextV2)
        {
        }

        public Task<UsuarioV2> ObtenerUsuarioByPersonIdAsync(long personId) =>
            Filter(u => u.PersonId == personId).FirstOrDefaultAsync();

        public Task<UsuarioV2> ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
            int documentCountryId,
            int documentTypeId,
            string documentNumber
        ) => Filter(u => u.DocumentCountryId == documentCountryId &&
                         u.DocumentTypeId == documentTypeId &&
                         Convert.ToInt64(u.DocumentNumber) == Convert.ToInt64(documentNumber)
        ).FirstOrDefaultAsync();

        public Task<UsuarioV2> ObtenerUsuarioAsync(string username, string documentNumber) =>
            Filter(u =>
                    u.Username.Equals(username) &&
                    Convert.ToInt64(u.DocumentNumber) == Convert.ToInt64(documentNumber)
                )
                .FirstOrDefaultAsync();

        public Task<UsuarioV2[]> ObtenerUsuariosByCountryIdDocumentTypeIdDocumentNumberAsync(
            int documentCountryId,
            int documentTypeId,
            string documentNumber
        ) => Filter(u => u.DocumentCountryId == (documentCountryId > 0 ? documentCountryId : u.DocumentCountryId) &&
                         u.DocumentTypeId == (documentTypeId > 0 ? documentTypeId : u.DocumentTypeId) &&
                         Convert.ToInt64(u.DocumentNumber) == Convert.ToInt64(documentNumber)
        ).ToArrayAsync();

        public IQueryable<UsuarioV2> ObtenerUsuarioByDocumentNumber(string documentNumber) =>
            Filter(u => Convert.ToInt64(u.DocumentNumber) == Convert.ToInt64(documentNumber));

        public async Task<bool> CambiarEstadoAsync(long personId, UserStatus userStatus)
        {
            var user = await FindAsync(f => f.PersonId == personId);

            if (user == null)
                return false;

            switch (userStatus)
            {
                case UserStatus.Active:
                    user.UserStatusId = (byte)userStatus;
                    user.LoginAttempts = 0;
                    break;
                case UserStatus.Pending:
                case UserStatus.InProgress:
                case UserStatus.Declined:
                case UserStatus.Blocked:
                case UserStatus.Migrated:
                case UserStatus.Inactive:
                case UserStatus.Suspended:
                    user.UserStatusId = (byte)userStatus;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(userStatus), userStatus, null);
            }

            await SaveChangesAsync();

            return true;
        }
    }
}
