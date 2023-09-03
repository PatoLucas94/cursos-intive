namespace Spv.Usuarios.Common.Dtos.UsuariosService.Input
{
    public class CambioDeCredencialesModelInputV2
    {
        public long PersonId { get; set; }

        public string NewUsername { get; set; }

        public string NewPassword { get; set; }
    }
}
