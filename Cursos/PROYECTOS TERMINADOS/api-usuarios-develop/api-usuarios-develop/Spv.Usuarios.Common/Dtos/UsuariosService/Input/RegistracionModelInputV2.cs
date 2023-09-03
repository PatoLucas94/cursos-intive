namespace Spv.Usuarios.Common.Dtos.UsuariosService.Input
{
    public class RegistracionModelInputV2
    {
        public long PersonId { get; set; }
        public int DocumentCountryId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
