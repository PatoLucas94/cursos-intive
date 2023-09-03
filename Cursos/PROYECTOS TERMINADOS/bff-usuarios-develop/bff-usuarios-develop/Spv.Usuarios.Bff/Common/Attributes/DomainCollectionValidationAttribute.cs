using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Spv.Usuarios.Bff.Converters;

namespace Spv.Usuarios.Bff.Common.Attributes
{
    /// <summary>
    /// DomainCollectionValidationAttribute
    /// </summary>
    public class DomainCollectionValidationAttribute : ValidationAttribute
    {
        private readonly IDomainValidation _validator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public DomainCollectionValidationAttribute(Type type)
        {
            var validatorType = typeof(DomainValidator<>).MakeGenericType(type);
            _validator = (IDomainValidation)Activator.CreateInstance(validatorType);
        }

        /// <summary>
        /// 
        /// </summary>
        public int CantidadMinima { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public int CantidadMaxima { get; set; } = 1000;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var nombre = SnakeCaseNamingPolicy.ToSnakeCase(validationContext?.MemberName);
            if (value is null)
            {
                return new ValidationResult($"'{nombre}' no puede ser null.");
            }

            if (!(value is IEnumerable enumerable))
            {
                return new ValidationResult($"'{nombre}' debe ser un array.");
            }

            var items = enumerable.OfType<object>().ToArray();
            if (items.Length < CantidadMinima)
            {
                return new ValidationResult(
                    $"'{nombre}' debe contener como mínimo '{CantidadMinima}' elemento{(CantidadMinima == 1 ? "" : "s")}.");
            }

            if (items.Length > CantidadMaxima)
            {
                return new ValidationResult(
                    $"'{nombre}' debe contener como máximo '{CantidadMaxima}' elemento{(CantidadMaxima == 1 ? "" : "s")}.");
            }

            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];
                var result = _validator.Validate(item?.ToString(), $"{nombre}[{i}]");
                if (result != ValidationResult.Success)
                {
                    return result;
                }
            }

            return ValidationResult.Success;
        }
    }
}
