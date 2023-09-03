using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces
{
    public interface IApiPersonasRepository
    {
        Task<ApiPersonaModelOutput> ObtenerPersonaAsync(string numeroDocumento, int tipoDocumento, int paisDocumento);
        Task<List<ApiPersonasFiltroModelOutput>> ObtenerPersonaFiltroAsync(string numeroDocumento);
        Task<ApiPersonaInfoModelOutput> ObtenerInfoPersonaAsync(string personId);
        Task<ApiPersonasFisicaInfoModelOutput> ObtenerInfoPersonaFisicaAsync(string personId);
        Task<ApiPersonasCreacionTelefonoModelOutput> CrearTelefonoAsync(
           string personId,
           ApiPersonasCreacionTelefonoModelInput body);
        Task<ApiPersonasCreacionTelefonoModelOutput> ActualizarTelefonoAsync(
            string personId,
            ApiPersonasActualizacionTelefonoModelInput body,
            string phoneId);
        Task<ApiPersonasCreacionTelefonoModelOutput> CrearTelefonoDobleFactorAsync(
            string personId, 
            ApiPersonasCreacionTelefonoModelInput body);
        Task<ApiPersonasCreacionTelefonoModelOutput> ActualizarTelefonoDobleFactorAsync(
            string personId, 
            ApiPersonasActualizacionTelefonoModelInput body,
            string phoneId = null);
        Task<ApiPersonasCreacionEmailModelOutput> CrearEmailAsync(
            string personId, 
            ApiPersonasCreacionEmailModelInput body);
        Task<ApiPersonasCreacionEmailModelOutput> ActualizarEmailAsync(
            string personId, 
            string emailId, 
            ApiPersonasCreacionEmailModelInput body);
        Task<ApiPersonasVerificacionTelefonoModelOutput> VerificarTelefonoAsync(
            long telefonoId, 
            ApiPersonasVerificacionTelefonoModelInput body);
        Task<ApiPersonasProductosMarcaClienteModelOutput> ObtenerProductosMarcaClienteAsync(string personId);
    }
}
