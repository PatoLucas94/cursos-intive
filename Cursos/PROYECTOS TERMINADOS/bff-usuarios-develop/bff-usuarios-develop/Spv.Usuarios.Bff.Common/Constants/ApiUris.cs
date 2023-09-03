namespace Spv.Usuarios.Bff.Common.Constants
{
    public static class ApiUris
    {
        #region Catalogo

        public static string Paises()
        {
            return $@"{AppConstants.BffCatalogoUrl}/paises";
        }

        public static string TiposDocumento()
        {
            return $@"{AppConstants.BffCatalogoUrl}/tipos-documento";
        }

        #endregion

        #region Claves

        public const string ValidacionClaveCanales = AppConstants.BffClaveCanalesUrl + "/validacion";

        public const string ValidacionClaveCanalesConIdPersona =
            AppConstants.BffClaveCanalesUrl + "/validacion-idpersona";

        public const string ValidacionClaveSms = AppConstants.BffClaveSmsUrl + "/validacion";

        public const string ObtenerEstadoClaveCanales = AppConstants.BffClaveCanalesUrl + "/obtener-estado";

        public static string GeneracionClaveSms()
        {
            return $@"{AppConstants.BffClaveSmsUrl}";
        }

        #endregion

        #region Personas

        public static string Persona(int idPais, int idTipoDocumento, string nroDocumento)
        {
            return
                $@"{AppConstants.BffPersonasUrl}?id_pais={idPais}&id_tipo_documento={idTipoDocumento}&numero_documento={nroDocumento}";
        }

        public static string PersonasFiltro(string nroDocumento, int? tipoDocumento, int? idPais)
        {
            return $@"{AppConstants.BffPersonasUrl}/filtro?{ParameterNames.NroDocumento}={nroDocumento}&" +
                   $@"{ParameterNames.IdTipoDocumento}={tipoDocumento?.ToString() ?? string.Empty}&" +
                   $@"{ParameterNames.IdPais}={idPais?.ToString() ?? string.Empty}";
        }

        #endregion

        #region Usuarios

        public static string Perfil(string nombreUsuario)
        {
            return $@"{AppConstants.BffUsuariosUrl}/perfil?{ParameterNames.Usuario}={nombreUsuario}";
        }

        public static string PerfilV2(long idPersona)
        {
            return $@"{AppConstants.BffUsuariosUrl}/perfilv2?{ParameterNames.IdPersona}={idPersona}";
        }

        public static string ValidacionExistenciaHbi()
        {
            var uri = $@"{AppConstants.BffUsuariosUrl}/validacion-existencia-hbi";

            return uri;
        }

        public static string ValidacionExistencia(string nroDocumento, int? tipoDocumento, int? idPais)
        {
            var uri =
                $@"{AppConstants.BffUsuariosUrl}/validacion-existencia?{ParameterNames.NroDocumento}={nroDocumento}&" +
                $@"{ParameterNames.IdTipoDocumento}={tipoDocumento?.ToString() ?? string.Empty}&" +
                $@"{ParameterNames.IdPais}={idPais?.ToString() ?? string.Empty}";

            return uri;
        }

        public const string CambioDeCredenciales = AppConstants.BffUsuariosUrl + "/cambio-de-credenciales";

        public const string CambioDeClave = AppConstants.BffUsuariosUrl + "/cambio-de-clave";

        public const string AutenticacionV2 = AppConstants.BffUsuariosUrl + "/autenticacion";

        public const string MigracionV2 = AppConstants.BffUsuariosUrl + "/migracion";

        public const string RegistracionV2 = AppConstants.BffUsuariosUrl + "/registracion";

        public const string RecuperarUsuario = AppConstants.BffUsuariosUrl + "/recuperar-usuario";

        public const string LoginHabilitado = AppConstants.BffConfiguracionesUrl + "/login-habilitado";

        public const string ObtenerImagesLogin = AppConstants.BffDynamicImagesUrl + "/obtener-imagenes-login";

        #endregion

        #region TyC

        public static string TyCVigente(string concepto) =>
            $@"{AppConstants.BffTyCUrl}/vigente?{ParameterNames.Concepto}={concepto}";

        public static string TyCAceptadosByPersonId(string personId) =>
            $"{AppConstants.BffTyCUrl}/aceptados/{personId}";

        public static string TyCAceptadosByNroDocumentoUsuario(string numeroDoc, string usuario) =>
            $"{AppConstants.BffTyCUrl}/aceptados?{ParameterNames.Usuario}={usuario}&{ParameterNames.NroDocumento}={numeroDoc}";

        #endregion

        #region SoftToken

        public static string SoftTokenValido()
        {
            return $@"{AppConstants.BffSoftTokenUrl}/validacion";
        }

        public static string SoftTokenHabilitado()
        {
            return $@"{AppConstants.BffSoftTokenUrl}/habilitado";
        }

        #endregion

        #region Captcha

        public static string ValidacionTokenCaptcha()
        {
            return $@"{AppConstants.BffRecaptchaUrl}/validacion";
        }

        #endregion

        #region RH-SSO

        public static string Autenticacion => $@"{AppConstants.BffRhSsoUrl}/autenticacion";

        #endregion

        #region Biometria

        public static string BiometriaAutenticacion => $@"{AppConstants.BffBiometriaUrl}/autenticacion";

        #endregion
    }
}
