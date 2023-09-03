namespace Spv.Usuarios.Common.Dtos.UsuariosService.Input
{
    public class CambioDeClaveModelInputV2
    {
        public long PersonId { get; set; }

        public string CurrentPasword { get; set; }

        public string NewPassword { get; set; }
        
        public string Gateway { get; set; }
    }
}
