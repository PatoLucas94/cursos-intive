using System;
using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Domain.Entities.V2;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities.V2
{
    public class UsuarioV2Test
    {
        [Fact]
        public void UsuarioV2Entity()
        {
            // Arrange
            var usuarioV2 = new UsuarioV2
            {
                UserId = 1,
                PersonId = 0,
                DocumentCountryId = 80,
                DocumentNumber = "123456789",
                DocumentTypeId = 0,
                Username = "UserName",
                Password = "Password",
                UserStatusId = 0,
                LastPasswordChange = DateTime.MinValue,
                LastLogon = DateTime.MinValue,
                CreatedDate = DateTime.MinValue,
                LoginAttempts = 0,
                Status = new EstadosUsuarioV2
                {
                    UserStatusId = 1
                },
                UserPasswordHistory = new List<HistorialClaveUsuariosV2>
                {
                    new HistorialClaveUsuariosV2
                    {
                        PasswordHistoryId = 1
                    }
                },
                UserUsernameHistory = new List<HistorialUsuarioUsuariosV2>
                {
                    new HistorialUsuarioUsuariosV2
                    {
                        UsernameHistoryId = 1
                    }
                }
            };

            // Assert
            usuarioV2.UserId.Should().Be(1);
            usuarioV2.PersonId.Should().Be(0);
            usuarioV2.DocumentCountryId.Should().Be(80);
            usuarioV2.DocumentTypeId.Should().Be(0);
            usuarioV2.DocumentNumber.Should().Be("123456789");
            usuarioV2.Username.Should().Be("UserName");
            usuarioV2.Password.Should().Be("Password");
            usuarioV2.UserStatusId.Should().Be(0);
            usuarioV2.Status.Should().NotBeNull();
            usuarioV2.UserPasswordHistory.Should().NotBeNullOrEmpty();
            usuarioV2.UserUsernameHistory.Should().NotBeNullOrEmpty();
            usuarioV2.LoginAttempts.Should().Be(0);
            usuarioV2.LastLogon.Should().Be(DateTime.MinValue);
            usuarioV2.LastPasswordChange.Should().Be(DateTime.MinValue);
            usuarioV2.CreatedDate.Should().Be(DateTime.MinValue);

            usuarioV2.GetDocumentCountryId().Should().Be(80);
            usuarioV2.GetLoginAttemptsDate().Should().BeNull();
            usuarioV2.GetDatosUsuario().Should().BeNull();

            usuarioV2.SetCreatedDate(DateTime.MinValue);
            usuarioV2.SetDocumentNumber("123789456");
            usuarioV2.SetDocumentType(2);
            usuarioV2.SetLastLogon(DateTime.MinValue);
            usuarioV2.SetLastPasswordChange(DateTime.MinValue);
            usuarioV2.SetLoginAttempts(5);
            usuarioV2.SetLoginAttemptsDate(DateTime.MinValue);
            usuarioV2.SetPassword("TestPassword01");
            usuarioV2.SetPersonId("123987");
            usuarioV2.SetUserId(99);
            usuarioV2.SetUserName("newusername");
            usuarioV2.SetUserStatusId(43);

            usuarioV2.GetCreatedDate().Should().Be(DateTime.MinValue);
            usuarioV2.GetDocumentNumber().Should().Be("123789456");
            usuarioV2.GetDocumentType().Should().Be(2);
            usuarioV2.GetLastLogon().Should().Be(DateTime.MinValue);
            usuarioV2.GetLastPasswordChange().Should().Be(DateTime.MinValue);
            usuarioV2.GetLoginAttempts().Should().Be(5);
            usuarioV2.GetLoginAttemptsDate().Should().BeNull();
            usuarioV2.GetPassword().Should().Be("TestPassword01");
            usuarioV2.GetPersonId().Should().Be(123987);
            usuarioV2.GetUserId().Should().Be(99);
            usuarioV2.GetUserName().Should().Be("newusername");
            usuarioV2.GetUserStatusId().Should().Be(43);
            usuarioV2.GetIsEmployee().Should().BeNull();

            Assert.Throws<NotImplementedException>(() => usuarioV2.GetLastName());
            Assert.Throws<NotImplementedException>(() => usuarioV2.GetName());
        }
    }
}
