using System.Collections.Generic;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Output;

namespace Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output
{
    public class ValidacionExistenciaModelOutput
    {
        public string Telefono { get; set; }
        public bool Migrado { get; set; }
        public long IdPersona { get; set; }
        public string Usuario { get; set; }
        public int IdEstadoUsuario { get; set; }
        public List<TipoDocumentoModelOutput> TiposDocumento { get; set; }
        public List<PaisModelOutput> Paises { get; set; }

        [JsonIgnore]
        public string EmailSemiOfuscado { get; set; }
    }
}
