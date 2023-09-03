using System.Collections.Generic;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Client.CatalogoClient.Output;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces
{
    public interface IApiCatalogoRepository
    {
        Task<List<ApiCatalogoPaisesModelOutput>> ObtenerPaisesAsync();
        Task<List<ApiCatalogoTiposDocumentoModelOutput>> ObtenerTiposDocumentoAsync();
    }
}
