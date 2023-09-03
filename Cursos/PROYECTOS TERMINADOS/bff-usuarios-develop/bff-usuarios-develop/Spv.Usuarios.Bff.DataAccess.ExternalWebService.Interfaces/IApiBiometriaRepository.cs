using System.Net.Http;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Input;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces
{
    public interface IApiBiometriaRepository
    {
        Task<HttpResponseMessage> AutenticacionAsync(BiometriaAutenticacionModelInput body);
    }
}
