namespace Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input
{
    public class ValidacionExistenciaModelInput
    {
        public string NumeroDocumento { get; set; }
        public int? TipoDocumento { get; set; }
        public int? IdPais { get; set; }
    }
}
