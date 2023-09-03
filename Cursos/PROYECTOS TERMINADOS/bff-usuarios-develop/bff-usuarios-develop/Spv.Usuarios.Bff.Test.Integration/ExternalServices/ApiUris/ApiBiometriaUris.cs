namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris
{
    public static class ApiBiometriaUris
    {
        private const string BaseAddress = "/supervielledev/desa/api-biometria-facephi";
        private const string Version1 = BaseAddress + "/v1.0";

        public static string Autenticacion => $"{Version1}/autenticaciones";
    }
}
