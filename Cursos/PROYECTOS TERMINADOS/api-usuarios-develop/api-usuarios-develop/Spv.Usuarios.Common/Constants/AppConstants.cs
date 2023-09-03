namespace Spv.Usuarios.Common.Constants
{
    public static class AppConstants
    {
        // Configuration
        public const string ConfigurationRolConfiguration = "CONFIGURATION";
        public const string ConfigurationTypeUsers = "Users";
        public const string ConfigurationTypeChannelsKey = "ChannelsKey";
        public const string ConfigurationTypeDigitalCredentials = "DigitalCredentials";

        public const string CantidadDeIntentosDeLoginKey = "LoginAttemptsNumber";

        public const string CantidadDeIntentosDeClaveDeCanalesKey = "ChannelsKeyAttempsNumber";
        public const string CantidadDeHistorialDeCambiosDeClave = "PasswordHistoryNumber";
        public const string CantidadDeHistorialDeCambiosDeNombreUsuario = "UserNameHistoryNumber";

        public const string DiasParaForzarCambioDeClaveKey = "ForceChangePasswordDays";

        public const int IntentosLoginFallidosNsbtDefault = 3;

        public const string CanalHabilitadoParaCambioDeClaveSoloModeloNuevo = "BTA";

        public const string RegistracionNuevoModeloHabilitado = "RegistrationInNewModelIsEnabled";
        public const string TerminosYCondicionesHabilitado = "EnableTyCLogin";
        public const string LogInDisabled = "LogInDisabled";
        public const string LogInDefaultDisabledMessage = "LogInDefaultDisabledMessage";
        public const string LogInDisabledMessage = "LogInDisabledMessage";

        public const string UsuariosUrlV1 = "v1/usuarios";
        public const string UsuariosUrlV2 = "v2/usuarios";
        public const string ClaveCanalesUrlV2 = "v2/clave-canales";
        public const string SsoUrlV2 = "v2/sso";
        public const string ConfiguracionesUrlV2 = "v2/configuraciones";
        public const string DynamicImagesUrlV2 = "v2/dynamicImages";
    }
}
