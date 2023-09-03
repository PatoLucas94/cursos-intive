namespace Spv.Usuarios.Bff.Common.Dtos.Client.CatalogoClient.Output
{
    public class ApiCatalogoTiposDocumentoModelOutput
    {
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public bool esParaInstitucionFinanciera { get; set; }
        public string tipoPersonaQueAplica { get; set; }
    }
}
