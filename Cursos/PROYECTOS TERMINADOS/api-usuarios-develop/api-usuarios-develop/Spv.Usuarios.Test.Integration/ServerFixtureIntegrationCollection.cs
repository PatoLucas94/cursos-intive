using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration
{
    [CollectionDefinition(Name)]
    public class ServerFixtureIntegrationCollection : ICollectionFixture<ServerFixture>
    {
        public const string Name = "ServerFixture collection";
    }
}
