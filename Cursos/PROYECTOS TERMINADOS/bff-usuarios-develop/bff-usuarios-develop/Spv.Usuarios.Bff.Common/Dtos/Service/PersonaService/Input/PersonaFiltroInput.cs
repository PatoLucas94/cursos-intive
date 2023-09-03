namespace Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Input
{
    public class PersonaFiltroInput
    {
        public string NumeroDocumento { get; set; }
        public int? TipoDocumento { get; set; }
        public int? IdPais { get; set; }
    }
}
