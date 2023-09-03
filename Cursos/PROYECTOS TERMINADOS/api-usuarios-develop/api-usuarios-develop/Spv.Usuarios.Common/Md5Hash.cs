using System.Security.Cryptography;
using System.Text;

namespace Spv.Usuarios.Common
{
    public static class Md5Hash
    {
        public static string Compute(string value)
        {
            var sb = new StringBuilder();

            // Initialize a MD5 hash object
            using (var md5 = MD5.Create())
            {
                // Compute the hash of the given string
                var hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(value));

                // Convert the byte array to string format
                foreach (var b in hashValue)
                    sb.Append($"{b:X2}");
            }

            return sb.ToString();
        }
    }
}
