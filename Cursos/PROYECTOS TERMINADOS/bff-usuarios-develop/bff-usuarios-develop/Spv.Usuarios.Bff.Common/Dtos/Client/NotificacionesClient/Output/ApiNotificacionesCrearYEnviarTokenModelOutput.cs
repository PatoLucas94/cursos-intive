namespace Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Output
{
    public class ApiNotificacionesCrearYEnviarTokenModelOutput
    {
        public long? id_notificacion { get; set; }
        public long? id_persona { get; set; }
        public string estado { get; set; }
        public string identificador { get; set; }
    }
}
