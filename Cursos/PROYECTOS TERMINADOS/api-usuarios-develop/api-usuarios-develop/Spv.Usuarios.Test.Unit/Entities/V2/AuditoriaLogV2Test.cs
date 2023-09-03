using System;
using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Domain.Entities.V2;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities.V2
{
    public class AuditoriaLogV2Test
    {
        [Fact]
        public void AuditoriaLogV2Entity()
        {
            //Arrange
            var auditoriaLogV2 = new AuditoriaLogV2
            {
                AuditLogId = 1,
                EventTypeId = 1,
                EventResultId = 1,
                Channel = "Channel",
                DateTime = DateTime.MinValue,
                ExtendedInfo = "Info",
                UserId = 1,
                EventResults = new ResultadosEventoV2
                {
                    EventResultId = 1,
                    Description = "Description",
                    Audits = new List<AuditoriaLogV2>()
                },
                EventTypes = new TiposEventoV2
                {
                    EventTypeId = 1,
                    Name = "Name",
                    Description = "Description",
                    Audits = new List<AuditoriaLogV2>()
                }
            };

            //Assert
            auditoriaLogV2.AuditLogId.Should().Be(1);
            auditoriaLogV2.UserId.Should().Be(1);
            auditoriaLogV2.EventTypeId.Should().Be(1);
            auditoriaLogV2.EventResultId.Should().Be(1);
            auditoriaLogV2.Channel.Should().Be("Channel");
            auditoriaLogV2.DateTime.Should().Be(DateTime.MinValue);
            auditoriaLogV2.ExtendedInfo.Should().Be("Info");

            auditoriaLogV2.EventResults.Should().NotBeNull();
            auditoriaLogV2.EventResults.EventResultId.Should().Be(1);
            auditoriaLogV2.EventResults.Description.Should().Be("Description");
            auditoriaLogV2.EventResults.Audits.Should().NotBeNull();
            
            auditoriaLogV2.EventTypes.Audits.Should().NotBeNull();
            auditoriaLogV2.EventTypes.EventTypeId.Should().Be(1);
            auditoriaLogV2.EventTypes.Name.Should().Be("Name");
            auditoriaLogV2.EventTypes.Description.Should().Be("Description");


        }
    }
}
