using System.Collections.Generic;
using FluentAssertions;
using Spv.Usuarios.Domain.Entities.V2;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities.V2
{
    public class ResultadosEventoV2Test
    {
        [Fact]
        public void EventResultsV2Entity()
        {
            //Arrange
            var eventResultsV2 = new ResultadosEventoV2
            {
                EventResultId = 1,
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
            eventResultsV2.EventResultId.Should().Be(1);
            eventResultsV2.Description.Should().Be("Description");
            eventResultsV2.Audits.Should().NotBeNullOrEmpty();
        }
    }
}
