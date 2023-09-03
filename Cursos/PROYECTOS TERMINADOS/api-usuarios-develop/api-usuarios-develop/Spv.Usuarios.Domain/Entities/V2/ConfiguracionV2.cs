namespace Spv.Usuarios.Domain.Entities.V2
{
    public class ConfiguracionV2
    {
        public int ConfigurationId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public bool IsSecurity { get; set; }
        public string Rol { get; set; }
    }
}
