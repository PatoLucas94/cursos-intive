using System.ComponentModel.DataAnnotations;

namespace Spv.Usuarios.Api.Common.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainValidation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        ValidationResult Validate(string value, string context);
    }
}
