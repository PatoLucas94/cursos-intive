using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Entities.V2
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ReglaValidacionV2EntityIntegrationTest
    {
        private readonly IReglaValidacionV2Repository _reglaValidacionV2Repository;
        public ReglaValidacionV2EntityIntegrationTest(ServerFixture server)
        {
            _reglaValidacionV2Repository = server.HttpServer.TestServer.Services.GetRequiredService<IReglaValidacionV2Repository>();
        }

        [Fact]
        public void ReglaValidacionActivaEntity()
        {
            // Act
            var reglaValidacionV2 = _reglaValidacionV2Repository.ObtenerReglasValidacionActivasByModelAndInputAsync(3, 1).Result.Find(r => r.IsActive == true);

            // Assert
            reglaValidacionV2.Should().NotBeNull();

            reglaValidacionV2.ValidationRuleId.Should().Be(1);
            reglaValidacionV2.ValidationRuleName.Should().Be("LetrasYNumeros");
            reglaValidacionV2.ValidationRuleText.Should().Be("Letras y números.");
            reglaValidacionV2.IsActive.Should().Be(true);
            reglaValidacionV2.IsRequired.Should().Be(true);
            reglaValidacionV2.RegularExpression.Should().Be("/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/");
            reglaValidacionV2.ModelId.Should().Be(3);
            reglaValidacionV2.InputId.Should().Be(1);
            reglaValidacionV2.ValidationRulePriority.Should().Be(5);
        }
    }
}
