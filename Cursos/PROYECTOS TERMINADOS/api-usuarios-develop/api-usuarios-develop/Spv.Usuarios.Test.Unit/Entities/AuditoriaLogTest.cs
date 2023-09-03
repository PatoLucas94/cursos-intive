using System;
using FluentAssertions;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Enums;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities
{
    public class AuditoriaLogTest
    {
        [Fact]
        public void AuditoriaLogEntity()
        {
            //Arrange
            var auditoria = new AuditoriaLog
            {
                ActionId = (int)AuditAction.LogOn,
                ActionResultId = (int)AuditActionResult.LoggedOn,
                DateTime = DateTime.MinValue,
                ChannelId = 0,
                ExtendedInfo = "ExtendedInfo",
                OrganizationId = 0,
                UserAgent = "UserAgent",
                UserIP = "UserIP",
                UserId = 0,
                AuditLogId = 0,
                SessionId = "SessionId"
            };

            //Assert
            auditoria.ActionId.Should().Be((int)AuditAction.LogOn);
            auditoria.ActionResultId.Should().Be((int)AuditActionResult.LoggedOn);
            auditoria.AuditLogId.Should().Be(0);
            auditoria.ChannelId.Should().Be(0);
            auditoria.DateTime.Should().Be(DateTime.MinValue);
            auditoria.ExtendedInfo.Should().Be("ExtendedInfo");
            auditoria.OrganizationId.Should().Be(0);
            auditoria.UserAgent.Should().Be("UserAgent");
            auditoria.UserId.Should().Be(0);
            auditoria.UserIP.Should().Be("UserIP");
            auditoria.SessionId.Should().Be("SessionId");
        }
    }
}
