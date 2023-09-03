using System;
using FluentAssertions;
using Spv.Usuarios.Domain.Entities.V2;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities.V2
{
    public class HistorialUsuarioUsuariosV2Test
    {
        [Fact]
        public void HistorialUsuarioUsuariosV2Entity()
        {
            //Arrange
            var historialUsuarioUsuariosV2 = new HistorialUsuarioUsuariosV2
            {
                UsernameHistoryId = 1,
                AuditLogId = 1,
                CreationDate = DateTime.MinValue,
                Username = "Username",
                UserId = 1,
                Usuario = new UsuarioV2
                {
                    UserId = 1
                }
            };

            //Assert
            historialUsuarioUsuariosV2.UsernameHistoryId.Should().Be(1);
            historialUsuarioUsuariosV2.AuditLogId.Should().Be(1);
            historialUsuarioUsuariosV2.CreationDate.Should().Be(DateTime.MinValue);
            historialUsuarioUsuariosV2.Username.Should().Be("Username");
            historialUsuarioUsuariosV2.UserId.Should().Be(1);
            historialUsuarioUsuariosV2.Usuario.Should().NotBeNull();
        }
    }
}
