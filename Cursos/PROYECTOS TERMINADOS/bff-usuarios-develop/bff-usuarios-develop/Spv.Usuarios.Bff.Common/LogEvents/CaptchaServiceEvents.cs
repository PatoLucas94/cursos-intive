using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Bff.Common.LogEvents
{
    [ExcludeFromCodeCoverage]
    public static class CaptchaServiceEvents
    {
        public static EventId ExceptionCallingValidacionCaptcha { get; } = new EventId(
            ServiceEventsRanges.CaptchaServiceEventsRange,
            "Excepción llamando a Spv.Usuarios.Bff.Service.CaptchaService.ValidarCaptchaAsync"
        );
    }
}
