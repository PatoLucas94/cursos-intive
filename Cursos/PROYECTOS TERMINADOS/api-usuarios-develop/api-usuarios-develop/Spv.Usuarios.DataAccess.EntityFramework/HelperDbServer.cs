using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.EntityFramework
{
    public class HelperDbServer : IHelperDbServer
    {
        private readonly GenericDbContext _genericDbContext;
        private readonly ILogger<HelperDbServer> _logger;

        public HelperDbServer(GenericDbContext genericDbContext, 
            ILogger<HelperDbServer> logger)
        {
            _genericDbContext = genericDbContext;
            _logger = logger;
        }

        public async Task<FechaDbServer> ObtenerFechaAsync()
        {
            _logger.LogDebug($"{nameof(ObtenerFechaAsync)}: {_genericDbContext.Database.ProviderName}");

            if (_genericDbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                return new FechaDbServer
                {
                    Now = DateTime.Now
                };
            }

            return await _genericDbContext.FechaDbServer.FromSqlRaw("Select GetDate() as [Now]").AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
