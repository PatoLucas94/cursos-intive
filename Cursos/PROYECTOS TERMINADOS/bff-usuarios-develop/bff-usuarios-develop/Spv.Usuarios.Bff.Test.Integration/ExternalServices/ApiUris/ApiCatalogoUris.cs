namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris
{
    public static class ApiCatalogoUris
    {
        private const string BaseAddress = "/supervielledev/desa/datos-de-referencia/datos-maestros/personas";
        private const string Version1 = BaseAddress + "/v1.0";

        public static string Paises() => $"{Version1}/paises";
        public static string TiposDocumento() => $"{Version1}/tipos-documento";
    }
}
