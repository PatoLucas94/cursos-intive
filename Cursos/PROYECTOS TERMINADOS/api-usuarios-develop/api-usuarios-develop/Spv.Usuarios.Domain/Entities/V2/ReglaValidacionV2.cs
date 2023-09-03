using System;
using System.ComponentModel.DataAnnotations;
using Spv.Usuarios.Domain.Interfaces;

namespace Spv.Usuarios.Domain.Entities.V2
{
    public class ReglaValidacionV2 : IReglaValidacion
    {
        [Key]
        public int ValidationRuleId { get; set; }
        public string ValidationRuleName { get; set; }
        public string ValidationRuleText { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? InactivationDate { get; set; }
        public bool? IsRequired { get; set; }
        public string RegularExpression { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ValidationRulePriority { get; set; }
        public int ModelId { get; set; }
        public int InputId { get; set; }

        // relations
        public ModelosV2 Models { get; set; }
        public InputsV2 Inputs { get; set; }

        public DateTime? GetActivationDate()
        {
            return ActivationDate;
        }
        public DateTime? GetCreatedDate()
        {
            return CreatedDate;
        }
        public DateTime? GetInactivationDate()
        {
            return InactivationDate;
        }
        public bool? GetIsActive()
        {
            return IsActive;
        }
        public bool? GetIsRequired()
        {
            return IsRequired;
        }
        public string GetRegularExpression()
        {
            return RegularExpression;
        }
        public int GetValidationRuleId()
        {
            return ValidationRuleId;
        }
        public string GetValidationRuleName()
        {
            return ValidationRuleName;
        }
        public string GetValidationRuleText()
        {
            return ValidationRuleText;
        }
        public int GetValidationRulePriority()
        {
            return ValidationRulePriority;
        }
        public int GetModelId()
        {
            return ModelId;
        }
        public int GetInputId()
        {
            return InputId;
        }
        public void SetActivationDate(DateTime? activationDate)
        {
            ActivationDate = activationDate;
        }
        public void SetCreatedDate(DateTime? createdDate)
        {
            CreatedDate = createdDate;
        }
        public void SetInactivationDate(DateTime? inactivationDate)
        {
            InactivationDate = inactivationDate;
        }
        public void SetIsActive(bool? isActive)
        {
            IsActive = isActive;
        }
        public void SetIsRequired(bool? isRequired)
        {
            IsRequired = isRequired;
        }
        public void SetRegularExpression(string regularExpression)
        {
            RegularExpression = regularExpression;
        }
        public void SetValidationRuleId(int id)
        {
            ValidationRuleId = id;
        }
        public void SetValidationRuleName(string validationRuleName)
        {
            ValidationRuleName = validationRuleName;
        }
        public void SetValidationRuleText(string validationRuleText)
        {
            ValidationRuleText = validationRuleText;
        }
        public void SetValidationRulePriority(int validationRulePriority)
        {
            ValidationRulePriority = validationRulePriority;
        }
        public void SetModelId(int modelId)
        {
            ModelId = modelId;
        }
        public void SetInputId(int inputId)
        {
            InputId = inputId;
        }
    }
}