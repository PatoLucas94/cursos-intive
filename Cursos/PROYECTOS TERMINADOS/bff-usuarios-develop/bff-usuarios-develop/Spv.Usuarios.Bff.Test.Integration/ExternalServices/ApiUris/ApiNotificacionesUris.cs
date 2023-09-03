namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris
{
    public static class ApiNotificacionesUris
    {
        private const string BaseAddress = "/supervielledev/desa/api-notificaciones";
        private const string Version1 = BaseAddress + "/v1.0";

        public static string CrearYEnviarToken() => $"{Version1}/token";

        public static string ValidarToken() => $"{Version1}/token/validacion";

        public static string EnviarEmail() => $"{Version1}/notificaciones";
    }
}
