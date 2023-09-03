using System.Threading.Tasks;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Domain.Enums;

namespace Spv.Usuarios.DataAccess.EntityFramework.V2
{
    public class AuditoriaLogV2Repository : GenericRepository<AuditoriaLogV2>, IAuditoriaLogV2Repository
    {
        public AuditoriaLogV2Repository(GenericDbContextV2 genericDbContextV2) : base(genericDbContextV2)
        {
        }

        public async Task<AuditoriaLogV2> SaveAuditLogAsync(
            int userId,
            EventTypes eventType,
            EventResults resultType,
            string channel,
            FechaDbServerV2 dateTimeV2,
            string extendedInfo = null
        )
        {
            var auditLog = new AuditoriaLogV2
            {
                Channel = channel,
                DateTime = dateTimeV2.Now,
                EventResultId = (int)resultType,
                EventTypeId = (int)eventType,
                UserId = userId,
                ExtendedInfo = extendedInfo
            };

            var result = await AddAsync(auditLog);
            await SaveChangesAsync();

            return result;
        }

        public async Task<AuditoriaLogV2> SaveErrorAuditLogAsync(
            EventTypes eventType,
            string channel,
            FechaDbServerV2 dateTimeV2,
            string extendedInfo = null,
            int userId = 0
        ) => await SaveAuditLogAsync(userId, eventType, EventResults.Error, channel, dateTimeV2, extendedInfo);
        
        public async Task<AuditoriaLogV2> SaveOkAuditLogAsync(
            EventTypes eventType,
            string channel,
            FechaDbServerV2 dateTimeV2,
            string extendedInfo = null,
            int userId = 0
        ) => await SaveAuditLogAsync(userId, eventType, EventResults.Ok, channel, dateTimeV2, extendedInfo);
    }
}
