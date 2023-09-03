using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.EntityFramework
{
    public class UsuarioRegistradoRepository : GenericRepository<UsuarioRegistrado>, IUsuarioRegistradoRepository
    {
        public UsuarioRegistradoRepository(GenericDbContext genericDbContext) : base(genericDbContext)
        {
        }

        public Task<UsuarioRegistrado> ObtenerUsuarioRegistradoAsync(int documentTypeId, string documentNumber) =>
            Filter(u => u.DocumentTypeId == documentTypeId &&
                        Convert.ToInt64(u.DocumentNumber) == Convert.ToInt64(documentNumber)
            ).FirstOrDefaultAsync();
    }
}