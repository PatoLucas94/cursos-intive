using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Common.Configurations;

namespace Spv.Usuarios.Service.Helpers
{
    public class Encryption : IEncryption
    {
        private readonly IOptions<ApiUsuariosConfigurationOptions> _apiUsuariosConfigurationOptions;

        public Encryption(IOptions<ApiUsuariosConfigurationOptions> apiUsuariosConfigurationOptions)
        {
            _apiUsuariosConfigurationOptions = apiUsuariosConfigurationOptions;
        }

        public string GetHash(string textToHash)
        {
            // secret key shared by sender and receiver.
            // get secret key
            var key = GetSecretKey();
            var secretKey = Encoding.UTF8.GetBytes(key);

            // Initialize the keyed hash object.
            HMACSHA256 myhmacsha256 = new HMACSHA256(secretKey);

            // Compute the hash of the text.
            byte[] bytedText = Encoding.UTF8.GetBytes(textToHash ?? string.Empty);

            byte[] hashValue = myhmacsha256.ComputeHash(bytedText);

            // Base-64 Encode the results and strip off ending '==', if it exists
            var result = Convert.ToBase64String(hashValue).TrimEnd("=".ToCharArray());

            // set response
            return result;
        }

        public string EncryptChannelsKey(string channelKey, string debitCardNumber)
        {
            //Bloque 1
            string block1 = "0" + channelKey.Length.ToString() + channelKey;
            block1 = block1.PadRight(16, 'F');

            //Bloque 2
            int prefixLength = debitCardNumber.Length - 13;
            string block2 = debitCardNumber.Substring(prefixLength, 12).PadLeft(16, '0');

            byte[] xorBlocks = new byte[8];
            byte[] byteBlock1 = HexStringToByteArray(block1);
            byte[] byteBlock2 = HexStringToByteArray(block2);

            for (int i = 0; i < 8; i++)
            {
                xorBlocks[i] = (byte)(byteBlock1[i] ^ byteBlock2[i]);
            }

            /*Se genera el crypto porvider para la encripción especificada*/
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            cryptoProvider.Padding = PaddingMode.None;
            cryptoProvider.Mode = CipherMode.ECB;

            string exchangeKey = GetBanelcoExchangeKey();
            cryptoProvider.Key = HexStringToByteArray(exchangeKey);
            cryptoProvider.IV = HexStringToByteArray(exchangeKey);

            string criptogram = EncryptExchangeKey(xorBlocks, cryptoProvider);
            /*************************************************************************/
            return criptogram.Replace("-", string.Empty);
        }

        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <returns>The encrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be 
        /// thrown when the original string is null or empty.</exception>
        private static string EncryptExchangeKey(byte[] cipherData, SymmetricAlgorithm cryptoProvider)
        {
            if (cipherData == null)
            {
                throw new ArgumentNullException("cipherData", "The string which needs to be encrypted can not be null.");
            }

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(cryptoProvider.Key, cryptoProvider.IV), CryptoStreamMode.Write);
            cryptoStream.Write(cipherData, 0, cipherData.Length);

            cryptoStream.Close();
            byte[] encryptedData = memoryStream.ToArray();
            return BitConverter.ToString(encryptedData);
        }

        /// <summary>
        /// Obtiene el valor de la clave ubicada en el config
        /// </summary>
        /// <returns>String representando la clave</returns>
        private string GetSecretKey()
        {
            var retoResponseKey = _apiUsuariosConfigurationOptions.Value.RetoResponseKey;

            return retoResponseKey ?? throw new Exception("No se encontró Secret Key.");
        }

        /// <summary>
        /// Obtiene el valor de la clave Banelco Exchange ubicada en el config
        /// </summary>
        /// <returns>String representando la clave</returns>
        private string GetBanelcoExchangeKey()
        {
            var banelcoExchangeKey = _apiUsuariosConfigurationOptions.Value.BanelcoExchangeKey;

            return banelcoExchangeKey ?? throw new Exception("No se encontró Banelco Exchange key.");
        }

        private byte[] HexStringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return bytes;
        }
    }
}
