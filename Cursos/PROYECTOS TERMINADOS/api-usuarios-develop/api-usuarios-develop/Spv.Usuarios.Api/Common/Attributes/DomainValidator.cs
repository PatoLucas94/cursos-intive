using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Api.Common.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DomainValidator<T> : IDomainValidation
    {
        private readonly MethodInfo _validator;

        /// <summary>
        /// 
        /// </summary>
        public DomainValidator()
        {
            _validator = GetTryParseMethod(typeof(T));
        }

        private static MethodInfo GetTryParseMethod(Type type)
        {
            const string name = "TryParse";
            const string firstParameterName = "s";
            const string secondParameterName = "context";
            var expectedReturnType = typeof(Result<>).MakeGenericType(typeof(T));
            var method = (
                from m in typeof(T).GetMethods()
                where m.IsStatic && name.Equals(m.Name, StringComparison.Ordinal) && m.ReturnType == expectedReturnType
                let pars = m.GetParameters()
                where pars.Length == 2
                let par1 = pars[0]
                where firstParameterName.Equals(par1.Name, StringComparison.Ordinal) && typeof(string) == par1.ParameterType
                let par2 = pars[1]
                where secondParameterName.Equals(par2.Name, StringComparison.Ordinal) && typeof(string) == par2.ParameterType && par2.HasDefaultValue &&
                      par2.DefaultValue == null
                select m
            ).FirstOrDefault();

            if (method is null)
            {
                throw new ArgumentException($"El tipo '{type.Name}' no declara un metodo 'public static {expectedReturnType.Name} {name}(string s, string context = null)'.");
            }

            return method;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public ValidationResult Validate(string value, string context)
        {
            var result = (Result<T>)_validator.Invoke(null, new object[] { value, context });
            return result?.Map(o => ValidationResult.Success)
                .OrElse(e => new ValidationResult(CleanMessage(e)));
        }

        private static string CleanMessage(Exception e)
        {
            var message = e.Message;
            var point = message.IndexOf('.', StringComparison.Ordinal);
            return point > 0 ? message.Substring(0, point + 1) : message;
        }
    }
}
