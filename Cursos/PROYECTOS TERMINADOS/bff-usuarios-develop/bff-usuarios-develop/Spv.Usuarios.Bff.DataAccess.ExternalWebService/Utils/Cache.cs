namespace Spv.Usuarios.Bff.DataAccess.ExternalWebService.Utils
{
    public static class Cache
    {
        public static class Usuario
        {
            private const string Key = "UsuariosService";

            public static string ScoreOperationUpdateCredentials(params string[] segment) =>
                $"{Key}_{nameof(ScoreOperationUpdateCredentials)}_{string.Join("_", segment)}";

            public static string ObtenerUsuario(params string[] segment) =>
                $"{Key}_{nameof(ObtenerUsuario)}_{string.Join("_", segment)}";
        }

        public static class DynamicImages
        {
            public const string Key = "DynamicImagesService";

            public static string ObtenerImagenesLogin => $"{Key}_{nameof(ObtenerImagenesLogin)}";
        }

        public static class ApiTyC
        {
            public const string Key = "ApiTyCService";

            public static string ObtenerVigente(params string[] segment) =>
                $"{Key}_{nameof(ObtenerVigente)}_{string.Join("_", segment)}";

            public static string ObtenerAceptados(params string[] segment) =>
                $"{Key}_{nameof(ObtenerAceptados)}_{string.Join("_", segment)}";
        }

        public static class ApiPersona
        {
            private const string Key = "ApiPersonaService";

            public static string ObtenerInfoPersona(params string[] segment) =>
                $"{Key}_{nameof(ObtenerInfoPersona)}_{string.Join("_", segment)}";
        }
    }
}
