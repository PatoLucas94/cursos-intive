using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.SofttokenClient.Input
{
    [ExcludeFromCodeCoverage]
    public class ApiSoftTokenValidoModelInput
    {
        public string Identificador { get; set; }
        public string Token { get; set; }
    }
}
