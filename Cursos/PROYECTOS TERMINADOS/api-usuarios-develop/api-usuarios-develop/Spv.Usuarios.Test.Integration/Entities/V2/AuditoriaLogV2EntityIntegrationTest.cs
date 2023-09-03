using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Test.Infrastructure;
using System;
using System.Linq;
using Xunit;

namespace Spv.Usuarios.Test.Integration.Entities.V2
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class AuditoriaLogV2EntityIntegrationTest
    {
        private readonly IAuditoriaLogV2Repository _auditoriaLogV2Repository;
        public AuditoriaLogV2EntityIntegrationTest(ServerFixture server)
        {
            _auditoriaLogV2Repository = server.HttpServer.TestServer.Services.GetRequiredService<IAuditoriaLogV2Repository>();
        }

        [Fact]
        public void AuditoriaLogEntity()
        {
            // Act
            var auditoriaLogV2 = _auditoriaLogV2Repository.Find(u => u.AuditLogId == 1);

            // Assert
            auditoriaLogV2.Should().NotBeNull();

            auditoriaLogV2.AuditLogId.Should().Be(1);
            auditoriaLogV2.EventTypeId.Should().Be(1);
            auditoriaLogV2.EventResultId.Should().Be(1);
            auditoriaLogV2.DateTime.Should().Be(DateTime.MinValue);
            auditoriaLogV2.ExtendedInfo.Should().Be("Info 1");
            auditoriaLogV2.UserId.Should().Be(1);
            auditoriaLogV2.Channel.Should().Be("Channel 1");
        }

        [Fact]
        public void AuditoriaLogConEventTypeYEventResultEntity()
        {
            // Act
            var auditoriaLogV2 = _auditoriaLogV2Repository
                .Get(u => u.AuditLogId == 1, o => o.OrderByDescending(u => u.AuditLogId), $"{nameof(AuditoriaLogV2.EventTypes)},{nameof(AuditoriaLogV2.EventResults)}")
                .FirstOrDefault();

            // Assert
            auditoriaLogV2.Should().NotBeNull();

            auditoriaLogV2.AuditLogId.Should().Be(1);
            auditoriaLogV2.EventTypeId.Should().Be(1);
            auditoriaLogV2.EventResultId.Should().Be(1);
            auditoriaLogV2.DateTime.Should().Be(DateTime.MinValue);
            auditoriaLogV2.ExtendedInfo.Should().Be("Info 1");
            auditoriaLogV2.UserId.Should().Be(1);
            auditoriaLogV2.Channel.Should().Be("Channel 1");

            auditoriaLogV2.EventResults.Should().NotBeNull();
            auditoriaLogV2.EventTypes.Should().NotBeNull();

            auditoriaLogV2.EventResults.EventResultId.Should().Be(1);
            auditoriaLogV2.EventResults.Description.Should().Be("Description 1");

            auditoriaLogV2.EventTypes.EventTypeId.Should().Be(1);
            auditoriaLogV2.EventTypes.Name.Should().Be("Autenticación");
            auditoriaLogV2.EventTypes.Description.Should().Be("Description 1");
        }
    }
}
