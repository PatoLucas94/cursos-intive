using System;
using FluentAssertions;
using Spv.Usuarios.Domain.Entities.V2;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Entities.V2
{
    public class ReglaValidacionActivaV2Test
    {
        [Fact]
        public void ReglaValidacionV2Entity()
        {
            // Arrange
            var reglaValidacion = new ReglaValidacionV2
            {
                ValidationRuleId = 1,
                ValidationRuleName = "LetrasYNumeros",
                ValidationRuleText = "Letras y números.",
                IsActive = true,
                ActivationDate = DateTime.MinValue,
                InactivationDate = DateTime.MinValue,
                IsRequired = true,
                RegularExpression = "/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/",
                ModelId = 3,
                InputId = 1,
                Models = new ModelosV2
                {
                    ModelId = 3
                },
                Inputs = new InputsV2
                {
                    InputId = 1
                },
                CreatedDate = DateTime.MinValue,
                ValidationRulePriority = 5
            };

            // Assert
            reglaValidacion.ValidationRuleId.Should().Be(1);
            reglaValidacion.ValidationRuleName.Should().Be("LetrasYNumeros");
            reglaValidacion.ValidationRuleText.Should().Be("Letras y números.");
            reglaValidacion.IsActive.Should().Be(true);
            reglaValidacion.ActivationDate.Should().Be(DateTime.MinValue);
            reglaValidacion.InactivationDate.Should().Be(DateTime.MinValue);
            reglaValidacion.IsRequired.Should().Be(true);
            reglaValidacion.RegularExpression.Should().Be("/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/");
            reglaValidacion.ModelId.Should().Be(3);
            reglaValidacion.InputId.Should().Be(1);
            reglaValidacion.Models.Should().NotBeNull();
            reglaValidacion.Inputs.Should().NotBeNull();
            reglaValidacion.CreatedDate.Should().Be(DateTime.MinValue);
            reglaValidacion.ValidationRulePriority.Should().Be(5);

            reglaValidacion.SetValidationRuleId(1);
            reglaValidacion.SetValidationRuleName("LetrasYNumeros");
            reglaValidacion.SetValidationRuleText("Letras y números.");
            reglaValidacion.SetIsActive(true);
            reglaValidacion.SetActivationDate(DateTime.MinValue);
            reglaValidacion.SetInactivationDate(DateTime.MinValue);
            reglaValidacion.SetIsRequired(true);
            reglaValidacion.SetRegularExpression("/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/");
            reglaValidacion.SetModelId(3);
            reglaValidacion.SetInputId(1);
            reglaValidacion.SetCreatedDate(DateTime.MinValue);
            reglaValidacion.SetValidationRulePriority(5);

            reglaValidacion.GetValidationRuleId().Should().Be(1);
            reglaValidacion.GetValidationRuleName().Should().Be("LetrasYNumeros");
            reglaValidacion.GetValidationRuleText().Should().Be("Letras y números.");
            reglaValidacion.GetIsActive().Should().Be(true);
            reglaValidacion.GetActivationDate().Should().Be(DateTime.MinValue);
            reglaValidacion.GetInactivationDate().Should().Be(DateTime.MinValue);
            reglaValidacion.GetIsRequired().Should().Be(true);
            reglaValidacion.GetRegularExpression().Should().Be("/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/");
            reglaValidacion.GetModelId().Should().Be(3);
            reglaValidacion.GetInputId().Should().Be(1);
            reglaValidacion.GetCreatedDate().Should().Be(DateTime.MinValue);
            reglaValidacion.GetValidationRulePriority().Should().Be(5);
        }
    }
}
