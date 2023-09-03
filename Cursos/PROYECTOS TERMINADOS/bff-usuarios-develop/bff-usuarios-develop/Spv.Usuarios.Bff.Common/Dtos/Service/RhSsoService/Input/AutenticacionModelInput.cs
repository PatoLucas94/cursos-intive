namespace Spv.Usuarios.Bff.Common.Dtos.Service.RhSsoService.Input
{
    public class AutenticacionModelInput
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DocumentNumber { get; set; }
        public string DeviceId { get; set; }
    }
}
