using System;

namespace Spv.Usuarios.Domain.Interfaces
{
    interface IReglaValidacion
    {
        int GetValidationRuleId();
        string GetValidationRuleName();
        string GetValidationRuleText();
        bool? GetIsActive();
        DateTime? GetActivationDate();
        DateTime? GetInactivationDate();
        bool? GetIsRequired();
        string GetRegularExpression();
        DateTime? GetCreatedDate();
        int GetValidationRulePriority();
        int GetModelId();
        int GetInputId();
        void SetValidationRuleId(int id);
        void SetValidationRuleName(string validationRuleName);
        void SetValidationRuleText(string validationRuleText);
        void SetIsActive(bool? isActive);
        void SetActivationDate(DateTime? activationDate);
        void SetInactivationDate(DateTime? inactivationDate);
        void SetIsRequired(bool? isRequired);
        void SetRegularExpression(string regularExpression);
        void SetCreatedDate(DateTime? createdDate);
        void SetValidationRulePriority(int validationRulePriority);
        void SetModelId(int modelId);
        void SetInputId(int inputId);
    }
}