using System;
using System.Threading.Tasks;
using Spv.Usuarios.DataAccess.Interface;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Enums;

namespace Spv.Usuarios.DataAccess.EntityFramework
{
    public class AuditoriaRepository : GenericRepository<AuditoriaLog>, IAuditoriaRepository
    {
        public AuditoriaRepository(GenericDbContext genericDbContext) : base(genericDbContext)
        {

        }
        public async Task SaveRegistrationAuditAsync(int pUserId, DateTime pDate)
        {
            var auditoria = new AuditoriaLog
            {
                ActionId = (int)AuditAction.RegistrationProcessIndividuals,
                ActionResultId = (int)AuditActionResult.OK,
                DateTime = pDate,
                UserId = pUserId
            };

            await AddAsync(auditoria);
            await SaveChangesAsync();
        }

        public async Task SaveLogOnAuditAsync(int pUserId, AuditAction pActionId, AuditActionResult pActionResultId, DateTime pDate)
        {
            var auditoria = new AuditoriaLog
            {
                ActionId = (int)AuditAction.LogOn,
                ActionResultId = (int)AuditActionResult.LoggedOn,
                DateTime = pDate,
                UserId = pUserId
            };

            await AddAsync(auditoria);
            await SaveChangesAsync();
        }

        public async Task SaveValidChannelKeyAuditAsync(int pUserId, AuditAction pActionId, AuditActionResult pActionResultId, DateTime pDate)
        {
            var auditoria = new AuditoriaLog
            {
                ActionId = (int)AuditAction.IsValidChannelsKey,
                ActionResultId = (int)AuditActionResult.OK,
                DateTime = pDate,
                UserId = pUserId
            };

            await AddAsync(auditoria);
            await SaveChangesAsync();
        }

        public async Task SaveWrongChannelKeyAttemptAuditAsync(int pUserId, AuditAction pActionId, AuditActionResult pActionResultId, DateTime pDate)
        {
            var auditoria = new AuditoriaLog
            {
                ActionId = (int)AuditAction.Unknown,
                ActionResultId = (int)AuditActionResult.Unknown,
                DateTime = pDate,
                UserId = pUserId
            };

            await AddAsync(auditoria);
            await SaveChangesAsync();
        }
    }
}
