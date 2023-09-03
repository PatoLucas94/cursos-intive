using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.EntityFramework.V2
{
    public class HelperDbServerV2 : IHelperDbServerV2
    {
        private readonly GenericDbContextV2 _genericDbContextV2;
        private readonly ILogger<HelperDbServerV2> _logger;

        public HelperDbServerV2(GenericDbContextV2 genericDbContextV2,
            ILogger<HelperDbServerV2> logger)
        {
            _genericDbContextV2 = genericDbContextV2;
            _logger = logger;
        }

        public async Task<FechaDbServerV2> ObtenerFechaAsync()
        {
            _logger.LogDebug($"{nameof(ObtenerFechaAsync)}: {_genericDbContextV2.Database.ProviderName}");

            if (_genericDbContextV2.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                return new FechaDbServerV2
                {
                    Now = DateTime.Now
                };
            }

            return await _genericDbContextV2.FechaDbServer.FromSqlRaw("Select GetDate() as [Now]").AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
