namespace Spv.Usuarios.Common.Constants
{
    public static class ApiUris
    {
        #region v1

        public const string Autenticacion = AppConstants.UsuariosUrlV1 + "/autenticacion";
        public const string Registracion = AppConstants.UsuariosUrlV1 + "/registracion";
        public const string ValidacionExistenciaUsuario = AppConstants.UsuariosUrlV1 + "/validacion-existencia";
        public static string Perfil_v1(string usuario) => $"v1/usuarios/{usuario}/perfil";

        #endregion

        #region v2

        public const string AutenticacionV2 = AppConstants.UsuariosUrlV2 + "/autenticacion";

        public const string AutenticacionConClaveNumericaV2 =
            AppConstants.UsuariosUrlV2 + "/autenticacion/clave-numerica";

        public const string CambiarClaveUsuarioV2 = AppConstants.UsuariosUrlV2 + "/cambio-de-clave";
        public const string ValidacionExistenciaBtaV2 = AppConstants.UsuariosUrlV2 + "/validacion-existencia-bta";
        public const string CambiarCredencialesUsuarioV2 = AppConstants.UsuariosUrlV2 + "/cambio-de-credenciales";
        public const string MigracionV2 = AppConstants.UsuariosUrlV2 + "/migracion";

        public const string RegistracionV2 = AppConstants.UsuariosUrlV2 + @"/registracion";
        public const string ClaveCanalesValidacionV2 = AppConstants.ClaveCanalesUrlV2 + "/validacion";
        public const string ClaveCanalesInhabilitacionV2 = AppConstants.ClaveCanalesUrlV2 + "/inhabilitacion";
        public const string ObtenerEstadoV2 = AppConstants.ClaveCanalesUrlV2 + "/obtener-estado";
        public const string ValidacionExistenciaUsuarioV2 = AppConstants.UsuariosUrlV2 + "/validacion-existencia";
        public const string ObtenerUsuarioV2 = AppConstants.UsuariosUrlV2 + "/obtener-usuario";

        public static string Perfil_v2(long idPersona) => @$"v2/usuarios/{idPersona}/perfil";
        public static string PerfilMigradoV2(long idPersona) => @$"v2/usuarios/perfil-migrado/{idPersona}";

        public static string PerfilMigradoV2(string documentNumber, int documentCountryId, int documentTypeId) =>
            $"v2/usuarios/perfil-migrado?nro_documento={documentNumber}&id_tipo_documento={documentTypeId}&id_pais={documentCountryId}";

        public const string CambiarEstadoV2 = "v2/usuarios/cambiar-estado";

        public const string AutenticacionKeycloakV2 = AppConstants.SsoUrlV2 + "/autenticacion";
        public const string ActualizarTokenKeycloakV2 = AppConstants.SsoUrlV2 + "/actualizar-token";
        public const string VerificarTokenKeycloakV2 = AppConstants.SsoUrlV2 + "/verificar-token";
        public const string CerrarSesionKeycloakV2 = AppConstants.SsoUrlV2 + "/cerrar-sesion";

        public const string TerminosCondicionesHabilitados = AppConstants.ConfiguracionesUrlV2 + "/terminos-condiciones-habilitado";
        public const string LoginHabilitado = AppConstants.ConfiguracionesUrlV2 + "/login-habilitado";
        public const string MensajeDefaultLoginDeshabilitado = AppConstants.ConfiguracionesUrlV2 + "/mensaje-default-login-deshabilitado";
        public const string MensajeLoginDeshabilitado = AppConstants.ConfiguracionesUrlV2 + "/mensaje-login-deshabilitado";

        public const string ObtenerImagesLogin = AppConstants.DynamicImagesUrlV2 + "/obtener-images-login";


        #endregion
    }
}
