using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.Constants
{
    [ExcludeFromCodeCoverage]
    public static class AppConstants
    {
        //Bff
        public const string BasePath = "obi/usuarios/api/";
        public const string HealthUrl = BasePath + "health";
        public const string SwaggerUrl = BasePath + "swagger";

        // Bff Endpoints base
        public const string BffCatalogoUrl = BasePath + "catalogo";
        public const string BffClaveCanalesUrl = BasePath + "claves/canales";
        public const string BffClaveSmsUrl = BasePath + "claves/sms";
        public const string BffPersonasUrl = BasePath + "personas";
        public const string BffUsuariosUrl = BasePath + "usuarios";
        public const string BffRecaptchaUrl = BasePath + "recaptcha";
        public const string BffRhSsoUrl = BasePath + "rh-sso";
        public const string BffTyCUrl = BasePath + "tyc";
        public const string BffSoftTokenUrl = BasePath + "softToken";
        public const string BffConfiguracionesUrl = BasePath + "configuraciones";
        public const string BffDynamicImagesUrl = BasePath + "dynamicImages";
        public const string BffBiometriaUrl = BasePath + "biometria";

        // Configuration
        public const string ClaveSmsDestinatarioMedio = "SMS";
        public const string ClaveSmsTemplateId = "token-email";
        public const string EmailDestinatarioMedio = "EMAIL";
        public const string EmailModoEnvio = "SECUENCIAL";
        public const string EmailTemplateIdCambioClave = "Usuarios-Mail-Cambio-de-Clave";
        public const string EmailTemplateIdCambioCredenciales = "Usuarios-Mail-Cambio-de-Credenciales";
        public const string EmailTemplateIdAltaCredenciales = "Usuarios-Mail-Alta-de-Credenciales";
        public const string EmailTemplateIdRecuperarUsuario = "Usuarios-Mail-Recupera-Usuario";
        public const string EmailVariableTemplateNombre = "Nombre";
        public const string EmailVariableTemplateUsuario = "Usuario";
        
        public const string Keycloak = nameof(Keycloak);

        public const string TipoTelefonoCelular = "CELULAR";

        // Argentina Código Bantotal
        public const int ArgentinaCodigoBantotal = 80;

        // TipoDocumento BT
        public const string Cuit = "C.U.I.T.";
        public const string Cuil = "C.U.I.L.";
        public const string Cdi = "C.D.I.";
        public const string Dni = "D.N.I.";
        public const string LibretaEnrolamiento = "Libreta de Enrolamiento";
        public const string LibretaCivica = "Libreta Cívica";
        public const string CedulaProv = "CEDULA PROV.";
        public const string CiPaisLimit = "C.I. PAIS LIMIT.";
        public const string PjExranjera = "P. J. EXTRANJERA";
        public const string Pasaporte = "PASAPORTE";
        public const string PfExtResExt = "P.F. EXT RES. EXT.";
        public const string ExpedJudicial = "EXPED.JUDICIAL";
        public const string CertifMigracion = "Certif. Migración";
        public const string Fci = "F.C.I.";
        public const string Vuelco = "VUELCO";
        public const string InstFin = "INST.FIN.";
        public const string TipoDocNoControlado = "Tipo Documento No Controlado";

        public static string GetTipoDocumentoDesc(int tipoDocumento)
        {
            switch (tipoDocumento)
            {
                case (int)TipoDocumento.Cuit:
                    return Cuit;
                case (int)TipoDocumento.Cuil:
                    return Cuil;
                case (int)TipoDocumento.Cdi:
                    return Cdi;
                case (int)TipoDocumento.Dni:
                    return Dni;
                case (int)TipoDocumento.Le:
                    return LibretaEnrolamiento;
                case (int)TipoDocumento.Lc:
                    return LibretaCivica;
                case (int)TipoDocumento.CedulaProv:
                    return CedulaProv;
                case (int)TipoDocumento.CiPaisLimit:
                    return CiPaisLimit;
                case (int)TipoDocumento.PjExranjera:
                    return PjExranjera;
                case (int)TipoDocumento.Pasaporte:
                    return Pasaporte;
                case (int)TipoDocumento.PfExtResExt:
                    return PfExtResExt;
                case (int)TipoDocumento.ExpedJudicial:
                    return ExpedJudicial;
                case (int)TipoDocumento.CertifMigracion:
                    return CertifMigracion;
                case (int)TipoDocumento.Fci:
                    return Fci;
                case (int)TipoDocumento.Vuelco:
                    return Vuelco;
                case (int)TipoDocumento.InstFin:
                    return InstFin;
                default:
                    return TipoDocNoControlado;
            }
        }
    }
}
