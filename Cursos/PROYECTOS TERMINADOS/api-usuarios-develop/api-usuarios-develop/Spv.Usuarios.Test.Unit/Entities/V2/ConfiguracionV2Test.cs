using FluentAssertions;
using Spv.Usuarios.Domain.Entities.V2;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities.V2
{
    public class ConfiguracionV2Test
    {
        [Fact]
        public void ConfiguracionV2Entity()
        {
            //Arrange
            var configuracionV2 = new ConfiguracionV2
            {
                ConfigurationId = 1,
                Description = "Description",
                IsSecurity = true,
                Name = "Name",
                Rol = "Rol",
                Type = "Type",
                Value = "Value"
            };

            //Assert
            configuracionV2.ConfigurationId.Should().Be(1);
            configuracionV2.Description.Should().Be("Description");
            configuracionV2.IsSecurity.Should().Be(true);
            configuracionV2.Name.Should().Be("Name");
            configuracionV2.Rol.Should().Be("Rol");
            configuracionV2.Type.Should().Be("Type");
            configuracionV2.Value.Should().Be("Value");
        }
    }
}
