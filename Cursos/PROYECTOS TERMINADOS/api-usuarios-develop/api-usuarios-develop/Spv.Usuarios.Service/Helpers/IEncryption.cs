namespace Spv.Usuarios.Service.Helpers
{
    public interface IEncryption
    {
        string GetHash(string textToHash);
        string EncryptChannelsKey(string channelKey, string debitCardNumber);
    }
}
