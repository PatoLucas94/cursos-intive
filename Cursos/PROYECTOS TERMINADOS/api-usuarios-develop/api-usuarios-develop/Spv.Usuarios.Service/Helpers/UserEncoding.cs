using System;
using System.Text;

namespace Spv.Usuarios.Service.Helpers
{
    public static class UserEncoding
    {
        public static string Base64Encode(
            string documentNumber,
            string userName,
            string canal,
            bool usernameLowerCase = false
        )
        {
            userName = usernameLowerCase ? userName.ToLower() : userName;

            return Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{documentNumber?.Trim()}|{userName?.Trim()}|{canal?.Trim().ToLower()}")
            );
        }

        public static (string DocumentNumber, string UserName, string Canal) Base64Decode(string base64)
        {
            var data = Encoding.UTF8.GetString(Convert.FromBase64String(base64)).Split("|");
            return (data[0], data[1], data[2]);
        }
    }
}
