namespace Spv.Usuarios.Common.Dtos.UsuariosService.Input
{
    public class PerfilMigradoModelInput
    {
        public PerfilMigradoModelInput()
        {
        }

        public PerfilMigradoModelInput(long idPersona)
        {
            IdPersona = idPersona;
            FindByIdPersona = true;
        }

        public int DocumentCountryId { get; set; }

        public int DocumentTypeId { get; set; }

        public string DocumentNumber { get; set; }

        public long IdPersona { get; set; }

        public bool FindByIdPersona { get; }
    }
}