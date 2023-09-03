using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.Constants
{
    [ExcludeFromCodeCoverage]
    public static class ExternalServicesNames
    {
        public const string ApiCatalogo = "ApiCatalogo";
        public const string ApiConnect = "ApiConnect";
        public const string ApiNotificaciones = "ApiNotificaciones";
        public const string ApiPersonas = "ApiPersonas";
        public const string ApiServices = "ApiServices";
        public const string ApiSoftToken = "ApiSoftToken";
        public const string ApiTyC = "ApiTyC";
        public const string ApiUsuarios = "ApiUsuarios";
        public const string ApiGoogle = "ApiGoogle";
        public const string ApiScoreOperaciones = "ApiScoreOperaciones";
        public const string OpenApiInfo = "OpenApiInfo";
        public const string Configuraciones = "Configuraciones";
        public const string Hangfire = nameof(Hangfire);
        public const string ApiBiometria = nameof(ApiBiometria);
    }
}
