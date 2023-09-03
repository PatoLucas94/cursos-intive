namespace Spv.Usuarios.Common.Dtos.UsuariosService.Input
{
    public class TokenBtaModelInput
    {
        public Btinreq Btinreq { get; set; }
        public string UserId { get; set; }
        public string UserPassword { get; set; }
    }

    public class Btinreq
    {
        public string Device { get; set; }
        public string Usuario { get; set; }
        public string Canal { get; set; }
        public string Requerimiento { get; set; }
    }
}
