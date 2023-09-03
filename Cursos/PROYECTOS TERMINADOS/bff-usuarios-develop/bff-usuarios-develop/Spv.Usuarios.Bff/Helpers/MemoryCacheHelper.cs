using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Spv.Usuarios.Bff.Helpers
{
    [ExcludeFromCodeCoverage]
    internal static class MemoryCacheHelper
    {
        public static int CleanCache(IMemoryCache memoryCache, string key)
        {
            var cacheValues = memoryCache.GetKeys<string>()
                .Where(w => w.StartsWith(key))
                .ToList();

            foreach (var value in cacheValues)
                memoryCache.Remove(value);

            return cacheValues.Count;
        }
    }
}
