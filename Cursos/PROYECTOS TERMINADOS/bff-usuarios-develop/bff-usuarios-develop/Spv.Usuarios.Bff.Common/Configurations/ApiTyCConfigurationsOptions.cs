namespace Spv.Usuarios.Bff.Common.Configurations
{
    public class ApiTyCConfigurationsOptions
    {
        public string BasePath { get; set; }

        public string TerminosYCondicionesVigentePath { get; set; }

        public string TerminosYCondicionesAceptadosPath { get; set; }

        public string ConceptoRegistracion { get; set; }

        public int CacheExpiracionTyCMinutos { get; set; }
    }
}
