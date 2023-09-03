using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Domain.Entities.V2;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities.V2
{
    public class TiposEventoV2Test
    {
        [Fact]
        public void EventTypesV2Entity()
        {
            //Arrange
            var eventTypesV2 = new TiposEventoV2
            {
                EventTypeId = 1,
                Name = "Name",
                Description = "Description",
                Audits = new List<AuditoriaLogV2>
                {
                    new AuditoriaLogV2
                    {
                        AuditLogId = 1
                    }
                }
            };

            //Assert
            eventTypesV2.EventTypeId.Should().Be(1);
            eventTypesV2.Name.Should().Be("Name");
            eventTypesV2.Description.Should().Be("Description");
            eventTypesV2.Audits.Should().NotBeNullOrEmpty();
        }
    }
}
