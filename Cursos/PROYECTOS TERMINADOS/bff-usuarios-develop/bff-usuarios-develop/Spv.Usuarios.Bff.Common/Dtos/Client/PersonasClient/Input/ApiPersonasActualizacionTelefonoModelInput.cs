namespace Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Input
{
    public class ApiPersonasActualizacionTelefonoModelInput
    {
        public bool confiable { get; set; }
        public string numero { get; set; }
        public int pais { get; set; }
        public bool principal { get; set; }
    }
}
