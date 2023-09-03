using System.Collections.Generic;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Output;

namespace Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Output
{
    public class PersonaModelOutput
    {
        public int Id { get; set; }
        public int PaisDocumento { get; set; }
        public int TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public bool TelefonoDobleFactor { get; set; }
        public string TipoPersona { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Genero { get; set; }
        public string FechaNacimiento { get; set; }
        public int? PaisNacimiento { get; set; }
        public List<TipoDocumentoModelOutput> TiposDocumento { get; set; }
        public List<PaisModelOutput> Paises { get; set; }
    }
}
