using Spv.Usuarios.Bff.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration
{
    [CollectionDefinition(Name)]
    public class ServerFixtureIntegrationCollection : ICollectionFixture<ServerFixture>
    {
        public const string Name = "ServerFixture collection";
    }
}
