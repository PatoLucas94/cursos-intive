namespace Spv.Usuarios.Common.Configurations
{
    public class SsoConfigurationOptions
    {
        private string OpenIdConnectWithRealmUrl => $"{OpenIdConnectUrl.Replace($"{{{nameof(Realm)}}}", Realm)}";

        public string AuthUrl { get; set; }

        public string Realm { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string OpenIdConnectUrl { get; set; }

        public string IntrospectUrl => $"{TokenUrl}/introspect";

        public string TokenUrl => $"{OpenIdConnectWithRealmUrl}/token";

        public string LogoutUrl => $"{OpenIdConnectWithRealmUrl}/logout";
    }
}
