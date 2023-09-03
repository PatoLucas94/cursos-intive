namespace Spv.Usuarios.Test.Integration.ExternalServices
{
    public static class ApiPersonasUris
    {
        private const string BaseAddress = "/supervielledev/desa/personas";
        private const string Version2 = BaseAddress + "/v2.0";

        private static string Personas() => $"{Version2}/personas";
        public static string Persona(string numeroDocumento, int tipoDocumento, int paisDocumento) => 
            $"{Personas()}?numero_documento={numeroDocumento}&tipo_documento={tipoDocumento}&pais_documento={paisDocumento}";
        public static string PersonaInfo(long personId) => $"{Personas()}/{personId}";
        private static string Fisicas() => $"{Version2}/fisicas";
        public static string PersonaFisicaInfo(long personId) => $"{Fisicas()}/{personId}";
    }
}
