namespace Spv.Usuarios.Service.Helpers
{
    public interface ITDesEncryption
    {
        string Encrypt(string plainText);

        string Decrypt(string encryptedText);
    }
}
