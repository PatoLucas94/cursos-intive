using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Common.Configurations;

namespace Spv.Usuarios.Service.Helpers
{
    public class TDesEncryption : ITDesEncryption
    {
        private readonly IOptions<ApiUsuariosConfigurationOptions> _apiUsuariosConfigurationOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        public TDesEncryption(IOptions<ApiUsuariosConfigurationOptions> apiUsuariosConfigurationOptions)
        {
            _apiUsuariosConfigurationOptions = apiUsuariosConfigurationOptions;
        }


        public string Decrypt(string encryptedText)
        {
            byte[] b_key = Encoding.UTF8.GetBytes(_apiUsuariosConfigurationOptions.Value.NsbtEncryptionKey ?? throw new Exception($"No se encontró '{nameof(_apiUsuariosConfigurationOptions.Value.NsbtEncryptionKey)}' key."));

            if(b_key.Length > 24) Array.Resize(ref b_key, 192 / 8);

            TripleDESCryptoServiceProvider Tripledes = new TripleDESCryptoServiceProvider();
            Tripledes.Mode = CipherMode.CBC;
            Tripledes.Padding = PaddingMode.PKCS7;
            Tripledes.IV = new byte[8];
            Tripledes.Key = b_key;

            var result = DecodeHex(encryptedText);

            ICryptoTransform cTransform = Tripledes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(result, 0, result.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        public string Encrypt(string plainText)
        {
            byte[] b_input = Encoding.UTF8.GetBytes(plainText);
            byte[] b_key = Encoding.UTF8.GetBytes(_apiUsuariosConfigurationOptions.Value.NsbtEncryptionKey ?? throw new Exception($"No se encontró '{nameof(_apiUsuariosConfigurationOptions.Value.NsbtEncryptionKey)}' key."));

            if (b_key.Length > 24) Array.Resize(ref b_key, 192 / 8);

            TripleDESCryptoServiceProvider Tripledes = new TripleDESCryptoServiceProvider();
            Tripledes.Mode = CipherMode.CBC;
            Tripledes.Padding = PaddingMode.PKCS7;
            Tripledes.IV = new byte[8];
            Tripledes.Key = b_key;

            ICryptoTransform cTransform = Tripledes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(b_input, 0, b_input.Length);

            return EncodeHex(resultArray.ToArray());
        }

        private static string EncodeHex(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private static byte[] DecodeHex(string hex)
        {
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
    }
}
