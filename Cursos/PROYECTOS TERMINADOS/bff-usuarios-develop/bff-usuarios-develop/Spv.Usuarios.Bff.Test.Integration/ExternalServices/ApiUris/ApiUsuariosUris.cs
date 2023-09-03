namespace Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris
{
    public static class ApiUsuariosUris
    {
        private const string BaseAddress = "/supervielledev/desa/soporte-negocio/gestion-identidad/usuarios";
        private const string Version1 = BaseAddress + "/v1";
        private const string Version2 = BaseAddress + "/v2.0";

        public static string Perfil(string username) => $"{Version1}/usuarios/{username}/perfil";
        public static string ValidarExistenciaHbi() => $"{Version1}/usuarios/validacion-existencia";
        public static string ValidacionClaveCanales() => $"{Version2}/clave-canales/validacion";
        public static string ObtenerEstadoClaveCanales() => $"{BaseAddress}/v2/clave-canales/obtener-estado";
        public static string InhabilitacionClaveCanales() => $"{Version2}/clave-canales/inhabilitacion";
        public static string RegistracionV2() => $"{Version2}/usuarios/registracion";
        public static string Migracion() => $"{Version2}/usuarios/migracion";
        public static string ValidarExistencia() => $"{Version2}/usuarios/validacion-existencia";
        public static string CambioDeCredenciales() => $"{Version2}/usuarios/cambio-de-credenciales";
        public static string CambioDeClave() => $"{Version2}/usuarios/cambio-de-clave";
        public static string PerfilPathV2(long personId) => $"{Version2}/usuarios/{personId}/perfil";
        public static string ActualizarPersonId() => $"{Version2}/usuarios/actualizar-personId";
        public static string ObtenerUsuario() => $"{Version2}/usuarios/obtener-usuario";

        public static string TerminosYCondicionesHabilitado() =>
            $"{BaseAddress}/v2/configuraciones/terminos-condiciones-habilitado";

        public static string LoginHabilitado() => $"{BaseAddress}/v2/configuraciones/login-habilitado";

        public static string ObtenerMensajeLoginDeshabilitado() =>
            $"{BaseAddress}/v2/configuraciones/mensaje-login-deshabilitado";

        public static string ObtenerImagenesLogin() => $"{BaseAddress}/v2/dynamicImages/obtener-imagenes-login";
        public static string SsoAutenticacion => $"{BaseAddress}/v2/sso/autenticacion";
    }
}
