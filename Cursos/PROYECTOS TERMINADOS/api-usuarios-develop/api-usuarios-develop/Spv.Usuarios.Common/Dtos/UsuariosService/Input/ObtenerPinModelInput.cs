namespace Spv.Usuarios.Common.Dtos.UsuariosService.Input
{
    public class ObtenerPinModelInput
    {
        public BtinreqPin Btinreq { get; set; }
        public string PaisDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Modo { get; set; }
    }

    public class BtinreqPin
    {
        public string Canal { get; set; }
        public string Token { get; set; }
        public string Usuario { get; set; }
        public string Requerimiento { get; set; }
        public string Device { get; set; }
    }
}
