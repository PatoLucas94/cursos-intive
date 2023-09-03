using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.Dtos.Service.SoftToken.Input
{
    [ExcludeFromCodeCoverage]
    public class SoftTokenModelInput
    {
        public string Canal { get; set; }
        public string Usuario { get; set; }
        public string Identificador { get; set; }
    }
}
