namespace Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input
{
    public class CambioDeClaveModelInput
    {
        public string PersonId { get; set; }
        public string NewPassword { get; set; }
        public string ChannelKey { get; set; }
        public bool IsChannelKey { get; set; }
        public string DeviceId { get; set; }      
    }
}
