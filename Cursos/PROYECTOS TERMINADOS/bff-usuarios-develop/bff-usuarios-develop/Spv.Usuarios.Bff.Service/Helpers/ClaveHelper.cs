using System.Net.Http;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;

namespace Spv.Usuarios.Bff.Service.Helpers
{
    public static class ClaveHelper
    {
        public static async Task<HttpResponseMessage> ValidarClaveCanales(
            IApiUsuariosRepositoryV2 usuariosRepositoryV2, 
            string claveCanales, 
            int tipoDocumento, 
            string numeroDocumento )
        {
            var validacionClaveBody = new ApiUsuariosValidacionClaveCanalesModelInput
            {
                clave_canales = claveCanales,
                id_tipo_documento = tipoDocumento,
                nro_documento = numeroDocumento
            };

            return await usuariosRepositoryV2.ValidarClaveCanalesAsync(validacionClaveBody);
        }
    }
}
