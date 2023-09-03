using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;

namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers
{
    [ExcludeFromCodeCoverage]
    public class RhSsoHelper : IRhSsoHelper
    {
        public string XAplicacion() => AppConstants.Keycloak;
    }
}
