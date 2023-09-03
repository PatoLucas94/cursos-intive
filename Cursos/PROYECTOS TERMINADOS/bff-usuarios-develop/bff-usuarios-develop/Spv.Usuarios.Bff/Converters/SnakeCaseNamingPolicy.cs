using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Spv.Usuarios.Bff.Converters
{
    /// <summary>
    /// SnakeCaseNamingPolicy
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string ConvertName(string name)
        {
            return ToSnakeCase(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToSnakeCase(string name)
        {
            name ??= string.Empty;
            var nameBuilder = new StringBuilder(name.Length * 2);

            var prev = '_';
            for (var i = 0; i < name.Length; i++)
            {
                var ch = name[i];
                if (char.IsUpper(ch))
                {
                    if (IsChangingCase(name, prev, i))
                    {
                        nameBuilder.Append('_');
                    }

                    nameBuilder.Append(char.ToLower(ch, CultureInfo.InvariantCulture));
                }
                else
                {
                    nameBuilder.Append(ch);
                }

                prev = ch;
            }

            return nameBuilder.ToString();
        }

        private static bool IsChangingCase(string name, char prev, int i)
        {
            return (char.IsUpper(prev) && i < name.Length - 1 && char.IsLower(name[i + 1])) ||
                   (prev != '_' && !char.IsUpper(prev));
        }
    }
}
