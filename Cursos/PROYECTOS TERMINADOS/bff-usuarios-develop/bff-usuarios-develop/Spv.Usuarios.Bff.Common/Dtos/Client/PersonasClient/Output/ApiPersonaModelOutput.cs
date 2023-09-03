namespace Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output
{
    public class ApiPersonaModelOutput
    {
        public int id { get; set; }
        public Links _links { get; set; }
        public string tipo_persona { get; set; }
    }
    public class Links
    {
        public bool empty { get; set; }
    }
}
