using System;
using System.ComponentModel.DataAnnotations;
using Spv.Usuarios.Bff.Converters;

namespace Spv.Usuarios.Bff.Common.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainValidationAttribute : ValidationAttribute
    {
        private readonly IDomainValidation _validator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public DomainValidationAttribute(Type type)
        {
            var genericValidatorType = typeof(DomainValidator<>);
            var validatorType = genericValidatorType.MakeGenericType(type);
            _validator = (IDomainValidation)Activator.CreateInstance(validatorType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return _validator.Validate(value?.ToString(), SnakeCaseNamingPolicy.ToSnakeCase(validationContext?.MemberName));
        }
    }
}
