using System.Collections.Generic;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input
{
    public class ApiNotificacionesEnviarEmailModelInput
    {
        public long id_persona { get; set; }
        public string modo_envio { get; set; }
        public List<Destinatario> destinatarios { get; set; }
        public string template_id { get; set; }
        public List<VariablesTemplate> variables_template { get; set; }
    }

    public class VariablesTemplate
    {
        public string clave { get; set; }
        public string valor { get; set; }
    }
}