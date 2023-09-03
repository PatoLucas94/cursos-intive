using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Common.Configurations
{
    [ExcludeFromCodeCoverage]
    public class HeaderPropagationOptions
    {
        public IList<string> HeaderNames { get; set; } = new List<string>();
    }
}
