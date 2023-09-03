namespace Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input
{
    public class ApiNotificacionesValidarTokenModelInput
    {
        public long id_persona { get; set; }
        public string identificador { get; set; }
        public string token { get; set; }
    }
}
