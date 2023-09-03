namespace Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input
{
    public class ApiUsuariosCambioDeCredencialesModelInput
    {
        public string id_persona { get; set; }

        public string nuevo_usuario { get; set; }

        public string nueva_clave { get; set; }
    }
}
