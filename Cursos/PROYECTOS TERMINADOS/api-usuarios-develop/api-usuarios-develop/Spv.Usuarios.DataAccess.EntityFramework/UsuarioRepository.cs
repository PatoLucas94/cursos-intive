using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.EntityFramework
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(GenericDbContext genericDbContext) : base(genericDbContext)
        {
        }

        public Task<Usuario> ObtenerUsuarioByPersonIdAsync(long personId) =>
            Get(u => u.UserData.PersonId == personId.ToString(),
                o => o.OrderByDescending(u => u.UserId),
                nameof(Usuario.UserData)
            ).FirstOrDefaultAsync();

        public Task<Usuario> ObtenerPerfilUsuarioAsync(string userName) =>
            Get(u => u.UserName == userName,
                o => o.OrderByDescending(u => u.UserId),
                nameof(Usuario.UserData)
            ).FirstOrDefaultAsync();

        public Task<Usuario> ObtenerUsuarioAsync(string userName) =>
            Get(u => u.UserName == userName,
                o => o.OrderByDescending(u => u.UserId),
                nameof(Usuario.UserData)
            ).FirstOrDefaultAsync();

        public Task<Usuario> ObtenerUsuarioAsync(string userName, string documentNumber) =>
            Get(u => u.UserName == userName &&
                     Convert.ToInt64(u.DocumentNumber) == Convert.ToInt64(documentNumber),
                o => o.OrderByDescending(u => u.UserId),
                nameof(Usuario.UserData)
            ).FirstOrDefaultAsync();

        public Task<Usuario> ObtenerUsuarioAsync(int documentTypeId, string documentNumber) =>
            Get(u => u.DocumentTypeId == documentTypeId &&
                     Convert.ToInt64(u.DocumentNumber) == Convert.ToInt64(documentNumber),
                o => o.OrderByDescending(u => u.UserId),
                nameof(Usuario.UserData)
            ).FirstOrDefaultAsync();

        public Task<Usuario> ObtenerUsuarioByCountryIdDocumentTypeIdDocumentNumberAsync(
            string countryId,
            int documentTypeId,
            string documentNumber
        ) => Get(u => u.DocumentCountryId.Equals(countryId) &&
                      u.DocumentTypeId == documentTypeId &&
                      Convert.ToInt64(u.DocumentNumber) == Convert.ToInt64(documentNumber),
            o => o.OrderByDescending(u => u.UserId),
            nameof(Usuario.UserData)
        ).FirstOrDefaultAsync();

        public Task<Usuario> ObtenerUsuarioByUsernameDocumentNumberAsync(string username, string documentNumber) =>
            Get(u =>
                        u.UserName.Equals(username) &&
                        Convert.ToInt64(u.DocumentNumber) == Convert.ToInt64(documentNumber),
                    o => o.OrderByDescending(u => u.UserId),
                    nameof(Usuario.UserData)
                )
                .FirstOrDefaultAsync();
    }
}
