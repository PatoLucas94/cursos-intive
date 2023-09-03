namespace Spv.Usuarios.Bff.Common.Configurations
{
    public class ApiNotificacionesConfigurationsOptions
    {
        public string BasePath { get; set; }
        public string CrearYEnviarTokenPath { get; set; }
        public string ValidarTokenPath { get; set; }
        public string EnviarEmailPath { get; set; }
        public string TiempoRespuestaEnvioMails { get; set; }        
    }
}
