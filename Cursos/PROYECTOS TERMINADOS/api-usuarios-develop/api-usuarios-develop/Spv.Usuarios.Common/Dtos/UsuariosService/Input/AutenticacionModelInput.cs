namespace Spv.Usuarios.Common.Dtos.UsuariosService.Input
{
    public class AutenticacionModelInput
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string DocumentNumber { get; set; }

        public string Hash() => Md5Hash.Compute($"{UserName}{DocumentNumber}{Password}");
    }
}
