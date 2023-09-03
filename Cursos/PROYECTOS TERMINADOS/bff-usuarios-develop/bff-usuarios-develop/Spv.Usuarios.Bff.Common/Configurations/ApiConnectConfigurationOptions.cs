using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.Configurations
{
    [ExcludeFromCodeCoverage]
    public class ApiConnectConfigurationOptions
    {
        public string ApiConnectGeneratedFilePath { get; set; }

        public string ApiConnectTemplatePath { get; set; }
    }
}
