using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Domain.Entities.V2;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities.V2
{
    public class EstadosUsuarioV2Test
    {
        [Fact]
        public void EstadosUsuarioV2Entity()
        {
            // Arrange
            var estadosUsuarioV2 = new EstadosUsuarioV2
            {
                UserStatusId = 1,
                Description = "Description",
                Users = new List<UsuarioV2>
                {
                    new UsuarioV2
                    {
                        UserId = 1
                    }
                }
            };

            //Assert
            estadosUsuarioV2.UserStatusId.Should().Be(1);
            estadosUsuarioV2.Description.Should().Be("Description");
            estadosUsuarioV2.Users.Should().NotBeNullOrEmpty();
        }
    }
}
