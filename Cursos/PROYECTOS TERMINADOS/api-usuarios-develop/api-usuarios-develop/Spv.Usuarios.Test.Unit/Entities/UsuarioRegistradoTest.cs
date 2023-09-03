using System;
using FluentAssertions;
using Spv.Usuarios.Domain.Entities;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities
{
    public class UsuarioRegistradoTest
    {
        [Fact]
        public void UsuarioRegistrado()
        {
            // Arrange
            var usuarioRegistrado = new UsuarioRegistrado
            {
                DocumentTypeId = 1,
                DocumentNumber = "11222333",
                ChannelKey = "88888888",
                Active = true,
                Migrated = false,
                PasswordExpired = false,
                ForgotPasswordAttempts = 0,
                CardNumber = "1234567890987654",
                PostDateTime = DateTime.MinValue,
                ExpiredDateTime = DateTime.MinValue.AddDays(4),
                ChannelSource = null
            };

            // Assert
            usuarioRegistrado.DocumentTypeId.Should().Be(1);
            usuarioRegistrado.DocumentNumber.Should().Be("11222333");
            usuarioRegistrado.ChannelKey.Should().Be("88888888");
            usuarioRegistrado.Active.Should().Be(true);
            usuarioRegistrado.Migrated.Should().Be(false);
            usuarioRegistrado.PasswordExpired.Should().Be(false);
            usuarioRegistrado.ForgotPasswordAttempts.Should().Be(0);
            usuarioRegistrado.CardNumber.Should().Be("1234567890987654");
            usuarioRegistrado.PostDateTime.Should().Be(DateTime.MinValue);
            usuarioRegistrado.ExpiredDateTime.Should().Be(DateTime.MinValue.AddDays(4));
            usuarioRegistrado.ChannelSource.Should().Be(null);
        }
    }
}
