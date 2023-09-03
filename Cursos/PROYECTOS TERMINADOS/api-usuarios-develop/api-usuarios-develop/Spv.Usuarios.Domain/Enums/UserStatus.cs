namespace Spv.Usuarios.Domain.Enums
{
    public enum UserStatus : byte
    {
        Pending = 1,
        InProgress = 2,
        Active = 3,
        Declined = 4,
        Blocked = 5,
        Migrated = 6,
        Inactive = 7,
        Suspended = 9
    }
}