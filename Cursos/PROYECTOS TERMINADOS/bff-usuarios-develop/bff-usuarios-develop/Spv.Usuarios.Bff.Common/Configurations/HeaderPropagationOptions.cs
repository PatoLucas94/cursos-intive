using System.Collections.Generic;

namespace Spv.Usuarios.Bff.Common.Configurations
{
    public class HeaderPropagationOptions
    {
        public IList<string> HeaderNames { get; set; } = new List<string>();
    }
}
