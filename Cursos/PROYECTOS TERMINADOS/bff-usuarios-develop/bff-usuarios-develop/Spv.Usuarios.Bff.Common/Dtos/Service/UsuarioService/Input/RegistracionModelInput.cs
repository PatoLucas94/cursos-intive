namespace Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input
{
    public class RegistracionModelInput
    {
        public string PersonId { get; set; }
        public int DocumentCountryId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool SmsValidated { get; set; }
        public string ChannelKey { get; set; }
        public string TyCId { get; set; }
        public string DeviceId { get; set; }
    }
}
