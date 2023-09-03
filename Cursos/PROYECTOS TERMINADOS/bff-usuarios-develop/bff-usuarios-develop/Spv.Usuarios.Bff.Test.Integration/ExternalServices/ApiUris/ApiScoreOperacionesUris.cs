namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris
{
    public static class ApiScoreOperacionesUris
    {
        private const string BaseAddress = "/supervielledev/desa/operaciones-cross-productos/cross-canales/score-operacion";
        private const string Version1 = BaseAddress + "/v1.0";
        public static string UpdateCredentials() => $"{Version1}/no-monetario/actualizar-credenciales";

        public static string RegistracionScore() => $"{Version1}/no-monetario/registracion";

        public static string IniciarSesionScore() => $"{Version1}/no-monetario/inicio-de-sesion";
    }
}
