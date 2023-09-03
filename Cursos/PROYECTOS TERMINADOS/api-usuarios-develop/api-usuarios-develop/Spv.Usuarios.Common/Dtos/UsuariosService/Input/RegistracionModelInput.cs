namespace Spv.Usuarios.Common.Dtos.UsuariosService.Input
{
    public class RegistracionModelInput
    {
        public string CustomerNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserStatusId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string WorkPhone { get; set; }
        public string CellPhone { get; set; }
        public string DocumentCountryId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string Cuil { get; set; }
        public bool? ReceiptExtract { get; set; }
        public bool? FullControl { get; set; }
        public int CellCompanyId { get; set; }
        public bool? ReceiveSms { get; set; }
        public bool? ReceiveMail { get; set; }
        public string PersonId { get; set; }
    }
}
