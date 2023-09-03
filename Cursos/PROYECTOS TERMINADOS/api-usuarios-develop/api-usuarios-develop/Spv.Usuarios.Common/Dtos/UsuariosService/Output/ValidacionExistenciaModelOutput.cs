namespace Spv.Usuarios.Common.Dtos.UsuariosService.Output
{
    public class ValidacionExistenciaModelOutput
    {
        public long? PersonId { get; set; }
        public bool Migrated { get; set; }
        public string Username { get; set; }
        public int UserStatusId { get; set; }
    }
}
