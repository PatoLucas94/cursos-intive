namespace Spv.Usuarios.Domain.Enums
{
    public enum EventTypes
    {
        Authentication = 1,
        Profile = 2,
        Registration = 3,
        Migration = 4,
        PasswordChange = 5,
        CredentialsChange = 6,
        AuthenticationNumericPassword = 7,
        RuleCreation = 8,
        RuleModification = 9,
        ChannelKey = 10,
        ChangeUserStatus = 11,
        AuthenticationKeycloak = 12,
        IntrospectKeycloak = 13,
        LogoutKeycloak = 14,
        RefreshAccessKeycloak = 15
    }
}
