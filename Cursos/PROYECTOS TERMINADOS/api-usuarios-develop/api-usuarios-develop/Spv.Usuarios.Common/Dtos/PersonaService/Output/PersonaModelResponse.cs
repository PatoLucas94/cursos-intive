namespace Spv.Usuarios.Common.Dtos.PersonaService.Output
{

    public class PersonaModelResponse
    {
        public int id { get; set; }
        public Links links { get; set; }
        public string tipo_persona { get; set; }
    }
    public class Links
    {
        public bool empty { get; set; }
    }
}
