using System.Diagnostics.CodeAnalysis;
using Hangfire;
using Spv.Usuarios.Bff.Service.Interface;

namespace Spv.Usuarios.Bff.Tasks
{
    [ExcludeFromCodeCoverage]
    internal static class RecurrentTasks
    {
        public static void Run()
        {
            RecurringJob.AddOrUpdate<ITyCService>(
                nameof(ITyCService.ObtenerVigenteAsync),
                service => service.ObtenerVigenteAsync(),
                Cron.Weekly
            );

            RecurringJob.AddOrUpdate<IDynamicImagesService>(
                nameof(IDynamicImagesService.ObtenerImagenesLoginAsync),
                service => service.ObtenerImagenesLoginAsync(),
                Cron.Weekly
            );

            RecurringJob.TriggerJob(nameof(ITyCService.ObtenerVigenteAsync));
            RecurringJob.TriggerJob(nameof(IDynamicImagesService.ObtenerImagenesLoginAsync));
        }
    }
}
