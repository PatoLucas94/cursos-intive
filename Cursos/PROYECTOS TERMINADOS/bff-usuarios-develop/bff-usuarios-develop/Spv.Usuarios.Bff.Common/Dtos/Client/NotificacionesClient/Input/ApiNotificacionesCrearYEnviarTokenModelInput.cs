namespace Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input
{
    public class ApiNotificacionesCrearYEnviarTokenModelInput
    {
        public long id_persona { get; set; }
        public string identificador { get; set; }
        public Destinatario destinatario { get; set; }
        public string template_id { get; set; }
    }

    public class Destinatario
    {
        public string medio { get; set; }
        public string email { get; set; }
        public string numero { get; set; }
    }
}