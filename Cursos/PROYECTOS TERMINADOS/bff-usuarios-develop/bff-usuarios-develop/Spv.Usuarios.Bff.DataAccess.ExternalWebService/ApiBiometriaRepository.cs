using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Input;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService
{
    public class ApiBiometriaRepository : IApiBiometriaRepository
    {
        private readonly IApiBiometriaHelper _apiBiometriaHelper;

        public ApiBiometriaRepository(IApiBiometriaHelper apiBiometriaHelper) =>
            _apiBiometriaHelper = apiBiometriaHelper;

        private string ObtenerPathConBase(string path) => string.Concat(_apiBiometriaHelper.BasePath(), path);

        public async Task<HttpResponseMessage> AutenticacionAsync(BiometriaAutenticacionModelInput body) =>
            await _apiBiometriaHelper.PostRequestAsync(
                ObtenerPathConBase(_apiBiometriaHelper.AutenticacionPath()),
                body,
                _apiBiometriaHelper.XCanal(),
                _apiBiometriaHelper.XUsuario(),
                _apiBiometriaHelper.XAplicacion()
            );
    }
}
