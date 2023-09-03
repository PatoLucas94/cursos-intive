namespace Spv.Usuarios.Bff.Common.Configurations
{
    public class ApiPersonasConfigurationsOptions
    {
        public string BasePath { get; set; }

        public string PersonasPath { get; set; }

        public string PersonasFiltroPath { get; set; }

        public string PersonasInfoPath { get; set; }

        public string PersonasFisicaInfoPath { get; set; }

        public string TelefonosPath { get; set; }

        public string TelefonosDobleFactorPath { get; set; }

        public string TelefonosVerificacionPath { get; set; }

        public string EmailsPath { get; set; }

        public string ProductosMarcaClientePath { get; set; }

        public int CacheExpiracionTyCMinutos { get; set; }
    }
}
