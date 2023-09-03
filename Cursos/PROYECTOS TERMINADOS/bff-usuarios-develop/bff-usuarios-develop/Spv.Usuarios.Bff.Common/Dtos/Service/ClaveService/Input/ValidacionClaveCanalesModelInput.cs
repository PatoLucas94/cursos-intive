namespace Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Input
{
    public class ValidacionClaveCanalesModelInput
    {
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string ChannelKey { get; set; }
    }
}
