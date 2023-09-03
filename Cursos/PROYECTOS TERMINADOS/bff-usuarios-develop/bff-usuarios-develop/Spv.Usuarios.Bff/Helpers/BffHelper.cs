using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Spv.Usuarios.Bff.Helpers
{
    /// <summary>
    /// Bff Helper
    /// </summary>
    public static class BffHelper
    {
        /// <summary>
        /// Load Helper
        /// </summary>
        /// <returns>Entrega un string con la versión y fecha del BFF</returns>
        public static string LoadVersion()
        {
            var fileVersion = new FileInfo(GetFileVersionPath()); 

            var date = string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-ddTHH:mm}", fileVersion.LastWriteTimeUtc);
            var version = string.Join("", File.ReadLines(fileVersion.FullName).Take(1)).Trim();
            return $"{version}_{date}";
        }

        /// <summary>
        /// Obtiene el path del archivo VERSION
        /// </summary>
        /// <returns>El path del archivo VERSION</returns>
        public static string GetFileVersionPath()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "VERSION");
        }
    }
}
