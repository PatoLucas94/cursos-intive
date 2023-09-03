using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.ReCaptchaService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.ReCaptchaService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface IReCaptchaService
    {
        Task<IResponse<ReCaptchaValidacionModelOutput>> ValidarAsync(IRequestBody<ReCaptchaValidacionModelInput> validacionCaptchaModel);
    }
}
