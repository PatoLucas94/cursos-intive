using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Domain.Enums;

namespace Spv.Usuarios.DataAccess.Interface.V2
{
    public interface IAuditoriaLogV2Repository : IGenericRepository<AuditoriaLogV2>
    {
        Task<AuditoriaLogV2> SaveAuditLogAsync(
            int userId,
            EventTypes eventType,
            EventResults resultType,
            string channel,
            FechaDbServerV2 dateTimeV2,
            string extendedInfo = null
        );

        Task<AuditoriaLogV2> SaveErrorAuditLogAsync(
            EventTypes eventType,
            string channel,
            FechaDbServerV2 dateTimeV2,
            string extendedInfo = null,
            int userId = 0
        );
        
        Task<AuditoriaLogV2> SaveOkAuditLogAsync(
            EventTypes eventType,
            string channel,
            FechaDbServerV2 dateTimeV2,
            string extendedInfo = null,
            int userId = 0
        );
    }
}
