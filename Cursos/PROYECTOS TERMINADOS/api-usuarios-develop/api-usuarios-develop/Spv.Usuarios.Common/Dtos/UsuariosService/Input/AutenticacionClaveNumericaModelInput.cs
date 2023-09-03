namespace Spv.Usuarios.Common.Dtos.UsuariosService.Input
{
    public class AutenticacionClaveNumericaModelInput
    {
        public string Password { get; set; }
        public int DocumentCountryId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
    }
}
