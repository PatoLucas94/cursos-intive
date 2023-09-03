using Spv.Usuarios.Bff.Common.Constants;

namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris
{
    public static class ApiTyCUris
    {
        private const string BaseAddress =
            "/supervielledev/desa/ventas-y-servicios/gestion-clientes/terminos-y-condiciones/api";

        private const string Version2 = BaseAddress + "/v2";

        public static string Vigente(string canal, string concepto) =>
            $"{Version2}/terminosycondiciones-vigente?{ParameterNames.Canal}={canal}&{ParameterNames.Concepto}={concepto}";
        
        public static string Aceptados() => @$"{Version2}/terminosycondiciones-aceptados";

        public static string Aceptados(string canal, string concepto, string personId) =>
            $"{Aceptados()}?{ParameterNames.Canal}={canal}&{ParameterNames.Concepto}={concepto}&{ParameterNames.IdPersona}={personId}";
    }
}
