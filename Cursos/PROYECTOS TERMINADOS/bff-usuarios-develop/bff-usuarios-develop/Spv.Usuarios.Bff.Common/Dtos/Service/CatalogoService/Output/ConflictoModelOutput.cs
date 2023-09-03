using System.Collections.Generic;

namespace Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Output
{
    public class ConflictoModelOutput
    {
        public List<TipoDocumentoModelOutput> TiposDocumento { get; set; }
        public List<PaisModelOutput> Paises { get; set; }
    }
}
