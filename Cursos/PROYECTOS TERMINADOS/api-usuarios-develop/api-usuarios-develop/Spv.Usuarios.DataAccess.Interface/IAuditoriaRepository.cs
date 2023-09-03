using System;
using System.Threading.Tasks;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Enums;

namespace Spv.Usuarios.DataAccess.Interface
{
    public interface IAuditoriaRepository : IGenericRepository<AuditoriaLog>
    {
        public Task SaveLogOnAuditAsync(int pUserId, AuditAction pActionId, AuditActionResult pActionResultId, DateTime pDate);
        public Task SaveValidChannelKeyAuditAsync(int pUserId, AuditAction pActionId, AuditActionResult pActionResultId, DateTime pDate);
        public Task SaveWrongChannelKeyAttemptAuditAsync(int pUserId, AuditAction pActionId, AuditActionResult pActionResultId, DateTime pDate);
        public Task SaveRegistrationAuditAsync(int pUserId, DateTime pDate);
    }
}
