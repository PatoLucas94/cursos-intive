namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris
{
    public static class ApiPersonasUris
    {
        private const string BaseAddress = "/supervielledev/desa/personas";
        private const string Version2 = BaseAddress + "/v2.0";

        public static string Personas() => $"{Version2}/personas";
        public static string Telefonos() => $"{Version2}/telefonos";
        public static string PersonasFiltro(string numeroDocumento) => $"{Personas()}/filtro?numero_documento={numeroDocumento}";
        public static string Persona(string numeroDocumento, int tipoDocumento, int paisDocumento) => 
            $"{Personas()}?numero_documento={numeroDocumento}&tipo_documento={tipoDocumento}&pais_documento={paisDocumento}";
        public static string PersonasInfo(int personId) => $"{Personas()}/{personId}";
        public static string Fisicas() => $"{Version2}/fisicas";
        public static string PersonasFisicaInfo(int personId) => $"{Fisicas()}/{personId}";
        public static string TelefonosCreacionPath(int personId) => $"{Personas()}/{personId}/telefonos";
        public static string TelefonosActualizacionPath(int personId, string telefonoId) => $"{Personas()}/{personId}/telefonos/{telefonoId}";
        public static string TelefonoDobleFactorPath(int personId) => $"{Personas()}/{personId}/telefonos/doble-factor";
        public static string TelefonoVerificacionPath(int telefonoId) => $"{Telefonos()}/{telefonoId}/verificaciones";
        public static string EmailsCreacionPath(int personId) => $"{Personas()}/{personId}/emails";
        public static string EmailsActualizacionPath(int personId, int telefonoId) => $"{EmailsCreacionPath(personId)}/{telefonoId}";
        public static string ProductosMarcaClientePath(int personId) => $"{Personas()}/{personId}/marca-cliente";
    }
}