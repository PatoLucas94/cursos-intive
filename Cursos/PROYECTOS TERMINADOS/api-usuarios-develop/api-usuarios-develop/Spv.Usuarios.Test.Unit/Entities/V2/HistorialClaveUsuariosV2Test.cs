using System;
using FluentAssertions;
using Spv.Usuarios.Domain.Entities.V2;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities.V2
{
    public class HistorialClaveUsuariosV2Test
    {
        [Fact]
        public void HistorialClaveUsuariosV2Entity()
        {
            //Arrange
            var historialClaveUsuariosV2 = new HistorialClaveUsuariosV2
            {
                PasswordHistoryId = 1,
                AuditLogId = 1,
                CreationDate = DateTime.MinValue,
                Password = "Password",
                UserId = 1,
                Usuario = new UsuarioV2
                {
                    UserId = 1
                }
            };

            //Assert
            historialClaveUsuariosV2.PasswordHistoryId.Should().Be(1);
            historialClaveUsuariosV2.AuditLogId.Should().Be(1);
            historialClaveUsuariosV2.CreationDate.Should().Be(DateTime.MinValue);
            historialClaveUsuariosV2.Password.Should().Be("Password");
            historialClaveUsuariosV2.UserId.Should().Be(1);
            historialClaveUsuariosV2.Usuario.Should().NotBeNull();
        }
    }
}
