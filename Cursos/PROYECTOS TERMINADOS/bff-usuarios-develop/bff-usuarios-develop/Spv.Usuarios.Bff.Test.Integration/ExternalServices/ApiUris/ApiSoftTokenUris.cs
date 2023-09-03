namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris
{
    /// <summary>
    /// ApiSoftTokenUris
    /// </summary>
    public static class ApiSoftTokenUris
    {
        private const string BaseAddress = "/supervielledev/desa/soporte-negocio/gestion-identidad/soft-token";
        public static string Habilitado(string identificador) => $"{BaseAddress}/v1.0/tokens/"+identificador;
        public static string Valido(string identificador) => $"{BaseAddress}/v1.0/tokens/"+identificador+"/validacion";
    }
}
