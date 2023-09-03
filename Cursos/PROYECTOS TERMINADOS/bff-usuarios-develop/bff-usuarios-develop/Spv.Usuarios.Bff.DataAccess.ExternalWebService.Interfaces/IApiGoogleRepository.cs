using System.Net.Http;
using System.Threading.Tasks;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces
{
    public interface IApiGoogleRepository
    {
        Task<HttpResponseMessage> ReCaptchaV3ValidarTokenAsync(string token);
    }
}
